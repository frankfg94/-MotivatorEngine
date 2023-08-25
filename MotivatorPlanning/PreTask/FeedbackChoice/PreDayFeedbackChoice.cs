using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.FeedbackChoice
{
    public class PreDayFeedbackChoice : PreMenuChoice
    {
        public PreDayFeedbackChoice()
        {
            this.showBeforeDay = true;
            this.showBeforeTask = false;
            this.showAfterDay = false;
        }
        
        public override string GetDescription()
        {
            return "Give a feedback on why you want to obtain good results with this planning, and why you are able to do it";
        }

        public override string GetName()
        {
            return "Motivation boost feedback";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            // TODO : Add persistence for reading results
            Form f = Form.builder.AddQuestion("Let's talk about your objectives", "Why do you want to have good results with this planning ?")
                                 .AddQuestion("...and your capabilities", "Tell me why are you able to do this planning?")
                                 .SetTitle("Motivation Boost Feedback")
                                 .Build();
            preMenu.planning.AskForm(f);
            preMenu.planning.Save();
            FeedbackHelper.GetInstance().AddAndSaveToFile(f);
            cancelUse = false;
        }
    }
}
