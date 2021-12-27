namespace BardMusicPlayer.Maestro.Events
{
    public sealed class SongLoadedEvent : MaestroEvent
    {

        internal SongLoadedEvent() : base(0, false)
        {
            EventType = GetType();
        }

        public override bool IsValid() => true;
    }
}
