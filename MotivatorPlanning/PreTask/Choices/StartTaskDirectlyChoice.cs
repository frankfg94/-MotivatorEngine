using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class StartDirectlyChoice : PreMenuChoice
    {
        public StartDirectlyChoice()
        {
            showBeforeDay = false;
            showBeforeTask = false;
            showWhenWaiting = true;
            autoCloseMenuAfterUse = true;
        }

        public override string GetDescription()
        {
            return "Don't wait before starting the next task for this day, start it now";
        }

        public override string GetName()
        {
            return "Start the next task directly";
        }
        public override bool IsSelectable(out string cantSelectReason)
        {
            if (preMenu.planning.noTimerBeforeTask)
            {
                cantSelectReason = "You are in a direct start mode (tasks dont have any wait time) so this task should start as soon as you close this menu";
                return false;
            }
            return base.IsSelectable(out cantSelectReason);
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            Console.WriteLine("Starting next Task (quick advance)...");
            preMenu.planning.StartNextTaskTimer(ref d,t,true);
            // We use a reference here to directly edit the day inside the planning with a strong reference
            cancelUse = false;
        }
    }
}
