using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BardMusicPlayer.Maestro;
using BardMusicPlayer.Transmogrify.Song;
using BardMusicPlayer.Transmogrify.Song.Config;
using BardMusicPlayer.Ui;
using Microsoft.Win32;

namespace BardMusicPlayer.Ui.Functions
{
    public static class PlaybackFunctions
    {
        public enum PlaybackState_Enum
        {
            PLAYBACK_STATE_STOPPED = 0,
            PLAYBACK_STATE_PLAYING,
            PLAYBACK_STATE_PAUSE
        };

        public static PlaybackState_Enum PlaybackState;
        public static BmpSong CurrentSong { get; set; }
        public static string InstrumentName;


        public static bool LoadSong()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "MIDI file|*.mid;*.midi|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() != true)
                return false;

            PlaybackState = PlaybackState_Enum.PLAYBACK_STATE_STOPPED;
            
            CurrentSong = BmpSong.OpenMidiFile(openFileDialog.FileName).Result;
            BmpMaestro.Instance.SetSong(openFileDialog.FileName);
            BmpMaestro.Instance.SetTracknumber(Globals.Globals.CurrentTrack);
            SetInstrumentName();
            return true;
        }

        public static void LoadSongFromPlaylist(BmpSong item)
        {
            PlaybackState = PlaybackState_Enum.PLAYBACK_STATE_STOPPED;
            CurrentSong = item;
            //BmpMaestro.Instance.DestroySongFromLocalPerformer();
            BmpMaestro.Instance.SetSong(CurrentSong);
            BmpMaestro.Instance.SetTracknumber(Globals.Globals.CurrentTrack);
            SetInstrumentName();
        }

        public static void PlaySong()
        {
            PlaybackState = PlaybackState_Enum.PLAYBACK_STATE_PLAYING;
            BmpMaestro.Instance.StartLocalPerformer();
        }

        public static void PauseSong()
        {
            PlaybackState = PlaybackState_Enum.PLAYBACK_STATE_PAUSE;
            BmpMaestro.Instance.PauseLocalPerformer();
        }

        public static void StopSong()
        {
            PlaybackState = PlaybackState_Enum.PLAYBACK_STATE_STOPPED;
            BmpMaestro.Instance.StopLocalPerformer();
        }

        public static string GetSongName()
        {
            if (CurrentSong == null)
                return "please load a song";
            return CurrentSong.Title;
        }

        public static void SetTrackNumber(int track)
        {
            Globals.Globals.CurrentTrack = track;
            BmpMaestro.Instance.SetTracknumberOnHost(Globals.Globals.CurrentTrack);
            SetInstrumentName();
        }

        public static string GetInstrumentName(int tracknumber)
        {
            if (tracknumber == 0)
                return "None";
            else
            {
                if (CurrentSong == null)
                    return "None";
                if (CurrentSong.TrackContainers.Count <= tracknumber)
                    return "None";
                try
                {
                    ClassicProcessorConfig classicConfig = (ClassicProcessorConfig)CurrentSong.TrackContainers[tracknumber].ConfigContainers[0].ProcessorConfig;
                    return classicConfig.Instrument.Name;
                }
                catch (KeyNotFoundException)
                {
                    return "Unknown";
                }
            }
        }

        public static void SetInstrumentName()
        {
            if (Globals.Globals.CurrentTrack == 0)
                InstrumentName = "None";
            else
            {
                if (CurrentSong == null)
                    return;
                if (CurrentSong.TrackContainers.Count <= Globals.Globals.CurrentTrack -1)
                    return;
                try
                {
                    ClassicProcessorConfig classicConfig = (ClassicProcessorConfig)CurrentSong.TrackContainers[Globals.Globals.CurrentTrack - 1].ConfigContainers[0].ProcessorConfig;
                    InstrumentName = classicConfig.Instrument.Name;
                }
                catch(System.Collections.Generic.KeyNotFoundException)
                {
                    InstrumentName = "Unknown";
                }
            }
        }
    }
}
