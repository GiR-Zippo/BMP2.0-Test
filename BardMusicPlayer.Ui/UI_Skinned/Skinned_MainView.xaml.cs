using BardMusicPlayer.Grunt;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BardMusicPlayer.Ui.Functions;

namespace BardMusicPlayer.Ui.Views
{
    /// <summary>
    /// Interaktionslogik für Skinned_MainView.xaml
    /// </summary>
    public partial class Skinned_MainView : UserControl
    {

        private bool _Playbar_dragStarted = false;

        public Skinned_MainView()
        {
            InitializeComponent();

            LoadSkin(Globals.Globals.DataPath + @"soundcheck1.wsz");

            Maestro.BmpMaestro.Instance.OnPlaybackTimeChanged += Instance_PlaybackTimeChanged;
            Maestro.BmpMaestro.Instance.OnSongMaxTime += Instance_PlaybackMaxTime;


            BmpSeer.Instance.EnsembleStarted += Instance_EnsembleStarted;
            BmpSeer.Instance.ChatLog += Instance_ChatLog;
            WriteTrackField("Track " + Globals.Globals.CurrentTrack.ToString());
            WriteSmallDigitField(Globals.Globals.CurrentTrack.ToString());
        }

        private void Instance_ChatLog(Seer.Events.ChatLog seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.AppendChatLog(seerEvent.ChatLogCode, seerEvent.ChatLogLine)));
        }

        private void Instance_EnsembleStarted(Seer.Events.EnsembleStarted seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.EnsembleStart()));
        }

        private void Instance_PlaybackMaxTime(object sender, ITimeSpan e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackMaxTime(e)));
        }

        private void Instance_PlaybackTimeChanged(object sender, ITimeSpan e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackTimeChanged(e)));
        }

        public void EnsembleStart()
        {
            if (Globals.Settings.AutostartType != Globals.Settings.Autostart_Types.VIA_METRONOME)
                return;
            if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYING)
                return;
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

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                ((MainWindow)System.Windows.Application.Current.MainWindow).DragMove();
        }

        private void MainLostFocus(object sender, System.EventArgs e)
        {
            //TitleBar.Fill = _titlebar_image[1];
        }

        private void PlaybackMaxTime(ITimeSpan t)
        {
            DisplayPlayTime(t);
            MetricTimeSpan d;
            MetricTimeSpan.TryParse(t.ToString(), out d);
            Playbar_Slider.Dispatcher.BeginInvoke(new Action(() => Playbar_Slider.Maximum = d.TotalMicroseconds));
        }
        private void PlaybackTimeChanged(ITimeSpan t)
        {
            if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYING)
                DisplayPlayTime(t);
            MetricTimeSpan d;
            MetricTimeSpan.TryParse(t.ToString(), out d);
            if (!_Playbar_dragStarted)
                Playbar_Slider.Dispatcher.BeginInvoke(new Action(() => Playbar_Slider.Value = d.TotalMicroseconds));
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
            Maestro.BmpMaestro.Instance.SetPlaybackStart(((Slider)sender).Value);
            this._Playbar_dragStarted = false;
        }

        private void DisplayPlayTime(ITimeSpan t)
        {
            string Seconds = t.ToString().Split(':')[2];
            this.Second_Last.Dispatcher.BeginInvoke(new Action(() => Second_Last.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Seconds.Length == 1) ? Convert.ToInt32(Seconds) : Convert.ToInt32(Seconds.Substring(1, 1)))]));
            this.Second_First.Dispatcher.BeginInvoke(new Action(() => Second_First.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Seconds.Length == 1) ? 0 : Convert.ToInt32(Seconds.Substring(0, 1)))]));

            string Minutes = t.ToString().Split(':')[1];
            this.Minutes_Last.Dispatcher.BeginInvoke(new Action(() => Minutes_Last.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Minutes.Length == 1) ? Convert.ToInt32(Minutes) : Convert.ToInt32(Minutes.Substring(1, 1)))]));
            this.Minutes_First.Dispatcher.BeginInvoke(new Action(() => Minutes_First.Fill = SkinContainer.NUMBERS[(SkinContainer.NUMBER_TYPES)((Minutes.Length == 1) ? 0 : Convert.ToInt32(Minutes.Substring(0, 1)))]));
        }
    }
}
