using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BardMusicPlayer.Ui.Globals;
using BardMusicPlayer.Ui.Views;

namespace BardMusicPlayer.Ui.Settings
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
        }

        private void Autostart_source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int d = Autostart_source.SelectedIndex;
            Globals.Settings.AutostartType = (Globals.Settings.Autostart_Types)d;
            Globals.Settings.SaveConfig();
        }

    }
}
