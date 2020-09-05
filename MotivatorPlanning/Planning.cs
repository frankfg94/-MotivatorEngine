using MotivatorEngine.PreTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MotivatorEngine
{
    public class DayArg : EventArgs
    {
        public Day day;
        public DateTime finishTime;
    }


    public abstract class Planning
    {
        [JsonIgnore]
        public const bool LOG_INFOS = true;
        [JsonIgnore]
        public const bool LOG_SAVE = false;
        // We are using the weeks only for loading / saving for more simplicity

        public abstract PreMenu AskPreDayMenu(Day day);

        /// <summary>
        /// Ask a menu, return it when all choices are made
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public abstract PreMenu AskPreTaskMenu(Day d, Task taskToDo);
        public abstract bool AskToTypeAbandonConfirmText(Day d);
        public abstract bool AskConfirmation();

        /// <summary>
        /// Get the time before a new day is started after all tasks are finished
        /// </summary>
        public abstract TimeSpan GetTimeBeforeNewDay();

        protected PreMenu preMenu;
        public void SetPreMenu(PreMenu menu)
        {
            this.preMenu = menu;
            preMenu.planning = this;
        }

        internal void OnRequestSelectNewTask(Task t)
        {
            RequestSelectNewTask?.Invoke(t, null);
        }

        /// <summary>
        /// For the freemode, find the delegate way that returns a Task
        /// so that we can move eveything in the option & still override it
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public abstract Task AskTaskToDo(Day day, Task t); // The second parameter is just here to indicate the current task before selecting a new one
        public SaveSettings saveSettings;
        public string name = "unnamed planning";
        public bool canShrink = true;
        public bool autofinishWhenNoTasks = true;
        public string description;
        public bool isFinished = false;
        public DateTime lastFinishDate = DateTime.MinValue;

        /// <summary>
        /// This variable must NOT be used or modified, it is just for practicity in the json 
        /// </summary>
        public List<Week> weeks;
        /// <summary>
        /// [0 - N] where N is the number of days in the planning minus one
        /// </summary>
        public int currentDayIndex = 0;
        /// <summary>
        /// This is used only for loading / unloading data
        /// </summary>
        [JsonIgnore]
        private List<Day> allDays;
        [JsonIgnore]
        private List<Task> allTasks;
        private bool handlersAdded = false;
        public readonly string testingPath = AppDomain.CurrentDomain.BaseDirectory + "planning.json";
        public event System.EventHandler PlanningFinished;
        public event System.EventHandler DayFinished;
        public event System.EventHandler TaskFinished;
        public event System.EventHandler PlanningStarted;
        public event EventHandler TaskSkipped;
        public event System.EventHandler TaskStarted;
        public event System.EventHandler TaskPaused;
        public event System.EventHandler TaskResumed;
        public event System.EventHandler PlanningAbandonned;
        public event System.EventHandler WeekFinished;
        public event System.EventHandler SendWarnings;
        public event EventHandler PlanningStartedFirstTime;
        public event EventHandler RequestSelectNewTask;
        public event EventHandler RequestMenuChoices;
        public event EventHandler Saved;

        /// <summary>
        /// Get all the days
        /// </summary>
        /// <returns></returns>
        public List<Day> GetDays()
        {
            return allDays;
        }



        public void AddDay(Day d)
        {
            if (allDays == null)
                allDays = new List<Day>();
            allDays.Add(d);
        }

        public void AddDays(List<Day> days)
        {
            if (allDays == null)
                allDays = new List<Day>();
            allDays.AddRange(days);
        }

        public Task GetRunningTask()
        {
            foreach (var t in allTasks)
            {
                if (t.IsRunning)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Should be used only when testing
        /// </summary>
        public void SetContent(List<Week> weeks)
        {
            foreach (var week in weeks)
            {
                AddDays(week.days);
            }
            // Force resync
            allTasks = GetTasks();
            foreach (var task in allTasks)
            {
                task.isCurrent = false;
            }

            // Search next task
            if (allDays[currentDayIndex].tasks != null)
            {
                foreach (var task in allDays[currentDayIndex].tasks)
                {
                    if (!task.IsFinished)
                    {
                        task.isCurrent = true;
                        break;
                    }
                }
            }

            // TODO, resync with plugins
        }

        /// <summary>
        /// Get all the tasks
        /// </summary>
        /// <returns></returns>
        public List<Task> GetTasks()
        {
            var days = GetDays();
            if (days != null)
            {
                allTasks = new List<Task>();
                foreach (var day in days)
                {
                    if (day.tasks != null)
                    {
                        allTasks.AddRange(day.tasks);
                    }
                }
            }
            return allTasks;
        }

        public void SkipDay(Day day)
        {
            foreach (var task in day.tasks)
            {
                task.IsFinished = true;
                task.isCurrent = false;
            }
        }

        /// <summary>
        /// Move the currrent day to the next day in the planning with tasks
        /// </summary>
        /// <param name="startDay"></param>
        public void SkipDaysUntilTask(Day startDay)
        {
            var posDay = allDays.IndexOf(startDay);
            currentDayIndex = allDays.FindIndex(x => allDays.IndexOf(x) > posDay && x.tasks != null && x.tasks.Count > 0);
            // Return null if the startDay was the last day with task
        }

        public void OnDayFinished(Day day)
        {
            DayFinished?.Invoke(this,
                new DayArg { day = day, finishTime = DateTime.Now });
        }

        public void Save()
        {
                Save(testingPath);
        }


        public Day GetCurrentDay()
        {
            var days = GetDays();
            if (days != null)
            {
                foreach (var day in days)
                {
                    if (day.IsToday(this))
                    {
                        return day;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Get the next planned or current task
        /// </summary>
        /// <returns></returns>
        public Task GetNextPlannedTask()
        {
            Task nextTask = null;

            foreach (var task in allTasks)
            {
                if (task.isCurrent)
                {
                    nextTask = task;
                    break;
                }
            }
            var curIndex = 0;
            if (nextTask == null)
            {
                if (allTasks != null)
                {
                    foreach (var day in allDays)
                    {
                        if (curIndex > currentDayIndex)
                        {
                            if (!day.isEmpty())
                            {
                                foreach (var task in day.tasks)
                                {
                                    if (!task.IsFinished)
                                    {
                                        return task;
                                    }
                                }
                            }
                        }
                        curIndex++;
                    }
                }
            }
            return nextTask;
        }

        public bool IsBeginningNewDay()
        {
            if (DateTime.Now > lastFinishDate)
            {
                lastFinishDate = DateTime.Now;
                return true;
            }
            return false;
        }

        /// <summary>
        /// TODO unit test as fast as possible
        /// </summary>
        /// <returns></returns>
        private void Verify()
        {
            // Verify that all event handlers are connected
            List<string> warnings = new List<string>();

            if (allDays == null || allDays.Count == 0)
            {
                throw new InvalidOperationException("This planning must have at least one week");
            }
            // Use online json schema generator
            // Only 1 
            // Abnormal dates
            // Must have 1 day
            // Abnormal estimated duration
            // Valid planning mode : parental control / project

            SendWarnings?.Invoke(this, null);
        }

        private void Save(string path)
        {
            PlanningManager.Save(path, this);
            if(LOG_INFOS && LOG_SAVE)
            {
                Console.WriteLine("\t>> Progress saved");
            }
        }
        public void StartFirstTime()
        {

            // Verify planning json first
            Verify();


            currentDayIndex = 0;
            // currentWeekIndex = 0;

            // Save Mock program
            Save();

            // Check if computer restart is needed (for plugins eg Matrix)
            if (PlanningManager.NeedsRestart())
            {
                // Prompt user, restart or abandon

                // Restart computer
            }

            // Trigger event
            PlanningStartedFirstTime?.Invoke(this, null);

            // Start the real planning
            Start();
        }

        /// <summary>
        /// Start the planning normally
        /// </summary>
        public void Start()
        {
            if(LOG_INFOS)
            {
                //Console.WriteLine("Starting the planning");
            }
            // Set the correct date
            if (IsBeginningNewDay())
            {
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>> Beginning a new day : {currentDayIndex+1}/{allDays.Count}");
            }
            Roadmap r = new Roadmap(this);
            r.PrintRoadmap();
            // Load all plugins if plugin system enabled

            // Start planning
            var curDay = GetCurrentDay();

            // Start the pre task menu
            this.preMenu = AskPreDayMenu(curDay);
            Save();

            // Start the first task if not free mode
            DoAllTasks(curDay);

            // Planning finished event, handled outside the library
            isFinished = true;
            PlanningFinished?.Invoke(this, null);
        }

        private bool PremuEnabled()
        {
            return preMenu != null && preMenu.enabled;
        }

        private void DoAllTasks(Day day)
        {
            if (LOG_INFOS)
            {
               // Console.WriteLine("Starting to do all the tasks for the day " + this.GetDays().IndexOf(day));
            }
            Task taskToDo = null;
            // We check that all the tasks are done
            if (!day.AreAllTasksFinished())
            {
                if (LOG_INFOS)
                {
                    Console.WriteLine($"There are still tasks to do {day.GetFinishedTaskCount()}/{day.tasks.Count} tasks done");
                }
                if (PremuEnabled())
                {
                    if(!handlersAdded)
                    {
                        handlersAdded = true;
                        preMenu.MenuChoiceBeforeUse += (s, e) =>
                        {
                            ChoiceArg choice = e as ChoiceArg;
                            //choice.selectedChoice.Use(day, taskToDo);
                            // choice.selectedChoice.isSelected = false; // unselect

                        };

                        RequestSelectNewTask += (s, e) =>
                        {
                            taskToDo = s as Task;
                            Console.WriteLine("Next task is now " + taskToDo.Infos.name);
                        };
                    
                    }
                    if (taskToDo == null)
                    {
                        taskToDo = day.GetNextTaskToDo();
                    }
                    preMenu = AskPreTaskMenu(day,taskToDo);
                    taskToDo.TaskFinished += (s, e) =>
                    {
                        Save();
                        // Loop the function until there is no more tasks to do
                        TaskFinished?.Invoke(taskToDo, null);
                        DoAllTasks(day);
                    };
                    taskToDo.Start();
                }
                else
                {
                    if (taskToDo == null)
                    {
                        taskToDo = day.GetNextTaskToDo();
                    }
                    taskToDo.TaskFinished += (s, e) =>
                    {
                        Save();
                        // Loop the function until there is no more tasks to do
                        TaskFinished?.Invoke(taskToDo, null);
                        DoAllTasks(day);
                    };
                    taskToDo.Start();
                }

            }
            else
            {
                if(LOG_INFOS)
                {
                    Console.WriteLine($"The day {currentDayIndex+1}/{allDays.Count} is finished !");
                }
                Save();
                DayFinished?.Invoke(this, null);
                if (currentDayIndex % 6 == 0)
                {
                    WeekFinished?.Invoke(this, null);
                }

                if (day == allDays.Last())
                {
                    FinishSuccessfully();
                }
                else
                {
                    StartNextDayTimer();
                }
            }
        }

        public override string ToString()
        {
            return new Roadmap(this).GetRoadmapText();
        }

        public void StartNextDayTimer()
        {
            if(LOG_INFOS)
            {
                // Console.WriteLine("Starting next day timer.." );
            }
            var interval = GetTimeBeforeNewDay().TotalMilliseconds;
            Timer t = new Timer
            {
                AutoReset = false,
                Interval = interval
            };
            t.Elapsed += (s, e) =>
            {
                currentDayIndex++;
                Console.WriteLine("Starting from next day timer");
                Start();
                //DoAllTasks(allDays[currentDayIndex]);
            };
            t.Start();
        }

        /// <summary>
        /// Starts from 1
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public int GetWeekNumber(Day d)
        {
            int curDayIndex = -1;
            for (int i = 0; i < allDays.Count; i++)
            {
                if (allDays[i] == d)
                {
                    curDayIndex = i;
                    break;
                }
            }
            return curDayIndex / 7 + 1;
        }


        public int GetWeekCount()
        {
            var curDayCount = allDays.Count;
            var curWeekCount = 1; // there is at least one week per planning
            while (curDayCount > 7)
            {
                curWeekCount++;
                curDayCount -= 7;
            }
            return curWeekCount;
        }

        /// <summary>
        /// Triggered when the user decided to abandon the planning
        /// </summary>
        public void Abandon()
        {
            PlanningAbandonned?.Invoke(this, null);
        }

        /// <summary>
        /// Triggered when the last task of the planning is finished
        /// </summary>
        private void FinishSuccessfully()
        {
            lastFinishDate = DateTime.Now;
            PlanningFinished?.Invoke(this, null);
            if (LOG_INFOS)
            {
                Console.WriteLine($"The planning is finished, well done !");
            }
        }
        public void DecalDay(Day dayToDecal, bool triggerEvents = true)
        {
            if (triggerEvents)
            {
                // Tell that skipped the task
                TaskSkipped?.Invoke(this, null);
            }

            // Insert empty day without changing the current day
            allDays.Insert(allDays.IndexOf(dayToDecal), new Day());
            // Decal this day to tomorrow
            // Change next task
            if (triggerEvents)
            {
                OnDayFinished(dayToDecal);
            }
        }


        public void DecalCustom(Day dayFromWhichToDecal, int decalCount)
        {
            // Tell that skipped the task
            TaskSkipped?.Invoke(this, null);

            for(int i = 0; i < decalCount; i++)
            {
                DecalDay(dayFromWhichToDecal,false);
            }


            OnDayFinished(dayFromWhichToDecal);
        }

        public void DecalWeek(Day dayFromWhichToDecal)
        {

            DecalCustom(dayFromWhichToDecal, 7);
        }

#if DEBUG
        public void DEBUG_SetCurrentDayIndex(int index)
        {
            this.currentDayIndex = index;
        }


#endif
    }
}
