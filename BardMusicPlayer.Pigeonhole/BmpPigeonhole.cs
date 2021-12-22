﻿/*
 * Copyright(c) 2021 MoogleTroupe
 * Licensed under the GPL v3 license. See https://github.com/BardMusicPlayer/BardMusicPlayer/blob/develop/LICENSE for full license information.
 */

using BardMusicPlayer.Pigeonhole.JsonSettings.Autosave;
using BardMusicPlayer.Quotidian;

namespace BardMusicPlayer.Pigeonhole
{
    public class BmpPigeonhole : JsonSettings.JsonSettings
    {
        private static BmpPigeonhole _instance;

        /// <summary>
        /// Initializes the pigeonhole file
        /// </summary>
        /// <param name="filename">full path to the json pigeonhole file</param>
        public static void Initialize(string filename)
        {
            if (Initialized) return;
            _instance = Load<BmpPigeonhole>(filename).EnableAutosave();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool Initialized => _instance != null;

        /// <summary>
        /// Gets this pigeonhole instance
        /// </summary>
        public static BmpPigeonhole Instance => _instance ?? throw new BmpException("This pigeonhole must be initialized first.");

        /// <summary>
        /// Sets PlayAllTracks
        /// </summary>
        public virtual bool PlayAllTracks { get; set; } = false;

        /// <summary>
        /// Sets PlaylistDelay
        /// </summary>
        public virtual float PlaylistDelay { get; set; } = 1;

        /// <summary>
        /// Sets PlayAllTracks
        /// </summary>
        public virtual bool PlaylistAutoPlay { get; set; } = true;

        /// <summary>
        /// last loaded song
        /// </summary>
        public virtual string LastLoaded { get; set; } = "";

        /// <summary>
        /// last loaded song
        /// </summary>
        public virtual string SongDirectory { get; set; } = "songs/";

        /// <summary>
        /// Sets PlayAllTracks
        /// </summary>
        public virtual bool HoldNotes { get; set; } = true;

        /// <summary>
        /// save the chatlog
        /// </summary>
        public virtual bool SaveChatLog { get; set; } = false;

        /// <summary>
        /// Sets the autostart method
        /// </summary>
        public virtual int AutostartMethod { get; set; } = 0;

        /// <summary>
        /// Sets the verbose mode
        /// </summary>
        public virtual bool Verbose { get; set; } = false;

        /// <summary>
        /// Sets UnequipPause
        /// </summary>
        public virtual bool UnequipPause { get; set; } = true;

        /// <summary>
        /// last selected midi input device
        /// </summary>
        public virtual int MidiInputDev { get; set; } = -1;

        /// <summary>
        /// force the playback
        /// </summary>
        public virtual bool ForcePlayback { get; set; } = false;

        /// <summary>
        /// brings the game to front
        /// </summary>
        public virtual bool BringGametoFront { get; set; } = true;

        /// <summary>
        /// brings the bmp to front
        /// </summary>
        public virtual bool BringBMPtoFront { get; set; } = true;

        /// <summary>
        /// unkown but used
        /// </summary>
        public virtual bool SigIgnore { get; set; } = false;

        /// <summary>
        /// LastCharId
        /// </summary>
        public virtual string LastCharId { get; set; } = "";

        /// <summary>
        /// BMP window location
        /// </summary>
        public virtual global::System.Drawing.Point BmpLocation { get; set; } = System.Drawing.Point.Empty;

        public virtual global::System.Drawing.Size BmpSize { get; set; } = System.Drawing.Size.Empty;

        /// <summary>
        /// Sets PlayAllTracks
        /// </summary>
        public virtual bool ClassicUi { get; set; } = true;

        /// <summary>
        /// Sets PlayAllTracks
        /// </summary>
        public virtual string LastSkin { get; set; } = "";

        /// <summary>
        /// open local orchestra after hooking new proc
        /// </summary>
        public virtual bool LocalOrchestra { get; set; } = false;

        /// <summary>
        /// Enable the 16 voice limit in Synthesizer
        /// </summary>
        public virtual bool EnableSynthVoiceLimiter { get; set; } = true;

        /// <summary>
        /// milliseconds between game process scans / seer scanner startups.
        /// </summary>
        public virtual int SeerGameScanCooldown { get; set; } = 20;

        /// <summary>
        /// Contains the last path of an opened midi file
        /// </summary>
        public virtual string LastOpenedMidiPath { get; set; } = "";

        /// <summary>
        /// Contains the delay used for note pressing. This should be no less then 1 and no greater then 25.
        /// </summary>
        public virtual int NoteKeyDelay { get; set; } = 1;

        /// <summary>
        /// Contains the delay used for tone pressing. This should be no less then 1 and no greater then 25.
        /// </summary>
        public virtual int ToneKeyDelay { get; set; } = 3;

        /// <summary>
        /// Defaults to log level Info
        /// </summary>
        public virtual BmpLog.Verbosity DefaultLogLevel { get; set; } = BmpLog.Verbosity.Info;
    }
}
