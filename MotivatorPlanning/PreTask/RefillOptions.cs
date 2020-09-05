using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    public class RefillOptions
    {
        public int refillDayInterval;
        public int refillCount;
        public bool? difficultyRefill; // Free special refill
        public int? difficultyRefillThreeshold;
        public bool? timeConsumingRefill; // Free special refill
        public TimeSpan? timeConsumingRefillThreeshold;

        public RefillOptions(int refillDayInterval, int refillCount)
        {
            this.refillDayInterval = refillDayInterval;
            this.refillCount = refillCount;
        }

        public bool DifficultyRefillEnabled()
        {
            return difficultyRefill.HasValue && difficultyRefillThreeshold.HasValue;
        }

        public bool TimeConsumingRefillEnabled()
        {
            return timeConsumingRefill.HasValue && timeConsumingRefillThreeshold.HasValue;
        } 
    }
}
