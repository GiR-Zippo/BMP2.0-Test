﻿using System;

namespace BardMusicPlayer.Maestro.Events
{
    public sealed class MaxPlayTimeEvent : MaestroEvent
    {
        internal MaxPlayTimeEvent(TimeSpan inTimeSpan, int inTick) : base(0, false)
        {
            EventType = GetType();
            timeSpan = inTimeSpan;
            tick = inTick;
        }

        public TimeSpan timeSpan { get; }

        public int tick { get; }

        public override bool IsValid() => true;
    }

}
