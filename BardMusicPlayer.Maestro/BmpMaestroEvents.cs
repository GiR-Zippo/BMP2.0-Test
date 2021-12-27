﻿using BardMusicPlayer.Maestro.Events;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BardMusicPlayer.Maestro
{
    public partial class BmpMaestro
    {
        public EventHandler<CurrentPlayPositionEvent> OnPlaybackTimeChanged;
        public EventHandler<MaxPlayTimeEvent> OnSongMaxTime;
        public EventHandler<bool> OnSongLoaded;
        public EventHandler<bool> OnPlaybackStopped;
        public EventHandler<bool> OnPerformerChanged;
        public EventHandler<TrackNumberChangedEvent> OnTrackNumberChanged;
        private ConcurrentQueue<MaestroEvent> _eventQueue;
        private bool _eventQueueOpen;

        private async Task RunEventsHandler(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                while (_eventQueue.TryDequeue(out var meastroEvent))
                {
                    if (token.IsCancellationRequested)
                        break;

                    try
                    {
                        switch (meastroEvent)
                        {
                            case CurrentPlayPositionEvent currentPlayPosition:
                                OnPlaybackTimeChanged(this, currentPlayPosition);
                                break;
                            case MaxPlayTimeEvent maxPlayTime:
                                OnSongMaxTime(this, maxPlayTime);
                                break;
                            case SongLoadedEvent songloaded:
                                if (OnSongLoaded == null)
                                    break;
                                OnSongLoaded(this, true);
                                break;
                            case PlaybackStoppedEvent playbackStopped:
                                if (OnPlaybackStopped == null)
                                    break;
                                OnPlaybackStopped(this, playbackStopped.Stopped);
                                break;
                            case PerformersChangedEvent performerChanged:
                                if (OnPerformerChanged == null)
                                    break;
                                OnPerformerChanged(this, performerChanged.Changed);
                                break;
                            case TrackNumberChangedEvent trackNumberChanged:
                                if (OnTrackNumberChanged == null)
                                    break;
                                OnTrackNumberChanged(this, trackNumberChanged);
                                break;

                        };
                    }
                    catch
                    { }
                }
                await Task.Delay(25, token);
            }
        }

        private CancellationTokenSource _eventsTokenSource;

        private void StartEventsHandler()
        {
            _eventQueue = new ConcurrentQueue<MaestroEvent>();
            _eventsTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => RunEventsHandler(_eventsTokenSource.Token), TaskCreationOptions.LongRunning);
            _eventQueueOpen = true;
        }

        private void StopEventsHandler()
        {
            _eventQueueOpen = false;
            _eventsTokenSource.Cancel();
            while (_eventQueue.TryDequeue(out _))
            {
            }
        }

        internal void PublishEvent(MaestroEvent meastroEvent)
        {
            if (!_eventQueueOpen)
                return;

            _eventQueue.Enqueue(meastroEvent);
        }
    }
}
