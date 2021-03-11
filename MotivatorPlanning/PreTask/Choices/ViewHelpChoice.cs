using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.Choices
{
    public class ViewHelpChoice : PreMenuChoice
    {
        public override string GetDescription()
        {
            return "Describe the other choices";
        }

        public override string GetName()
        {
            return "View Help";
        }


        private string getTabString(int count)
        {
            string s = "";
            for(int i = 0; i < count; i++)
            {
                s+="\t";
            }
            return s;
        }

        protected override void _Use(ref AbstractDay d, AbstractTask t, out bool cancelUse)
        {
            Console.WriteLine(" {0,-50} | {1}","Option","Description\n");
            preMenu.availableChoices.ForEach(delegate(PreMenuChoice c)
            {
                string s = $"{ c.GetName() }";
                int tabCount =  s.Length / 4;
                Console.WriteLine(" {0,-50} | {1}",c.GetName(),c.GetDescription());
            });
            Console.WriteLine("///////////////////////////////////////////");
            cancelUse = false;
        }
    }
}
