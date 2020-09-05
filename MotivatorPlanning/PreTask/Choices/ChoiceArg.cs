using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    public class ChoiceArg : EventArgs
    {
        public PreMenuChoice selectedChoice;
        public Day day;
        public Task task;
    }
}
