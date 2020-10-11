using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine
{
    public class Day : AbstractDay
    {

        public Day() { }

        public Day(List<AbstractTask> tasks){
            if(tasks == null || tasks.Count == 0)
            {
                throw new ArgumentException("If you pass a tasks parameter, please initialize the list and set at list one task");
            }
            this.tasks = tasks;
        }

    }
}
