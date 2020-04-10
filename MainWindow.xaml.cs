using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

//https://ourcodeworld.com/articles/read/128/how-to-play-pause-music-or-go-to-next-and-previous-track-from-windows-using-c-valid-for-all-windows-music-players

namespace Player
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileMusic> Musics
        {
            get { return (ObservableCollection<FileMusic>)GetValue(MusicsProperty); }
            set { SetValue(MusicsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Musics.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MusicsProperty =
            DependencyProperty.Register("Musics", typeof(ObservableCollection<FileMusic>), typeof(MainWindow));

        private Playing player = null;
        private Chronometer chronometer = null;
        private FileMusic music_current = null;
        private int index = 0;

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.Musics = new ObservableCollection<FileMusic>();
            this.player = new Playing();
            this.player.Finish += new EventHandler(Finish);

            this.chronometer = new Chronometer();
            this.chronometer.Event_Tick += new EventHandler(ChroTick);
        }

        private void Finish(object sender, EventArgs e)
        {
            this.ExecuteTick();
        }

        private void ChroTick(object sender, EventArgs e)
        {
            this.lblCronometro.Text = this.chronometer.Label;
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Proxima_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Anterior_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                this.Musics.Clear();
                foreach (string file in openFileDialog.FileNames)
                {
                    FileInfo info = new FileInfo(file);
                    this.Musics.Add(new FileMusic() { Path = info.FullName, Name = info.Name });
                }
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (this.Musics.Count > 0)
            {
                if (!this.player.isPause && this.player.CanPause)
                {
                    this.chronometer.Stop();
                    this.player.Pause();
                    this.Changed(PlayTypes.Play);
                }
                else
                {
                    if (this.player.isPause)
                    {
                        this.chronometer.Start();
                        this.player.RePlay();
                        this.Changed(PlayTypes.Pause);
                    }
                    else
                    {
                        this.music_current = null;
                        this.index = 0;
                        this.Changed(PlayTypes.Pause);

                        this.ExecuteTick();
                    }
                }
            }
        }

        private enum PlayTypes
        {
            Play,
            Pause
        }

        private void Changed(PlayTypes playTypes)
        {
            if (playTypes == PlayTypes.Pause)
                this.btnPlay.Tag = System.Windows.Application.Current.FindResource("player_pause") as ImageSource;
            else
                this.btnPlay.Tag = System.Windows.Application.Current.FindResource("player_play") as ImageSource;
        }

        private void Tick(object sender, EventArgs e)
        {
            this.ExecuteTick();
        }

        private void ExecuteTick()
        {
            bool execute_again = true;
            if (this.music_current == null || this.chronometer.Elapsed.Seconds >= 10)
            {
                if (index > (this.Musics.Count - 1))
                {
                    this.Stop();
                    execute_again = false;
                }
                else
                {
                    this.music_current = this.Musics[index] as FileMusic;
                    this.chronometer.Restart();
                    this.music_current.IsPlay = true;
                    index++;
                }
            }
            if (execute_again)
                this.player.Play(music_current.Path);
        }

        private void Stop()
        {
            this.chronometer.Stop();
            this.Changed(PlayTypes.Play);
        }

        private void a(object sender, EventArgs e)
        {

        }
    }
}