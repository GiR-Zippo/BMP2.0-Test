using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
using System.Windows.Media;
using System.Windows;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using BardMusicPlayer.Ui.Globals.SkinContainer;
using System.Drawing.Text;

namespace BardMusicPlayer.Ui.Views
{
    public partial class Skinned_MainView : UserControl
    {

        Dictionary<SkinContainer.CBUTTON_TYPES, List<int>> cbuttonsdata = new Dictionary<SkinContainer.CBUTTON_TYPES, List<int>>
        {
            {SkinContainer.CBUTTON_TYPES.MAIN_PREVIOUS_BUTTON, new List<int> {0,0,23,18}},
            {SkinContainer.CBUTTON_TYPES.MAIN_PREVIOUS_BUTTON_ACTIVE, new List<int> {0,18,23,18}},
            {SkinContainer.CBUTTON_TYPES.MAIN_PLAY_BUTTON, new List<int> {23,0,23,18}},
            {SkinContainer.CBUTTON_TYPES.MAIN_PLAY_BUTTON_ACTIVE, new List<int> {23,18,23,18}},
            {SkinContainer.CBUTTON_TYPES.MAIN_PAUSE_BUTTON, new List<int> {46, 0, 23, 18}},
            {SkinContainer.CBUTTON_TYPES.MAIN_PAUSE_BUTTON_ACTIVE, new List<int> {46, 18, 23, 18}},
            {SkinContainer.CBUTTON_TYPES.MAIN_STOP_BUTTON, new List<int> {69, 0, 23, 18 }},
            {SkinContainer.CBUTTON_TYPES.MAIN_STOP_BUTTON_ACTIVE,new List<int> {69, 18, 23, 18 }},
            {SkinContainer.CBUTTON_TYPES.MAIN_NEXT_BUTTON,new List<int> {92, 0, 22, 18 }},
            {SkinContainer.CBUTTON_TYPES.MAIN_NEXT_BUTTON_ACTIVE,new List<int> {92, 18, 22, 18 } },
            {SkinContainer.CBUTTON_TYPES.MAIN_EJECT_BUTTON,new List<int> {114, 0, 22, 16 } },
            {SkinContainer.CBUTTON_TYPES.MAIN_EJECT_BUTTON_ACTIVE,new List<int> {114, 16, 22, 16 } }
        };

