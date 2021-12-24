using BardMusicPlayer.Coffer;
using BardMusicPlayer.Transmogrify.Song;
using BardMusicPlayer.Ui.Functions;
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
    /// <summary>
    /// Interaktionslogik für Classic_MainView.xaml
    /// </summary>
    public partial class Classic_MainView : UserControl
    {

        private bool ShowingPlaylists = false;
        IPlaylist _currentPlaylist;

        /// <summary>
        /// Create a new playlist but don't save it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playlist_New_Button_Click(object sender, RoutedEventArgs e)
        {
            var inputbox = new TextInputWindow("Playlistname");
            if (inputbox.ShowDialog() == true)
            {
                _currentPlaylist = PlaylistFunctions.CreatePlaylist(inputbox.ResponseText);
                PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems(_currentPlaylist);
                ShowingPlaylists = false;
            }
        }

        /// <summary>
        /// Load a playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playlist_Load_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowingPlaylists = true;
            PlaylistContainer.ItemsSource = BmpCoffer.Instance.GetPlaylistNames();
        }

        /// <summary>
        /// Save a playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playlist_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            BmpCoffer.Instance.SavePlaylist(_currentPlaylist);
        }


        private void Playlist_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPlaylist == null)
                return;

            if (PlaybackFunctions.CurrentSong == null)
                return;

            BmpCoffer.Instance.SaveSong(PlaybackFunctions.CurrentSong);
            _currentPlaylist.Add(PlaybackFunctions.CurrentSong);
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems(_currentPlaylist);
        }

        /// <summary>
        /// remove a song from the playlist but don't save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playlist_Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            string name = PlaylistContainer.SelectedItem as string;
            foreach (BmpSong s in _currentPlaylist)
            {
                if (s.Title == name)
                {
                    _currentPlaylist.Remove(s);
                    break;
                }
            }
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems(_currentPlaylist);
        }

        /// <summary>
        /// Delete a playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Playlist_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowingPlaylists = true;
            BmpCoffer.Instance.DeletePlaylist(_currentPlaylist);
            PlaylistContainer.ItemsSource = BmpCoffer.Instance.GetPlaylistNames();
        }

        private void PlaylistContainer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ShowingPlaylists)
            {
                _currentPlaylist = BmpCoffer.Instance.GetPlaylist((string)PlaylistContainer.SelectedItem);
                ShowingPlaylists = false;
                PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems(_currentPlaylist);
                return;
            }

            PlaybackFunctions.LoadSongFromPlaylist(PlaylistFunctions.GetSongFromPlaylist(_currentPlaylist, (string)PlaylistContainer.SelectedItem));
            this.SongName.Text = PlaybackFunctions.CurrentSong.Title;
            this.InstrumentInfo.Content = PlaybackFunctions.InstrumentName;
            return;
        }
    }
}
