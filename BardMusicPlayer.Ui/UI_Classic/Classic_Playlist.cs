using BardMusicPlayer.Coffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BardMusicPlayer.Ui.Views
{
    /// <summary>
    /// Interaktionslogik für Classic_MainView.xaml
    /// </summary>
    public partial class Classic_MainView : UserControl
    {

        public class playlist_entry
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Track { get; set; }
        }
        List<playlist_entry> playlist_entries = new List<playlist_entry>();

        public void Initplaylists()
        {
            string nn = "default";
            var pl = BmpCoffer.Instance.CreatePlaylist(nn);
            BmpCoffer.Instance.SavePlaylist(pl);
            var playlist = BmpCoffer.Instance.GetPlaylistNames();
            if (playlist == null)
                Console.WriteLine("");
            //PlaylistContainer.ItemsSource = Coffer.BmpCoffer.Instance.GetPlaylist("unkown");
        }
        public void AddSong(string name)
        {

        }

        private void Playlist_New_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Playlist_Load_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Playlist_Save_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Playlist_Add_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Playlist_Remove_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Playlist_Delete_Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
