using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BardMusicPlayer.Grunt;
using BardMusicPlayer.Maestro.Events;
using BardMusicPlayer.Maestro.Performance;
using BardMusicPlayer.Maestro.Sequencing;
using BardMusicPlayer.Quotidian.Enums;
using BardMusicPlayer.Quotidian.Structs;
using BardMusicPlayer.Seer;
using BardMusicPlayer.Transmogrify.Song;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace BardMusicPlayer.Maestro
{
    public class Orchestrator : IDisposable
    {
        private Sequencer sequencer { get; set; } = null;
        List<KeyValuePair<int, Performer>> performer { get; set; } = null;

        public IEnumerable<Game> GetPerformer()
        {
            List<Game> games =  new List<Game>();
            foreach (KeyValuePair<int, Performer> performer in performer)
                games.Add(performer.Value.game);
            return games;
        }

        private CancellationTokenSource _updaterTokenSource;

        public Orchestrator()
        {
            performer = new List<KeyValuePair<int, Performer>>();
            sequencer = new Sequencer();
            BmpSeer.Instance.GameStarted += e => EnsureGameExists(e.Game);
            BmpSeer.Instance.GameStopped += OnInstanceOnGameStopped;
        }

        private void EnsureGameExists(Game game)
        {
            if (BmpSeer.Instance.Games.Count == 1)
                _ = AddPerformer(game, true);
            else
                _ = AddPerformer(game, false);
        }

        private void OnInstanceOnGameStopped(Seer.Events.GameStopped g)
        {
            RemovePerformer(g.Pid);
        }

        public void LoadMidiFile(string midifilename)
        {
            sequencer.Load(midifilename);

            foreach (var perf in performer)
                perf.Value.Sequencer = sequencer;
            InitNewPerformance();
        }

        public void LoadBMPSong(BmpSong song)
        {
            sequencer.Load(song);

            foreach (var perf in performer)
                perf.Value.Sequencer = sequencer;
            InitNewPerformance();
        }

        public void ForceAddPerformer()
        {
            Game game = BmpSeer.Instance.Games.Values.First();

            var result = performer.Find(kvp => kvp.Key == game.Pid);
            if (result.Key == game.Pid)
                return;
            while (true)
            {
                if (game.ConfigId.Length > 0)
                {
                    //Bard is loaded and prepared
                    Performer perf = new Performer(game);
                    perf.HostProcess = true;
                    perf.Sequencer = sequencer;
                    perf.TrackNum = 0;
                    performer.Add(new KeyValuePair<int, Performer>(game.Pid, perf));    //Add the performer
                    BmpMaestro.Instance.PublishEvent(new PerformersChangedEvent());     //And trigger an event
                    return;
                }
            }
        }

        public async Task<bool> AddPerformer(Game game, bool IsHost)
        {
            var result = performer.Find(kvp => kvp.Key == game.Pid);
            if (result.Key == game.Pid)
                return true;
            while (true)
            {
                if (game.ConfigId.Length > 0)
                {
                    //Bard is loaded and prepared
                    Performer perf = new Performer(game);
                    perf.HostProcess = IsHost;
                    perf.Sequencer = sequencer;
                    perf.TrackNum = 0;
                    performer.Add(new KeyValuePair<int, Performer>(game.Pid, perf));    //Add the performer
                    BmpMaestro.Instance.PublishEvent(new PerformersChangedEvent());     //And trigger an event
                    return true;
                }
                await Task.Delay(200);
            }
        }

        public void RemovePerformer(int Pid)
        {
            foreach (var perf in performer)
            {
                if (perf.Key == Pid)
                {
                    performer.Remove(perf);
                    perf.Value.Close();
                    BmpMaestro.Instance.PublishEvent(new PerformersChangedEvent());     //trigger the event
                    return;
                }
            }
        }

        public void ChangeTracknumber(int track)
        {
            foreach (var perf in performer)
                perf.Value.TrackNum = track;
            BmpMaestro.Instance.PublishEvent(new TrackNumberChangedEvent(track));
        }

        /// <summary>
        /// Set the MidiInput for the first performer
        /// </summary>
        /// <param name="device"></param>
        public void OpenInputDevice(int device)
        {
            if (performer.Count > 0)
            {
                performer.First().Value.Sequencer.CloseInputDevice();
                performer.First().Value.Sequencer.OpenInputDevice(device);
            }
        }

        /// <summary>
        /// Seeks the song to absolute position
        /// </summary>
        /// <param name="ticks"></param>
        public void Seek(int ticks)
        {
            foreach (var perf in performer)
                perf.Value.Sequencer.Seek(ticks);
        }

        /// <summary>
        /// Seeks the song to absolute position
        /// </summary>
        /// <param name="miliseconds"></param>
        public void Seek(double miliseconds)
        {
            foreach (var perf in performer)
                perf.Value.Sequencer.Seek(miliseconds);
        }

        /// <summary>
        /// Starts the playback
        /// </summary>
        public void Start()
        {
            foreach (var perf in performer)
                perf.Value.Play(true);
        }

        /// <summary>
        /// Pause the playback
        /// </summary>
        public void Pause()
        {
            foreach (var perf in performer)
                perf.Value.Play(false);
        }

        /// <summary>
        /// Stops the playback
        /// </summary>
        public void Stop()
        {
            foreach (var perf in performer)
                perf.Value.Stop();
        }

        /// <summary>
        /// adds the host performer, assuming first game is the host
        /// TODO: Machs mal schicker
        /// </summary>
        private void addHostPerformer()
        {
           /* while (true)
            {
                Game game = BmpSeer.Instance.Games.ToList().First().Value;
                if (game.ConfigId.Length > 0)
                {
                    Performer perf = new Performer(game);
                    perf.HostProcess = true;
                    perf.Sequencer = sequencer;
                    perf.TrackNum = 0;
                    performer.Add(perf);
                    return;
                }
                Task.Delay(200);
            }*/
        }

        /// <summary>
        /// Sets all events and starts the updater
        /// Called at every new song
        /// </summary>
        private void InitNewPerformance()
        {
            BmpMaestro.Instance.PublishEvent(new MaxPlayTimeEvent(sequencer.MaxTimeAsTimeSpan, sequencer.MaxTick));
            BmpSeer.Instance.InstrumentHeldChanged += delegate (Seer.Events.InstrumentHeldChanged e) {
                Instance_InstrumentHeldChanged(e);
            };

            _updaterTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => Updater(_updaterTokenSource.Token), TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Seer event for stopping the bards performance
        /// </summary>
        private void Instance_InstrumentHeldChanged(Seer.Events.InstrumentHeldChanged seerEvent)
        {
            Game game = seerEvent.Game;
            foreach (var perf in performer)
            {
                if (perf.Value.game.Equals(game))
                {
                    if (game.InstrumentHeld.Equals(Instrument.None))
                        perf.Value.Stop();
                    perf.Value.PerformerEnabled = !game.InstrumentHeld.Equals(Instrument.None);
                }
            }
        }

        /// <summary>
        /// the updater
        /// </summary>
        /// <param name="token"></param>
        private async Task Updater(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //Get first performer
                Performer perf = performer.First().Value;

                BmpMaestro.Instance.PublishEvent(new CurrentPlayPositionEvent(perf.Sequencer.CurrentTimeAsTimeSpan, perf.Sequencer.CurrentTick));
                if (!perf.Sequencer.IsPlaying)
                    BmpMaestro.Instance.PublishEvent(new PlaybackStoppedEvent());
                await Task.Delay(200, token);
            }
            return;
        }

        /// <summary>
        /// Disposing
        /// </summary>
        public void Dispose()
        {
            if (sequencer != null)
                sequencer.Dispose();
            // Dispose managed resources.
            if (_updaterTokenSource != null)
                _updaterTokenSource.Cancel();

            foreach (var perf in performer)
                perf.Value.Close();

            GC.SuppressFinalize(this);
        }
    }
}
