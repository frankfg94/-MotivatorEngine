using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotivatorEngine;
using MotivatorEngine.PreTask;
using MotivatorEngine.Tasks;
using MotivatorPluginCore;
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
            p.SetContent(new List<IWeek>
            {
                new IWeek(new List<AbstractDay>{
                new Day( new List<AbstractTask> { firstTask} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                }),
                new IWeek(new List<AbstractDay>{
                new Day( new List<AbstractTask> { secondTask } ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<AbstractTask> { new MockTask()} ),
                })
            });

            var rm = new ConsoleRoadmap(p);
            rm.ShowRoadmap();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Test save.json";
            new PlanningLoader().Save(path, p);
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
            p.SetContent(new List<IWeek>
            {
                new IWeek(new List<AbstractDay>{
                new Day( new List<AbstractTask> { firstTask} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                }),
                new IWeek(new List<AbstractDay>{
                new Day( new List<AbstractTask> { secondTask } ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<AbstractTask> { new MockTask()} ),
                })
            });
            var rm = new ConsoleRoadmap(p);
            rm.ShowRoadmap();
            var loader = new PlanningLoader();
            var json = loader.GetJson(p);
            AbstractPlanning loadedPlanningMemory = loader.LoadFromJson(json, typeof(MockPlanning));
            Assert.IsNotNull(loadedPlanningMemory);
            Assert.IsNotNull(loadedPlanningMemory.GetDays());
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Loaded planning test.json";
            loader.Save(path,loadedPlanningMemory);
            AbstractPlanning loadedPlanningWithFile = loader.Load(path, typeof(MockPlanning));
            Assert.IsNotNull(loadedPlanningWithFile);
            Assert.IsNotNull(loadedPlanningMemory.GetDays());
            File.Delete(path);

            Assert.AreEqual(index,p.currentDayIndex);

        }


        [TestMethod()]
        public void LoadConsoleTest()
        {
            var p = new ConsolePlanning();
            var firstTask = new MockTask();
            var secondTask = new MockTask();
            int index = 10;
            p.currentDayIndex = index;
            p.SetContent(new List<IWeek>
            {
                new IWeek(new List<AbstractDay>{
                new Day( new List<AbstractTask> { firstTask} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                }),
                new IWeek(new List<AbstractDay>{
                new Day( new List<AbstractTask> { secondTask } ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<AbstractTask> { new MockTask()} ),
                })
            });

            p.SetPreMenu(new PreMenu(new List<PreMenuChoice>
            {
                new ShortPauseChoice()
            }));
            var rm = new ConsoleRoadmap(p);
            rm.ShowRoadmap();
            var loader = new PlanningLoader();
            var json = loader.GetJson(p);
            AbstractPlanning loadedPlanningMemory = loader.LoadFromJson(json, typeof(ConsolePlanning));
            Assert.IsNotNull(loadedPlanningMemory);
            Assert.IsNotNull(loadedPlanningMemory.GetDays());
            Assert.IsNotNull(loadedPlanningMemory.preMenu.availableChoices);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Loaded planning test.json";
            loader.Save(path, loadedPlanningMemory);
            AbstractPlanning loadedPlanningWithFile = loader.Load(path, typeof(ConsolePlanning));
            Assert.IsNotNull(loadedPlanningWithFile);
            Assert.IsNotNull(loadedPlanningMemory.GetDays());
            Assert.IsTrue(loadedPlanningMemory.GetDays().Count > 0);
            Assert.IsNotNull(loadedPlanningMemory.preMenu.availableChoices);
            File.Delete(path);

            Assert.AreEqual(index, p.currentDayIndex);

        }

    }
}