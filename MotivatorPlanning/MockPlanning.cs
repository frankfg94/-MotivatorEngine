using MotivatorPluginCore;
using System;
using System.Collections.Generic;

namespace MotivatorEngine
{
    public class MockPlanning : AbstractPlanning
    {
        public bool showCurrentDayIndexText = true;
        public MockPlanning()
        {
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

        public override AbstractPreMenu AskPostDayMenu(ref AbstractDay day)
        {
            Console.WriteLine("[IA] Selecting the menu options when we finished the day...");
            return preMenu;
        }


        public override AbstractPreMenu AskPreTaskMenu(ref AbstractDay d, AbstractTask taskToDo)
        {
            Console.WriteLine("[IA] Selecting the options for the task..." + (d.tasks.FindAll(t => t.IsFinished).Count + 1) + "/" + d.tasks.Count);
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

        public override void HandlePluginEvents(List<IPlugin> plugins)
        {
            foreach (IPlugin p in plugins)
            {
                p.LoadProgressChanged += (s, e) =>
                {
                    Console.WriteLine($"[PLUGINS] Using plugin {e.progress}% : {e.text}");
                };
            }
        }

        public override void SelectPlugins()
        {
            // No plugins to select at the moment
        }

        public override AbstractDay CurrentDayIndexAlgorithm()
        {
            AbstractDay curDay = GetCurrentDay();
            // Set the correct date
            if (currentDayIndex == 0 && lastFinishDate == DateTime.MinValue)
            {
                if (showCurrentDayIndexText)
                {
                    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>> Beginning a new day : {currentDayIndex + 1}/{GetDays().Count}");
                }
                OnDayStarted(curDay);
            }
            else if (IsBeginningNewDay())
            {
                currentDayIndex++;
                if (showCurrentDayIndexText)
                {
                    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>> Beginning a new day : {currentDayIndex + 1}/{GetDays().Count}");
                }
                OnDayStarted(curDay);
            }

            return curDay;
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

        /// <summary>
        /// This method enables an inherited roadmap to be directly used in base classes
        /// </summary>
        /// <returns></returns>
        protected override AbstractRoadmap InstantiateRoadmap()
        {
            return new ConsoleRoadmap(this);
        }

        public override Form AskForm(Form data)
        {
            Console.WriteLine("[IA] Not completing the form");
            return data;
        }

        public override string AskToTypeText()
        {
            Console.WriteLine("[IA] Typing a random text");
            return "typed text";
        }
    }
}
