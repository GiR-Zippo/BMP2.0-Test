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

namespace BardMusicPlayer.Ui.Views
{
    /// <summary>
    /// Interaktionslogik für Classic_MainView.xaml
    /// </summary>
    public partial class Classic_MainView : UserControl
    {
        public Classic_MainView()
        {
            InitializeComponent();

            Autostart_source.SelectedIndex = (int)Globals.Settings.AutostartType;

            this.SongName.Text = PlaybackFunctions.GetSongName();
            Maestro.BmpMaestro.Instance.OnPlaybackTimeChanged += Instance_PlaybackTimeChanged;
            Maestro.BmpMaestro.Instance.OnSongMaxTime += Instance_PlaybackMaxTime;
            BmpSeer.Instance.ChatLog += Instance_ChatLog;
            BmpSeer.Instance.EnsembleStarted += Instance_EnsembleStarted;

            
        }

        private void Instance_ChatLog(Seer.Events.ChatLog seerEvent)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.AppendChatLog(seerEvent.ChatLogCode, seerEvent.ChatLogLine)));
        }

        private void Instance_PlaybackMaxTime(object sender, ITimeSpan e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackMaxTime(e)));
        }

        private void Instance_PlaybackTimeChanged(object sender, ITimeSpan e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.PlaybackTimeChanged(e)));
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
            PlaybackFunctions.PlaySong();
            Play_Button.Content = @"⏸";
        }

        private void PlaybackMaxTime(ITimeSpan e)
        {
            string time;
            string Seconds = e.ToString().Split(':')[2];
            string Minutes = e.ToString().Split(':')[1];
            time = ((Minutes.Length == 1) ? "0" + Minutes : Minutes) + ":" + ((Seconds.Length == 1) ? "0" + Seconds : Seconds);
            TotalTime.Content = time;

            MetricTimeSpan d;
            MetricTimeSpan.TryParse(e.ToString(), out d);
            Playbar_Slider.Maximum = d.TotalMicroseconds;

        }

        private void PlaybackTimeChanged(ITimeSpan e)
        {
            string time;
            string Seconds = e.ToString().Split(':')[2];
            string Minutes = e.ToString().Split(':')[1];
            time = ((Minutes.Length == 1) ? "0" + Minutes : Minutes) + ":" + ((Seconds.Length == 1) ? "0" + Seconds : Seconds);
            ElapsedTime.Content = time;
            
            MetricTimeSpan d;
            MetricTimeSpan.TryParse(e.ToString(), out d);
            if (!_Playbar_dragStarted)
                Playbar_Slider.Value = d.TotalMicroseconds;
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

        private void Autostart_source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int d = Autostart_source.SelectedIndex;
            Globals.Settings.AutostartType=(Globals.Settings.Autostart_Types)d;
            Globals.Settings.SaveConfig();
        }
    }
}