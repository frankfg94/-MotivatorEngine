using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotivatorEngine;
using MotivatorEngine.PreTask.Choices;
using MotivatorEngine.Tasks;
using MotivatorPluginCore;
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
            new Thread(() =>
            {
                // your code
                var p = new MockPlanning();
                var firstTask = new MockTask();
                var secondTask = new MockTask();
                p.SetContent(new List<IWeek>
                { 
                    new IWeek(new List<AbstractDay>{
                    new Day( new List<AbstractTask> { firstTask, new MockTask()} ),
                    new Day(),
                    new Day(), 
                    new Day(),
                    new Day(), 
                    new Day(),
                    new Day(),
                    }),
                    new IWeek(new List<AbstractDay>{
                    new Day( new List<AbstractTask> { secondTask, new MockTask() } ),
                    new Day(),
                    new Day(),
                    new Day(),
                    new Day(),
                    new Day(),
                    new Day( new List<AbstractTask> { new MockTask()} ),
                    })
                });
            p.SetPreMenu(new MotivatorEngine.PreTask.PreMenu(new List<MotivatorEngine.PreTask.PreMenuChoice> { 
                new GiveupChoice()
            }));
            p.StartFirstTime();
            }).Start();
            Thread.Sleep(2000);
        }
    }
}
