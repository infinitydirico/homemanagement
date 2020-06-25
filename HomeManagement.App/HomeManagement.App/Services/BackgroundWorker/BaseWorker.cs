using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.BackgroundWorker
{
    public abstract class BaseWorker
    {
        protected Timer timer;

        public bool Started { get; private set; }

        public abstract int GetTimerPeriod();

        protected abstract Task Process();

        public virtual async void RunWork(object state)
        {
            await Process();
        }

        public void Start()
        {
            timer = new Timer(RunWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(GetTimerPeriod()));
            Started = true;
        }

        public void Stop()
        {
            timer?.Dispose();
            Started = false;
        }
    }
}
