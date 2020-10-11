using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine
{
    public class Week : IWeek
    {
        public string description;
        public bool optionnal = false;
        public List<AbstractDay> days;
        public Week()
        {

        }

        public Week(List<AbstractDay> days)
        {
            if(days == null )
            {
                throw new ArgumentException("Passed list of days to create the week is null");
            }
            else if (days.Count != 7)
            {
                Console.WriteLine("(!) Warning day count is not 7 for new week, found :" + 7);
                // throw new ArgumentException("A week must have 7 days, even if they are empty for more clarity, count is :" + days.Count);
            }
            this.days = days;
        }

        internal TimeSpan GetTotalDuration()
        {
            TimeSpan dur = new TimeSpan(0);
            foreach (var day in days)
            {
                dur = dur.Add(day.estimatedDuration);
            }
            return dur;
        }
    }
}
