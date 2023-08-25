using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MotivatorEngine.PreTask.FeedbackChoice
{
    public class PostDayFeedbackChoice : PreMenuChoice
    {
        public PostDayFeedbackChoice()
        {
            this.showBeforeDay = false;
            this.showBeforeTask = false;
            this.showAfterDay = true;
        }
        
        public override string GetDescription()
        {
            return "Give a feedback about you felt about your work today";
        }

        public override string GetName()
        {
            return "Give a feedback :)";
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            // TODO : Add persistence for reading results
            Form f = Form.builder.AddQuestion("The question", "Well done for doing the exercices, how do you feel now ?")
                                 .SetTitle("Feedback about your work")
                                 .Build();
            preMenu.planning.AskForm(f);
            preMenu.planning.Save();
            FeedbackHelper.GetInstance().AddAndSaveToFile(f);
            cancelUse = false;
        }
    }
}
