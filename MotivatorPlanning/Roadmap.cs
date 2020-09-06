using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine
{
    public class Roadmap
    {
        string nom;
        private readonly Planning planning;

        public Roadmap(Planning p)
        {
            this.planning = p;
        }

        public void PrintRoadmapCurrentDay()
        {
            PrintRoadmapDay(planning.GetCurrentDay());
        }

        public void PrintRoadmapDay(Day d)
        {
            Console.WriteLine( "======== Day Roadmap ===========");
            Console.WriteLine($"| Day\t\t: {planning.currentDayIndex+1}/{planning.GetDays().Count}");
            Console.WriteLine($"| Description\t: {d.description ?? "Empty"}");
            Console.WriteLine($"| Difficulty\t: {d.GetEstimatedDifficulty()}");
            Console.WriteLine($"| Duration\t:{d.GetTotalDuration()}");
            if(d.tasks != null)
            {
                Console.WriteLine($"| Progress\t: {d.GetFinishedTaskCount()}/{d.tasks.Count} ({d.GetProgressPercent()}%)");
                Console.WriteLine($"| ---------------------");
                Console.WriteLine($"| Tasks");
                for(int i = 0; i < d.tasks.Count; i++)
                {
                    var t = d.tasks[i];
                    Console.Write($"| \t{i+1}) {t.Infos.name}");
                    if (t.IsFinished)
                    {
                        Console.Write(" [Finished]");
                    }
                    Console.Write("\n");
                    Console.WriteLine($"| \t|\tDescription\t: {t.Infos.shortDescription}");
                    Console.WriteLine($"| \t|\tDifficulty\t: {t.Infos.difficultyLvl}/5");
                    Console.WriteLine($"| \t|\tDuration\t: {t.Infos.durationLvl}");
                }
            }
            else
            {
                Console.WriteLine("| Empty Day");
            }
            Console.WriteLine( "================================");
        }

        public void PrintRoadmap()
        {
            Console.WriteLine("/////////////ROADMAP////////////");
            var days = this.planning.GetDays();
            Console.WriteLine($"Planning name : {planning.name}");
            Console.WriteLine($"Current day index {planning.currentDayIndex} / {days.Count}");
            var taskDoneCount = planning.GetTasks().FindAll(x => x.IsFinished).Count;
            var tCount =  planning.GetTasks().Count;
            Console.WriteLine($"Current progress {taskDoneCount / tCount * 100} % ( {taskDoneCount} / {tCount} tasks done ) ");
            var oldWeekNumber = -1;
            var planningWeekCount = planning.GetWeekCount();
            for(int i = 0; i < days.Count; i++)
            {
                var curDay = days[i];
                var curWeekNumber = planning.GetWeekNumber(curDay);
                if (curWeekNumber != oldWeekNumber)
                {
                    oldWeekNumber = curWeekNumber;
                    Console.WriteLine($"------------- Week number {curWeekNumber} / {planningWeekCount} --------------");
                }
                if (curDay.IsToday(planning))
                {
                    Console.Write(" ===> ");
                }
                else
                {
                    Console.Write(" -- ");
                }
                 Console.Write($"Day {i+1} / {days.Count}");
                if(curDay.IsToday(planning))
                {
                    Console.Write(" [Current day]");
                } 
                if(curDay.AreAllTasksFinished())
                {
                    if(curDay.isEmpty())
                    {
                        Console.Write(" [Empty day]");
                    }
                    else
                    {
                        Console.Write($" [Finished  {curDay.GetFinishedTaskCount()} / {curDay.tasks.Count} ]");
                    }
                } 
                else
                {
                    Console.Write($" [ {curDay.GetFinishedTaskCount()} / {curDay.tasks.Count} ]");
                }
                Console.Write("\n");
            }
        }

        public string GetRoadmapText()
        {
            Console.WriteLine("/////////////ROADMAP////////////");
            StringBuilder sb = new StringBuilder();
            var days = this.planning.GetDays();
            sb.AppendLine($"Planning name : {planning.name}");
            sb.AppendLine($"Current day index {planning.currentDayIndex} / {days.Count}");
            var taskDoneCount = planning.GetTasks().FindAll(x => x.IsFinished).Count;
            var tCount = planning.GetTasks().Count;
            sb.AppendLine($"Current progress {taskDoneCount / tCount * 100} % ( {taskDoneCount} / {tCount} tasks done ) ");
            var oldWeekNumber = -1;
            var planningWeekCount = planning.GetWeekCount();
            for (int i = 0; i < days.Count; i++)
            {
                var curDay = days[i];
                var curWeekNumber = planning.GetWeekNumber(curDay);
                if (curWeekNumber != oldWeekNumber)
                {
                    oldWeekNumber = curWeekNumber;
                    sb.AppendLine($"------------- Week number {curWeekNumber} / {planningWeekCount} --------------");
                }
                if (curDay.IsToday(planning))
                {
                    sb.Append(" ===> ");
                }
                else
                {
                    sb.Append(" -- ");
                }
                sb.Append($"Day {i + 1} / {days.Count}");
                if (curDay.IsToday(planning))
                {
                    sb.Append(" [Current day]");
                }
                if (curDay.AreAllTasksFinished())
                {
                    if (curDay.isEmpty())
                    {
                        sb.Append(" [Empty day]");
                    }
                    else
                    {
                        sb.Append($" [Finished  {curDay.GetFinishedTaskCount()} / {curDay.tasks.Count} ]");
                    }
                }
                else
                {
                    sb.Append($" [ {curDay.GetFinishedTaskCount()} / {curDay.tasks.Count} ]");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
