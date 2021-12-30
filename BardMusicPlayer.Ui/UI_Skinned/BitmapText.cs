﻿using BardMusicPlayer.Ui.Globals.SkinContainer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BardMusicPlayer.Ui.Skinned
{
    public partial class Skinned_MainView : UserControl
    {
        /*void WriteTrackField(string data)
        {
            Bitmap bitmap = new Bitmap(50, 6);
            var graphics = Graphics.FromImage(bitmap);
            int index = 0;
            foreach (var a in data)
            {
                graphics.DrawImage(SkinContainer.FONT[a], 5 * index, 0);
                index++;
            }
            //TrackDigit.Source = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())).ImageSource;
        }*/

        void WriteSmallDigitField(string data)
        {
            data = data.Insert(0, "T");
            data = data.Insert(1, ":");

            Bitmap bitmap = new Bitmap(30, 8);
            var graphics = Graphics.FromImage(bitmap);
            int index = 0;
            foreach (var a in data)
            {
                System.Drawing.Image img;
                if (SkinContainer.FONT.ContainsKey(a))
                    img = SkinContainer.FONT[a];
                else
                    img = SkinContainer.FONT[32];
                graphics.DrawImage(img, 5 * index, 0);
                index++;
            }
            SmallTrackDigit.Source = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())).ImageSource;
            SmallTrackDigit.Stretch = Stretch.UniformToFill;
        }

        private int scrollpos = 0;
        private bool scrolldir = true;
        protected async Task UpdateScroller(CancellationToken stoppingToken, string data)
        {
            string songname = data ;
            scrollpos = 0;
            scrolldir = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                WriteSongField(songname);
                await Task.Delay(500, stoppingToken);
            }
        }

        void WriteSongField(string data)
        {
            Bitmap bitmap = new Bitmap(305, 12);
            var graphics = Graphics.FromImage(bitmap);
            for (int i = 0; i < 33; i++)
            {
                System.Drawing.Image img;
                char a = ' ';

                if ((scrollpos == -1) && !scrolldir)
                    scrolldir = true;
                else
                {
                    if (i + scrollpos >= data.Length)
                        scrolldir = false;
                    else
                        a = data.ToArray()[i + scrollpos];
                }

                if (SkinContainer.FONT.ContainsKey(a))
                    img = SkinContainer.FONT[a];
                else
                    img = SkinContainer.FONT[32];
                graphics.DrawImage(img, 5 * i, 0);
            }
            if (scrolldir)
                scrollpos++;
            else
                scrollpos--;
            SongDigit.Source = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())).ImageSource;
            SongDigit.Stretch = Stretch.UniformToFill;
        }

        void WriteInstrumentDigitField(string data)
        {
            if (data == null)
                return;
            Bitmap bitmap = new Bitmap(110, 8);
            var graphics = Graphics.FromImage(bitmap);
            int index = 0;
            foreach (var a in data)
            {
                System.Drawing.Image img;
                if (SkinContainer.FONT.ContainsKey(a))
                    img = SkinContainer.FONT[a];
                else
                    img = SkinContainer.FONT[32];
                graphics.DrawImage(img, 5 * index, 0);
                index++;
            }
            InstrumentDigit.Source = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())).ImageSource;
        }
    }
}

