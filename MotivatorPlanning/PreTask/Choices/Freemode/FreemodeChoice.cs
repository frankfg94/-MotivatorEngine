using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    public class FreemodeChoice : PreMenuChoice
    {
        public event EventHandler<FreemodeArg> SelectedByUser;
        public FreemodeChoice()
        {
            refillOptions = new RefillOptions(0, 999)
            {
                difficultyRefill = true,
                timeConsumingRefill= true
            };
            showBeforeTask = true;
            showBeforeDay = false;
        }

        public override bool IsSelectable(out string msg)
        {

            var curDay = preMenu.planning.GetCurrentDay();
            if(curDay.tasks == null || curDay.isEmpty())
            {
                msg = "We can't choose the task to start because the day doesn't have any";
                return false;
            }
            var remainingTasks =  curDay.tasks.FindAll(x => !x.IsFinished);
            bool atLeastTwoTasks = remainingTasks.Count >= 2; // Check that there is at least two AbstractTasks to do
            // We need at least two AbstractTasks to use freemode, to choose between the two AbstractTasks
            
            if(!atLeastTwoTasks)
            {
                msg = "No need to choose the task if you have 1 task only";
                return false;
            }

            return base.IsSelectable(out msg)  ;
        }
        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancel)
        {
            // Ask to select a AbstractTask in the planning implementation
            t = preMenu.planning.AskTaskToDo(d,t);
            if(t == null)
            {
                cancel = true;
            } 
            else
            {
                cancel = false;
                Console.WriteLine("Selected");
                preMenu.planning.OnRequestSelectNewTask(t);
                SelectedByUser?.Invoke(this, new FreemodeArg { selectedTask = t });
            }
        }

        public override string GetName()
        {
            return "Free Choice Mode";
        }

        public override string GetDescription()
        {
            return "You can select the next task you want to execute";
        }
    }
}
