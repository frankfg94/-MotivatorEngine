using MotivatorPluginCore;
using System;

namespace MotivatorEngine
{
    public class ConsolePlanning : MockPlanning
    {
        public bool waitBeforeTaskTimeSpan = true;
        public bool waitBeforeDayTimeSpan = true;
        public override bool AskConfirmation(string text)
        {
            Console.WriteLine($"{text} (y/n) ");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                return true;
            }
            else if (key.KeyChar == 'n' || key.KeyChar == 'N')
            {
                return false;
            }
            return AskConfirmation(text);
        }

        public override string AskToTypeText()
        {
            Console.Write("You text : ");
            return Console.ReadLine();
        }

        public override Form AskForm(Form form)
        {
            form.date = DateTime.Now;
            Console.WriteLine("////////////////////////////////////////////////");
            Console.WriteLine($"/////// Motivator Form : {form.formTitle} /////");
            Console.WriteLine("////////////////////////////////////////////////");
            form.formDatas.ForEach(d =>
            {
                if (d is TextFormData data)
                {
                    Console.WriteLine("///// Title   : " + data.title);
                    Console.WriteLine("/ Description : " + data.description);
                    string s = AskToTypeText();
                    data.response = s;
                    Console.WriteLine("////////////////////////////////");
                }
                else if (d is YesNoFormData dataYN)
                {
                    Console.WriteLine("///// Yes no question");
                    if (AskConfirmation(dataYN.title))
                    {
                        dataYN.response = true;
                    }
                    else
                    {
                        dataYN.response = false;
                    }
                    Console.WriteLine("/////////////////////");
                }
                else if (d is RadioFormData dataRadio)
                {
                    Console.WriteLine("Radio form data is not implemented yet");
                }
            });
            Console.WriteLine("Form complete");
            Console.WriteLine("//////////////////////////////////");
            return form;
        }

