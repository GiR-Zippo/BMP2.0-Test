using BardMusicPlayer.Transmogrify.Song;
using BardMusicPlayer.Ui.Functions;
using BardMusicPlayer.Ui.Globals.SkinContainer;
using BardMusicPlayer.UI.Functions;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BardMusicPlayer.Ui.Skinned
{
    /// <summary>
    /// Interaktionslogik für Skinned_PlaylistView.xaml
    /// </summary>
    public partial class Skinned_PlaylistView : Window
    {
        public EventHandler<bool> OnLoadSongFromPlaylist;

        public Skinned_PlaylistView()
        {
            InitializeComponent();
            ApplySkin();
#if SIREN
            Siren.BmpSiren.Instance.SynthTimePositionChanged += Instance_SynthTimePositionChanged;
#endif
            RefreshPlaylist();
        }

        public void ApplySkin()
        {
            var col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMALBG];
            this.Background = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));

            this.Playlist_Top_Left.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_LEFT_CORNER];
            this.PLAYLIST_TITLE_BAR.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_TITLE_BAR];
            this.PLAYLIST_TOP_TILE.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_TILE];
            this.PLAYLIST_TOP_TILE_II.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_TILE];
            this.PLAYLIST_TOP_RIGHT_CORNER.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_RIGHT_CORNER];

            this.PLAYLIST_LEFT_TILE.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_LEFT_TILE];
            this.PLAYLIST_RIGHT_TILE.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_RIGHT_TILE];

            this.PLAYLIST_BOTTOM_LEFT_CORNER.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_BOTTOM_LEFT_CORNER];
            this.PLAYLIST_BOTTOM_TILE.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_BOTTOM_TILE];
            this.PLAYLIST_BOTTOM_RIGHT_CORNER.Fill = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_BOTTOM_RIGHT_CORNER];

            this.Close_Button.Background = SkinContainer.PLAYLIST[SkinContainer.PLAYLIST_TYPES.PLAYLIST_CLOSE_SELECTED];
            this.Close_Button.Background.Opacity = 0;

            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMALBG];
            this.PlaylistContainer.Background = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMAL];
            this.PlaylistContainer.Foreground = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));

            PlaylistContainer_SelectionChanged(null, null);
        }

        private void RefreshPlaylist()
        {
            PlaylistContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylistItems();
        }

        private void PlaylistContainer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PlaybackFunctions.StopSong();
            PlaylistFunctions.SetCurrentCurrentSong((string)PlaylistContainer.SelectedItem);
            OnLoadSongFromPlaylist?.Invoke(this, true);
        }

        private void PlaylistContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentsongIndex = PlaylistContainer.SelectedIndex;
            var col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMAL];
            var fcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMALBG];
            var bcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            for (int i = 0; i < PlaylistContainer.Items.Count; i++)
            {
                ListViewItem lvitem = PlaylistContainer.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                if (lvitem == null)
                    continue;
                lvitem.Foreground = fcol;
                lvitem.Background = bcol;
            }
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_CURRENT];
            fcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_SELECTBG];
            bcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));

            var lvtem = PlaylistContainer.ItemContainerGenerator.ContainerFromItem(PlaylistContainer.SelectedItem) as ListViewItem;
            if (lvtem == null)
                return;
            lvtem.Foreground = fcol;
            lvtem.Background = bcol;
        }

        private void AddFiles_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "MIDI file|*.mid;*.midi|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() != true)
                return;
            foreach (var d in openFileDialog.FileNames)
            {
                BmpSong song = BmpSong.OpenMidiFile(d).Result;
                PlaylistFunctions.AddSongToCurrentPlaylist(song);
                PlaylistFunctions.SaveCurrentPlaylist();
            }
            RefreshPlaylist();
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in PlaylistContainer.SelectedItems)
            {
                PlaylistFunctions.RemoveSongFromCurrentPlaylist(s as string);
                PlaylistFunctions.SaveCurrentPlaylist();
            }
            RefreshPlaylist();
        }

        private void ClearPlaylist_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in PlaylistContainer.Items)
            {
                PlaylistFunctions.RemoveSongFromCurrentPlaylist(s as string);
                PlaylistFunctions.SaveCurrentPlaylist();
            }
            RefreshPlaylist();
        }

        private void MenuButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Rectangle rectangle = sender as Rectangle;
                ContextMenu contextMenu = rectangle.ContextMenu;
                contextMenu.PlacementTarget = rectangle;
                contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
                contextMenu.IsOpen = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MediaBrowser mb = new MediaBrowser();
            mb.Show();
            mb.OnPlaylistChanged += OnPlaylistChanged;
        }

        private void OnPlaylistChanged(object sender, string e)
        {
            PlaylistFunctions.SetCurrentPlaylist(e);
            RefreshPlaylist();
        }

        #region Titlebar functions and buttons
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        private void Close_Button_Down(object sender, MouseButtonEventArgs e)
        {
            this.Close_Button.Background.Opacity = 1;
        }
        private void Close_Button_Up(object sender, MouseButtonEventArgs e)
        {
            this.Close_Button.Background.Opacity = 0;
        }
        #endregion

    }
}
