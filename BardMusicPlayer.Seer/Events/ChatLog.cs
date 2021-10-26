/*
 * Copyright(c) 2021 MoogleTroupe
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

namespace BardMusicPlayer.Seer.Events
{
    public sealed class ChatLog : SeerEvent
    {
        internal ChatLog(EventSource readerBackendType, Reader.Backend.Sharlayan.Core.ChatLogItem item) : base(readerBackendType,0,false)
        {
            EventType = GetType();
            ChatLogCode = item.Code;
            ChatLogLine = item.Line;
        }

        public string ChatLogCode { get; }
        public string ChatLogLine { get; }

        public override bool IsValid() => true;
    }
}