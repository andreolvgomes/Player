using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Player
{
    public class Playing
    {
        public event EventHandler Finish;

        private void OnFinish()
        {
            if (this.Finish != null)
                this.Finish(this, EventArgs.Empty);
        }

        private MediaPlayer mediaPlayer = null;
        
        private DispatcherTimer timer = null;
        public bool isPause = false;
        public bool CanPause { get; set; }

        public Playing()
        {
            this.mediaPlayer = new MediaPlayer();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Tick;
        }

        private void Tick(object sender, EventArgs e)
        {
            this.TickExecuting();
        }

        private void TickExecuting()
        {
            if (mediaPlayer.NaturalDuration != System.Windows.Duration.Automatic)
            {
                bool isPlaying = (mediaPlayer.Position != mediaPlayer.NaturalDuration.TimeSpan);
                if (!isPlaying)
                {
                    this.timer.Stop();
                    this.Stop();
                    this.OnFinish();
                }
            }
        }

        public void Play(string pathMusic)
        {
            this.isPause = false;
            this.CanPause = true;
            this.timer.Stop();
            this.mediaPlayer.Open(new Uri(pathMusic));
            this.mediaPlayer.Play();

            this.TickExecuting();
            this.timer.Start();
        }

        public void RePlay()
        {
            this.isPause = false;
            this.CanPause = true;
            this.timer.Start();
            this.mediaPlayer.Play();
            this.isPause = false;
        }

        public void Stop()
        {
            this.CanPause = false;
            this.timer.Stop();
            this.mediaPlayer.Stop();
        }

        public void Pause()
        {
            this.timer.Stop();
            this.mediaPlayer.Pause();
            this.isPause = true;
            this.CanPause = false;
        }
    }
}