using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    public class SkipDayChoice : PreMenuChoice
    {
        public int canEnableIfLongThreesholdMilliseconds = 60 * 1000;
        public bool enableSkipTaskIfDifficult = true;

        public SkipDayChoice()
        {
            showBeforeDay = true;
            showBeforeTask = true;
        }
        
        public override string GetDescription()
        {
            return "Skip the current day and all its tasks";
        }

        public override string GetName()
        {
            return "Skip the day";
        }

        public override bool IsSelectable()
        {
            var curDay = preMenu.planning.GetCurrentDay();
            if (curDay.canSkip)
            {
                if(curDay.tasks != null && curDay.tasks.Count > 0)
                {
                    if(!curDay.AreAllTasksFinished())
                    {
                        return true && base.IsSelectable();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected override void _Use(ref Day d, Task t, out bool cancelUse)
        {
            d.tasks.ForEach(delegate(Task t)
            {
                t.IsFinished = true;
            });
            cancelUse = false;
        }
    }
}
