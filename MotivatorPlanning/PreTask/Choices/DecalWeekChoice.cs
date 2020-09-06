using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class DecalWeekChoice : PreMenuChoice
    {
        public DecalWeekChoice()
        {
            showBeforeDay = true;
            showBeforeTask = true;
        }

        public override string GetDescription()
        {
            return "Postpone the planning 7 days later (starting from this day)" ;
        }

        public override string GetName()
        {
            return "Decal this week";
        }

        protected override void _Use(ref Day d, Task t, out bool cancelUse)
        {
            preMenu.planning.DecalWeek(d);
            cancelUse = false;
        }
    }
}
