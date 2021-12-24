/*
 * Copyright(c) 2021 MoogleTroupe, trotlinebeercan
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using System;
using System.Collections.Generic;
using BardMusicPlayer.Maestro.Performance;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Seer;
using BardMusicPlayer.Transmogrify.Song;

namespace BardMusicPlayer.Maestro
{
    public partial class BmpMaestro : IDisposable
    {
        private static readonly Lazy<BmpMaestro> LazyInstance = new(() => new BmpMaestro());

        public Game SelectedBard { get; set; }
        private int _NoteKeyDelay;

        private Orchestrator _orchestrator;
        /// <summary>
        /// 
        /// </summary>
        public bool Started { get; private set; }

        private BmpMaestro()
        {
            _orchestrator = new Orchestrator();
        }

        public static BmpMaestro Instance => LazyInstance.Value;

        /// <summary>
        /// Get all game the orchestrator is accessing
        /// </summary>
        public IEnumerable<Game> GetAllGames()
        {
            return _orchestrator.GetAllGames();
        }

        /// <summary>
        /// Get all performers the orchestrator has created
        /// </summary>
        public IEnumerable<Performer> GetAllPerformers()
        {
            return _orchestrator.GetAllPerformers();
        }

        /// <summary>
        /// Sets a new song for the sequencer
        /// </summary>
        /// <param name="bmpSong"></param>
        /// <param name="track">the tracknumber which should be played; 0 all tracks</param>
        /// <returns></returns>
        public void SetSong(BmpSong bmpSong, int track)
        {
            _orchestrator.Stop();
            _orchestrator.LoadBMPSong(bmpSong);
        }

        /// <summary>
        /// Sets the song for the sequencer
        /// </summary>
        /// <param name="filename">midi file with full path</param>
        public void SetSong(string filename)
        {
           _orchestrator.Stop();
           _orchestrator.LoadMidiFile(filename);
        }

        /// <summary>
        /// Starts the playback
        /// </summary>
        /// <returns></returns>
        public void StartLocalPerformer()
        {
            if (_orchestrator != null)
            {
                _NoteKeyDelay = BmpPigeonhole.Instance.NoteKeyDelay;
                BmpPigeonhole.Instance.NoteKeyDelay = 1;
                _orchestrator.Start();
            }
        }

        /// <summary>
        /// Pause the song playback
        /// </summary>
        /// <returns></returns>
        public void PauseLocalPerformer()
        {
            if (_orchestrator != null)
            {
                BmpPigeonhole.Instance.NoteKeyDelay = _NoteKeyDelay;
                _orchestrator.Pause();
            }
        }

        /// <summary>
        /// Stops the song playback
        /// </summary>
        /// <returns></returns>
        public void StopLocalPerformer()
        {
            if (_orchestrator != null)
            {
                BmpPigeonhole.Instance.NoteKeyDelay = _NoteKeyDelay;
                _orchestrator.Stop();
            }
        }

        /// <summary>
        /// Change the tracknumber (-1 all tracks)
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public void SetTracknumber(int track)
        {
            if (_orchestrator != null)
                _orchestrator.SetTracknumber(track);
        }

        /// <summary>
        /// Set the tracknumber (-1 all tracks)
        /// </summary>
        /// <param name="game">the bard</param>
        /// <param name="track">track</param>
        /// <returns></returns>
        public void SetTracknumber(Game game, int track)
        {
            if (_orchestrator != null)
                _orchestrator.SetTracknumber(game, track);
        }

        /// <summary>
        /// Sets the host bard
        /// </summary>
        /// <param name="game"></param>
        public void SetHostBard(Game game)
        {
            if (_orchestrator != null)
                _orchestrator.SetHostBard(game);
        }

        /// <summary>
        /// Sets the host bard
        /// </summary>
        /// <param name="performer"></param>
        public void SetHostBard(Performer performer)
        {
            if (_orchestrator != null)
                _orchestrator.SetHostBard(performer);
        }

        /// <summary>
        /// Destroys the sequencer
        /// </summary>
        /// <returns></returns>
        public void DestroySongFromLocalPerformer()
        {
            if (_orchestrator != null)
                _orchestrator.Dispose();
        }

        /// <summary>
        /// Start the eventhandler
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            if (Started) return;
            StartEventsHandler();
            Started = true;
        }

        /// <summary>
        /// Stop the eventhandler
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            if (!Started) return;
            StopEventsHandler();
            Started = false;
            Dispose();
        }

        /// <summary>
        /// Sets the playback at position (timeindex in ticks)
        /// </summary>
        /// <param name="timeindex"></param>
        /// <returns></returns>
        public void SetPlaybackStart(int ticks)
        {
            if (_orchestrator != null)
                _orchestrator.Seek(ticks);
        }

        /// <summary>
        /// Sets the playback at position (timeindex in miliseconds)
        /// </summary>
        /// <param double="miliseconds"></param>
        /// <returns></returns>
        public void SetPlaybackStart(double miliseconds)
        {
            if (_orchestrator != null)
                _orchestrator.Seek(miliseconds);
        }

        /// <summary>
        /// Opens a MidiInput device
        /// </summary>
        /// <param int="device"></param>
        /// <returns></returns>
        public void OpenInputDevice(int device)
        {
            if (_orchestrator == null)
                _orchestrator = new Orchestrator();
            _orchestrator.OpenInputDevice(device);
        }


        public void ForceAddPerformer()
        {
            if (_orchestrator == null)
                _orchestrator = new Orchestrator();
            _orchestrator.ForceAddPerformer();
        }


        ~BmpMaestro() { Dispose(); }

        public void Dispose()
        {
            Stop();
            _orchestrator.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}