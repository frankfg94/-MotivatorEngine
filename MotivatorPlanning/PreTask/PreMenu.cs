using MotivatorPluginCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MotivatorEngine.PreTask
{
    /// <summary>
    /// Console Version
    /// </summary>
    public class PreMenu : AbstractPreMenu
    {
        public PreMenu(): base()
        {

        }


        // Inherit the constructor
        public PreMenu(List<PreMenuChoice> choices) : base(choices)
        {
        }

        // Inherit this class for UI implementation
        private bool IsConsoleAnswerPostive(string message)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return IsConsoleAnswerPostive(input);
            try
            {
                if (input.Equals("yes", StringComparison.OrdinalIgnoreCase)
               || int.Parse(input) == 1)
                    return true;
                else if (input.Equals("no", StringComparison.OrdinalIgnoreCase) || int.Parse(input) == 0)
                    return false;
                else
                    return IsConsoleAnswerPostive(message);
            }
            catch (Exception)
            {
                return IsConsoleAnswerPostive(message);
            }
        }

        public override void ShowPreTaskOptions( AbstractDay curDay)
        {
            if (curDay.hasFreemode)
            {
                Console.WriteLine("Choose the AbstractTask you want to use");
            }
        }

        internal void ShowPreDayOptions(AbstractDay curDay)
        {
            /*
            if (curDay.canSkip)
            {
                if (IsConsoleAnswerPostive(">>> Do you want to skip this day?"))
                {
                    curDay.hasSkip = true;
                    p.OnDayFinished(curDay);
                }
            }
            if (curDay.canFreemode)
            {
                if (curDay.tasks.Count > 0)
                {
                    if (IsConsoleAnswerPostive(">>> Do you want to enable any AbstractTask order?"))
                    {
                        curDay.hasFreemode = true;
                    }
                }
                else
                {
                    Console.WriteLine("Didn't propose the freemode because this Day doesn't have AbstractTasks");
                }
            }
            */
        }
    }
}
