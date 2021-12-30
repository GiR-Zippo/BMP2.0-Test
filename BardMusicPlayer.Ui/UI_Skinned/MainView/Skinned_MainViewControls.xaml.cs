using BardMusicPlayer.Ui.Globals.SkinContainer;
using System.Windows;
using System.Windows.Input;
using BardMusicPlayer.Ui.Functions;
using System.Threading;
using BardMusicPlayer.Maestro;

namespace BardMusicPlayer.Ui.Skinned
{
    public partial class Skinned_MainView : System.Windows.Controls.UserControl
    {
        /// <summary>
        ///     load the prev song in the playlist
        /// </summary>
        private void Prev_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Prev_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PREVIOUS_BUTTON];
            _PlaylistView.PlayPrevSong();
        }
        private void Prev_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Prev_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PREVIOUS_BUTTON_ACTIVE]; }
        private void Prev_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Prev_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PREVIOUS_BUTTON]; }

        /// <summary>
        ///     play a loaded song
        /// </summary>
        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Play_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PLAY_BUTTON];
            PlaybackFunctions.PlaySong();
        }
        private void Play_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Play_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PLAY_BUTTON_ACTIVE];}
        private void Play_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Play_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PLAY_BUTTON];}

        /// <summary>
        ///     pause the song playback
        /// </summary>
        private void Pause_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Pause_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PAUSE_BUTTON];
            PlaybackFunctions.PauseSong();
        }
        private void Pause_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Pause_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PAUSE_BUTTON_ACTIVE]; }
        private void Pause_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Pause_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PAUSE_BUTTON]; }

        /// <summary>
        ///     stop song playback
        /// </summary>
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Stop_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_STOP_BUTTON];
            PlaybackFunctions.StopSong();
        }
        private void Stop_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Stop_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_STOP_BUTTON_ACTIVE]; }
        private void Stop_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Stop_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_STOP_BUTTON]; }

        /// <summary>
        ///     Plays the next song in the playlist
        /// </summary>
        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Next_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_NEXT_BUTTON];
            _PlaylistView.PlayNextSong();
        }
        private void Next_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Next_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_NEXT_BUTTON_ACTIVE]; }
        private void Next_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Next_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_NEXT_BUTTON]; }

        /// <summary>
        ///     opens a song for single playback
        /// </summary>
        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Load_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_EJECT_BUTTON];
            if (PlaybackFunctions.LoadSong())
            {
                Scroller.Cancel();
                Scroller = new CancellationTokenSource();
                UpdateScroller(Scroller.Token, PlaybackFunctions.GetSongName()).ConfigureAwait(false);
                WriteInstrumentDigitField(PlaybackFunctions.GetInstrumentNameForHostPlayer());
            }
        }
        private void Load_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Load_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_EJECT_BUTTON_ACTIVE]; }
        private void Load_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Load_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_EJECT_BUTTON]; }


        /// <summary>
        ///     switch a track down
        /// </summary>
        private void TrackDown_Button_Click(object sender, RoutedEventArgs e)
        {
            int track = BmpMaestro.Instance.GetHostBardTrack();
            this.TrackDown_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED];
            if (track <= 0)
                return;
            BmpMaestro.Instance.SetTracknumberOnHost(track -1);
        }
        private void TrackDown_Button_Down(object sender, MouseButtonEventArgs e)
        { this.TrackDown_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_PRESSED]; }
        private void TrackDown_Button_Up(object sender, MouseButtonEventArgs e)
        { this.TrackDown_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED]; }


        /// <summary>
        ///     switch a track Up
        /// </summary>
        private void TrackUp_Button_Click(object sender, RoutedEventArgs e)
        {
            int track = BmpMaestro.Instance.GetHostBardTrack();
            this.TrackUp_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_UNPRESSED];
            if (track == MaxTracks)
                return;
            BmpMaestro.Instance.SetTracknumberOnHost(track +1);
        }
        private void TrackUp_Button_Down(object sender, MouseButtonEventArgs e)
        { this.TrackUp_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_PRESSED]; }
        private void TrackUp_Button_Up(object sender, MouseButtonEventArgs e)
        { this.TrackUp_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_UNPRESSED]; }

        /// <summary>
        ///     open the settings
        /// </summary>
        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Settings_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_OPTIONS_BUTTON];
            SettingsView _settings = new SettingsView();
            _settings.Show();
        }
        private void Settings_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Settings_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_OPTIONS_BUTTON_DEPRESSED]; }
        private void Settings_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Settings_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_OPTIONS_BUTTON]; }

        /// <summary>
        ///     close the player
        /// </summary>
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_CLOSE_BUTTON];
            Scroller.Cancel();
            Application.Current.Shutdown();
        }
        private void Close_Button_Down(object sender, MouseButtonEventArgs e)
        { this.Close_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_CLOSE_BUTTON_DEPRESSED]; }
        private void Close_Button_Up(object sender, MouseButtonEventArgs e)
        { this.Close_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_CLOSE_BUTTON]; }

        /// <summary>
        ///     Show/Hide Playlist
        /// </summary>
        private void Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_PlaylistView.Visibility == Visibility.Visible)
            {
                _PlaylistView.Visibility = Visibility.Hidden;
                this.Playlist_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_PLAYLIST_BUTTON];
            }
            else
            {
                _PlaylistView.Visibility = Visibility.Visible;
                this.Playlist_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_PLAYLIST_BUTTON_SELECTED];
            }
        }
        private void Playlist_Button_Down(object sender, MouseButtonEventArgs e)
        {
           if (_PlaylistView.Visibility == Visibility.Visible)
                this.Playlist_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_PLAYLIST_BUTTON_DEPRESSED];
            else
                this.Playlist_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_PLAYLIST_BUTTON_DEPRESSED_SELECTED];
        }
        private void Playlist_Button_Up(object sender, MouseButtonEventArgs e)
        {
            if (_PlaylistView.Visibility == Visibility.Visible)
                this.Playlist_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_PLAYLIST_BUTTON];
            else
                this.Playlist_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_PLAYLIST_BUTTON_SELECTED];
        }


        private void Random_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_PlaylistView.NormalPlay)
            {
                _PlaylistView.NormalPlay = false;
                this.Random_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_SHUFFLE_BUTTON_SELECTED];
            }
            else
            {
                _PlaylistView.NormalPlay = true;
                this.Random_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_SHUFFLE_BUTTON];
            }
        }
        private void Random_Button_Down(object sender, MouseButtonEventArgs e)
        {
            if (_PlaylistView.NormalPlay)
                this.Random_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_SHUFFLE_BUTTON_DEPRESSED];
            else
                this.Random_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_SHUFFLE_BUTTON_SELECTED_DEPRESSED];
        }
        private void Random_Button_Up(object sender, MouseButtonEventArgs e)
        {
            if (_PlaylistView.NormalPlay)
                this.Random_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_SHUFFLE_BUTTON_SELECTED];
            else
                this.Random_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_SHUFFLE_BUTTON];
        }

        private void Loop_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_PlaylistView.LoopPlay)
            {
                _PlaylistView.LoopPlay = false;
                this.Loop_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_REPEAT_BUTTON];
            }
            else
            {
                _PlaylistView.LoopPlay = true;
                this.Loop_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_REPEAT_BUTTON_SELECTED];
            }
        }
        private void Loop_Button_Down(object sender, MouseButtonEventArgs e)
        {
            if (_PlaylistView.LoopPlay)
                this.Loop_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_REPEAT_BUTTON_DEPRESSED];
            else
                this.Loop_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_REPEAT_BUTTON_SELECTED_DEPRESSED];
        }
        private void Loop_Button_Up(object sender, MouseButtonEventArgs e)
        {
            if (_PlaylistView.LoopPlay)
                this.Loop_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_REPEAT_BUTTON];
            else
                this.Loop_Button.Background = SkinContainer.SHUFREP[SkinContainer.SHUFREP_TYPES.MAIN_REPEAT_BUTTON_SELECTED];
        }
    }
}
