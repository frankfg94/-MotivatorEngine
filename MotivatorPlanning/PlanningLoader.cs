using MotivatorPluginCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MotivatorEngine
{
    public class PlanningLoader : AbstractPlanningLoader
    {
        public override AbstractPlanning Load(string path, Type t)
        {
            return LoadFromJson(GetJson(path),t);
            // todo Also init variables
        }

        public override AbstractPlanning LoadFromJson(string json, Type t) {
            AbstractPlanning  p = null;
            if (t == typeof(MockPlanning))
            {
                p = JsonConvert.DeserializeObject<MockPlanning>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                });
            }
            else if (t == typeof(ConsolePlanning))
            {
                p = JsonConvert.DeserializeObject<ConsolePlanning>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                });
            }
            
            if (p != null)
            {
                if(p.weeks != null)
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
            if(p.preMenu != null)
            {
                // Solve circular dependency issues here
                foreach (var choice in p.preMenu.availableChoices)
                {
                    choice.preMenu = p.preMenu;
                    choice.preMenu.planning = p;
                }
            }

            if(p.plugins != null)
            {
                foreach (var plugin in p.plugins)
                {
                    plugin.planning = p;
                }
            }

            return p;
        }


            public override string GetJson(AbstractPlanning p)
        {
            if(p.weeks == null)
            {
                if (AbstractPlanning.LOG_SAVE)
                {
                    Console.WriteLine("Getting json mode : Saving");
                }
                p.weeks = new List<IWeek>();
                var allDays = p.GetDays();

                // Console.WriteLine("Days to save : " + allDays.Count);
                var dayIndex = 0;
                for (int i = 0; i < p.GetWeekCount(); i++)
                {
                    var days = new List<AbstractDay>();
                    for (int j=0; j < 7 && dayIndex < allDays.Count; j++)
                    {
                        days.Add(allDays[dayIndex]);
                        dayIndex++;
                    }
                    // If the week is not complete, fill it with empty days
                    p.weeks.Add(new IWeek(days));
               }
            } else
            {
                Console.WriteLine("Getting json mode : Loading");
                var allDays = p.GetDays();
                if (allDays == null)
                {
                    p.SetContent(p.weeks);
                }

            }
            return JsonConvert.SerializeObject(p, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto});
        }

        public override void Save(string path, AbstractPlanning p)
        {
            p.weeks = null;
            var planningJson = GetJson(p);
            SetPlanningFileJson(path, planningJson);
        }

        public override void SetPlanningFileJson(string path, string json)
        {
            // Add encryption?
            File.WriteAllText(path, json);
        }

        public override  string GetJson(string path)
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
