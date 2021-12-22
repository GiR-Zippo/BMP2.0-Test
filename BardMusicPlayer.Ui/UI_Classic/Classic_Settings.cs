using BardMusicPlayer.Coffer;
using BardMusicPlayer.Maestro;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Ui.Functions;
using BardMusicPlayer.UI.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Resources;

namespace BardMusicPlayer.Ui.Classic
{
    public partial class Classic_MainView : UserControl
    {
        private void LoadConfig()
        {
            this.Autostart_source.SelectedIndex = BmpPigeonhole.Instance.AutostartMethod;

            this.HoldNotesBox.IsChecked = BmpPigeonhole.Instance.HoldNotes;
            this.ForcePlaybackBox.IsChecked = BmpPigeonhole.Instance.ForcePlayback;

            //TODO: Remove
            MIDI_Input_DeviceBox.Items.Clear();
            foreach (var input in Maestro.Utils.MidiInput.ReloadMidiInputDevices())
                MIDI_Input_DeviceBox.Items.Add(input);

            this.MIDI_Input_DeviceBox.SelectedIndex = BmpPigeonhole.Instance.MidiInputDev;
            this.SkinUiBox.IsChecked = !BmpPigeonhole.Instance.ClassicUi;
            Globals.Globals.CurrentTrack = 1;
        }

        private void Autostart_source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int d = Autostart_source.SelectedIndex;
            Globals.Settings.AutostartType = (Globals.Settings.Autostart_Types)d;
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
            int data = MIDI_Input_DeviceBox.SelectedIndex;
            BmpMaestro.Instance.OpenInputDevice(data);
            BmpPigeonhole.Instance.MidiInputDev = data;
        }

        private void SkinUiBox_Checked(object sender, RoutedEventArgs e)
        {
            BmpPigeonhole.Instance.ClassicUi = !(SkinUiBox.IsChecked ?? true);
        }
    }
}
