using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using BardMusicPlayer.Ui.Functions;

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
        public static Autostart_Types AutostartType;

        public static void DefaultConfigs()
        {
            AutostartType = Autostart_Types.NONE;
        }

        public static void LoadConfig()
        {
            Globals.CurrentTrack = 1;
            AutostartType = (Autostart_Types)Convert.ToInt16(ConfigurationManager.AppSettings["AutostartType"]);
            
            //AutostartType = (Autostart_Types)SettingsFile.Default.AutostartType;
        }
        
        public static void SaveConfig()
        {
            saveConfig("AutostartType", Convert.ToInt32(AutostartType).ToString());
        }

        private static void saveConfig(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
