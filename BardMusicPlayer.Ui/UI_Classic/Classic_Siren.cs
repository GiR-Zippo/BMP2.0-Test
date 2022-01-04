using BardMusicPlayer.Siren;
using BardMusicPlayer.Ui.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BardMusicPlayer.Ui.Classic
{
    /// <summary>
    /// Interaktionslogik für Classic_MainView.xaml
    /// </summary>
    public partial class Classic_MainView : UserControl
    {
        private void Siren_Load_Click(object sender, RoutedEventArgs e)
        {
            _ = BmpSiren.Instance.Load(PlaybackFunctions.CurrentSong);
        }

        private void Siren_Play_Click(object sender, RoutedEventArgs e)
        {
            BmpSiren.Instance.Play();
        }

        private void Siren_Stop_Click(object sender, RoutedEventArgs e)
        {
            BmpSiren.Instance.Stop();
        }

        private void Siren_Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /*Slider slider = e.OriginalSource as Slider;
            BmpSiren.Instance.Setup((float)slider.Value, 2, 100);*/
        }
    }
}
