using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MotivatorEngine
{
    public class PlanningManager
    {
        public static Planning Load(string path, Type t)
        {
            return LoadFromJson(GetJson(path),t);
            // todo Also init variables
        }

        public static Planning LoadFromJson(string json, Type t) {
            Planning  p = null;
            if (t == typeof(MockPlanning))
            {
                p = JsonConvert.DeserializeObject<MockPlanning>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                });
            };
            
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
            return p;
        }


        public static string GetJson(Planning p)
        {
            if(p.weeks == null)
            {
                if (Planning.LOG_SAVE)
                {
                    Console.WriteLine("Getting json mode : Saving");
                }
                p.weeks = new List<Week>();
                var allDays = p.GetDays();

                // Console.WriteLine("Days to save : " + allDays.Count);
                var dayIndex = 0;
                for (int i = 0; i < p.GetWeekCount(); i++)
                {
                    var days = new List<Day>();
                    for (int j = 0; j < 7; j++)
                    {
                        days.Add(allDays[dayIndex]);
                        dayIndex++;
                    }
                    p.weeks.Add(new Week(days));
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

        public static void Save(string path, Planning p)
        {
            p.weeks = null;
            var planningJson = GetJson(p);
            SetPlanningFileJson(path, planningJson);
        }

        public static void SetPlanningFileJson(string path, string json)
        {
            // Add encryption?
            File.WriteAllText(path, json);
        }

        public static string GetJson(string path)
        {
            // Add encryption?
            return File.ReadAllText(path);
        }

        internal static bool NeedsRestart()
        {
            return false;
        }
    }
}
