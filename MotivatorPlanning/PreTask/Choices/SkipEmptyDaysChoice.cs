using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class SkipEmptyDaysChoice : PreMenuChoice
    {
        private int dayNumber;

        public SkipEmptyDaysChoice()
        {
            showBeforeDay = true;
            showBeforeTask = false;
            showAfterDay = true;
            autoCloseMenuAfterUse = true;
            keepIncrementingDay = false; // We chose to increment the day manually with this choice, so no need to increment in the planning
        }

        public override bool IsSelectable(out string msg)
        {
            if (base.IsSelectable(out string bMsg))
            {
                var curDay = preMenu.planning.GetCurrentDay();
                if(curDay.isEmpty())
                {
                    msg = null;
                    return true;
                } 
                else if(curDay == preMenu.planning.GetDays().Last())
                {
                     msg = "We are the last day of this planning, can't skip to other days";
                     return false;
                }
                else if (curDay.AreAllTasksFinished())
                {

                    msg = null;
                    return true;
                }
                else
                {
                    // TODO make empty or finished optional
                    msg = "The day must be empty or finished to be skipped";
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
            return "Skip all days that are empty (no Tasks), shorten your planning";
        }

        public override string GetName()
        {
            var futureDay = preMenu.planning.GetNextNonEmptyDayIndex(preMenu.planning.GetCurrentDay()) + 1;
            if(futureDay <= 0)
            {
                return "Start the next day with Tasks";
            }
            return "Start the next day with Tasks (jump to day number " + futureDay + ")";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            this.dayNumber = preMenu.planning.GetNextNonEmptyDayIndex(d) + 1;
            if(preMenu.planning.AskConfirmation($"Do you want to jump to day number {dayNumber}, current number is {preMenu.planning.currentDayIndex+1} ? This is not reversible"))
            {
                cancelUse = false;
                preMenu.planning.SkipDaysUntilTask(d);
                d = preMenu.planning.GetCurrentDay();
                if(preMenu.isPostMenuOpen)
                {
                    // We want to start the next day directly without waiting
                    preMenu.planning.overrideTimeBeforeNewDay = TimeSpan.FromMilliseconds(1);
                }
                Console.WriteLine("Current day after skip is now : " + (preMenu.planning.currentDayIndex+1));
            }
            else
            {
                cancelUse = true;
            }

        }
    }
}
