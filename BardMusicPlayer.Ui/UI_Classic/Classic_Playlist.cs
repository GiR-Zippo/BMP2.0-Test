using BardMusicPlayer.Coffer;
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

namespace BardMusicPlayer.Ui.Views
{
    /// <summary>
    /// Interaktionslogik für Classic_MainView.xaml
    /// </summary>
    public partial class Classic_MainView : UserControl
    {

        private bool ShowingPlaylists = false;

        private void Playlist_New_Button_Click(object sender, RoutedEventArgs e)
        {
            var inputbox = new TextInputWindow("Playlistname");
            if (inputbox.ShowDialog() == true)
            {
                PlaylistFunctions.CreatePlaylist(inputbox.ResponseText);
                PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems();
                ShowingPlaylists = false;
            }
        }

        private void Playlist_Load_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowingPlaylists = true;
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylists();
        }

        private void Playlist_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            PlaylistFunctions.SaveCurrentPlaylist();
        }

        private void Playlist_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            PlaylistFunctions.AddSongToCurrentPlaylist();
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems();
        }

        private void Playlist_Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            PlaylistFunctions.RemoveSongFromCurrentPlaylist();
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems();
        }

        private void Playlist_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowingPlaylists = true;
            PlaylistFunctions.DeleteCurrentPlaylist();
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylists();
        }

        private void PlaylistContainer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ShowingPlaylists)
            {
                PlaylistFunctions.SetCurrentPlaylist((string)PlaylistContainer.SelectedItem);
                ShowingPlaylists = false;
                PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems();
                return;
            }
            PlaylistFunctions.SetCurrentCurrentSong((string)PlaylistContainer.SelectedItem);
            this.SongName.Text = PlaybackFunctions.GetSongName();
            this.InstrumentInfo.Content = PlaybackFunctions.InstrumentName;
            return;
        }
    }
}
