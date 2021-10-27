
using BardMusicPlayer.Ui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


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

            SwitchClassicStyle();
        }

        public void SwitchClassicStyle()
        {
            this.DataContext = new Classic_MainView();
            this.AllowsTransparency = false;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.Height = 500;
            this.Width = 830;
            this.ResizeMode = ResizeMode.NoResize;
        }

        public void SwitchSkinnedStyle()
        {
            this.DataContext = new Skinned_MainView();
            this.AllowsTransparency = true;
            this.Height = 200;
            this.Width = 550;
            this.ResizeMode = ResizeMode.NoResize;
        }
    }
}
