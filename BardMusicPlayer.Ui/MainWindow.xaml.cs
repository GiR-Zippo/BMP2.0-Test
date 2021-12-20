﻿using BardMusicPlayer.Ui.Skinned;
using BardMusicPlayer.Ui.Classic;
using System.Windows;

namespace BardMusicPlayer.Ui
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Globals.Settings.LoadConfig();
            //SwitchClassicStyle();
            SwitchSkinnedStyle();
        }

        public void SwitchClassicStyle()
        {
            this.DataContext = new Classic_MainView();
            this.AllowsTransparency = false;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.Height = 500;
            this.Width = 830;
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
        }

        public void SwitchSkinnedStyle()
        {
            this.DataContext = new Skinned_MainView();
            this.AllowsTransparency = true;
            this.Height = 174;
            this.Width = 412;
            this.ResizeMode = ResizeMode.NoResize;
        }
    }
}
