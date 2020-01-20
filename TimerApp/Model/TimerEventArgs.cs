using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimerApp.Model
{
    public class TimerEventArgs : EventArgs
    {
        public TimerEventArgs(int seconds)
        {
            this.Seconds = seconds;
        }

        public int Seconds { get; set; }

    }
}
