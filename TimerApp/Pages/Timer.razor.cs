using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimerApp.Model;

namespace TimerApp.Pages
{
    partial class Timer
    {

        /// <summary>
        /// The shared event source that is used to signal all connected clients when the data is changed.
        /// </summary>
        private static event EventHandler<TimerEventArgs> OnTimerChanged;

        /// <summary>
        /// The shared data store thta holds the data to share across clients.
        /// </summary>
        private static int SharedTotalSeconds { get; set; }

        [Parameter]
        public string DisplayValue { get; set; }



        public Task StartAsync()
        {
            this.Tmr.Change(1000, 1000);
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            this.Tmr.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            return Task.CompletedTask;
        }

        System.Threading.Timer Tmr = null;

        protected override Task OnInitializedAsync()
        {
            this.Tmr = new System.Threading.Timer((state) => {
                // Increment the shared count, and signal the change to all other instances
                // of this class using the static OnTimerChanged event.
                SharedTotalSeconds++;
                if(null != OnTimerChanged)
                {
                    OnTimerChanged.Invoke(this, new TimerEventArgs(SharedTotalSeconds));
                }
            });
            
            OnTimerChanged += (o, e) =>
            {
                this.InvokeAsync(() =>
                {
                    // Since we're not necessarily on the thread that has proper access to the renderer context
                    // we need to use the InvokeAsync() method, which takes care of running our code on the right thread.
                    this.CalculateDisplayValue();
                    this.StateHasChanged();
                });
            };

            // Calculate the initial value for the timer.
            this.CalculateDisplayValue();


            return base.OnInitializedAsync();
        }


        private void CalculateDisplayValue()
        {
            int mins = SharedTotalSeconds / 60;
            int secs = SharedTotalSeconds % 60;

            this.DisplayValue = string.Format("{0:00}:{1:00}", mins, secs);
        }
    }
}
