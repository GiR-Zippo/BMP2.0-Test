using BardMusicPlayer.Coffer;
using BardMusicPlayer.Transmogrify.Song;
using BardMusicPlayer.Ui.Functions;
using System;
using System.Collections.Generic;

namespace BardMusicPlayer.UI.Functions
{
    public static class PlaylistFunctions
    {
        public static IPlaylist currentPlaylist;

        public static void CreatePlaylist(string playlistname)
        {
            currentPlaylist = BmpCoffer.Instance.CreatePlaylist(playlistname);
        }

        public static IList<string> GetCurrentPlaylists()
        {
            return BmpCoffer.Instance.GetPlaylistNames();
        }

        public static List<string> GetCurrentPlaylistItems()
        {
            List<string> data = new List<string>();
            if (currentPlaylist == null)
                return data;

            foreach (var item in currentPlaylist)
                data.Add(item.Title);
            return data;
        }

        public static void SetCurrentPlaylist(string playlistname)
        {
            currentPlaylist = BmpCoffer.Instance.GetPlaylist(playlistname);
        }

        public static void SetCurrentCurrentSong(string songname)
        {
            if (currentPlaylist == null)
                return;

            foreach (var item in currentPlaylist)
            {
                if (item.Title == songname)
                {
                    PlaybackFunctions.LoadSongFromPlaylist(item);
                    return;
                }
            }
        }

        public static BmpSong GetSong(string songname)
        {
            if (currentPlaylist == null)
                return null;

            foreach (var item in currentPlaylist)
            {
                if (item.Title == songname)
                    return item;
            }
            return null;
        }

        public static void SaveCurrentPlaylist()
        {
            if (currentPlaylist == null)
                return;

            BmpCoffer.Instance.SavePlaylist(currentPlaylist);
        }

        public static void AddSongToCurrentPlaylist()
        {
            if ((currentPlaylist == null) || (PlaybackFunctions.CurrentSong == null))
                return;

            BmpCoffer.Instance.SaveSong(PlaybackFunctions.CurrentSong);
            currentPlaylist.Add(PlaybackFunctions.CurrentSong);
        }

        public static void AddSongToCurrentPlaylist(BmpSong song)
        {
            if ((currentPlaylist == null) || (song == null))
                return;

            BmpCoffer.Instance.SaveSong(song);
            currentPlaylist.Add(song);
        }

        public static void RemoveSongFromCurrentPlaylist()
        {
            if ((currentPlaylist == null) || (PlaybackFunctions.CurrentSong == null))
                return;

            var index = 0;
            foreach (var item in currentPlaylist)
                if (item == PlaybackFunctions.CurrentSong)
                    break;
                else index++;

            currentPlaylist.Remove(index);
            //BmpCoffer.Instance.DeleteSong(PlaybackFunctions.CurrentSong);
        }

        public static void RemoveSongFromCurrentPlaylist(BmpSong song)
        {
            if ((currentPlaylist == null) || (song == null))
                return;

            var index = 0;
            foreach (var item in currentPlaylist)
                if (item == song)
                    break;
                else index++;

            currentPlaylist.Remove(index);
            //BmpCoffer.Instance.DeleteSong(PlaybackFunctions.CurrentSong);
        }

        public static void RemoveSongFromCurrentPlaylist(string song)
        {
            if ((currentPlaylist == null) || (song == null))
                return;

            var index = 0;
            foreach (var item in currentPlaylist)
                if (item.Title == song)
                    break;
                else index++;

            currentPlaylist.Remove(index);
            //BmpCoffer.Instance.DeleteSong(PlaybackFunctions.CurrentSong);
        }

        public static void DeleteCurrentPlaylist()
        {
            BmpCoffer.Instance.DeletePlaylist(currentPlaylist);
        }

    }
}
