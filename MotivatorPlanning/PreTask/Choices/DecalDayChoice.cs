using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class DecalDayChoice : PreMenuChoice
    {
        public DecalDayChoice()
        {
            showBeforeDay = true;
            showBeforeTask = true;
            autoCloseMenuAfterUse = true;
        }

        public override string GetDescription()
        {
            return "This day will be postponed to tomorrow (the planning size will gain 1 day)";
        }

        public override string GetName()
        {
            return "Decal this day";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            preMenu.planning.DecalDay(d);
            Console.WriteLine("Day is postponed to tomorrow");
            // We use a reference here to directly edit the day inside the planning with a strong reference
            // 
            d = preMenu.planning.GetCurrentDay();
            cancelUse = false;
        }
    }
}
