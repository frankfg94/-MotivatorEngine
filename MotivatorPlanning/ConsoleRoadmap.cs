using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivatorEngine
{
    public class ConsoleRoadmap : AbstractRoadmap
    {
        // Inherit constructor
        public ConsoleRoadmap(AbstractPlanning p) : base(p)
        {

        }



        public override void ShowRoadmapDay(AbstractDay d)
        {
            if(d == null)
            {
                Console.WriteLine("(!) Could not load the planning for the day because the day is unknown, the planning is probably finished");
                return;
            }
            var days = this.planning.GetDays();
            Console.WriteLine( "======== Day Roadmap ===========");
            Console.WriteLine("| {0,-25} {1}", "Current day index :", planning.currentDayIndex + 1 + "/" + days.Count);
            Console.WriteLine($"| Description\t: {d.description ?? "Empty"}");
            Console.WriteLine($"| Difficulty\t: {d.GetEstimatedDifficulty()}");
            Console.WriteLine($"| Duration\t: {d.GetTotalDuration().TotalMinutes} minutes");
            if (d.tasks != null)
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
                    Console.WriteLine($"| \t|\tDuration\t: {t.Infos.durationLvl} ({Math.Round(t.GetDuration().TotalMinutes,1)} min)");
                    if (t.isFinished)
                    {
                        if(t.FinalDuration != null)
                        {
                           Console.WriteLine($"| \t|\tFinal Duration\t: {Math.Round(t.FinalDuration.TotalMinutes, 1)} min");
                           
                        } 
                        else
                        {
                            Console.WriteLine($"| \t|\tFinal Duration\t: 0 min");
                        }
                    }
                    if (t.ScheduledTime.HasValue)
                    {
                        Console.WriteLine($"| \t|\tStarts at\t: {t.ScheduledTime}");
                    }
                    else
                    {
                        Console.WriteLine($"| \t|\tNo scheduled\t");
                    }
                }
            }
            else
            {
                Console.WriteLine("| Empty Day");
            }
            Console.WriteLine("=====================================");
        }

        public override void ShowRoadmap()
        {
            Console.WriteLine("/////////////ROADMAP////////////");
            var taskDoneCount = planning.GetTasks().FindAll(x => x.IsFinished).Count;
            var days = this.planning.GetDays();
            var tCount =  planning.GetTasks().Count;
            Console.WriteLine("| {0,-25} {1}", "Planning name :",planning.name);
            Console.WriteLine("| {0,-25} {1}", "Current day index :", planning.currentDayIndex + 1 + "/" + days.Count);
            Console.WriteLine("| {0,-25} {1} days" , "Effective study days:", days.FindAll(d => !d.isEmpty()).Count );
            Console.WriteLine("| {0,-25} {1:0.##} hours", "Total intellect time:", days.Sum(d => d.GetTotalDuration().TotalHours));
            Console.WriteLine("| {0,-25} {1:0.##} hours", "Average intellect time:", days.Sum(d => d.GetTotalDuration().TotalHours)/tCount);
            Console.WriteLine("| {0,-25} {1}", "Current progress:", $"{taskDoneCount * 100 / tCount } % ( {taskDoneCount} / {tCount} Tasks done )");
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
                if(!curDay.isEmpty() && curDay.tasks[0].ScheduledTime.HasValue)
                Console.Write("\t\t"+curDay.tasks[0].ScheduledTime.Value);
                Console.Write("\n");
            }
            Console.WriteLine("///////////////////////////");
        }

        public override string GetRoadmapText()
        {
            Console.WriteLine("/////////////ROADMAP////////////");
            StringBuilder sb = new StringBuilder();
            var days = this.planning.GetDays();
            sb.AppendLine($"Planning name : {planning.name}");
            sb.AppendLine($"Current day index {planning.currentDayIndex+1} / {days.Count}");
            var taskDoneCount = planning.GetTasks().FindAll(x => x.IsFinished).Count;
            var tCount = planning.GetTasks().Count;
            sb.AppendLine($"Current progress {taskDoneCount / tCount * 100} % ( {taskDoneCount} / {tCount} Tasks done ) ");
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
