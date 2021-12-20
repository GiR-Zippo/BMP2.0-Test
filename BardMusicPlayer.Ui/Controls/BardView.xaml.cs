using BardMusicPlayer.Maestro;
using BardMusicPlayer.Maestro.Events;
using BardMusicPlayer.Seer;
using BardMusicPlayer.Seer.Events;
using BardMusicPlayer.Ui.Functions;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace BardMusicPlayer.Ui.Controls
{
    /// <summary>
    /// Interaktionslogik für BardView.xaml
    /// </summary>
    public partial class BardView : UserControl
    {
        public BardView()
        {
            InitializeComponent();
            this.DataContext = this;
            Bards = new ObservableCollection<Game>();

            BmpMaestro.Instance.OnPerformerChanged += OnPerfomerChanged;
            BmpMaestro.Instance.OnTrackNumberChanged += OnTrackNumberChanged;
            BmpSeer.Instance.PlayerNameChanged += OnPlayerNameChanged;
            BmpSeer.Instance.InstrumentHeldChanged += OnInstrumentHeldChanged;
            BmpSeer.Instance.HomeWorldChanged += OnHomeWorldChanged;
        }

        public ObservableCollection<Game> Bards { get; private set; }

        public Game SelectedBard { get; set; }

        private void OnPerfomerChanged(object sender, bool e)
        {
            this.Bards = new ObservableCollection<Game>(BmpMaestro.Instance.GetPerformer());
            this.Dispatcher.BeginInvoke(new Action(() => this.BardsList.ItemsSource = Bards));
        }

        private void OnTrackNumberChanged(object sender, TrackNumberChangedEvent e)
        {
            
        }

        private void OnPlayerNameChanged(PlayerNameChanged e)
        {
            UpdateList();
        }

        private void OnHomeWorldChanged(HomeWorldChanged e)
        {
            UpdateList();
        }

        private void OnInstrumentHeldChanged(InstrumentHeldChanged e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            this.Bards = new ObservableCollection<Game>(BmpMaestro.Instance.GetPerformer());
            this.Dispatcher.BeginInvoke(new Action(() => this.BardsList.ItemsSource = Bards));
        }

        private void BardsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(this.BardsList.SelectedItem);
        }

        /* Track UP/Down */
        private int _numValue = 1;
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                PlaybackFunctions.SetTrackNumber(_numValue);
                return;
            }
        }
        private void track_cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
            var tb = sender as Button;
            var spn = tb.Parent as Grid;
            var grd = spn.Parent as Grid;
            var num = grd.Children[0] as TextBox;
            num.Text = "T" + NumValue.ToString();
        }

        private void track_cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue == 0)
                return;
            NumValue--;
            var tb = sender as Button;
            var spn = tb.Parent as Grid;
            var grd = spn.Parent as Grid;
            var num = grd.Children[0] as TextBox;
            num.Text = "T" + NumValue.ToString();
        }
    }
}
