using System.Collections.Generic;

namespace ImprovedTimers
{
    public static class TimerManager
    {
        static readonly IList<Timer> timers = new List<Timer>();

        public static void RegisterTimer(Timer timer) => timers.Add(timer);
        public static void DeregisterTimer(Timer timer) => timers.Remove(timer);

        public static void UpdaTeTimers()
        {
            // foreach(Timer timer in timers)
            // {
            //     timer.Tick();
            // }

            for(int i=0; i<timers.Count; ++i)
            {
                timers[i].Tick();
            }
        }

        public static void Clear()
        {
            timers.Clear();
        }
    }
}