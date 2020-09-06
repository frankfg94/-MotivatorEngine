using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    public class SkipTaskChoice : PreMenuChoice
    {
        public int canEnableIfLongThreesholdMilliseconds = 60 * 1000;
        public bool enableSkipTaskIfDifficult = true;


        public SkipTaskChoice()
        {
            this.showBeforeDay = false;
            this.showBeforeTask = true;
        }

        public override string GetDescription()
        {
            return "Skip this task without any consequences";
        }

        public override string GetName()
        {
            return "Skip the Task";
        }

        protected override void _Use(ref Day d, Task t, out bool cancelUse)
        {
            if(t != null)
            {
                t.IsFinished = true;
                cancelUse = false;
            } 
            else
            {
                Console.WriteLine("Error while using option: Task is null");
                cancelUse = true;
            }
        }
    }
}
