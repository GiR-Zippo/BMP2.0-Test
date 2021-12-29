using BardMusicPlayer.Seer;
using BardMusicPlayer.Ui.Globals.SkinContainer;
using Melanchall.DryWetMidi.Interaction;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using BardMusicPlayer.Ui.Functions;
using BardMusicPlayer.Maestro;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Transmogrify.Song;

namespace BardMusicPlayer.Ui.Skinned
{
    /// <summary>
    /// Interaktionslogik für Skinned_MainView.xaml
    /// </summary>
    public partial class Skinned_MainView : UserControl
    {
        private bool _Playbar_dragStarted = false;
        public Skinned_PlaylistView _PlaylistView;
        public BardsWindow _BardListView;

        private CancellationTokenSource Scroller = new CancellationTokenSource();
        public Skinned_MainView()
        {
            InitializeComponent();
            LoadSkin(BmpPigeonhole.Instance.LastSkin);

            _PlaylistView = new Skinned_PlaylistView();
            _BardListView = new BardsWindow();

            var mw = (MainWindow)Application.Current.MainWindow;
            _PlaylistView.Show();
            _BardListView.Show();
            
            _PlaylistView.Top = ((MainWindow)Application.Current.MainWindow).Top + ((MainWindow)Application.Current.MainWindow).ActualHeight;
            _PlaylistView.Left = ((MainWindow)Application.Current.MainWindow).Left;
            _PlaylistView.OnLoadSongFromPlaylist += OnLoadSongFromPlaylist;

            BmpMaestro.Instance.OnSongMaxTime += Instance_PlaybackMaxTime;
            BmpMaestro.Instance.OnPlaybackTimeChanged += Instance_PlaybackTimeChanged;
            BmpMaestro.Instance.OnTrackNumberChanged += Instance_TrackNumberChanged;
            BmpMaestro.Instance.OnPlaybackStopped += Instance_PlaybackStopped;

            BmpSeer.Instance.EnsembleStarted += Instance_EnsembleStarted;
            BmpSeer.Instance.ChatLog += Instance_ChatLog;
            WriteTrackField("Track " + Globals.Globals.CurrentTrack.ToString());
            WriteSmallDigitField(Globals.Globals.CurrentTrack.ToString());
        }

        private void OnLoadSongFromPlaylist(object sender, BmpSong e)
        {
            Scroller.Cancel();
            Scroller = new CancellationTokenSource();
            UpdateScroller(Scroller.Token, PlaybackFunctions.GetSongName()).ConfigureAwait(false);
            WriteInstrumentDigitField(PlaybackFunctions.InstrumentName);

            if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYNEXT)
                PlaybackFunctions.PlaySong();
        }

