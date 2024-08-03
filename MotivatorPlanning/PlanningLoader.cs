using MotivatorPluginCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MotivatorEngine
{
    public class PlanningLoader : AbstractPlanningLoader
    {
        public override T Load<T>(string path)
        {
            return LoadFromJson<T>(GetJson(path));
            // todo Also init variables
        }

        public override T LoadFromJson<T>(string json)
        {
            AbstractPlanning p = null;
            if (typeof(T) == typeof(MockPlanning))
            {
                p = JsonConvert.DeserializeObject<MockPlanning>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                });
            }
            else if (typeof(T) == typeof(ConsolePlanning))
            {
                p = JsonConvert.DeserializeObject<ConsolePlanning>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                });
            }

            if (p != null)
            {
                if (p.weeks != null)
                {
                    if (p.weeks[0].days != null)
                    {
                        p.SetContent(p.weeks);
                    }
                    else
                    {
                        throw new JsonReaderException("Planning loaded but didn't parse correctly the weeks");
                    }
                }
                else
                {
                    throw new NullReferenceException("The planning to load doesn't have a 'week' field ");
                }
            }
            if (p.preMenu != null)
            {
                // Solve circular dependency issues here
                foreach (PreTask.PreMenuChoice choice in p.preMenu.availableChoices)
                {
                    choice.preMenu = p.preMenu;
                    choice.preMenu.planning = p;
                }
            }

            if (p.plugins != null)
            {
                foreach (IPlugin plugin in p.plugins)
                {
                    plugin.planning = p;
                }
            }

            AbstractDay d = p.CurrentDayIndexAlgorithm();
            return (T)p;
        }


        public override string GetJson(AbstractPlanning p)
        {
            if (p.weeks == null)
            {
                if (AbstractPlanning.LOG_SAVE)
                {
                    Console.WriteLine("Getting json mode : Saving");
                }
                p.weeks = new List<IWeek>();
                List<AbstractDay> allDays = p.GetDays();

                // Console.WriteLine("Days to save : " + allDays.Count);
                int dayIndex = 0;
                for (int i = 0; i < p.GetWeekCount(); i++)
                {
                    List<AbstractDay> days = new List<AbstractDay>();
                    for (int j = 0; j < 7 && dayIndex < allDays.Count; j++)
                    {
                        days.Add(allDays[dayIndex]);
                        dayIndex++;
                    }
                    // If the week is not complete, fill it with empty days
                    p.weeks.Add(new IWeek(days));
                }
            }
            else
            {
                Console.WriteLine("Getting json mode : Loading");
                List<AbstractDay> allDays = p.GetDays();
                if (allDays == null)
                {
                    p.SetContent(p.weeks);
                }

            }
            return JsonConvert.SerializeObject(p, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        public override void Save(string path, AbstractPlanning p)
        {
            p.weeks = null;
            string planningJson = GetJson(p);
            SetPlanningFileJson(path, planningJson);
        }

        public override void SetPlanningFileJson(string path, string json)
        {
            // Add encryption?
            File.WriteAllText(path, json);
        }

        public override string GetJson(string path)
        {
            // Add encryption?
            return File.ReadAllText(path);
        }

        public override bool NeedsRestart()
        {
            return false;
        }
    }
}
