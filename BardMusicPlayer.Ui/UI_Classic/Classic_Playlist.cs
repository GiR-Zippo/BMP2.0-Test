using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddSong(string name)
        {
            playlist_entries.Add(new playlist_entry() { Id = 1, Name = "John Doe", Track = "1" });
            PlaylistContainer.ItemsSource = playlist_entries;
        }

    }
}
