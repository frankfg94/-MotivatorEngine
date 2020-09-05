using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine
{
    public class Day
    {
        public string description;
        public bool canSkip = true; // allow selection by the user
        public bool hasSkip = false; // allow selection by the user
        public bool canFreemode = false; // Allow the tasks to be done in any order
        public bool hasFreemode = false;
        public TimeSpan estimatedDuration;
        internal List<Task> tasks;

        public Day() { }

        public Day(List<Task> tasks){
            if(tasks == null || tasks.Count == 0)
            {
                throw new ArgumentException("If you pass a tasks parameter, please initialize the list and set at list one task");
            }
            this.tasks = tasks;
        }

        public double GetProgressPercent()
        {
            return Math.Round((double)GetFinishedTaskCount() / tasks.Count, 2) * 100;
        }

        public int GetFinishedTaskCount()
        {
            var finishedCount = 0;
            if(tasks != null)
            {
                foreach (var task in tasks)
                {
                    if (task.IsFinished)
                    {
                        finishedCount++;
                    }
                }
            }
            return finishedCount;
        }

        
        // TODO: check how to manage time for testing
        public bool IsToday(Planning planning)
        {
            // return Date.DayOfYear == DateTime.Now.DayOfYear && Date.Year == DateTime.Now.Year;
            return planning.currentDayIndex.Equals(planning.GetDays().IndexOf(this));
        }

        internal double GetEstimatedDifficulty()
        {
            if(tasks != null && tasks.Count > 0)
            {
                int totDifficulty = 0;
                foreach (var task in tasks)
                {
                    totDifficulty += task.EstimatedDifficulty;
                }
                double res = totDifficulty / tasks.Count;
                return Math.Round(res, 1);
            }
            return 0;
        }

        internal TimeSpan GetTotalDuration()
        {
            if(tasks != null && tasks.Count > 0)
            {
                TimeSpan dur = new TimeSpan(0);
                foreach (var t in tasks)
                {
                    dur = dur.Add(t.EstimatedDuration);
                }
                return dur;
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// Returns true if all tasks are finished
        /// </summary>
        /// <returns></returns>
        internal bool AreAllTasksFinished()
        {
             return GetNextTaskToDo() == null;
        }


        public bool isEmpty()
        {
            return tasks == null;
        }

        internal Task GetNextTaskToDo()
        {
            if(tasks != null)
            {
                foreach (var task in tasks)
                {
                    if (!task.IsFinished)
                    {
                        return task;
                    }
                }
            }
            return null;
        }
    }
}
