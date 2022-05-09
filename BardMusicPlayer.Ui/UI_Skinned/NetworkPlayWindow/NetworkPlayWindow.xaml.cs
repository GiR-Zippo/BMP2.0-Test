﻿using BardMusicPlayer.Ui.Globals.SkinContainer;
using System;
using System.Windows;
using System.Windows.Input;
using BardMusicPlayer.Jamboree;
using BardMusicPlayer.Jamboree.Events;

namespace BardMusicPlayer.Ui.Skinned
{
    /// <summary>
    /// Interaktionslogik für BardsWindow.xaml
    /// </summary>
    public partial class NetworkPlayWindow : Window
    {
        public NetworkPlayWindow()
        {
            InitializeComponent();
            //NetEvents
            BmpJamboree.Instance.OnPartyCreated += Instance_PartyCreated;
            BmpJamboree.Instance.OnPartyDebugLog += Instance_PartyDebugLog;

            ApplySkin();
            SkinContainer.OnNewSkinLoaded += SkinContainer_OnNewSkinLoaded;
        }
#region Loading window, apply skindata and buttons
        private void SkinContainer_OnNewSkinLoaded(object sender, EventArgs e)
        { ApplySkin(); }

        public void ApplySkin()
        {
            this.NETWORK_TOP_LEFT.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_TOP_LEFT_CORNER];
            this.NETWORK_TOP_TILE.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_TOP_TILE];
            this.NETWORK_TOP_RIGHT.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_TOP_RIGHT_CORNER];

            this.NETWORK_BOTTOM_LEFT_CORNER.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_BOTTOM_LEFT_CORNER];
            this.NETWORK_BOTTOM_TILE.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_BOTTOM_TILE];
            this.NETWORK_BOTTOM_RIGHT_CORNER.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_BOTTOM_RIGHT_CORNER];

            this.NETWORK_LEFT_TILE.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_LEFT_TILE];
            this.NETWORK_RIGHT_TILE.Fill = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_RIGHT_TILE];

            this.Close_Button.Background = SkinContainer.SWINDOW[SkinContainer.SWINDOW_TYPES.SWINDOW_CLOSE_SELECTED];
            this.Close_Button.Background.Opacity = 0;

        }
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        private void Close_Button_Down(object sender, MouseButtonEventArgs e)
        {
            this.Close_Button.Background.Opacity = 1;
        }
        private void Close_Button_Up(object sender, MouseButtonEventArgs e)
        {
            this.Close_Button.Background.Opacity = 0;
        }
#endregion

        private void Instance_PartyCreated(object sender, PartyCreatedEvent e)
        {
            string Token = e.Token;
            this.Dispatcher.BeginInvoke(new Action(() => this.PartyToken_Text.Text = Token));
        }

        private void Instance_PartyDebugLog(object sender, PartyDebugLogEvent e)
        {
            string logtext = e.LogString;
            this.Dispatcher.BeginInvoke(new Action(() => this.PartyLog_Text.Text = this.PartyLog_Text.Text + logtext));
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            string token = PartyToken_Text.Text;
            PartyToken_Text.Text = "Please wait...";
            BmpJamboree.Instance.JoinParty(token, 0, "Test Player"); // BmpMaestro.Instance.GetHostGame().PlayerName);
        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            BmpJamboree.Instance.LeaveParty();
        }
        private void ForcePlay_Click(object sender, RoutedEventArgs e)
        {
            BmpJamboree.Instance.SendPerformanceStart();
        }

    }
}