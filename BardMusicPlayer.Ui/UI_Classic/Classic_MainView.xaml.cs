
using BardMusicPlayer.Seer;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using BardMusicPlayer.Ui.Functions;
using BardMusicPlayer.Coffer;
using BardMusicPlayer.UI.Functions;
using BardMusicPlayer.Maestro;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if SIREN
using BardMusicPlayer.Siren;
#endif

namespace BardMusicPlayer.Ui.Classic
{
    /// <summary>
    /// Interaktionslogik für Classic_MainView.xaml
    /// </summary>
    public partial class Classic_MainView : UserControl
    {
        public class MidiInputDevice
        {
            public int Key;
            public string Value;
        }


        public Classic_MainView()
        {
            InitializeComponent();

            ShowingPlaylists = true;
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylists();

            this.SongName.Text = PlaybackFunctions.GetSongName();
            BmpMaestro.Instance.OnPlaybackTimeChanged += Instance_PlaybackTimeChanged;
            BmpMaestro.Instance.OnSongMaxTime += Instance_PlaybackMaxTime;
            BmpMaestro.Instance.OnPlaybackStopped += Instance_PlaybackStopped;
            BmpSeer.Instance.ChatLog += Instance_ChatLog;
            BmpSeer.Instance.EnsembleStarted += Instance_EnsembleStarted;
            LoadConfig();
        }

        private void Instance_ChatLog(Seer.Events.ChatLog seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.AppendChatLog(seerEvent.ChatLogCode, seerEvent.ChatLogLine)));
        }

        private void Instance_PlaybackMaxTime(object sender, Maestro.Events.MaxPlayTimeEvent e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackMaxTime(e)));
        }

        private void Instance_PlaybackTimeChanged(object sender, Maestro.Events.CurrentPlayPositionEvent e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackTimeChanged(e)));
        }

        private void Instance_PlaybackStopped(object sender, bool e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackStopped()));
        }

        private void Instance_EnsembleStarted(Seer.Events.EnsembleStarted seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.EnsembleStart()));
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
                    Play_Button.Content = @"⏸";
                }
            }
            this.ChatBox.AppendText(line + "\r\n");
        }

        public void EnsembleStart()
        {
            if (Globals.Settings.AutostartType != Globals.Settings.Autostart_Types.VIA_METRONOME)
                return;
            if (PlaybackFunctions.PlaybackState == PlaybackFunctions.PlaybackState_Enum.PLAYBACK_STATE_PLAYING)
                return;
            Thread.Sleep(2475);
            PlaybackFunctions.PlaySong();
            Play_Button.Content = @"⏸";
        }

        private void PlaybackMaxTime(Maestro.Events.MaxPlayTimeEvent e)
        {
            string time;
            string Seconds = e.timeSpan.Seconds.ToString();
            string Minutes = e.timeSpan.Minutes.ToString();
            time = ((Minutes.Length == 1) ? "0" + Minutes : Minutes) + ":" + ((Seconds.Length == 1) ? "0" + Seconds : Seconds);
            TotalTime.Content = time;

            Playbar_Slider.Maximum = e.tick;

        }

        private void PlaybackTimeChanged(Maestro.Events.CurrentPlayPositionEvent e)
        {
            string time;
            string Seconds = e.timeSpan.Seconds.ToString();
            string Minutes = e.timeSpan.Minutes.ToString();
            time = ((Minutes.Length == 1) ? "0" + Minutes : Minutes) + ":" + ((Seconds.Length == 1) ? "0" + Seconds : Seconds);
            ElapsedTime.Content = time;

            if (!_Playbar_dragStarted)
                Playbar_Slider.Value = e.tick;
        }

        public void PlaybackStopped()
        {
            Play_Button.Content = @"▶";
        }


        /* Track UP/Down */
        private int _numValue = 1;
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                track_txtNum.Text = "t" + value.ToString();
                PlaybackFunctions.SetTrackNumber(_numValue);
                InstrumentInfo.Content = PlaybackFunctions.InstrumentName;
            }
        }
        private void track_cmdUp_Click(object sender, RoutedEventArgs e)
        { 
            NumValue++;
        }

        private void track_cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue == 1)
                return;
            NumValue--;
        }

        private void track_txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (track_txtNum == null)
                return;

            if (!int.TryParse(track_txtNum.Text.Replace("t", ""), out _numValue))
                track_txtNum.Text = _numValue.ToString();
        }

        private void Siren_Load_Click(object sender, RoutedEventArgs e)
        {
#if SIREN
            BmpSiren.Instance.Load(PlaybackFunctions.CurrentSong);
#endif
        }

        private void Siren_Play_Click(object sender, RoutedEventArgs e)
        {
#if SIREN
            BmpSiren.Instance.Play();
#endif
        }

        private void Siren_Stop_Click(object sender, RoutedEventArgs e)
        {
#if SIREN
            BmpSiren.Instance.Stop();
#endif
        }

        private void Siren_Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /*Slider slider = e.OriginalSource as Slider;
            BmpSiren.Instance.Setup((float)slider.Value, 2, 100);*/
        }
    }
}