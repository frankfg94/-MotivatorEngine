using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MotivatorEngine.PreTask
{
    /// <summary>
    /// Console Version
    /// </summary>
    public class PreMenu
    {
        [JsonIgnore]
        public Planning planning;
        public readonly List<PreMenuChoice> availableChoices;
        public bool refillEnabled = true;
        public bool freeRefillEnabled = true;
        /// <summary>
        /// Setting it to false is highly unrecommended, for testing purposes
        /// </summary>
        internal bool enabled = true;
        public event EventHandler<ChoiceArg> MenuChoiceBeforeUse;
        public event EventHandler<ChoiceArg> MenuChoiceAfterUse;
        /// <summary>
        /// Must be triggered when the user finished choosing its tasks
        /// </summary>
        public event EventHandler MenuClosed;

        public PreMenu(List<PreMenuChoice> choices)
        {
            this.availableChoices = choices;
            foreach (var choice in this.availableChoices)
            {
                choice.preMenu = this;
            }
        }


        internal void OnMenuClosed()
        {
            MenuClosed?.Invoke(this, null);
        }

        /// <summary>
        /// Function to signal just before an option is used
        /// </summary>
        /// <param name="choice"></param>
        internal void OnBeforeUseChoice(PreMenuChoice choice)
        {
            MenuChoiceBeforeUse?.Invoke(this, new ChoiceArg {selectedChoice = choice });
        }


        /// <summary>
        /// Function to signal after an option is used
        /// </summary>
        /// <param name="choice"></param>
        internal void OnAfterUseChoice(PreMenuChoice choice)
        {
            MenuChoiceAfterUse?.Invoke(this, new ChoiceArg { selectedChoice = choice});
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

        public void ShowPreTaskOptions( Day curDay)
        {
            if (curDay.hasFreemode)
            {
                Console.WriteLine("Choose the task you want to use");
            }
        }

        internal void ShowPreDayOptions(Day curDay)
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
                    if (IsConsoleAnswerPostive(">>> Do you want to enable any task order?"))
                    {
                        curDay.hasFreemode = true;
                    }
                }
                else
                {
                    Console.WriteLine("Didn't propose the freemode because this Day doesn't have tasks");
                }
            }
            */
        }
    }
}
