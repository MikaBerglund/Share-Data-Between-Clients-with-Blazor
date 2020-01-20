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

        private static event EventHandler<TimerEventArgs> OnTimerChanged;

        private static int SharedTotalSeconds { get; set; }


        [Parameter]
        public int TotalSeconds { get; set; }

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
                System.Diagnostics.Debug.WriteLine(DateTime.Now);

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
                    this.TotalSeconds = e.Seconds;
                    this.CalculateDisplayValue();
                    this.StateHasChanged();
                });
            };

            this.CalculateDisplayValue();

            return base.OnInitializedAsync();
        }


        private void CalculateDisplayValue()
        {
            int mins = this.TotalSeconds / 60;
            int secs = this.TotalSeconds % 60;

            this.DisplayValue = string.Format("{0:00}:{1:00}", mins, secs);
        }
    }
}
