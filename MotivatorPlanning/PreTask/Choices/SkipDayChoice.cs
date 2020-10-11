using MotivatorPluginCore;
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
            autoCloseMenuAfterUse = true;
        }
        
        public override string GetDescription()
        {
            return "Skip the current day and all its AbstractTasks";
        }

        public override string GetName()
        {
            return "Skip the day";
        }

        public override bool IsSelectable(out string msg)
        {
            var curDay = preMenu.planning.GetCurrentDay();
            if (curDay.canSkip)
            {
                if(curDay.tasks != null && curDay.tasks.Count > 0)
                {
                    if(!curDay.AreAllTasksFinished())
                    {
                        return true && base.IsSelectable(out msg);
                    }
                    else
                    {
                        msg = "Don't need to skip, because the day is already finished";
                        return false;
                    }
                }
                else
                {
                    msg = "Don't need to skip, because the day has no AbstractTasks";
                    return false;
                }
            }
            else
            {
                msg = "Can't skip, because the day doesn't allow skipping at all";
                return false;
            }
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            d.tasks.ForEach(delegate(AbstractTask t)
            {
                t.IsFinished = true;
            });
            cancelUse = false;
        }
    }
}
