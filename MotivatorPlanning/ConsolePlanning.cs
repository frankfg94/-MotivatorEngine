using MotivatorEngine.PreTask;
using System;

namespace MotivatorEngine
{
    public class ConsolePlanning : MockPlanning
    {
        public override bool AskConfirmation(string msg)
        {
            Console.WriteLine($"{msg} (y/n) ");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                return true;
            }
            else if (key.KeyChar == 'n' || key.KeyChar == 'N')
            {
                return false;
            }
            return AskConfirmation(msg);
        }

        public override PreMenu AskPreDayMenu(ref Day day)
        {
            var choices = preMenu.availableChoices.FindAll(x => x.count > 0 && x.ShowBeforeDay());
            if (choices.Count == 0)
            {
                Console.WriteLine("Skipping Pre Day menu because no options available");
                return preMenu;
            }

            int selectedChoice = -1;

            Console.WriteLine("\n////////////Pre day menu////////////////");
            Console.WriteLine("This menu allows you to choose some options before starting the planning");
            var taskCount = (day.tasks != null) ? day.tasks.Count : 0;
            Roadmap r = new Roadmap(this);
            r.PrintRoadmap();
            int curIndex = 1;
            foreach (var choice in choices)
            {
                if (choice.IsSelectable(out string msg))
                {
                    Console.WriteLine(curIndex + ")\t" + choice.GetName() + $" {choice.count}/{choice.maxCount}");
                }
                else
                {
                    Console.WriteLine("[Locked]\t" + choice.GetName());
                    Console.WriteLine("\t\t|\t"+msg);
                }
                curIndex++;
            }
            int quitChoice = curIndex;
            Console.WriteLine(quitChoice + ")\tClose the menu and continue");
            selectedChoice = SecureIntInput(1, curIndex, "Choose your option before starting the day");
            if (selectedChoice != quitChoice)
            {
                Console.WriteLine("Using choice ...");
                if (choices[selectedChoice - 1].IsSelectable(out string errorMsg))
                {
                    choices[selectedChoice - 1].Use(ref day, null);
                    AskPreDayMenu(ref day);
                }
                else
                {
                    Console.WriteLine("This feature cannot be used at the moment, try something else :\n" + errorMsg);
                    AskPreDayMenu(ref day);
                }
            }
            return preMenu;
        }

        private int SecureIntInput(int min, int max, string chooseOptionText)
        {
            try
            {
                Console.WriteLine(chooseOptionText + $" ({min} - {max})");
                var integ = int.Parse(Console.ReadLine());
                if (integ < min || integ > max)
                {
                    return SecureIntInput(min, max, chooseOptionText);
                }
                else
                {
                    // Int Input is valid
                    return integ;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("(!) Unrecognized number");
                return SecureIntInput(min, max, chooseOptionText);
            }
        }

        public override PreMenu AskPreTaskMenu(ref Day d, Task t)
        {
            var choices = preMenu.availableChoices.FindAll(x => x.count > 0 && x.ShowBeforeTask());
            if (choices.Count == 0)
            {
                Console.WriteLine("Skipping Pre Day menu because no options available");
                return preMenu;
            }

            int selectedChoice = -1;
            Console.WriteLine("\n//////////////Pre Task menu////////////////");
            Console.WriteLine("This menu allows you to choose some options before starting the planning");
            int curIndex = 1;
            Roadmap r = new Roadmap(this);
            r.PrintRoadmapDay(d);
            foreach (var choice in choices)
            {
                if (choice.IsSelectable(out string msg))
                {
                    Console.WriteLine(curIndex + ")\t" + choice.GetName() + $" {choice.count}/{choice.maxCount}");
                }
                else
                {
                    Console.WriteLine("[Locked]\t" + choice.GetName());
                    Console.WriteLine("\t|\t" + msg);
                }
                curIndex++;
            }
            int quitChoice = curIndex;
            Console.WriteLine(quitChoice + ")\tClose the menu and continue");
            selectedChoice = SecureIntInput(1, curIndex, "Choose your option before starting the Task : " + t.Infos.name);
            if (selectedChoice != quitChoice)
            {
                Console.WriteLine("Using choice ...");
                preMenu.availableChoices[selectedChoice - 1].Use(ref d, t);
                AskPreTaskMenu(ref d, t);
            }
            return preMenu;
        }

        public override Task AskTaskToDo(Day day, Task plannedTask)
        {
            int curTaskIndex = 0;
            var tasksToDo = day.tasks.FindAll(x => !x.IsFinished);
            foreach (var t in tasksToDo)
            {
                Console.Write($"\n{curTaskIndex + 1} | Task Name : {t.Infos.name}");
                if (plannedTask != null && plannedTask == t)
                {
                    Console.Write($" [Selected]");
                }
                Console.Write("\n");
                Console.WriteLine($"\t| Difficulty\t:\t {t.Infos.difficultyLvl}/5");
                Console.WriteLine($"\t| Duration\t:\t {t.EstimatedDuration}");
                curTaskIndex++;
            }
            var cancelChoice = curTaskIndex;
            Console.WriteLine($"{cancelChoice}) Cancel");
            var chosenTaskIndex = SecureIntInput(1, cancelChoice, "Choose the next task that you want to do");
            if (chosenTaskIndex != cancelChoice)
            {
                Console.WriteLine("Using choice ...");
                return tasksToDo[chosenTaskIndex - 1];
            }
            else
            {
                // The user cancelled so we give nothing
                return null;
            }
        }
        bool handlerSet = false;
        public override bool AskToTypeAbandonConfirmText(Day d)
        {
            if (!handlerSet)
            {
                this.PlanningAbandonned += (s, e) =>
                  {
                      Console.WriteLine(">> You abandonned the planning");
                      return;
                  };
                handlerSet = true;
            }
            string hellText = "#[`FSD73Q5Q/¨op£*¤èé'(è_fdsfxghaq.+!²&";
            hellText = "a";
            if (AskConfirmation("///// Are you sure you want to abandon the planning ? ////"))
            {
                Console.WriteLine("Type the following text to abandon the planning, this is not undoable : ");
                Console.WriteLine("\t\t" + hellText);
                if (Console.ReadLine().Equals(hellText))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("The text is not matching, restarting");
                    return AskToTypeAbandonConfirmText(d);
                }
            }
            else
            {
                return false;
            }
        }

        public override TimeSpan GetTimeBeforeNewDay()
        {
            return TimeSpan.FromMilliseconds(1);
        }
    }
}
