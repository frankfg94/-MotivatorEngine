using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class GiveupChoice : PreMenuChoice
    {
        public string abandonConfirmText = "I want to give up and i am sure of it, 100%";

        public GiveupChoice()
        {
            showBeforeDay = true;
            showBeforeTask = true;
            autoCloseMenuAfterUse = true;
        }

        public override string GetDescription()
        {
            return "You can abandon the planning at any moment";
        }

        public override string GetName()
        {
            return "Give Up";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancel)
        {
            if (preMenu.planning.AskToTypeAbandonConfirmText(d))
            {
                preMenu.planning.Abandon();
            }
            else
            {
                cancel = true;
            }
            cancel = false;
        }
    }
}
