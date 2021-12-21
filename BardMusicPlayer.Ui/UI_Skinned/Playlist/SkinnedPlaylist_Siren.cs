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
    /// logic for the siren controls
    /// </summary>
    public partial class Skinned_PlaylistView : Window
    {
        int scrollpos = 0;          //position of the title scroller
        double lasttime = 0;        //last poll time of Instance_SynthTimePositionChanged
        public int CurrentsongIndex { get; set; } = 0;   //index of the currentSong for siren
        /// <summary>
        /// Triggered from Siren
        /// </summary>
        /// <param name="songTitle"></param>
        /// <param name="currentTime"></param>
        /// <param name="endTime"></param>
        /// <param name="activeVoices"></param>
        private void Instance_SynthTimePositionChanged(string songTitle, double currentTime, double endTime, int activeVoices)
        {
            //Scrolling
            if (lasttime + 500 < currentTime)
            {
                this.Dispatcher.BeginInvoke(new Action(() => this.WriteSongTitle(songTitle)));
                this.Dispatcher.BeginInvoke(new Action(() => this.WriteSongTime(currentTime)));
                lasttime = currentTime;
            }
        }

        /// <summary>
        /// Writes the song title in the lower right corner
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// write the current playtime from siren in the lower right corner
        /// </summary>
        /// <param name="data">time in ms</param>
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

        /// <summary>
        /// selects the previous song and load it into siren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrevButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            if (CurrentsongIndex <= 0)
                return;

            CurrentsongIndex--;
            string t = PlaylistContainer.Items[CurrentsongIndex] as string;
            var song = PlaylistFunctions.GetSong(t);
            if (song == null)
                return;
            scrollpos = 0;
            lasttime = 0;
            this.WriteSongTitle(song.Title);
            _ = BmpSiren.Instance.Load(song);
#endif
        }

        /// <summary>
        /// plays the loaded siren song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playbutton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            if (BmpSiren.Instance.IsReadyForPlayback)
                _ = BmpSiren.Instance.Play();
#endif
        }

        /// <summary>
        /// pause the loaded siren song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            _ = BmpSiren.Instance.Pause();
#endif
        }

        /// <summary>
        /// stops the siren playback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            _ = BmpSiren.Instance.Stop();
#endif
        }

        /// <summary>
        /// load the selected song into siren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// load next song in siren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
#if SIREN
            if (CurrentsongIndex == PlaylistContainer.Items.Count)
                return;

            CurrentsongIndex++;
            string t = PlaylistContainer.Items[CurrentsongIndex] as string;
            var song = PlaylistFunctions.GetSong(t);
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
