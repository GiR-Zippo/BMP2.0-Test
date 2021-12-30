namespace BardMusicPlayer.Maestro.Events
{
    public sealed class SongLoadedEvent : MaestroEvent
    {

        internal SongLoadedEvent(int maxtracks) : base(0, false)
        {
            EventType = GetType();
            MaxTracks = maxtracks;
        }

        public int MaxTracks { get; }

        public override bool IsValid() => true;
    }
}