        public override AbstractPreMenu AskPostDayMenu(ref AbstractDay day)
        {
            var choices = preMenu.availableChoices.FindAll(x => x.count > 0 && x.ShowAfterDay());
            if (choices.Count == 0)
            {
                Console.WriteLine("Skipping Post Day menu because no options available");
                return preMenu;
            }

            int selectedChoice = -1;

            Console.WriteLine("\n////////////Post day menu////////////////");
            Console.WriteLine("This menu allows you to choose some options before finishing your day");
            var taskCount = (day.tasks != null) ? day.tasks.Count : 0;
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
                    Console.WriteLine("\t\t|\t" + msg);
                }
                curIndex++;
            }
            int quitChoice = curIndex;
            Console.WriteLine(quitChoice + ")\tClose the menu and finish the day");
            selectedChoice = SecureIntInput(1, curIndex, "Choose your option before starting the day");
            if (selectedChoice != quitChoice)
            {
                Console.WriteLine("Using choice ...");
                if (choices[selectedChoice - 1].IsSelectable(out string errorMsg))
                {
                    choices[selectedChoice - 1].Use(ref day, null);
                    AskPostDayMenu(ref day);
                }
                else
                {
                    Console.WriteLine("This feature cannot be used at the moment, try something else :\n" + errorMsg);
                    AskPostDayMenu(ref day);
                }
            }
            return preMenu;
        }

        public override AbstractPreMenu AskPreDayMenu(ref AbstractDay day)
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
            ConsoleRoadmap r = new ConsoleRoadmap(this);
            r.ShowRoadmap();
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
                    Console.WriteLine("\t\t|\t" + msg);
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

        public override AbstractPreMenu AskWaitTaskMenu(ref AbstractDay d, AbstractTask t)
        {
            var choices = preMenu.availableChoices.FindAll(x => x.count > 0 && x.ShowWhenWaiting());
            if (choices.Count == 0)
            {
                Console.WriteLine("Skipping Wait task menu because no options available");
                return preMenu;
            }

            int selectedChoice = -1;
            Console.WriteLine("\n//////////////Wait menu////////////////");
            Console.WriteLine("This menu allows you to choose some options while waiting for the task to start");
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
                    Console.WriteLine("\t|\t" + msg);
                }
                curIndex++;
            }
            int quitChoice = curIndex;
            Console.WriteLine(quitChoice + ")\tClose the menu and wait/continue ");
            selectedChoice = SecureIntInput(1, curIndex, "Choose your option while waiting for the task : " + t.Infos.name);
            if (selectedChoice != quitChoice)
            {
                Console.WriteLine("Using choice ...");
                var choice = choices[selectedChoice - 1];
                if (!choice.autoCloseMenuAfterUse)
                {
                    AskWaitTaskMenu(ref d, t);
                }
                else
                {
                    Console.WriteLine("Using choice & auto closing wait menu...");
                }
                choice.Use(ref d, t);
            }
            return preMenu;
        }

        public override AbstractPreMenu AskPreTaskMenu(ref AbstractDay d, AbstractTask t)
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
            ConsoleRoadmap r = new ConsoleRoadmap(this);
            r.ShowRoadmapDay(d);
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
            Console.WriteLine(quitChoice + ")\tClose the menu and start task");
            selectedChoice = SecureIntInput(1, curIndex, "Choose your option before starting the task : " + t.Infos.name);
            if (selectedChoice != quitChoice)
            {
                Console.WriteLine("Using choice ...");
                choices[selectedChoice - 1].Use(ref d, t);
                AskPreTaskMenu(ref d, t);
            }
            return preMenu;
        }

        public override AbstractTask AskTaskToDo(AbstractDay day, AbstractTask plannedTask)
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
                Console.WriteLine($"\t| Duration\t:\t {t.GetDuration()} minutes");
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
        public override bool AskToTypeAbandonConfirmText(AbstractDay d)
        {
            if (!handlerSet)
            {
                PlanningAbandonned += (s, e) =>
                  {
                      Console.WriteLine("\n\n>> You abandonned the planning");
                      Environment.Exit(0);
                  };
                handlerSet = true;
            }
            string hellText = "#[`FSD73Q5Q/¨op£*¤èé'(è_fdsfxghaq.+!²&";
            // For debugging, we use a very simple abandon text
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

        /// <summary>
        /// Dynamic day calculation system 
        /// What does that mean ? 
        /// Simply, the time will go on if you don't do the planning.
        /// For example if you have a planning that must last 2 weeks, and you don't anything for 1 week, then 
        /// when you go back to your planning, you only have 1 week remaining instead of 2 weeks, because day calculation is now linked to the datetime system.
        /// This can be useful if you need to finish a planning before a certain date.
        /// </summary>
        /// <returns></returns>
        protected override AbstractDay CurrentDayIndexAlgorithm()
        {
            currentDayIndex = (int)(DateTime.Now - beginDate).TotalDays;
            currentDayIndex += GetPluginsDayIndexModifiers(); // Get day decal from plugins
            currentDayIndex += GetChoicesDayIndexModifiers(); // Get day decal from choice
            var curDay = GetCurrentDay();
            // Set the correct date
            if (currentDayIndex == 0 && lastFinishDate == DateTime.MinValue)
            {
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>> Beginning a new day : {currentDayIndex + 1}/{GetDays().Count}");
                OnDayStarted(curDay);
            }
            else if (IsBeginningNewDay())
            {
                currentDayIndex++;
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>> Beginning a new day : {currentDayIndex + 1}/{GetDays().Count}");
                OnDayStarted(curDay);
            }

            return curDay;
        }

        public override TimeSpan GetTimeBeforeNewDay()
        {
            var timeBeforeNextDay = DateTime.Now.AddDays(1).Date - DateTime.Now;
            if (waitBeforeDayTimeSpan)
            {
                return timeBeforeNextDay;
            }
            return TimeSpan.FromMilliseconds(1);
        }

        public override TimeSpan GetTimeBeforeNewTask()
        {
            var next = GetNextPlannedTask();
            if (waitBeforeTaskTimeSpan && next.ScheduledTime.HasValue)
            {
                var timespan = next.ScheduledTime.Value - DateTime.Now.TimeOfDay;
                if (timespan.Ticks < 0)
                {
                    timespan = TimeSpan.FromMilliseconds(1);
                }
                return timespan;
            }
            return base.GetTimeBeforeNewTask();
        }
        public override void SelectPlugins()
        {
            Console.WriteLine("-------  Plugin selection  ----------");
            int i = 1;
            int quitChoice = plugins.Count + 1;
            foreach (var p in plugins)
            {
                if (!pluginProgressSet)
                {
                    p.LoadProgressChanged += (s, e) =>
                    {
                        Console.WriteLine($"[PLUGINS] Using plugin {e.progress}% : {e.text}");
                    };
                    pluginProgressSet = true;
                }

                if (p.usePlanningStartFirstTime)
                {
                    if (p.IsSelectable(out string reason))
                    {
                        Console.WriteLine($"{i}) {p.name}");
                    }
                    else
                    {
                        Console.WriteLine($"{i}) {p.name} | [Locked : {reason}]");
                    }
                }
                else
                {
                    Console.WriteLine($"{i}) {p.name} | [Locked : cannot be used at the beginning of the planning]");
                }
                i++;
            }
            Console.WriteLine($"{quitChoice}) Close & continue");
            var input = SecureIntInput(1, quitChoice, "Choose your plugin? ");
            if (input == quitChoice)
            {
                Console.WriteLine("Finished plugin selection");
            }
            else if (input < quitChoice)
            {
                if (plugins[input - 1].IsSelectable(out string reason))
                {
                    Console.WriteLine($"Using : {plugins[input - 1].name}");
                    plugins[input - 1].UsePlanningStartFirstTime();
                }
                else
                {
                    Console.WriteLine($"Can't use plugin | Reason : {reason}");
                }
                // Problem loading is async for console, ugly
                SelectPlugins();
            }
            else
            {
                SelectPlugins();
            }
            Console.WriteLine("-------------------------------------");
        }
    }
}
