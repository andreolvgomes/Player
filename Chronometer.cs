using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Player
{
    public class Chronometer
    {
        public TimeSpan Elapsed
        {
            get
            {
                return this.cronometro.Elapsed;
            }
        }

        public string Label { get; set; }
        private DispatcherTimer timer = null;
        private Stopwatch cronometro = null;
        public event EventHandler Event_Tick;

        public Chronometer()
        {
            this.timer = new DispatcherTimer();
            this.cronometro = new Stopwatch();

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            this.timer.Tick += new EventHandler(Tick);
        }

        private void OnEvent_Tick(object sender, EventArgs e)
        {
            if (this.Event_Tick != null)
                this.Event_Tick(sender, e);
        }

        private void Tick(object sender, EventArgs e)
        {
            this.Label = string.Format("{0:00}:{1:00}", this.cronometro.Elapsed.Minutes, this.cronometro.Elapsed.Seconds);
            this.OnEvent_Tick(sender, e);
        }        

        public void Start()
        {
            this.timer.Start();
            this.cronometro.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
            this.cronometro.Stop();
        }

        public void Reset()
        {
            this.cronometro.Reset();
        }

        public void Restart()
        {
            this.Stop();
            this.Reset();
            this.Start();
        }
    }
}