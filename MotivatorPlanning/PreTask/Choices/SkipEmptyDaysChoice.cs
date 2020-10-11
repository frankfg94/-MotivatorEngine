using MotivatorPluginCore;
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
            return "Skip all days that are empty (no Tasks), shorten your planning";
        }

        public override string GetName()
        {
            return "Start next day with Tasks (jump X days)";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
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
