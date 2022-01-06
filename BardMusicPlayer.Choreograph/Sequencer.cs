using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BardMusicPlayer.Grunt;
using BardMusicPlayer.Maestro.Events;
using BardMusicPlayer.Quotidian.Enums;
using BardMusicPlayer.Quotidian.Structs;
using BardMusicPlayer.Seer;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace BardMusicPlayer.Choreograph
{
    public class Sequencer
    {
        private Game _game;
        private Playback _playback;

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Thread _thread;

        private ConcurrentQueue<(Game, MidiEventPlayedEventArgs, int)> _eventQueue;
        public delegate void Del(Game game, MidiEventPlayedEventArgs e, int tracknumber);

        private int _tracknumber = 0;
        ITimeSpan _startingpoint;

        public Sequencer(Game game, MidiFile container, int tracknr = -1)
        {
            _eventQueue = new ConcurrentQueue<(Game, MidiEventPlayedEventArgs, int)>();

            _game = game;
            _playback = container.GetPlayback();

            /*_playback = PlaybackUtilities.GetPlayback(container, new PlaybackSettings
            {
                ClockSettings = new MidiClockSettings
                {
                    CreateTickGeneratorCallback = () => null
                }
            });*/
            //Start the melanchall sequencer
            PlaybackCurrentTimeWatcher.Instance.AddPlayback(_playback, TimeSpanType.Metric);
            PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += OnTick;
            PlaybackCurrentTimeWatcher.Instance.PollingInterval = TimeSpan.FromMilliseconds(1);  //Not sure, but seems to affect OnNoteEvent polling too
            PlaybackCurrentTimeWatcher.Instance.Start();

            _playback.Speed = 1;                    //Yep that's the playback speed and we'll set it
            //_playback.NotesPlaybackStarted += _playback_NotesPlaybackStarted;
            //_playback.NotesPlaybackFinished += _playback_NotesPlaybackFinished;
            _playback.EventPlayed += OnNoteEvent;
            _playback.Finished    += OnPlaybackStopped;
            _tracknumber = tracknr;

            //BmpChoreograph.Instance.PublishEvent(new MaxPlayTimeEvent(_playback.GetDuration(TimeSpanType.Metric)));
        }

        private void _playback_NotesPlaybackFinished(object sender, NotesEventArgs e)
        {
            foreach (Note non in e.Notes)
            {
                if ((non.Channel == _tracknumber) || (_tracknumber == -1))
                {
                    var note = non.NoteNumber - 60;
                    _ = GameExtensions.SyncReleaseKey(_game, _game.NoteKeys[(NoteKey)note], 20);
                }
                    //GameExtensions.SendNoteOff(_game, non.NoteNumber).ConfigureAwait(false);
            }
        }

        private void _playback_NotesPlaybackStarted(object sender, NotesEventArgs e)
        {
            foreach (Note non in e.Notes)
            {
                if ((non.Channel == _tracknumber) || (_tracknumber == -1))
                {
                    var note = non.NoteNumber - 60;
                    _ = GameExtensions.SyncPushKey(_game, _game.NoteKeys[(NoteKey)note], 20);
                }
                    //GameExtensions.SendNoteOn(_game, non.NoteNumber, non.Channel).ConfigureAwait(false);
            }
        }

        public void SetPlaybackStart(double f)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(f/1000); //We have microseconds and want some milis....
            _startingpoint = new MetricTimeSpan(hours: time.Hours, minutes: time.Minutes, seconds: time.Seconds, milliseconds: time.Milliseconds);

            _playback.MoveToTime(_startingpoint);
        }

        public void OnTick(object sender, PlaybackCurrentTimeChangedEventArgs e)
        {
            //BmpChoreograph.Instance.PublishEvent(new CurrentPlayPositionEvent(_playback.GetCurrentTime(TimeSpanType.Metric)));
        }

        private void OnPlaybackStopped(object sender, EventArgs e)
        {
            //BmpChoreograph.Instance.PublishEvent(new PlaybackStoppedEvent());
        }

        public void OnNoteEvent(object sender, MidiEventPlayedEventArgs e)
        {
            RunEventsHandler(_game, e, _tracknumber);

        }

        private void RunEventsHandler(Game game, MidiEventPlayedEventArgs e, int tracknumber)
        {
            switch (e.Event.EventType)
            {
                case MidiEventType.SetTempo:
                    var tempo = e.Event as SetTempoEvent;
                    break;
                case MidiEventType.NoteOn:
                    NoteOnEvent non = e.Event as NoteOnEvent;
                    if ((non.Channel == _tracknumber) || (_tracknumber == -1))
                    {
                        var note = non.NoteNumber - 60;
                        _ = GameExtensions.SyncPushKey(_game, _game.NoteKeys[(NoteKey)note], 20);
                    }
                    break;
                case MidiEventType.NoteOff:
                    NoteOffEvent noff = e.Event as NoteOffEvent;
                    if ((noff.Channel == tracknumber) || (tracknumber == -1))
                    {
                        var note = noff.NoteNumber - 60;
                        _ = GameExtensions.SyncReleaseKey(_game, _game.NoteKeys[(NoteKey)note], 20);
                    }
                    break;
                case MidiEventType.Text:
                    TextEvent text = e.Event as TextEvent;
                    //_ = GameExtensions.SendLyricLine(_game, text.Text);
                    break;
                default:
                    break;
            }
            return;
        }

        public void ChangeTracknumer(int track)
        { _tracknumber = track; }

        public void Start()
        {
            if (!BmpGrunt.Instance.Started)
                return;
            //if (_game.InstrumentHeld.Equals(Instrument.None) || _game.ChatStatus || !_game.IsBard)
            //    return;

            //more precise Timer for melanchall
            /*_thread = new Thread(() =>
            {
                _thread.Priority = ThreadPriority.Highest;
                var stopwatch = new Stopwatch();
                var lastMs = 0L;

                stopwatch.Start();

                while (!tokenSource.IsCancellationRequested)
                {
                    var elapsedMs = stopwatch.ElapsedTicks;
                    if (elapsedMs - lastMs >= 1)
                    {
                        _playback.TickClock();
                        lastMs = elapsedMs;
                    }
                }
            });

            _thread.Start();*/

            _playback.Start();      //start from the point you stopped
        }

        public void Pause()
        {
            //_thread.Abort();
            _playback.Stop();       //missleading, it only pauses
        }

        public void Stop()
        {
            //_thread.Abort();
            _playback.Stop();        //Stop
            _playback.MoveToStart(); //To the beginning of da song
        }

        public void Destroy()
        {
            _playback.Stop();
            _playback.Dispose();
        }

    }
}
