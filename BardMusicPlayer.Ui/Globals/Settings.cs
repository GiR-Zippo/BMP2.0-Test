using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using BardMusicPlayer.Ui.Functions;
using BardMusicPlayer.Pigeonhole;

namespace BardMusicPlayer.Ui.Globals
{
    public static class Settings
    {
        public enum Autostart_Types
        {
            NONE = 0,
            VIA_CHAT,
            VIA_METRONOME,
            UNUSED
        }

        public static Autostart_Types AutostartType
        {
            get { return (Autostart_Types)Convert.ToInt16(BmpPigeonhole.Instance.AutostartMethod); }
            set { BmpPigeonhole.Instance.AutostartMethod = (int)value; }
        }
    }
}
