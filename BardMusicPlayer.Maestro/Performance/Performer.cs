﻿using BardMusicPlayer.Grunt;
using BardMusicPlayer.Maestro.FFXIV;
using BardMusicPlayer.Maestro.Sequencing;
using BardMusicPlayer.Maestro.Utils;
using BardMusicPlayer.Pigeonhole;
using BardMusicPlayer.Quotidian.Enums;
using BardMusicPlayer.Quotidian.Structs;
using BardMusicPlayer.Seer;
using System;
using System.Collections.Generic;
using System.Timers;

namespace BardMusicPlayer.Maestro.Performance
{
    public class Performer
    {
        private FFXIVKeybindDat hotkeys = new FFXIVKeybindDat();
        private FFXIVHotbarDat hotbar = new FFXIVHotbarDat();
        private FFXIVHook hook = new FFXIVHook();

        public Instrument ChosenInstrument { get; set; } = Instrument.Piano;
        public int OctaveShift { get; set; } = 0;
        public int TrackNumber { get; set; } = 1;
        public bool PerformerEnabled { get; set; } = true;
        private bool holdNotes { get; set; } = true;
        private bool forcePlayback { get; set; } = false;

        public EventHandler onUpdate;

        public bool HostProcess { get; set; } = false;

        public int PId = 0;

        public Game game;

        public string PlayerName { get { return game.PlayerName ?? "Unknown"; } }
        public string HomeWorld { get { return game.HomeWorld ?? "Unknown"; } }
        public string TrackInstrument 
        { 
            get {
                    if (sequencer == null)
                        return "Unknown";
                if (TrackNumber == 0)
                    return Instrument.Piano;
                return sequencer.GetTrackPreferredInstrument(TrackNumber -1).Name; // -1 cuz all tracks doesn't have an instrument
                }
        }

        private Sequencer sequencer;
        public Sequencer Sequencer
        {
            get{ return sequencer; }
            set
            {
                if (value != null)
                {
                    if ((value.LoadedFileType == Sequencer.FILETYPES.None) && !HostProcess)
                        return;
                    
                    //Close the input else it will hang
                    if (sequencer is Sequencer)
                        sequencer.CloseInputDevice();

                    sequencer = new Sequencer();
                    if (value.LoadedFileType == Sequencer.FILETYPES.BmpSong)
                    {
                        sequencer.Load(value.LoadedBmpSong, this.TrackNumber);
                        this.OctaveShift = +1;
                    }
                    else if (value.LoadedFileType == Sequencer.FILETYPES.MidiFile)
                        sequencer.Load(value.LoadedFilename, this.TrackNumber);

                    if (HostProcess)
                    {
                        if (BmpPigeonhole.Instance.MidiInputDev != -1)
                            sequencer.OpenInputDevice(BmpPigeonhole.Instance.MidiInputDev);
                    }

                    sequencer.OnNote += InternalNote;
                    sequencer.OffNote += InternalNote;
                    sequencer.ProgChange += InternalProg;

                    holdNotes = BmpPigeonhole.Instance.HoldNotes;
                    if (HostProcess && BmpPigeonhole.Instance.ForcePlayback)
                        forcePlayback = true;

                    // set the initial octave shift here, if we have a track to play
                    if (this.TrackNumber < sequencer.Sequence.Count)
                        OctaveShift = sequencer.GetTrackPreferredOctaveShift(sequencer.Sequence[this.TrackNumber]);
                    this.Update(value);
                }
            }
        }

        public Performer(Game arg)
        {
            this.ChosenInstrument = this.ChosenInstrument;

            if (arg != null)
            {
                hook.Hook(arg.Process, false);
                hotkeys.LoadKeybindDat(arg.ConfigId);
                hotbar.LoadHotbarDat(arg.ConfigId);
                PId = arg.Pid;
                game = arg;
            }
        }

        public void Close()
        {
            if (sequencer is Sequencer)
            {
                sequencer.CloseInputDevice();
                sequencer.Dispose();
            }
            hook.ClearLastPerformanceKeybinds();
        }

        private void InternalNote(Object o, Sanford.Multimedia.Midi.ChannelMessageEventArgs args)
        {
            Sanford.Multimedia.Midi.ChannelMessageBuilder builder = new Sanford.Multimedia.Midi.ChannelMessageBuilder(args.Message);

            NoteEvent noteEvent = new NoteEvent
            {
                note = builder.Data1,
                origNote = builder.Data1,
                trackNum = sequencer.GetTrackNum(args.MidiTrack),
                track = args.MidiTrack,
            };

            if ((sequencer.GetTrackNum(noteEvent.track) == this.TrackNumber) || !sequencer.IsPlaying)
            {
                noteEvent.note = NoteHelper.ApplyOctaveShift(noteEvent.note, this.OctaveShift);

                Sanford.Multimedia.Midi.ChannelCommand cmd = args.Message.Command;
                int vel = builder.Data2;
                if ((cmd == Sanford.Multimedia.Midi.ChannelCommand.NoteOff) || (cmd == Sanford.Multimedia.Midi.ChannelCommand.NoteOn && vel == 0))
                {
                    this.ProcessOffNote(noteEvent);
                }
                if ((cmd == Sanford.Multimedia.Midi.ChannelCommand.NoteOn) && vel > 0)
                {
                    this.ProcessOnNote(noteEvent);
                }
            }
        }

