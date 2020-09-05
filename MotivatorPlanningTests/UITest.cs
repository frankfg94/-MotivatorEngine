using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotivatorEngine;
using MotivatorEngine.PreTask.Choices;
using MotivatorEngine.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace MotivatorEngineTests
{

    [TestClass()]
    public class UITest
    {


        [TestMethod()]
        public void SPEED_OF_LIGHT_TEST()
        {
                // your code
                var p = new MockPlanning();
                var firstTask = new MockTask();
                var secondTask = new MockTask();
                p.SetContent(new List<Week>
                {
                    new Week(new List<Day>{
                    new Day( new List<Task> { firstTask, new MockTask()} ),
                    new Day(),
                    new Day(), 
                    new Day(),
                    new Day(), 
                    new Day(),
                    new Day(),
                    }),
                    new Week(new List<Day>{
                    new Day( new List<Task> { secondTask, new MockTask() } ),
                    new Day(),
                    new Day(),
                    new Day(),
                    new Day(),
                    new Day(),
                    new Day( new List<Task> { new MockTask()} ),
                    })
                });
            p.SetPreMenu(new MotivatorEngine.PreTask.PreMenu(new List<MotivatorEngine.PreTask.PreMenuChoice> { 
                new GiveupChoice()
            }));
            p.StartFirstTime();
        }
    }
}
