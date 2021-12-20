/*
 * Copyright(c) 2021 MoogleTroupe, trotlinebeercan
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Seer;
using BardMusicPlayer.Transmogrify.Song;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace BardMusicPlayer.Maestro
{
    public partial class BmpMaestro : IDisposable
    {
        private static readonly Lazy<BmpMaestro> LazyInstance = new(() => new BmpMaestro());

        public IEnumerable<Game> Bards { get; private set; }
        public Game SelectedBard { get; set; }
        private int _NoteKeyDelay;

        private Orchestrator _orchestrator;
        /// <summary>
        /// 
        /// </summary>
        public bool Started { get; private set; }

        private BmpMaestro()
        {
            Bards = BmpSeer.Instance.Games.Values;
            BmpSeer.Instance.GameStarted += e => EnsureGameExists(e.Game);
            _orchestrator = new Orchestrator();
        }

        private void EnsureGameExists(Game game)
        {
            if (!Bards.Contains(game))
            {
                Bards.Append(game);
                SelectedBard ??= game;
            }
        }

        public static BmpMaestro Instance => LazyInstance.Value;

        public IEnumerable<Game> GetPerformer()
        {
            return _orchestrator.GetPerformer();
        }

        /// <summary>
        /// Sets a new song for the sequencer
        /// </summary>
        /// <param name="bmpSong"></param>
        /// <param name="track">the tracknumber which should be played; 0 all tracks</param>
        /// <returns></returns>
        public void PlayWithLocalPerformer(BmpSong bmpSong, int track)
        {
            _orchestrator.Stop();
            _orchestrator.LoadBMPSong(bmpSong);
        }

        public void PlayWithLocalPerformer(string filename)
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
        public void ChangeTracknumber(int track)
        {
            if (_orchestrator != null)
                _orchestrator.ChangeTracknumber(track);
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