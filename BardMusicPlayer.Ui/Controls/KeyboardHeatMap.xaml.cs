using BardMusicPlayer.Transmogrify.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace BardMusicPlayer.Ui.Controls
{
    public struct NoteRectInfo
    {
        public NoteRectInfo(string n, bool blk, int freq )
        {
            name = n;
            black_key = blk;
            frequency = freq;
        }
        public string name;
        public bool black_key;
        public int frequency;
    };

    /// <summary>
    /// Interaktionslogik für KeyboardHeatMap.xaml
    /// </summary>
    public partial class KeyboardHeatMap : UserControl
    {
        //note frequencies
        private int mOctave = 4;    // default octave (octaves can be from 1 to 7)

        Dictionary<int, NoteRectInfo> noteInfo = new Dictionary<int, NoteRectInfo>
        {
            {  0, new NoteRectInfo("C", false, 60) },
            {  1, new NoteRectInfo("CSharp", true, 61) },
            {  2, new NoteRectInfo("D", false, 62) },
            {  3, new NoteRectInfo("DSharp", true, 63) },
            {  4, new NoteRectInfo("E", false, 64) },
            {  5, new NoteRectInfo("F", false, 65) },
            {  6, new NoteRectInfo("FSharp", true, 66) },
            {  7, new NoteRectInfo("G", false, 67) },
            {  8, new NoteRectInfo("GSharp", true, 68) },
            {  9, new NoteRectInfo("A", false, 69) },
            {  10, new NoteRectInfo("ASharp", true, 70) },
            {  11, new NoteRectInfo("B", false, 71) },

            {  12, new NoteRectInfo("C1", false, 60) },
            {  13, new NoteRectInfo("CSharp1", true, 61) },
            {  14, new NoteRectInfo("D1", false, 62) },
            {  15, new NoteRectInfo("DSharp1", true, 63) },
            {  16, new NoteRectInfo("E1", false, 64) },
            {  17, new NoteRectInfo("F1", false, 65) },
            {  18, new NoteRectInfo("FSharp1", true, 66) },
            {  19, new NoteRectInfo("G1", false, 67) },
            {  20, new NoteRectInfo("GSharp1", true, 68) },
            {  21, new NoteRectInfo("A1", false, 69) },
            {  22, new NoteRectInfo("ASharp1", true, 70) },
            {  23, new NoteRectInfo("B1", false, 71) },

            {  24, new NoteRectInfo("C2", false, 60) },
            {  25, new NoteRectInfo("CSharp2", true, 61) },
            {  26, new NoteRectInfo("D2", false, 62) },
            {  27, new NoteRectInfo("DSharp2", true, 63) },
            {  28, new NoteRectInfo("E2", false, 64) },
            {  29, new NoteRectInfo("F2", false, 65) },
            {  30, new NoteRectInfo("FSharp2", true, 66) },
            {  31, new NoteRectInfo("G2", false, 67) },
            {  32, new NoteRectInfo("GSharp2", true, 68) },
            {  33, new NoteRectInfo("A2", false, 69) },
            {  34, new NoteRectInfo("ASharp2", true, 70) },
            {  35, new NoteRectInfo("B2", false, 71) },

            {  36, new NoteRectInfo("C3", false, 72) }
        };


        private List<Rectangle> mRects = new List<Rectangle>();
        private List<Rectangle> mBlackRects = new List<Rectangle>();

        private bool mLoaded = false;

        public KeyboardHeatMap()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Inits the Ui
        /// </summary>
        public void InitUi()
        {
            initUI();
        }

        public int getOctave() { return mOctave; }


        #region UI handling *************************************************************************************


        private Dictionary<int, double> getNoteCountForKey(BmpSong song, int tracknumber)
        {
            var midiFile = song.GetProcessedMidiFile().Result;
            var trackChunks = midiFile.GetTrackChunks().ToList();
            var notedict = new Dictionary<int, int>();
            foreach (var note in trackChunks[tracknumber-1].GetNotes())
            {
                int noteNum = note.NoteNumber;
                noteNum -= 48;
                int count = 1;
                if (notedict.ContainsKey(noteNum))
                {
                    notedict.TryGetValue(noteNum, out count);
                    count++;
                    notedict.Remove(noteNum);
                }
                if (noteNum >= 0)
                    notedict.Add(noteNum, count);
            }

            int t = trackChunks[tracknumber - 1].GetNotes().Count;
            var result = new Dictionary<int, double>();
            foreach (var note in notedict)
            {
                double f = ((double)note.Value / (double)t) * 100;
                result.Add(note.Key, (int)f);
            }
            return result;
        }

        /// <summary>
        /// Init the keyboard rects and the Grid constraints
        /// </summary>
        public void initUI(BmpSong song = null, int tracknumber = -1)
        {
            Dictionary<int, double> noteCountDict = null;
            if (song != null)
                noteCountDict = getNoteCountForKey(song, tracknumber);

            mLoaded = false;
            grd.Width = this.ActualWidth;
            grd.Height = this.ActualHeight;
            grd.HorizontalAlignment = HorizontalAlignment.Left;
            grd.VerticalAlignment = VerticalAlignment.Top;
            grd.ShowGridLines = false;
            grd.Background = new SolidColorBrush(Colors.White);

            grd.RowDefinitions.Clear();
            grd.ColumnDefinitions.Clear();

            List<string> nnames = new List<string>();
            int max_col = 0;
            foreach (var i in noteInfo)
            {
                if (!i.Value.black_key)
                {
                    max_col++;
                    nnames.Add(i.Value.name);
                }
            }

            for (int j = 0; j < max_col; j++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grd.ColumnDefinitions.Add(col);
            }
            
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            grd.RowDefinitions.Add(row);

            int noteindex = 0;
            for (int i = 0; i < max_col; i++)
            {
                Rectangle rect = new Rectangle();
                foreach (var n in noteInfo)
                {
                    if (n.Value.name.Equals(nnames[i]))
                    {
                        noteindex = n.Key;
                        break;
                    }
                }
                rect.Fill = new SolidColorBrush(Colors.Ivory);
                rect.Height = this.Height;
                rect.Margin = new Thickness(0, 0, 0, 0);
                rect.StrokeThickness = 1;
                rect.Stroke = new SolidColorBrush(Colors.Black);
                rect.MouseLeftButtonDown += rect_MouseLeftButtonDown;
                rect.MouseLeftButtonUp += rect_MouseLeftButtonUp;
                rect.MouseEnter += rect_MouseEnter;
                rect.MouseLeave += rect_MouseLeave;

                if (song != null)
                    rect.Fill = FullNoteFill(noteindex, noteCountDict);

                rect.Name = nnames[i];
                Grid.SetRow(rect, 0);
                Grid.SetColumn(rect, i);

                grd.Children.Add(rect);
                mRects.Add(rect);
            }

            initBlkUI(song, tracknumber);
            mLoaded = true;

        }

        public void initBlkUI(BmpSong song = null, int tracknumber = -1)
        {
            Dictionary<int, double> noteCountDict = null;
            if (song != null)
                noteCountDict = getNoteCountForKey(song, tracknumber);

            blk_grd.Width = this.ActualWidth;
            blk_grd.Height = this.ActualHeight/1.7;
            blk_grd.HorizontalAlignment = HorizontalAlignment.Left;
            blk_grd.VerticalAlignment = VerticalAlignment.Top;
            blk_grd.ShowGridLines = false;
            blk_grd.Background = new SolidColorBrush(Colors.Transparent);


            blk_grd.RowDefinitions.Clear();
            blk_grd.ColumnDefinitions.Clear();

            int max_col = 0;
            foreach (var i in noteInfo)
            {
                if (!i.Value.black_key)
                    max_col++;
            }

            for (int j = 0; j < max_col; j++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                blk_grd.ColumnDefinitions.Add(col);
            }

            var f = this.ActualWidth;
            blk_grd.Margin = new Thickness(-((f/ max_col-1) /2), 0, 5, 0);


            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            blk_grd.RowDefinitions.Add(row);

            int noteindex = 0;
            int gridCol = 0;
            // white keys interspersed with black keys

                foreach (var i in noteInfo)
                {
                    if (i.Value.name.Equals("F"))
                        gridCol++;
                    if (i.Value.name.Equals("C"))
                        gridCol++;
                    if (i.Value.name.Equals("F1"))
                        gridCol++;
                    if (i.Value.name.Equals("C1"))
                        gridCol++;
                    if (i.Value.name.Equals("F2"))
                        gridCol++;
                    if (i.Value.name.Equals("C2"))
                        gridCol++;

                if (i.Value.black_key)
                {
                    noteindex = i.Key;

                    Rectangle rect = new Rectangle();
                    rect.Fill = new SolidColorBrush(Colors.Black);
                    rect.Height = this.Height;
                    rect.Margin = new Thickness(0, 0, 0, 0);
                    rect.StrokeThickness = 1;
                    rect.Stroke = new SolidColorBrush(Colors.Ivory);
                    rect.MouseLeftButtonDown += rect_MouseLeftButtonDown;
                    rect.MouseLeftButtonUp += rect_MouseLeftButtonUp;
                    rect.MouseEnter += rect_MouseEnter;
                    rect.MouseLeave += rect_MouseLeave;
                    rect.Name = i.Value.name;

                    if (song != null)
                        rect.Fill = HalfNoteFill(noteindex, noteCountDict);
                    Grid.SetRow(rect, 0);
                    Grid.SetColumn(rect, gridCol);
                    blk_grd.Children.Add(rect);
                    mRects.Add(rect);
                    gridCol++;
                }
            }
            mLoaded = true;
        }

        private LinearGradientBrush FullNoteFill(int noteindex, Dictionary<int, double> noteCountDict)
        {
            double noteCount;
            if (noteCountDict.TryGetValue(noteindex, out noteCount))
                noteCount = ((double)10 - noteCount) / (double)8;
            else
                noteCount = 1;

            LinearGradientBrush fiveColorLGB = new LinearGradientBrush();
            fiveColorLGB.StartPoint = new Point(0, 0);
            fiveColorLGB.EndPoint = new Point(1, 1);

            GradientStop stop1 = new GradientStop();
            stop1.Color = Colors.Ivory;
            stop1.Offset = 0.0;
            fiveColorLGB.GradientStops.Add(stop1);

            GradientStop stop2 = new GradientStop();
            stop2.Color = Colors.Ivory;
            stop2.Offset = noteCount;
            fiveColorLGB.GradientStops.Add(stop2);

            GradientStop stop3 = new GradientStop();
            stop3.Color = Colors.Red;
            stop3.Offset = noteCount;
            fiveColorLGB.GradientStops.Add(stop3);

            GradientStop stop4 = new GradientStop();
            stop4.Color = Colors.Red;
            stop4.Offset = 1.0;
            fiveColorLGB.GradientStops.Add(stop4);

            return fiveColorLGB;
        }

        private LinearGradientBrush HalfNoteFill(int noteindex, Dictionary<int, double> noteCountDict)
        {
            double noteCount;
            if (noteCountDict.TryGetValue(noteindex, out noteCount))
                noteCount = ((double)10 - noteCount) / (double)10;
            else
                noteCount = 1;

            LinearGradientBrush fiveColorLGB = new LinearGradientBrush();
            fiveColorLGB.StartPoint = new Point(0, 0);
            fiveColorLGB.EndPoint = new Point(1, 1);

            GradientStop stop1 = new GradientStop();
            stop1.Color = Colors.Black;
            stop1.Offset = 0.0;
            fiveColorLGB.GradientStops.Add(stop1);

            GradientStop stop2 = new GradientStop();
            stop2.Color = Colors.Black;
            stop2.Offset = noteCount;
            fiveColorLGB.GradientStops.Add(stop2);

            GradientStop stop3 = new GradientStop();
            stop3.Color = Colors.Yellow;
            stop3.Offset = noteCount;
            fiveColorLGB.GradientStops.Add(stop3);

            GradientStop stop4 = new GradientStop();
            stop4.Color = Colors.Yellow;
            stop4.Offset = 1.0;
            fiveColorLGB.GradientStops.Add(stop4);

            return fiveColorLGB;
        }

        /// <summary>
        /// Check if a rectangle is Black key
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private bool isBlackRect(Rectangle r)
        {
            bool retVal = false;
            mBlackRects.ForEach((elem) =>
            {
                if (elem == r)
                {
                    retVal = true;
                }
            });
            return retVal;
        }

        /// <summary>
        /// get name of black rectangle(key) by name and octave
        /// </summary>
        /// <param name="name"></param>
        /// <param name="octave"></param>
        /// <returns></returns>
        private Rectangle getBlackKeyByName(string name, int octave)
        {
            foreach (Rectangle r1 in mBlackRects)
            {
                if (r1.Name == name && (int)r1.Tag == octave)
                {
                    return r1;
                }
            }
            return null;
        }


        /// <summary>
        /// Force the redraw of the keys when the container resizes
        /// </summary>
        public void resizeUI()
        {
            if (!mLoaded)
                return;
            grd.Width = this.ActualWidth;
            grd.Height = this.ActualHeight;

            blk_grd.Width = this.ActualWidth;
            blk_grd.Height = this.ActualHeight / 1.7;

            var f = this.ActualWidth;
            blk_grd.Margin = new Thickness(-((f / blk_grd.ColumnDefinitions.Count - 1) / 2), 0, 5, 0);
        }

        /// <summary>
        /// Highlight key as it is pressed
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="isBlack"></param>
        public void highlightKey(Rectangle rect, bool isBlack)
        {
            /*if (!isBlack)
                rect.Fill = new SolidColorBrush(Colors.Yellow);
            else
            {
                rect.Fill = new SolidColorBrush(Colors.DarkGray);
                Rectangle rectPair = null;
                if (rect.Name == CSHARPKEY1)
                    rectPair = getBlackKeyByName(CSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == CSHARPKEY2)
                    rectPair = getBlackKeyByName(CSHARPKEY1, (int)rect.Tag);

                if (rect.Name == DSHARPKEY1)
                    rectPair = getBlackKeyByName(DSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == DSHARPKEY2)
                    rectPair = getBlackKeyByName(DSHARPKEY1, (int)rect.Tag);

                if (rect.Name == FSHARPKEY1)
                    rectPair = getBlackKeyByName(FSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == FSHARPKEY2)
                    rectPair = getBlackKeyByName(FSHARPKEY1, (int)rect.Tag);

                if (rect.Name == GSHARPKEY1)
                    rectPair = getBlackKeyByName(GSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == GSHARPKEY2)
                    rectPair = getBlackKeyByName(GSHARPKEY1, (int)rect.Tag);

                if (rect.Name == ASHARPKEY1)
                    rectPair = getBlackKeyByName(ASHARPKEY2, (int)rect.Tag);
                else if (rect.Name == ASHARPKEY2)
                    rectPair = getBlackKeyByName(ASHARPKEY1, (int)rect.Tag);

                if (rectPair != null)
                    rectPair.Fill = new SolidColorBrush(Colors.DarkGray);
            }*/
        }

        /// <summary>
        /// Remove highlight from key as it is stopped being pressed
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="isBlack"></param>
        public void unHighlightKey(Rectangle rect, bool isBlack)
        {
            /*if (!isBlack)
                rect.Fill = new SolidColorBrush(Colors.Ivory);
            else
            {
                rect.Fill = new SolidColorBrush(Colors.Black);

                Rectangle rectPair = null;
                if (rect.Name == CSHARPKEY1)
                    rectPair = getBlackKeyByName(CSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == CSHARPKEY2)
                    rectPair = getBlackKeyByName(CSHARPKEY1, (int)rect.Tag);

                if (rect.Name == DSHARPKEY1)
                    rectPair = getBlackKeyByName(DSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == DSHARPKEY2)
                    rectPair = getBlackKeyByName(DSHARPKEY1, (int)rect.Tag);

                if (rect.Name == FSHARPKEY1)
                    rectPair = getBlackKeyByName(FSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == FSHARPKEY2)
                    rectPair = getBlackKeyByName(FSHARPKEY1, (int)rect.Tag);

                if (rect.Name == GSHARPKEY1)
                    rectPair = getBlackKeyByName(GSHARPKEY2, (int)rect.Tag);
                else if (rect.Name == GSHARPKEY2)
                    rectPair = getBlackKeyByName(GSHARPKEY1, (int)rect.Tag);

                if (rect.Name == ASHARPKEY1)
                    rectPair = getBlackKeyByName(ASHARPKEY2, (int)rect.Tag);
                else if (rect.Name == ASHARPKEY2)
                    rectPair = getBlackKeyByName(ASHARPKEY1, (int)rect.Tag);

                if (rectPair != null)
                    rectPair.Fill = new SolidColorBrush(Colors.Black);
            }*/
        }


        /// <summary>
        /// Handle left button clicked event for a rectangle
        /// </summary>
        /// <param name="r"></param>
        private void evtLeftButtonDown(Rectangle r)
        {
            Console.WriteLine("left button down");
            highlightKey(r, isBlackRect(r));
        }

        /// <summary>
        /// Handle left button up event for a rectangle
        /// </summary>
        /// <param name="r"></param>
        private void evtLeftButtonUp(Rectangle r)
        {
            Console.WriteLine("left button up");
            unHighlightKey(r, isBlackRect(r));
        }

        /// <summary>
        /// Handle mouse leave event for a rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <param name="e"></param>
        private void evtMouseLeave(Rectangle r, MouseEventArgs e)
        {
            Console.WriteLine("mouse leave");
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                unHighlightKey(r, isBlackRect(r));
            }
        }

        /// <summary>
        /// Handle mouse entered event for a rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <param name="e"></param>
        private void evtMouseEnter(Rectangle r, MouseEventArgs e)
        {
            Console.WriteLine("mouse enter");
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                highlightKey(r, isBlackRect(r));
            }

        }

        #endregion ****************************************************************************************

        #region event handling ***************************************************************************

        private void rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            evtLeftButtonDown((Rectangle)sender);
        }

        private void rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            evtLeftButtonUp((Rectangle)sender);
        }

        private void rect_MouseLeave(object sender, MouseEventArgs e)
        {
            evtMouseLeave((Rectangle)sender, e);
        }

        private void rect_MouseEnter(object sender, MouseEventArgs e)
        {
            evtMouseEnter((Rectangle)sender, e);
        }

        private void rect_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeUI();
        }

        #endregion ********************************************************

        private void grd_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void grd_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeUI();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeUI();
        }
    }
}
