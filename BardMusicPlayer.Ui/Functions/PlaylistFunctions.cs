using BardMusicPlayer.Coffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BardMusicPlayer.UI.Functions
{
    public static class PlaylistFunctions
    {
        public static IPlaylist currentPlaylist;


        public static void CreatePlaylist(string playlistname)
        {
            currentPlaylist = BmpCoffer.Instance.CreatePlaylist(playlistname);
        }

        
    }
}
