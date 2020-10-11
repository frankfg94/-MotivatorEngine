using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask
{
    public class ShortPauseChoice : PreMenuChoice
    {
        public List<Delayer> delayers;

        public ShortPauseChoice()
        {
            showBeforeDay = true;
            showBeforeTask = true;
        }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancel)
        {
            throw new NotImplementedException();
        }
    }
    public class Delayer
    {
        public string displayName;
        public string description;
        public object triggers;
        public string type;
        public string duration;
        public bool? moreDelays;
    }






}
