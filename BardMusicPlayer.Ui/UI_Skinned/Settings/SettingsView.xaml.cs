using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Maestro;

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
            Autostart_source.SelectedIndex = (int)Globals.Settings.AutostartType;

            this.LocalOrchestraBox.IsChecked = BmpPigeonhole.Instance.LocalOrchestra;
            this.HoldNotesBox.IsChecked = BmpPigeonhole.Instance.HoldNotes;
            this.ForcePlaybackBox.IsChecked = BmpPigeonhole.Instance.ForcePlayback;

            MIDI_Input_DeviceBox.Items.Clear();
            _ = MIDI_Input_DeviceBox.Items.Add((0, "None"));
            foreach (var input in Maestro.Utils.MidiInput.ReloadMidiInputDevices())
                _ = MIDI_Input_DeviceBox.Items.Add(input);

            this.MIDI_Input_DeviceBox.SelectedIndex = BmpPigeonhole.Instance.MidiInputDev+1;
            ClassicSkin.IsChecked = BmpPigeonhole.Instance.ClassicUi;
        }

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

        private void Autostart_source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int d = Autostart_source.SelectedIndex;
            Globals.Settings.AutostartType = (Globals.Settings.Autostart_Types)d;
        }
        private void MIDI_Input_Device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int data = MIDI_Input_DeviceBox.SelectedIndex;
            data--;
            BmpMaestro.Instance.OpenInputDevice(data);
            BmpPigeonhole.Instance.MidiInputDev = data;
        }

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

        private void ClassicSkin_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.ClassicUi = ClassicSkin.IsChecked ?? true;
        }
    }
}
