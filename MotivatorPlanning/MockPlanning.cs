using MotivatorEngine.PreTask;
using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MotivatorEngine
{
    public class MockPlanning : AbstractPlanning
    {
        public MockPlanning() {
            description = "This is a mock planning generated for testing only";
        }

        public override bool AskConfirmation(string text)
        {
            Console.WriteLine($"[IA] I'm asked for confirming an input ({text}), i say yes");
            return true;
        }

        public override AbstractPreMenu AskPreDayMenu(ref AbstractDay day)
        {
            Console.WriteLine("[IA] Selecting the menu options for the day...");
            return preMenu;
        }

        public override AbstractPreMenu AskPreTaskMenu(ref AbstractDay d, AbstractTask t)
        {
            Console.WriteLine("[IA] Selecting the options for the task..." + (d.tasks.FindAll(t => t.IsFinished).Count+1) + "/" + d.tasks.Count);
            return preMenu;
        }

        public override AbstractTask AskTaskToDo(AbstractDay day, AbstractTask t)
        {
            Console.WriteLine("[IA] Choosing the next task to do...");
            return day.GetNextTaskToDo();
        }

        public override bool AskToTypeAbandonConfirmText(AbstractDay d)
        {
            Console.WriteLine("[IA] typing confirmation text for giving up...");
            return true;
        }

        public override AbstractPreMenu AskWaitTaskMenu(ref AbstractDay d, AbstractTask t)
        {
            Console.WriteLine("[IA] Selecting the wait options for the task...");
            return preMenu;
        }

        public override TimeSpan GetTimeBeforeNewDay()
        {
            return TimeSpan.FromMilliseconds(1);
        }

        public override TimeSpan GetTimeBeforeNewTask()
        {
            return TimeSpan.FromMilliseconds(1);
        }

        public override void SelectPlugins()
        {
            // No plugins to select at the moment
        }
        /// <summary>
        /// Convert to singleton ?
        /// </summary>
        /// <returns></returns>
        protected override AbstractPlanningLoader GetPlanningLoader()
        {
            return new PlanningLoader();
        }

        protected override AbstractDay instantiateDay()
        {
            return new Day();
        }

        protected override AbstractRoadmap InstantiateRoadmap()
        {
            return new ConsoleRoadmap(this);
        }
    }
}
