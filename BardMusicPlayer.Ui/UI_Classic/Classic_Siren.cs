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
#if SIREN
            _ = BmpSiren.Instance.Load(PlaybackFunctions.CurrentSong);
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
