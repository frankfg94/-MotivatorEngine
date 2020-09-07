using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class SkipEmptyDaysChoice : PreMenuChoice
    {
        public SkipEmptyDaysChoice()
        {
            showBeforeDay = true;
            showBeforeTask = false;
        }

        public override bool IsSelectable(out string msg)
        {
            var curDay = preMenu.planning.GetCurrentDay();
            if (base.IsSelectable(out string bMsg))
            {
                if(curDay.isEmpty())
                {
                    msg = null;
                    return true;
                } 
                else
                {
                    msg = "The day must be empty to be skipped";
                }
            }
            else
            {
                msg = bMsg;
            }
            return false;
        }

        public override string GetDescription()
        {
            return "Skip all days that are empty (no tasks), shorten your planning";
        }

        public override string GetName()
        {
            return "Start next day with tasks (jump X days)";
        }

        protected override void _Use(ref Day d, Task t, out bool cancelUse)
        {
            var dayNumber = preMenu.planning.GetNextNonEmptyDayIndex(d) + 1;
            if(preMenu.planning.AskConfirmation($"Do you want to jump to day number {dayNumber}, current number is {preMenu.planning.currentDayIndex} ? This is not reversible"))
            {
                cancelUse = false;
                preMenu.planning.SkipDaysUntilTask(d);
                d = preMenu.planning.GetCurrentDay();
            }
            else
            {
                cancelUse = true;
            }

        }
    }
}