        public void ProcessOnNote(NoteEvent note)
        {
            if (!forcePlayback)
            {
                if (!this.PerformerEnabled)
                    return;

                if (game.InstrumentHeld.Equals(Instrument.None))
                    return;
            }

            if (hotkeys.GetKeybindFromNoteByte(note.note) is FFXIVKeybindDat.Keybind keybind)
            {
                if (game.ChatStatus && !forcePlayback)
                    return;

                if (BmpPigeonhole.Instance.HoldNotes)
                {
                    hook.SendKeybindDown(keybind);
                }
                else
                {
                    hook.SendAsyncKeybind(keybind);
                }
            }
        }

        public void ProcessOffNote(NoteEvent note)
        {
            if (!this.PerformerEnabled)
                return;

            if (hotkeys.GetKeybindFromNoteByte(note.note) is FFXIVKeybindDat.Keybind keybind)
            {
                if (game.ChatStatus && !forcePlayback)
                    return;

                if (holdNotes)
                {
                    hook.SendKeybindUp(keybind);
                }
            }
        }

        private void InternalProg(object sender, Sanford.Multimedia.Midi.ChannelMessageEventArgs args)
        {
            if (!forcePlayback)
            {
                if (!this.PerformerEnabled)
                    return;

                if (game.InstrumentHeld.Equals(Instrument.None))
                    return;
            }

            var builder = new Sanford.Multimedia.Midi.ChannelMessageBuilder(args.Message);
            var programEvent = new ProgChangeEvent
            {
                track = args.MidiTrack,
                trackNum = sequencer.GetTrackNum(args.MidiTrack),
                voice = args.Message.Data1,
            };
            if (programEvent.voice < 27 || programEvent.voice > 31)
                return;

            if (sequencer.GetTrackNum(programEvent.track) == this.TrackNumber)
            {
                if (game.ChatStatus && !forcePlayback)
                    return;

                if (hotkeys.GetKeybindFromVoiceByte(programEvent.voice) is FFXIVKeybindDat.Keybind keybind)
                    hook.SendSyncKeybind(keybind);
            }
        }

        public void SetProgress(int progress)
        {
            if (sequencer is Sequencer)
            {
                sequencer.Position = progress;
            }
        }

        public void Play(bool play)
        {
            if (sequencer is Sequencer)
            {
                if (play)
                {
                    sequencer.Play();
                }
                else
                {
                    sequencer.Pause();
                }
            }
        }

        public void Stop()
        {
            if (sequencer is Sequencer)
            {
                sequencer.Stop();
                hook.ClearLastPerformanceKeybinds();
            }
        }

        public void Update(Sequencer bmpSeq)
        {
            int tn = TrackNumber;

            if (!(bmpSeq is Sequencer))
            {
                return;
            }

            Sanford.Multimedia.Midi.Sequence seq = bmpSeq.Sequence;
            if (!(seq is Sanford.Multimedia.Midi.Sequence))
            {
                return;
            }

            if ((tn >= 0 && tn < seq.Count) && seq[tn] is Sanford.Multimedia.Midi.Track track)
            {
                // OctaveNum now holds the track octave and the selected octave together
                Console.WriteLine(String.Format("Track #{0}/{1} setOctave: {2} prefOctave: {3}", tn, bmpSeq.MaxTrack, OctaveShift, bmpSeq.GetTrackPreferredOctaveShift(track)));
                List<int> notes = new List<int>();
                foreach (Sanford.Multimedia.Midi.MidiEvent ev in track.Iterator())
                {
                    if (ev.MidiMessage.MessageType == Sanford.Multimedia.Midi.MessageType.Channel)
                    {
                        Sanford.Multimedia.Midi.ChannelMessage msg = (ev.MidiMessage as Sanford.Multimedia.Midi.ChannelMessage);
                        if (msg.Command == Sanford.Multimedia.Midi.ChannelCommand.NoteOn)
                        {
                            int note = msg.Data1;
                            int vel = msg.Data2;
                            if (vel > 0)
                            {
                                notes.Add(NoteHelper.ApplyOctaveShift(note, this.OctaveShift));
                            }
                        }
                    }
                }
                ChosenInstrument = bmpSeq.GetTrackPreferredInstrument(track);
            }
        }

        public void OpenInstrument()
        {
            // Exert the effort to check memory i guess
            if (HostProcess)
            {
                if (!game.Equals(Instrument.None))
                    return;
            }

            // don't open instrument if we don't have anything loaded
            if (sequencer == null || sequencer.Sequence == null)
                return;

            // don't open instrument if we're not on a valid track
            if (TrackNumber == 0 || TrackNumber >= sequencer.Sequence.Count)
                return;

            _ = GameExtensions.EquipInstrument(game, TrackInstrument);
        }

        public void CloseInstrument()
        {
            if (HostProcess)
            {
                if (game.InstrumentHeld.Equals(Instrument.None))
                    return;
            }

            hook.ClearLastPerformanceKeybinds();

            _ = GameExtensions.EquipInstrument(game, Instrument.None);
        }

        public void EnsembleAccept()
        {
            _ = GameExtensions.AcceptEnsemble(game);
            /*FFXIVKeybindDat.Keybind keybind = hotkeys["OK"];
            if (keybind is FFXIVKeybindDat.Keybind && keybind.GetKey() != Keys.None)
            {
                hook.SendSyncKeybind(keybind);
                hook.SendSyncKeybind(keybind);
            }*/
        }
        public void NoteKey(string noteKey)
        {
            if (hotkeys.GetKeybindFromNoteKey(noteKey) is FFXIVKeybindDat.Keybind keybind)
            {
                hook.SendAsyncKeybind(keybind);
            }
        }
    }
}