        private void Instance_ChatLog(Seer.Events.ChatLog seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.AppendChatLog(seerEvent.ChatLogCode, seerEvent.ChatLogLine)));
        }

        private void Instance_EnsembleStarted(Seer.Events.EnsembleStarted seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.EnsembleStart()));
        }

        private void Instance_PlaybackMaxTime(object sender, Maestro.Events.MaxPlayTimeEvent e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackMaxTime(e)));
        }

        private void Instance_PlaybackTimeChanged(object sender, Maestro.Events.CurrentPlayPositionEvent e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackTimeChanged(e)));
        }

        private void Instance_TrackNumberChanged(object sender, Maestro.Events.TrackNumberChangedEvent e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.UpdateTrackNumberAndInstrument(e)));
        }

        private void Instance_PlaybackStopped(object sender, bool e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.OnSongStopped()));
        }

        public void EnsembleStart()
        {
            if (Globals.Settings.AutostartType != Globals.Settings.Autostart_Types.VIA_METRONOME)
                return;
            if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYING)
                return;
            Thread.Sleep(2475);
            PlaybackFunctions.PlaySong();
        }

        public void AppendChatLog(string code, string line)
        {
            if (code == "0039")
            {
                if (line.Contains(@"Anzählen beginnt") ||
                    line.Contains("The count-in will now commence.") ||
                    line.Contains("orchestre est pr"))
                {
                    if (Globals.Settings.AutostartType != Globals.Settings.Autostart_Types.VIA_CHAT)
                        return;

                    if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYING)
                        return;
                    Thread.Sleep(3000);
                    PlaybackFunctions.PlaySong();
                }
            }
        }

        private void UpdateTrackNumberAndInstrument(Maestro.Events.TrackNumberChangedEvent e)
        {
            if (!e.IsHost)
                return;
            Globals.Globals.CurrentTrack = e.TrackNumber;
            PlaybackFunctions.SetInstrumentName();
            WriteTrackField("Track " + Globals.Globals.CurrentTrack.ToString());
            WriteSmallDigitField(Globals.Globals.CurrentTrack.ToString());
            WriteInstrumentDigitField(PlaybackFunctions.InstrumentName);
        }

        private void OnSongStopped()
        {
            if (_PlaylistView.LoopPlay)
                _PlaylistView.PlayNextSong();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).DragMove();
                Window mainWindow = Application.Current.MainWindow;
                _PlaylistView.Width = mainWindow.Width;
                _PlaylistView.Top = (mainWindow.Top + mainWindow.Height);
                _PlaylistView.Left = mainWindow.Left;
                mainWindow = null;
            }
        }

        private void MainLostFocus(object sender, System.EventArgs e)
        {
            //TitleBar.Fill = _titlebar_image[1];
        }

        private void PlaybackMaxTime(Maestro.Events.MaxPlayTimeEvent e)
        {
            DisplayPlayTime(e.timeSpan);
            Playbar_Slider.Dispatcher.BeginInvoke(new Action(() => Playbar_Slider.Maximum = e.tick));
        }
        private void PlaybackTimeChanged(Maestro.Events.CurrentPlayPositionEvent e)
        {
            if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYING)
                DisplayPlayTime(e.timeSpan);
            if (!_Playbar_dragStarted)
                Playbar_Slider.Dispatcher.BeginInvoke(new Action(() => Playbar_Slider.Value = e.tick));
        }

        private void Playbar_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void Playbar_Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            this._Playbar_dragStarted = true;
        }

        private void Playbar_Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            BmpMaestro.Instance.SetPlaybackStart((int)Playbar_Slider.Value);
            this._Playbar_dragStarted = false;
        }

        private void DisplayPlayTime(TimeSpan t)
        {
            string Seconds = t.ToString().Split(':')[2];
            this.Second_Last.Dispatcher.BeginInvoke(new Action(() => Second_Last.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Seconds.Length == 1) ? Convert.ToInt32(Seconds) : Convert.ToInt32(Seconds.Substring(1, 1)))]));
            this.Second_First.Dispatcher.BeginInvoke(new Action(() => Second_First.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Seconds.Length == 1) ? 0 : Convert.ToInt32(Seconds.Substring(0, 1)))]));

            string Minutes = t.ToString().Split(':')[1];
            this.Minutes_Last.Dispatcher.BeginInvoke(new Action(() => Minutes_Last.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Minutes.Length == 1) ? Convert.ToInt32(Minutes) : Convert.ToInt32(Minutes.Substring(1, 1)))]));
            this.Minutes_First.Dispatcher.BeginInvoke(new Action(() => Minutes_First.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Minutes.Length == 1) ? 0 : Convert.ToInt32(Minutes.Substring(0, 1)))]));
        }

        private void ShowBardsWindow_Click(object sender, RoutedEventArgs e)
        {
            this._BardListView.Visibility = Visibility.Visible;
        }

        private void ShowPlaylistWindow_Click(object sender, RoutedEventArgs e)
        {
            this._PlaylistView.Visibility = Visibility.Visible;
        }


    }
}
