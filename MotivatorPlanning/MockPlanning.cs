using MotivatorEngine.PreTask;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MotivatorEngine
{
    public class MockPlanning : Planning
    {
        public MockPlanning() {
            description = "This is a mock planning generated for testing only";
        }

        public override bool AskConfirmation(string text)
        {
            Console.WriteLine($"[IA] I'm asked for confirming an input ({text}), i say yes");
            return true;
        }

        public override PreMenu AskPreDayMenu(ref Day day)
        {
            Console.WriteLine("[IA] Selecting the menu options for the day...");
            return preMenu;
        }

        public override PreMenu AskPreTaskMenu(ref Day d, Task t)
        {
            Console.WriteLine("[IA] Selecting the options for the task..." + (d.tasks.FindAll(t => t.IsFinished).Count+1) + "/" + d.tasks.Count);
            return preMenu;
        }

        public override Task AskTaskToDo(Day day, Task t)
        {
            Console.WriteLine("[IA] Choosing the next task to do...");
            return day.GetNextTaskToDo();
        }

        public override bool AskToTypeAbandonConfirmText(Day d)
        {
            Console.WriteLine("[IA] typing confirmation text for giving up...");
            return true;
        }

        public override TimeSpan GetTimeBeforeNewDay()
        {
            return TimeSpan.FromMilliseconds(1);
        }
    }
}
