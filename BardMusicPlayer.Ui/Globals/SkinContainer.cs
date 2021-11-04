using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BardMusicPlayer.Ui.Globals.SkinContainer
{
    public static class SkinContainer
    {
        public enum CBUTTON_TYPES
        {
            MAIN_PREVIOUS_BUTTON =0,
            MAIN_PREVIOUS_BUTTON_ACTIVE,
            MAIN_PLAY_BUTTON,
            MAIN_PLAY_BUTTON_ACTIVE,
            MAIN_PAUSE_BUTTON,
            MAIN_PAUSE_BUTTON_ACTIVE,
            MAIN_STOP_BUTTON,
            MAIN_STOP_BUTTON_ACTIVE,
            MAIN_NEXT_BUTTON,
            MAIN_NEXT_BUTTON_ACTIVE,
            MAIN_EJECT_BUTTON,
            MAIN_EJECT_BUTTON_ACTIVE
        }
        public enum TITLEBAR_TYPES
        {
            MAIN_TITLE_BAR = 0,
            MAIN_TITLE_BAR_SELECTED,
            MAIN_EASTER_EGG_TITLE_BAR,
            MAIN_EASTER_EGG_TITLE_BAR_SELECTED,
            MAIN_OPTIONS_BUTTON,
            MAIN_OPTIONS_BUTTON_DEPRESSED,
            MAIN_MINIMIZE_BUTTON,
            MAIN_MINIMIZE_BUTTON_DEPRESSED,
            MAIN_SHADE_BUTTON,
            MAIN_SHADE_BUTTON_DEPRESSED,
            MAIN_CLOSE_BUTTON,
            MAIN_CLOSE_BUTTON_DEPRESSED,
            MAIN_CLUTTER_BAR_BACKGROUND,
            MAIN_CLUTTER_BAR_BACKGROUND_DISABLED,
            MAIN_CLUTTER_BAR_BUTTON_O_SELECTED,
            MAIN_CLUTTER_BAR_BUTTON_A_SELECTED,
            MAIN_CLUTTER_BAR_BUTTON_I_SELECTED,
            MAIN_CLUTTER_BAR_BUTTON_D_SELECTED,
            MAIN_CLUTTER_BAR_BUTTON_V_SELECTED,
            MAIN_SHADE_BACKGROUND,
            MAIN_SHADE_BACKGROUND_SELECTED,
            MAIN_SHADE_BUTTON_SELECTED,
            MAIN_SHADE_BUTTON_SELECTED_DEPRESSED,
            MAIN_SHADE_POSITION_BACKGROUND,
            MAIN_SHADE_POSITION_THUMB,
            MAIN_SHADE_POSITION_THUMB_LEFT,
            MAIN_SHADE_POSITION_THUMB_RIGHT
        }

        public enum NUMBER_TYPES
        {
            DIGIT_0 = 0,
            DIGIT_1,
            DIGIT_2,
            DIGIT_3,
            DIGIT_4,
            DIGIT_5,
            DIGIT_6,
            DIGIT_7,
            DIGIT_8,
            DIGIT_9,
            NO_MINUS_SIGN,
            MINUS_SIGN
        }

        public enum PLAYLIST_TYPES
        {
            PLAYLIST_TOP_TILE = 0,
            PLAYLIST_TOP_LEFT_CORNER,
            PLAYLIST_TITLE_BAR,
            PLAYLIST_TOP_RIGHT_CORNER,
            PLAYLIST_TOP_TILE_SELECTED,
            PLAYLIST_TOP_LEFT_SELECTED,
            PLAYLIST_TITLE_BAR_SELECTED,
            PLAYLIST_TOP_RIGHT_CORNER_SELECTED,
            PLAYLIST_LEFT_TILE,
            PLAYLIST_RIGHT_TILE,
            PLAYLIST_BOTTOM_TILE,
            PLAYLIST_BOTTOM_LEFT_CORNER,
            PLAYLIST_BOTTOM_RIGHT_CORNER,
            PLAYLIST_VISUALIZER_BACKGROUND,
            PLAYLIST_SHADE_BACKGROUND,
            PLAYLIST_SHADE_BACKGROUND_LEFT,
            PLAYLIST_SHADE_BACKGROUND_RIGHT,
            PLAYLIST_SHADE_BACKGROUND_RIGHT_SELECTED,
            PLAYLIST_SCROLL_HANDLE_SELECTED,
            PLAYLIST_SCROLL_HANDLE,
            PLAYLIST_ADD_URL,
            PLAYLIST_ADD_URL_SELECTED,
            PLAYLIST_ADD_DIR,
            PLAYLIST_ADD_DIR_SELECTED,
            PLAYLIST_ADD_FILE,
            PLAYLIST_ADD_FILE_SELECTED,
            PLAYLIST_REMOVE_ALL,
            PLAYLIST_REMOVE_ALL_SELECTED,
            PLAYLIST_CROP,
            PLAYLIST_CROP_SELECTED,
            PLAYLIST_REMOVE_SELECTED,
            PLAYLIST_REMOVE_SELECTED_SELECTED,
            PLAYLIST_REMOVE_MISC,
            PLAYLIST_REMOVE_MISC_SELECTED,
            PLAYLIST_INVERT_SELECTION,
            PLAYLIST_INVERT_SELECTION_SELECTED,
            PLAYLIST_SELECT_ZERO,
            PLAYLIST_SELECT_ZERO_SELECTED,
            PLAYLIST_SELECT_ALL,
            PLAYLIST_SELECT_ALL_SELECTED,
            PLAYLIST_SORT_LIST,
            PLAYLIST_SORT_LIST_SELECTED,
            PLAYLIST_FILE_INFO,
            PLAYLIST_FILE_INFO_SELECTED,
            PLAYLIST_MISC_OPTIONS,
            PLAYLIST_MISC_OPTIONS_SELECTED,
            PLAYLIST_NEW_LIST,
            PLAYLIST_NEW_LIST_SELECTED,
            PLAYLIST_SAVE_LIST,
            PLAYLIST_SAVE_LIST_SELECTED,
            PLAYLIST_LOAD_LIST,
            PLAYLIST_LOAD_LIST_SELECTED,
            PLAYLIST_ADD_MENU_BAR,
            PLAYLIST_REMOVE_MENU_BAR,
            PLAYLIST_SELECT_MENU_BAR,
            PLAYLIST_MISC_MENU_BAR,
            PLAYLIST_LIST_BAR,
            PLAYLIST_CLOSE_SELECTED,
            PLAYLIST_COLLAPSE_SELECTED,
            PLAYLIST_EXPAND_SELECTED
        };

        public enum EQ_TYPES
        {
            EQ_WINDOW_BACKGROUND =0,
            EQ_TITLE_BAR,
            EQ_TITLE_BAR_SELECTED,
            EQ_SLIDER_BACKGROUND,
            EQ_SLIDER_THUMB,
            EQ_SLIDER_THUMB_SELECTED,
            EQ_CLOSE_BUTTON,
            EQ_CLOSE_BUTTON_ACTIVE,
            EQ_MAXIMIZE_BUTTON_ACTIVE_FALLBACK,
            EQ_ON_BUTTON,
            EQ_ON_BUTTON_DEPRESSED,
            EQ_ON_BUTTON_SELECTED,
            EQ_ON_BUTTON_SELECTED_DEPRESSED,
            EQ_AUTO_BUTTON,
            EQ_AUTO_BUTTON_DEPRESSED,
            EQ_AUTO_BUTTON_SELECTED,
            EQ_AUTO_BUTTON_SELECTED_DEPRESSED,
            EQ_GRAPH_BACKGROUND,
            EQ_GRAPH_LINE_COLORS,
            EQ_PRESETS_BUTTON,
            EQ_PRESETS_BUTTON_SELECTED,
            EQ_PREAMP_LINE
        }

        public enum GENEX_TYPES
        {
            GENEX_BUTTON_BACKGROUND_LEFT_UNPRESSED = 0,
            GENEX_BUTTON_BACKGROUND_CENTER_UNPRESSED,
            GENEX_BUTTON_BACKGROUND_RIGHT_UNPRESSED,
            GENEX_BUTTON_BACKGROUND_PRESSED,
            GENEX_SCROLL_UP_UNPRESSED,
            GENEX_SCROLL_DOWN_UNPRESSED,
            GENEX_SCROLL_UP_PRESSED,
            GENEX_SCROLL_DOWN_PRESSED,
            GENEX_SCROLL_LEFT_UNPRESSED,
            GENEX_SCROLL_RIGHT_UNPRESSED,
            GENEX_SCROLL_LEFT_PRESSED,
            GENEX_SCROLL_RIGHT_PRESSED,
            GENEX_VERTICAL_SCROLL_HANDLE_UNPRESSED,
            GENEX_VERTICAL_SCROLL_HANDLE_PRESSED,
            GENEX_HORIZONTAL_SCROLL_HANDLE_UNPRESSED,
            GENEX_HORIZONTAL_SCROLL_HANDLE_PRESSED
        }

        public enum VISCOLOR_TYPES
        {
            VISCOLOR_BACKGROUND = 0,
            VISCOLOR_PEAKS = 23
        }

        public static Dictionary<CBUTTON_TYPES, ImageBrush> CBUTTONS = new Dictionary<CBUTTON_TYPES, ImageBrush> { };
        public static Dictionary<NUMBER_TYPES, ImageBrush>  NUMBERS  = new Dictionary<NUMBER_TYPES, ImageBrush> { };
        public static Dictionary<TITLEBAR_TYPES, ImageBrush> TITLEBAR = new Dictionary<TITLEBAR_TYPES, ImageBrush> { };
        public static Dictionary<PLAYLIST_TYPES, ImageBrush> PLAYLIST = new Dictionary<PLAYLIST_TYPES, ImageBrush> { };
        public static Dictionary<EQ_TYPES, ImageBrush> EQUALIZER = new Dictionary<EQ_TYPES, ImageBrush> { };
        public static Dictionary<GENEX_TYPES, ImageBrush> GENEX = new Dictionary<GENEX_TYPES, ImageBrush> { };
        public static Dictionary<VISCOLOR_TYPES, System.Drawing.Color> VISCOLOR = new Dictionary<VISCOLOR_TYPES, System.Drawing.Color> { };
        public static Dictionary<int, Image> FONT = new Dictionary<int, Image> { };
    }
}
