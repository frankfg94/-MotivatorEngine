using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    // Create new choices in the plugin
    public abstract class PreMenuChoice
    {
        /// <summary>
        /// If set to true, will appear in the preday menu
        /// </summary>
        public bool showBeforeDay = true;
        /// <summary>
        /// If set to true, will appear in the pretask menu
        /// </summary>
        public bool showBeforeTask = false;
        private string description = "no description";
        public int count = 999;
        public int maxCount = 999;
        [JsonIgnore]
        public bool isSelected = false;

        public void BeforeUseChoice()
        {
            preMenu.OnBeforeUseChoice(this);
        }

        public void AfterUseChoice()
        {
            preMenu.OnAfterUseChoice(this);
        }

        public virtual bool IsSelectable()
        {
            return preMenu.enabled && count > 0;
        }

        public virtual bool IsSelectableBeforeDay()
        {
            return IsSelectable() && showBeforeDay;
        }

        public virtual bool IsSelectableBeforeTask()
        {
            return IsSelectable() && showBeforeTask;
        }

        public void Use(Day d, Task t)
        {
            preMenu.OnBeforeUseChoice(this);
            if (preMenu.enabled)
            {
                isSelected = false;
                _Use(d,t,out bool cancelUse);
                if (!cancelUse)
                {
                    count--;
                }
            }
            else
            {
                throw new InvalidOperationException("Programming error, cannot use a menu option if the menu is disabled");
            }
            preMenu.OnAfterUseChoice(this);
        }

        protected abstract void _Use(Day d, Task t, out bool cancelUse);
        public bool ShowBeforeTask()
        {
            return showBeforeTask;
        }

        public bool ShowBeforeDay()
        {
            return showBeforeDay;
        }
        public abstract string GetName();
        public abstract string GetDescription();

        public RefillOptions refillOptions;
        [JsonIgnore]
        public PreMenu preMenu;
    }
}