        Dictionary<SkinContainer.NUMBER_TYPES, List<int>> numbersdata = new Dictionary<SkinContainer.NUMBER_TYPES, List<int>>
        {
            {SkinContainer.NUMBER_TYPES.DIGIT_0, new List<int> {0, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_1, new List<int> {9, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_2, new List<int> {18, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_3, new List<int> {27, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_4, new List<int> {36, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_5, new List<int> {45, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_6, new List<int> {54, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_7, new List<int> {63, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_8, new List<int> {72, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.DIGIT_9, new List<int> {81, 0, 9, 13 } },
            {SkinContainer.NUMBER_TYPES.NO_MINUS_SIGN, new List<int> { 9, 6, 5, 1 } },
            {SkinContainer.NUMBER_TYPES.MINUS_SIGN, new List<int> {20, 6, 5, 1 } }
        };

        Dictionary<SkinContainer.TITLEBAR_TYPES, List<int>> titlebardata = new Dictionary<SkinContainer.TITLEBAR_TYPES, List<int>>
        {
            {SkinContainer.TITLEBAR_TYPES.MAIN_TITLE_BAR, new List<int> {27, 15, 275, 14 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_TITLE_BAR_SELECTED, new List<int> {27, 0, 275, 14 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_EASTER_EGG_TITLE_BAR, new List<int> { 27, 72, 275, 14} },
            {SkinContainer.TITLEBAR_TYPES.MAIN_EASTER_EGG_TITLE_BAR_SELECTED, new List<int> {27, 57, 275, 14 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_OPTIONS_BUTTON, new List<int> {0, 0, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_OPTIONS_BUTTON_DEPRESSED, new List<int> {0, 9, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_MINIMIZE_BUTTON, new List<int> {9, 0, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_MINIMIZE_BUTTON_DEPRESSED, new List<int> {9, 9, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_BUTTON, new List<int> {0, 18, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_BUTTON_DEPRESSED, new List<int> {9, 18, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLOSE_BUTTON, new List<int> {18, 0, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLOSE_BUTTON_DEPRESSED, new List<int> {18, 9, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BACKGROUND, new List<int> {304, 0, 8, 43,} },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BACKGROUND_DISABLED, new List<int> {312, 0, 8, 43} },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BUTTON_O_SELECTED, new List<int> {304, 47, 8, 8 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BUTTON_A_SELECTED, new List<int> {312, 55, 8, 7 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BUTTON_I_SELECTED, new List<int> {320, 62, 8, 7 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BUTTON_D_SELECTED, new List<int> {328, 69, 8, 8 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_CLUTTER_BAR_BUTTON_V_SELECTED, new List<int> {336, 77, 8, 7 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_BACKGROUND, new List<int> {27, 42, 275, 14 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_BACKGROUND_SELECTED, new List<int> {27, 29, 275, 14 }},
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_BUTTON_SELECTED, new List<int> {0, 27, 9, 9 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_BUTTON_SELECTED_DEPRESSED, new List<int> {9, 27, 9, 9 }},
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_POSITION_BACKGROUND, new List<int> {0, 36, 17, 7 }},
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_POSITION_THUMB, new List<int> {20, 36, 3, 7 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_POSITION_THUMB_LEFT, new List<int> {17, 36, 3, 7 } },
            {SkinContainer.TITLEBAR_TYPES.MAIN_SHADE_POSITION_THUMB_RIGHT, new List<int> {23, 36, 3, 7 } },
        };


        Dictionary<SkinContainer.EQ_TYPES, List<int>> eqdata = new Dictionary<SkinContainer.EQ_TYPES, List<int>>
        {
            { SkinContainer.EQ_TYPES.EQ_WINDOW_BACKGROUND, new List<int> {0,0,275, 116}},
            { SkinContainer.EQ_TYPES.EQ_TITLE_BAR, new List<int> {0,149,275, 14}},
            { SkinContainer.EQ_TYPES.EQ_TITLE_BAR_SELECTED, new List<int> {0,134,275, 14}},
            { SkinContainer.EQ_TYPES.EQ_SLIDER_BACKGROUND, new List<int> {13,164,209, 129}},
            { SkinContainer.EQ_TYPES.EQ_SLIDER_THUMB, new List<int> {0,164,11, 11}},
            { SkinContainer.EQ_TYPES.EQ_SLIDER_THUMB_SELECTED, new List<int> {0,176,11, 11}},
            { SkinContainer.EQ_TYPES.EQ_CLOSE_BUTTON, new List<int> {0,116,9, 9}},
            { SkinContainer.EQ_TYPES.EQ_CLOSE_BUTTON_ACTIVE, new List<int> {0,125,9, 9}},
            { SkinContainer.EQ_TYPES.EQ_MAXIMIZE_BUTTON_ACTIVE_FALLBACK, new List<int> {254,152,9,9,}},
            { SkinContainer.EQ_TYPES.EQ_ON_BUTTON, new List<int> {10,119,26, 12}},
            { SkinContainer.EQ_TYPES.EQ_ON_BUTTON_DEPRESSED, new List<int> {128,119,26, 12}},
            { SkinContainer.EQ_TYPES.EQ_ON_BUTTON_SELECTED, new List<int> {69,119,26, 12}},
            { SkinContainer.EQ_TYPES.EQ_ON_BUTTON_SELECTED_DEPRESSED, new List<int> {187,119,26,12,}},
            { SkinContainer.EQ_TYPES.EQ_AUTO_BUTTON, new List<int> {36,119,32, 12}},
            { SkinContainer.EQ_TYPES.EQ_AUTO_BUTTON_DEPRESSED, new List<int> {154,119,32,12,}},
            { SkinContainer.EQ_TYPES.EQ_AUTO_BUTTON_SELECTED, new List<int> {95,119,32, 12}},
            { SkinContainer.EQ_TYPES.EQ_AUTO_BUTTON_SELECTED_DEPRESSED, new List<int> {213,119,32,12,}},
            { SkinContainer.EQ_TYPES.EQ_GRAPH_BACKGROUND, new List<int> {0,294,113, 19}},
            { SkinContainer.EQ_TYPES.EQ_GRAPH_LINE_COLORS, new List<int> {115,294,1, 19}},
            { SkinContainer.EQ_TYPES.EQ_PRESETS_BUTTON, new List<int> {224,164,44, 12}},
            { SkinContainer.EQ_TYPES.EQ_PRESETS_BUTTON_SELECTED, new List<int> {224,176,44,12}},
            { SkinContainer.EQ_TYPES.EQ_PREAMP_LINE, new List<int> {0,314,113, 1}}
        };

        Dictionary<SkinContainer.GENEX_TYPES, List<int>> genexdata = new Dictionary<SkinContainer.GENEX_TYPES, List<int>>
        {
            {SkinContainer.GENEX_TYPES.GENEX_BUTTON_BACKGROUND_LEFT_UNPRESSED, new List<int> {0,0,15,4 } },
            {SkinContainer.GENEX_TYPES.GENEX_BUTTON_BACKGROUND_CENTER_UNPRESSED, new List<int> {4,0,15,39 } },
            {SkinContainer.GENEX_TYPES.GENEX_BUTTON_BACKGROUND_RIGHT_UNPRESSED, new List<int> {43,0,15,4 } },
            {SkinContainer.GENEX_TYPES.GENEX_BUTTON_BACKGROUND_PRESSED, new List<int> {0,1,15,47 }},
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_UP_UNPRESSED, new List<int> {0, 31, 14, 14 } },
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_DOWN_UNPRESSED, new List<int> {14,31,14,14 }},
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_UP_PRESSED, new List<int> {28, 31, 14, 14 } },
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_DOWN_PRESSED, new List<int> {42, 31, 14, 14 } },
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED, new List<int> {0, 45, 14, 14 } },
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_UNPRESSED, new List<int> {14,45,14,14 } },
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_PRESSED, new List<int> {28, 45, 14, 14 } },
            {SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_PRESSED, new List<int> {42, 45, 14, 14 } },
            {SkinContainer.GENEX_TYPES.GENEX_VERTICAL_SCROLL_HANDLE_UNPRESSED, new List<int> {56,31,28,14 } },
            {SkinContainer.GENEX_TYPES.GENEX_VERTICAL_SCROLL_HANDLE_PRESSED, new List<int> {70,31,28,14 } },
            {SkinContainer.GENEX_TYPES.GENEX_HORIZONTAL_SCROLL_HANDLE_UNPRESSED, new List<int> {84,31,14,28 } },
            {SkinContainer.GENEX_TYPES.GENEX_HORIZONTAL_SCROLL_HANDLE_PRESSED, new List<int> {84,45,14,28 } },
        };

        Dictionary<SkinContainer.PLAYLIST_TYPES, List<int>> playlistdata = new Dictionary<SkinContainer.PLAYLIST_TYPES, List<int>>
        {
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_TILE, new List<int> {127,21,25,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_LEFT_CORNER, new List<int> {0,21,25,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TITLE_BAR, new List<int> {26,21,100,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_RIGHT_CORNER, new List<int> {153,21,25,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_TILE_SELECTED, new List<int> {127,0,25,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_LEFT_SELECTED, new List<int> {0,0,25,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TITLE_BAR_SELECTED, new List<int> {26,0,100,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_TOP_RIGHT_CORNER_SELECTED, new List<int> {153,0,25,20 } },
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_LEFT_TILE, new List<int> {0,42,12,29}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_RIGHT_TILE, new List<int> {31,42,20,29}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_BOTTOM_TILE, new List<int> {179,0,25,38}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_BOTTOM_LEFT_CORNER, new List<int> {0,72,125,38}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_BOTTOM_RIGHT_CORNER, new List<int> {126,72,150,38}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_VISUALIZER_BACKGROUND, new List<int> {205,0,75,38}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SHADE_BACKGROUND, new List<int> {72,57,25,14}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SHADE_BACKGROUND_LEFT, new List<int> {72,42,25,14}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SHADE_BACKGROUND_RIGHT, new List<int> {99,57,50,14}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SHADE_BACKGROUND_RIGHT_SELECTED, new List<int> {99,42,50,14}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SCROLL_HANDLE_SELECTED, new List<int> {61,53,8,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SCROLL_HANDLE, new List<int> {52,53,8,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_URL, new List<int> {0,111,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_URL_SELECTED, new List<int> {23,111,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_DIR, new List<int> {0,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_DIR_SELECTED, new List<int> {23,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_FILE, new List<int> {0,149,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_FILE_SELECTED, new List<int> {23,149,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_ALL, new List<int> {54,111,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_ALL_SELECTED, new List<int> {77,111,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_CROP, new List<int> {54,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_CROP_SELECTED, new List<int> {77,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_SELECTED, new List<int> {54,149,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_SELECTED_SELECTED, new List<int> {77,149,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_MISC, new List<int> {54,168,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_MISC_SELECTED, new List<int> {77,168,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_INVERT_SELECTION, new List<int> {104,111,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_INVERT_SELECTION_SELECTED, new List<int> {127,111,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SELECT_ZERO, new List<int> {104,130,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_SELECT_ZERO_SELECTED, new List<int> {127,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SELECT_ALL, new List<int> {104,149,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_SELECT_ALL_SELECTED, new List<int> {127,149,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SORT_LIST, new List<int> {154,111,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_SORT_LIST_SELECTED, new List<int> {177,111,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_FILE_INFO, new List<int> {154,130,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_FILE_INFO_SELECTED, new List<int> {177,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_MISC_OPTIONS, new List<int> {154,149,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_MISC_OPTIONS_SELECTED, new List<int> {177,149,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_NEW_LIST, new List<int> {204,111,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_NEW_LIST_SELECTED, new List<int> {227,111,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SAVE_LIST, new List<int> {204,130,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_SAVE_LIST_SELECTED, new List<int> {227,130,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_LOAD_LIST, new List<int> {204,149,22,18}},
            {SkinContainer.PLAYLIST_TYPES.PLAYLIST_LOAD_LIST_SELECTED, new List<int> {227,149,22,18}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_ADD_MENU_BAR, new List<int> {48,111,3,54}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_REMOVE_MENU_BAR, new List<int> {100,111,3,72}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_SELECT_MENU_BAR, new List<int> {150,111,3,54}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_MISC_MENU_BAR, new List<int> {200,111,3,54}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_LIST_BAR, new List<int> {250,111,3,54}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_CLOSE_SELECTED, new List<int> {52,42,9,9}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_COLLAPSE_SELECTED, new List<int> {62,42,9,9}},
            { SkinContainer.PLAYLIST_TYPES.PLAYLIST_EXPAND_SELECTED, new List<int> {150,42,9,9}}
        };

        public void LoadSkin(string filename)
        {
            //Load the background image
            this.Background = new ImageBrush(ExtractBitmapFromZip(filename, "main.bmp"));

            SkinContainer.TITLEBAR.Clear();
            SkinContainer.CBUTTONS.Clear();
            SkinContainer.FONT.Clear();
            SkinContainer.GENEX.Clear();
            SkinContainer.NUMBERS.Clear();
            SkinContainer.PLAYLIST.Clear();
            SkinContainer.EQUALIZER.Clear();
            SkinContainer.VISCOLOR.Clear();

            List<string> visdata = ExtractViscolorFromZip(filename, "viscolor.txt");
            SkinContainer.VISCOLOR.Add(SkinContainer.VISCOLOR_TYPES.VISCOLOR_BACKGROUND, GetColor(visdata[(int)SkinContainer.VISCOLOR_TYPES.VISCOLOR_BACKGROUND]));
            SkinContainer.VISCOLOR.Add(SkinContainer.VISCOLOR_TYPES.VISCOLOR_PEAKS, GetColor(visdata[(int)SkinContainer.VISCOLOR_TYPES.VISCOLOR_PEAKS]));

            //Titlebar and buttons
            Image img = ExtractImageFromZip(filename, "titlebar.bmp");
            foreach (KeyValuePair<SkinContainer.TITLEBAR_TYPES, List<int>> data in titlebardata)
            {
                Bitmap bitmap = new Bitmap(data.Value.ElementAt(2), data.Value.ElementAt(3));
                var graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, data.Value.ElementAt(2), data.Value.ElementAt(3)), new Rectangle(data.Value.ElementAt(0), data.Value.ElementAt(1), data.Value.ElementAt(2), data.Value.ElementAt(3)), GraphicsUnit.Pixel);
                SkinContainer.TITLEBAR.Add(data.Key, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions())));
                bitmap.Dispose();
            }

            //CBUTTONS
            img = ExtractImageFromZip(filename, "CBUTTONS.BMP");
            foreach (KeyValuePair<SkinContainer.CBUTTON_TYPES, List<int>> data in cbuttonsdata)
            {
                Bitmap bitmap = new Bitmap(data.Value.ElementAt(2), data.Value.ElementAt(3));
                var graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, data.Value.ElementAt(2), data.Value.ElementAt(3)), new Rectangle(data.Value.ElementAt(0), data.Value.ElementAt(1), data.Value.ElementAt(2), data.Value.ElementAt(3)), GraphicsUnit.Pixel);
                SkinContainer.CBUTTONS.Add(data.Key, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions())));
                bitmap.Dispose();
            }

            //Numbers
            img = ExtractImageFromZip(filename, "numbers.bmp");
            if(img == null)
                img = ExtractImageFromZip(filename, "nums_ex.bmp");
            foreach (KeyValuePair<SkinContainer.NUMBER_TYPES, List<int>> data in numbersdata)
            {
                Bitmap bitmap = new Bitmap(data.Value.ElementAt(2), data.Value.ElementAt(3));
                var graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, data.Value.ElementAt(2), data.Value.ElementAt(3)), new Rectangle(data.Value.ElementAt(0), data.Value.ElementAt(1), data.Value.ElementAt(2), data.Value.ElementAt(3)), GraphicsUnit.Pixel);
                SkinContainer.NUMBERS.Add(data.Key, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions())));
                bitmap.Dispose();
            }

            //Transportbar
            img = ExtractImageFromZip(filename, "posbar.bmp");
            this.PlayBar_Background.Fill = ExtractImage(img, 248, 9, 0, 0);
            Application.Current.Resources["MAIN_POSITION_SLIDER_THUMB"] = ExtractImage(img, 27, 9, 249, 0);
            Application.Current.Resources["MAIN_POSITION_SLIDER_THUMB_SELECTED"] = ExtractImage(img, 28, 9, 278, 0);

            img = ExtractImageFromZip(filename, "volume.bmp");
            {
                Bitmap bitmap = new Bitmap(65, 12);
                var graphics = Graphics.FromImage(bitmap);
                using (SolidBrush brush = new SolidBrush(SkinContainer.VISCOLOR[SkinContainer.VISCOLOR_TYPES.VISCOLOR_BACKGROUND]))
                {
                    graphics.FillRectangle(brush, 0, 0, 65, 12);
                }
                Application.Current.Resources["MAIN_VOLUME_BACKGROUND"] = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
            }

            //pledit
            img = ExtractImageFromZip(filename, "pledit.bmp");
            foreach (KeyValuePair<SkinContainer.PLAYLIST_TYPES, List<int>> data in playlistdata)
            {
                Bitmap bitmap = new Bitmap(data.Value.ElementAt(2), data.Value.ElementAt(3));
                var graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, data.Value.ElementAt(2), data.Value.ElementAt(3)), new Rectangle(data.Value.ElementAt(0), data.Value.ElementAt(1), data.Value.ElementAt(2), data.Value.ElementAt(3)), GraphicsUnit.Pixel);
                SkinContainer.PLAYLIST.Add(data.Key, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions())));
                bitmap.Dispose();
            }

            //The "Font"
            img = ExtractImageFromZip(filename, "text.bmp");
            {
                Bitmap bitmap = new Bitmap(5,6);
                var graphics = Graphics.FromImage(bitmap);
                //Top Row
                for (int i = 0; i < 29; i++)
                {
                    graphics.DrawImage(img, new Rectangle(0, 0, 5, 6), new Rectangle(i*5, 0, 5, 6), GraphicsUnit.Pixel);
                    if (65+i <= 90)
                    {
                        SkinContainer.FONT.Add(65 + i, new Bitmap(bitmap));
                        SkinContainer.FONT.Add(97 + i, new Bitmap(bitmap));
                    }
                    else if(i==26 || i== 27)
                    {
                        SkinContainer.FONT.Add(((i == 26)? 34:64), new Bitmap(bitmap));
                    }
                    else
                    {
                        SkinContainer.FONT.Add(32, new Bitmap(bitmap));
                    }
                }
                //Numbers
                for (int i = 0; i < 10; i++)
                {
                    graphics.DrawImage(img, new Rectangle(0, 0, 5, 6), new Rectangle(i * 5, 6, 5, 6), GraphicsUnit.Pixel);
                    SkinContainer.FONT.Add(48 + i, new Bitmap(bitmap));
                }
                bitmap.Dispose();
            }

            //Buttons / or similar
            img = ExtractImageFromZip(filename, "genex.bmp");
            if (img != null)
            {
                foreach (KeyValuePair<SkinContainer.GENEX_TYPES, List<int>> data in genexdata)
                {
                    Bitmap bitmap = new Bitmap(data.Value.ElementAt(2), data.Value.ElementAt(3));
                    var graphics = Graphics.FromImage(bitmap);
                    graphics.DrawImage(img, new Rectangle(0, 0, data.Value.ElementAt(2), data.Value.ElementAt(3)), new Rectangle(data.Value.ElementAt(0), data.Value.ElementAt(1), data.Value.ElementAt(2), data.Value.ElementAt(3)), GraphicsUnit.Pixel);
                    SkinContainer.GENEX.Add(data.Key, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                                    Int32Rect.Empty,
                                                                                    BitmapSizeOptions.FromEmptyOptions())));
                    bitmap.Dispose();
                }
            }
            else
            {
                List<int> data = genexdata[SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED];
                Bitmap bitmap = new Bitmap(data.ElementAt(2)-2, data.ElementAt(3)-2);
                var graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(SkinContainer.FONT[68], 0, 0);
                SkinContainer.GENEX.Add(SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                                Int32Rect.Empty,
                                                                                BitmapSizeOptions.FromEmptyOptions())));
                SkinContainer.GENEX.Add(SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_PRESSED, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                                Int32Rect.Empty,
                                                                                BitmapSizeOptions.FromEmptyOptions())));
                graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(SkinContainer.FONT[85], 0, 0);
                SkinContainer.GENEX.Add(SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_UNPRESSED, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                                Int32Rect.Empty,
                                                                                BitmapSizeOptions.FromEmptyOptions())));
                SkinContainer.GENEX.Add(SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_PRESSED, new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                                                                                Int32Rect.Empty,
                                                                                BitmapSizeOptions.FromEmptyOptions())));

                SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_UNPRESSED].Stretch = Stretch.UniformToFill;
                SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED].Stretch = Stretch.UniformToFill;
                bitmap.Dispose();
            }
            ApplySkin();
        }


        private void ApplySkin()
        {
            this.TitleBar.Fill = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_TITLE_BAR];
            this.Settings_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_OPTIONS_BUTTON];
            this.Close_Button.Background = SkinContainer.TITLEBAR[SkinContainer.TITLEBAR_TYPES.MAIN_CLOSE_BUTTON];

            this.Prev_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PREVIOUS_BUTTON];
            this.Play_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PLAY_BUTTON];
            this.Pause_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_PAUSE_BUTTON];
            this.Stop_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_STOP_BUTTON];
            this.Next_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_NEXT_BUTTON];
            this.Load_Button.Background = SkinContainer.CBUTTONS[SkinContainer.CBUTTON_TYPES.MAIN_EJECT_BUTTON];

            this.Second_First.Fill = SkinContainer.NUMBERS[0];
            this.Second_Last.Fill = SkinContainer.NUMBERS[0];
            this.Minutes_First.Fill = SkinContainer.NUMBERS[0];
            this.Minutes_Last.Fill = SkinContainer.NUMBERS[0];

            TrackDown_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_LEFT_UNPRESSED];
            TrackUp_Button.Background = SkinContainer.GENEX[SkinContainer.GENEX_TYPES.GENEX_SCROLL_RIGHT_UNPRESSED];
        }

        Image ExtractImageFromZip(string archivename, string imagename)
        {
            var zip = ZipFile.OpenRead(@archivename);
            var ent = zip.Entries;
            string regex = @"\b(" + imagename + @")\b";
            foreach (var entry in ent)
            {
                if (Regex.IsMatch(entry.Name, regex, RegexOptions.IgnoreCase))
                {
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        using (var memoryStream = new MemoryStream())
                        {
                            zipStream.CopyTo(memoryStream);
                            memoryStream.Position = 0;
                            var image = Image.FromStream(memoryStream);
                            memoryStream.Close();
                            zipStream.Close();
                            return image;
                        }
                    }
                }
            }
            return null;
        }

        BitmapImage ExtractBitmapFromZip(string archivename, string imagename)
        {
            var zip = ZipFile.OpenRead(@archivename);
            var ent = zip.Entries;
            string regex = @"\b(" + imagename + @")\b";
            foreach (var entry in ent)
            {
                if (Regex.IsMatch(entry.Name, regex, RegexOptions.IgnoreCase))
                {
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        using (var memoryStream = new MemoryStream())
                        {
                            zipStream.CopyTo(memoryStream);
                            memoryStream.Position = 0;
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = memoryStream;
                            bitmap.EndInit();
                            memoryStream.Close();
                            zipStream.Close();
                            return bitmap;
                        }
                    }
                }
            }
            return null;
        }

        List<string> ExtractViscolorFromZip(string archivename, string imagename)
        {
            var zip = ZipFile.OpenRead(@archivename);
            var ent = zip.Entries;
            string regex = @"\b(" + imagename + @")\b";
            foreach (var entry in ent)
            {
                if (Regex.IsMatch(entry.Name, regex, RegexOptions.IgnoreCase))
                {
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        using (var memoryStream = new MemoryStream())
                        {
                            zipStream.CopyTo(memoryStream);
                            memoryStream.Position = 0;
                            var data = new List<string>();
                            using (var reader = new StreamReader(memoryStream, Encoding.ASCII))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    data.Add(line);
                                }
                            }
                            memoryStream.Close();
                            zipStream.Close();
                            return data;
                        }
                    }
                }
            }
            return null;
        }

        ImageBrush ExtractImage(Image img, int x, int y, int offset_x, int offset_y)
        {
            Bitmap p = new Bitmap(x, y);
            var graphics = Graphics.FromImage(p);
            graphics.DrawImage(img, new Rectangle(0, 0, x, y), new Rectangle(offset_x, offset_y, x, y), GraphicsUnit.Pixel);
            graphics.Dispose();
            return new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(p.GetHbitmap(), IntPtr.Zero,
                                                                                        Int32Rect.Empty,
                                                                                        BitmapSizeOptions.FromEmptyOptions()));
        }

        System.Drawing.Color GetColor(string data)
        {
            byte[] colval = new byte[3];
            string[] numbers = Regex.Split(data, @"\D+");
            int idx = 0;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (idx >= 3)
                        break;
                    int i = int.Parse(value);
                    colval[idx] = ((byte)int.Parse(value));
                    idx++;
                }
            }
            return System.Drawing.Color.FromArgb(colval[0], colval[1], colval[2]);
        }
    }
}
