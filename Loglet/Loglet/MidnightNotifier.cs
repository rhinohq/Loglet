using System;
using System.Timers;

namespace Loglet
{
    public static class MidnightNotifier
    {
        private static readonly Timer timer;

        static MidnightNotifier()
        {
            timer = new Timer(GetSleepTime());

            timer.Elapsed += (s, e) =>
            {
                OnDayChanged();
                timer.Interval = GetSleepTime();
            };

            timer.Start();

#if NETFRAMEWORK
            Microsoft.Win32.SystemEvents.TimeChanged += OnSystemTimeChanged;
#endif
        }

        private static double GetSleepTime()
        {
            DateTime midnightTonight = DateTime.Today.AddDays(1);
            double differenceInMilliseconds = (midnightTonight - DateTime.Now).TotalMilliseconds;

            return differenceInMilliseconds;
        }

        private static void OnDayChanged()
        {
            DayChanged?.Invoke(null, null);
        }

        private static void OnSystemTimeChanged(object sender, EventArgs e)
        {
            timer.Interval = GetSleepTime();
            Log.UpdateWriter();
        }

        public static event EventHandler<EventArgs> DayChanged;
    }
}
