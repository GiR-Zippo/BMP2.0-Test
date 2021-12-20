

namespace BardMusicPlayer.Maestro.Events
{
    public sealed class TrackNumberChangedEvent : MaestroEvent
    {
        internal TrackNumberChangedEvent(int trackNumber) : base(0, false)
        {
            EventType = GetType();
            TrackNumber = trackNumber;
        }

        public int TrackNumber { get; }

        public override bool IsValid() => true;
    }

}
