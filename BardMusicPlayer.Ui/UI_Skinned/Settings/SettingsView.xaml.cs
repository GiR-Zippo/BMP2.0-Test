using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Maestro;
using System.Collections.Generic;
using BardMusicPlayer.Jamboree;
using BardMusicPlayer.Jamboree.Events;
using System;

namespace BardMusicPlayer.Ui.Skinned
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            //Design Tab
            ClassicSkin.IsChecked = BmpPigeonhole.Instance.ClassicUi;

            //Playback Tab
            this.LocalOrchestraBox.IsChecked = BmpPigeonhole.Instance.LocalOrchestra;
            this.HoldNotesBox.IsChecked = BmpPigeonhole.Instance.HoldNotes;
            this.ForcePlaybackBox.IsChecked = BmpPigeonhole.Instance.ForcePlayback;
            MIDI_Input_DeviceBox.Items.Clear();
            MIDI_Input_DeviceBox.ItemsSource = Maestro.Utils.MidiInput.ReloadMidiInputDevices();
            this.MIDI_Input_DeviceBox.SelectedIndex = BmpPigeonhole.Instance.MidiInputDev + 1;

            //Syncsettings
            Autostart_source.SelectedIndex = BmpPigeonhole.Instance.AutostartMethod;

            //Effects
            this.Settings_EffectsHost.IsChecked = BmpPigeonhole.Instance.IsChoreoHost;

            //Networkparty Tab
            BmpJamboree.Instance.OnPartyCreated += Instance_PartyCreated;
        }

        private void Instance_PartyCreated(object sender, PartyCreatedEvent e)
        {
            string Token = e.Token;
            this.Dispatcher.BeginInvoke(new Action(() => this.PartyToken_Text.Text = Token));
        }

        #region DesignTab controls
        private void ClassicSkin_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.ClassicUi = ClassicSkin.IsChecked ?? true;
        }
        #endregion

        #region PlaybackTab controls
        private void LocalOrchestraBox_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.LocalOrchestra = LocalOrchestraBox.IsChecked ?? false;
        }

        private void Hold_Notes_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.HoldNotes = (HoldNotesBox.IsChecked ?? false);
        }

        private void Force_Playback_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.ForcePlayback = (ForcePlaybackBox.IsChecked ?? false);
        }

        private void MIDI_Input_Device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var d = (KeyValuePair<int, string>)MIDI_Input_DeviceBox.SelectedItem;
            BmpPigeonhole.Instance.MidiInputDev = d.Key;
            if (d.Key == -1)
            {
                BmpMaestro.Instance.CloseInputDevice();
                return;
            }

            BmpMaestro.Instance.OpenInputDevice(d.Key);
        }
        #endregion

        #region SyncsettingsTab controls
        private void Autostart_source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int d = Autostart_source.SelectedIndex;
            BmpPigeonhole.Instance.AutostartMethod = (int)d;
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "SKIN file|*.wsz;*.WSZ|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            ((Skinned_MainView)System.Windows.Application.Current.MainWindow.DataContext).LoadSkin(openFileDialog.FileName);
            BmpPigeonhole.Instance.LastSkin = openFileDialog.FileName;
        }

        private void Settings_EffectsHost_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.IsChoreoHost = Settings_EffectsHost.IsChecked ?? false;
        }

        #region NetworkPartyTab Controls
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string token = PartyToken_Text.Text;
            PartyToken_Text.Text = "Please wait...";
            BmpJamboree.Instance.CreateParty(token, 0, "Test Player"); // BmpMaestro.Instance.GetHostGame().PlayerName);
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            BmpJamboree.Instance.JoinParty(this.PartyToken_Text.Text);
        }
        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            BmpJamboree.Instance.LeaveParty();
        }
        private void ForcePlay_Click(object sender, RoutedEventArgs e)
        {
            BmpJamboree.Instance.SendPerformanceStart();
        }
        #endregion
    }
}
