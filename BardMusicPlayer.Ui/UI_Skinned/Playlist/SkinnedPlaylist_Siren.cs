using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BardMusicPlayer.UI.Functions;
using System.Windows.Media;
using System.Drawing;
using BardMusicPlayer.Ui.Globals.SkinContainer;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

#if SIREN
using BardMusicPlayer.Siren;
#endif

namespace BardMusicPlayer.Ui.Skinned
{
    /// <summary>
    /// Interaktionslogik für Skinned_PlaylistView.xaml
    /// </summary>
    public partial class Skinned_PlaylistView : Window
    {
        int scrollpos = 0;
        double lasttime = 0;
        private void WriteSongTitle(string data)
        {
            Bitmap bitmap = new Bitmap(305, 12);
            var graphics = Graphics.FromImage(bitmap);
            for (int i = 0; i < 20; i++)
            {
                Image img;
                char a =' ';
                if (i + scrollpos >= data.Length)
                {
                    if (i + scrollpos >= data.Length + 10)
                    {
                        scrollpos = 0;
                        break;
                    }
                }
                else
                    a = data.ToArray()[i+scrollpos];

                if (SkinContainer.FONT.ContainsKey(a))
                    img = SkinContainer.FONT[a];
                else
                    img = SkinContainer.FONT[32];
                graphics.DrawImage(img, 5 * i, 0);
            }
            scrollpos++;
            SongDigit.Source = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())).ImageSource;
            SongDigit.Stretch = Stretch.UniformToFill;
        }

        private void WriteSongTime(double data)
        {
            Bitmap bitmap = new Bitmap(305, 12);
            var graphics = Graphics.FromImage(bitmap);

            TimeSpan t = TimeSpan.FromMilliseconds(data);

            string Seconds = t.Seconds.ToString();
            string Minutes = t.Minutes.ToString();
            Image img;
            img = SkinContainer.FONT[(Minutes.Length == 1) ? '0' : Minutes.ToArray()[0]];
            graphics.DrawImage(img, 5 * 0, 0);
            img = SkinContainer.FONT[(Minutes.Length == 1) ? Minutes.ToArray()[0] : Minutes.ToArray()[1]];
            graphics.DrawImage(img, 5 * 1, 0);
            img = SkinContainer.FONT[32];
            graphics.DrawImage(img, 5 * 2, 0);
            img = SkinContainer.FONT[(Seconds.Length == 1) ? '0' : Seconds.ToArray()[0]];
            graphics.DrawImage(img, 5 * 3, 0);
            img = SkinContainer.FONT[(Seconds.Length == 1) ? Seconds.ToArray()[0] : Seconds.ToArray()[1]];
            graphics.DrawImage(img, 5 * 4, 0);


            SongTime.Source = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())).ImageSource;
            SongTime.Stretch = Stretch.UniformToFill;
        }

        private void Instance_SynthTimePositionChanged(string songTitle, double currentTime, double endTime, int activeVoices)
        {
            if (lasttime +500 < currentTime)
            {
                this.Dispatcher.BeginInvoke(new Action(() => this.WriteSongTitle(songTitle)));
                lasttime = currentTime;

                this.Dispatcher.BeginInvoke(new Action(() => this.WriteSongTime(currentTime)));
                
            }
        }

        private void Playbutton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            if (BmpSiren.Instance.IsReadyForPlayback)
                _ = BmpSiren.Instance.Play();
#endif
        }

        private void PauseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            _ = BmpSiren.Instance.Pause();
#endif
        }

        private void StopButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            _ = BmpSiren.Instance.Stop();
#endif
        }

        private void LoadButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            var song = PlaylistFunctions.GetSong(PlaylistContainer.SelectedItem as string);
            if (song == null)
                return;
            scrollpos = 0;
            lasttime = 0;
            this.WriteSongTitle(song.Title);
            _ = BmpSiren.Instance.Load(song);
#endif
        }
    }
}
