using MotivatorPluginCore;
using System;
using System.Collections.Generic;
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
            return "Start the next day with Tasks (jump "+ (preMenu.planning.GetNextNonEmptyDayIndex(preMenu.planning.GetCurrentDay()) + 1) + " days)";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            this.dayNumber = preMenu.planning.GetNextNonEmptyDayIndex(d) + 1;
            if(preMenu.planning.AskConfirmation($"Do you want to jump to day number {dayNumber}, current number is {preMenu.planning.currentDayIndex} ? This is not reversible"))
            {
                cancelUse = false;
                preMenu.planning.SkipDaysUntilTask(d);
                d = preMenu.planning.GetCurrentDay();
                if(preMenu.isPostMenuOpen)
                {
                    // We want to start the next day directly without waiting
                    preMenu.planning.overrideTimeBeforeNewDay = TimeSpan.FromMilliseconds(1);
                }
                Console.WriteLine("Current day after skip  is now : " + (preMenu.planning.currentDayIndex+1));
            }
            else
            {
                cancelUse = true;
            }

        }
    }
}
