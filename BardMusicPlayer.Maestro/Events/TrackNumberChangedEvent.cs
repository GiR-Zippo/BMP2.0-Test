using BardMusicPlayer.Seer;

namespace BardMusicPlayer.Maestro.Events
{
    public sealed class TrackNumberChangedEvent : MaestroEvent
    {
        internal TrackNumberChangedEvent(Game g, int trackNumber) : base(0, false)
        {
            EventType = GetType();
            TrackNumber = trackNumber;
            game = g;
        }

        public int TrackNumber { get; }
        public Game game { get; }

        public override bool IsValid() => true;
    }

}
