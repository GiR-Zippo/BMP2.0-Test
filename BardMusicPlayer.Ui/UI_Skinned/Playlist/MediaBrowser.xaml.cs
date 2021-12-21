using BardMusicPlayer.Ui.Globals.SkinContainer;
using BardMusicPlayer.UI.Functions;
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
using UI.Resources;

namespace BardMusicPlayer.Ui.Skinned
{
    /// <summary>
    /// Interaktionslogik für MediaBrowser.xaml
    /// </summary>
    public partial class MediaBrowser : Window
    {
        public EventHandler<string> OnPlaylistChanged;
        public MediaBrowser()
        {
            InitializeComponent();
            ApplySkin();
            PlaylistsContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylists();
        }

        public void ApplySkin()
        {
            //top
            this.MEDIABROWSER_TOP_LEFT.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_TOP_LEFT];
            this.MEDIABROWSER_TOP_TILE.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_TOP_TILE];
            this.MEDIABROWSER_TOP_TITLE.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_TOP_TITLE];
            this.MEDIABROWSER_TOP_TILE_II.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_TOP_TILE];
            this.MEDIABROWSER_TOP_RIGHT.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_TOP_RIGHT];
            //mid
            this.MEDIABROWSER_LEFT_TILE.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_MID_LEFT];
            this.MEDIABROWSER_RIGHT_TILE.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_MID_RIGHT];
            //bottom
            this.MEDIABROWSER_BOTTOM_LEFT_CORNER.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_BOTTOM_LEFT];
            this.MEDIABROWSER_BOTTOM_TILE.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_BOTTOM_TILE];
            this.MEDIABROWSER_BOTTOM_RIGHT_CORNER.Fill = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_BOTTOM_RIGHT];
            this.Close_Button.Background = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_CLOSE];
            this.Close_Button.Background.Opacity = 0;

            this.Prev_Button.Background = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_PREV];
            this.Prev_Button.Background.Opacity = 0;
            this.Next_Button.Background = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_NEXT];
            this.Next_Button.Background.Opacity = 0;
            this.New_Button.Background = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_NEW];
            this.New_Button.Background.Opacity = 0;
            this.Reload_Button.Background = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_RELOAD];
            this.Reload_Button.Background.Opacity = 0;
            this.Remove_Button.Background = SkinContainer.MEDIABROWSER[SkinContainer.MEDIABROWSER_TYPES.MEDIABROWSER_REMOVE];
            this.Remove_Button.Background.Opacity = 0;

            var col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMALBG];
            this.Background = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMALBG];
            this.PlaylistsContainer.Background = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMAL];
            this.PlaylistsContainer.Foreground = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
        }

        private void PlaylistsContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMAL];
            var fcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_NORMALBG];
            var bcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            for (int i = 0; i < PlaylistsContainer.Items.Count; i++)
            {
                ListViewItem lvitem = PlaylistsContainer.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                if (lvitem == null)
                    continue;
                lvitem.Foreground = fcol;
                lvitem.Background = bcol;
            }
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_CURRENT];
            fcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));
            col = SkinContainer.PLAYLISTCOLOR[SkinContainer.PLAYLISTCOLOR_TYPES.PLAYLISTCOLOR_SELECTBG];
            bcol = new SolidColorBrush(Color.FromArgb(col.A, col.R, col.G, col.B));

            var lvtem = PlaylistsContainer.ItemContainerGenerator.ContainerFromItem(PlaylistsContainer.SelectedItem) as ListViewItem;
            if (lvtem == null)
                return;
            lvtem.Foreground = fcol;
            lvtem.Background = bcol;
        }

        private void PlaylistsContainer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnPlaylistChanged.Invoke(this, (string)PlaylistsContainer.SelectedItem);
        }


        #region Titlebar functions and buttons
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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


        #region minibar functions and buttons
        private void Prev_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Prev_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Prev_Button.Background.Opacity = 1; }
        private void Prev_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Prev_Button.Background.Opacity = 0; }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Next_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Next_Button.Background.Opacity = 1; }
        private void Next_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Next_Button.Background.Opacity = 0; }

        private void New_Button_Click(object sender, RoutedEventArgs e)
        {
            var inputbox = new TextInputWindow("Playlistname");
            if (inputbox.ShowDialog() == true)
            {
                PlaylistFunctions.CreatePlaylist(inputbox.ResponseText);
                PlaylistFunctions.SaveCurrentPlaylist();
                PlaylistsContainer.ItemsSource = PlaylistFunctions.GetCurrentPlaylists();
            }
        }
        private void New_Button_Down(object sender, MouseButtonEventArgs e)
        { this.New_Button.Background.Opacity = 1; }
        private void New_Button_Up(object sender, MouseButtonEventArgs e)
        { this.New_Button.Background.Opacity = 0; }

        private void Reload_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Reload_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Reload_Button.Background.Opacity = 1; }
        private void Reload_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Reload_Button.Background.Opacity = 0; }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Remove_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Remove_Button.Background.Opacity = 1; }
        private void Remove_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Remove_Button.Background.Opacity = 0; }
        #endregion
    }
}
