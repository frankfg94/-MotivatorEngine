using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotivatorEngine;
using MotivatorEngine.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MotivatorEngine.Tests
{
    [TestClass()]
    public class PlanningManagerTests
    {

        [TestMethod()]
        public void SaveTest()
        {
            var p = new MockPlanning();
            var firstTask = new MockTask();
            var secondTask = new MockTask();
            p.SetContent(new List<Week>
            {
                new Week(new List<Day>{
                new Day( new List<Task> { firstTask} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                }),
                new Week(new List<Day>{
                new Day( new List<Task> { secondTask } ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<Task> { new MockTask()} ),
                })
            });
            Roadmap rm = new Roadmap(p);
            rm.PrintRoadmap();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Test save.json";
            PlanningManager.Save(path, p);
            //File.Delete(path);
        }

        [TestMethod()]
        public void LoadTest()
        {
            var p = new MockPlanning();
            var firstTask = new MockTask();
            var secondTask = new MockTask();
            int index = 10;
            p.currentDayIndex = index;
            p.SetContent(new List<Week>
            {
                new Week(new List<Day>{
                new Day( new List<Task> { firstTask} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                }),
                new Week(new List<Day>{
                new Day( new List<Task> { secondTask } ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<Task> { new MockTask()} ),
                })
            });
            Roadmap rm = new Roadmap(p);
            rm.PrintRoadmap();
            var json = PlanningManager.GetJson(p);
            Planning loadedPlanningMemory = PlanningManager.LoadFromJson(json, typeof(MockPlanning));
            Assert.IsNotNull(loadedPlanningMemory);
            Assert.IsNotNull(loadedPlanningMemory.GetDays());
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Loaded planning test.json";
            PlanningManager.Save(path,loadedPlanningMemory);
            Planning loadedPlanningWithFile = PlanningManager.Load(path, typeof(MockPlanning));
            Assert.IsNotNull(loadedPlanningWithFile);
            Assert.IsNotNull(loadedPlanningMemory.GetDays());
            File.Delete(path);

            Assert.AreEqual(index,p.currentDayIndex);

        }

    }
}