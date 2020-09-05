using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotivatorEngine;
using MotivatorEngine.Tasks;
using MotivatorEngineTests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MotivatorEngine.Tests
{
    [TestClass()]
    public class PlanningTests
    {

        [TestMethod()]
        public void GetDaysTest()
        {
            var p = new MockPlanning();
            if (p.GetTasks() != null)
            {
                Assert.Fail("The planning had the day list initialized but we didn't add them");
            }
            p.SetContent(new List<Week>
            {
                new Week(new List<Day>{
                new Day(new List<Task>{
                    new MockTask()
                }),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day()
                })
            });
        }

        [TestMethod()]
        public void GetTasksTest()
        {
            var p = new MockPlanning();
            if (p.GetDays() != null)
            {
                Assert.Fail("The planning had the day list initialized but we didn't add them");
            }
            p.SetContent(new List<Week>
            {
                new Week(new List<Day>{
                new Day( new List<Task> { new MockTask()} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day()
                }),
                new Week(new List<Day>{
                new Day( new List<Task> { new MockTask()} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<Task> { new MockTask()} ),
                })
            });

            if (p.GetTasks().Count != 3)
            {
                Assert.Fail("There should be 14 days after we added 2 weeks");
            }
        }
        [TestMethod()]
        public void VerifyCurrentDayTest()
        {
            var curDay = new Day();
            var daySecondWeek = new Day();
            var p = new MockPlanning();

            p.SetContent(new List<Week>
            {
                new Week(new List<Day>{
                new Day( new List<Task> { new MockTask()} ),
                new Day(),
                new Day(),
                curDay,
                new Day(),
                new Day(),
                new Day()
                }),
                new Week(new List<Day>{
                new Day( new List<Task> { new MockTask()} ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                daySecondWeek,
                new Day( new List<Task> { new MockTask()} ),
                })
            });
            if (p.GetCurrentDay() == null)
            {
                Assert.Fail("The first day should be set automatically when setting the content");
            }

            p.currentDayIndex = 3;
            Assert.AreEqual(p.GetCurrentDay(), curDay);
            p.currentDayIndex = 12;
            Assert.AreEqual(p.GetCurrentDay(), daySecondWeek);
        }

        [TestMethod()]
        public void DecalDayTest()
        {
            var p = new MockPlanning();
            var dayToDecal = new Day(new List<Task> {
                new MockTask()
            });
            p.SetContent(new List<Week>
            {
                new Week
                (
                    new List<Day>
                    {
                        dayToDecal,
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day()
                    }
                )
            });
            var curDayBeforeDecal = 0;
            // At first its normal that the day isn't as pos 1, its pos 0
            Assert.AreNotEqual(p.GetDays()[1], dayToDecal);
            p.DecalDay(dayToDecal);
            // But we decaled it
            Assert.AreEqual(p.GetDays()[1], dayToDecal);
            // After we decaled the day, the current day index didn't change, we don't want to start the next day & work!
            Assert.IsTrue(curDayBeforeDecal == 0);
            // Verify that the week is unchanged
            // Check If the task is still available, it should be available because we didn't cancel the task, we posponed it
            Assert.IsTrue(p.GetNextPlannedTask() != null);
        }

        [TestMethod()]
        public void DecalWeekTest()
        {
            var p = new MockPlanning();
            var dayToDecal = new Day(new List<Task> {
                new MockTask()
            });
            p.SetContent(new List<Week>
            {
                new Week
                (
                    new List<Day>
                    {
                        dayToDecal,
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day()
                    }
                )
            });
            var curDayBeforeDecal = 0;
            // At first its normal that the day isn't as pos 1, its pos 0
            Assert.AreNotEqual(p.GetDays()[1], dayToDecal);
            //Roadmap r = new Roadmap(p);
            //r.PrintRoadmap();
            p.DecalWeek(dayToDecal);
            // But we decaled it
            //r.PrintRoadmap();
            Assert.AreEqual(p.GetDays()[7], dayToDecal);
            // After we decaled the day, the current day index didn't change, we don't want to start the next day & work!
            Assert.IsTrue(curDayBeforeDecal == 0);
            // Verify that the week is unchanged
            // Check If the task is still available, it should be available because we didn't cancel the task, we posponed it
            Assert.IsTrue(p.GetNextPlannedTask() != null);
        }

        [TestMethod()]
        public void SkipDayTest()
        {
            var p = new MockPlanning();
            var dayToSkip = new Day(new List<Task> {
                new MockTask()
            });
            p.SetContent(new List<Week>
            {
                new Week
                (
                    new List<Day>
                    {
                        dayToSkip,
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day()
                    }
                )
            });
            var curDayBeforeSkip = 0;
            // The current day, which is also the day to skip, has a task
            Assert.IsNotNull(p.GetNextPlannedTask());

            // At first its normal that the day isn't as pos 1, its pos 0
            Assert.AreNotEqual(p.GetDays()[1], dayToSkip);
            p.SkipDay(dayToSkip);

            // We verify that we are still the same day
            Assert.AreEqual(0, curDayBeforeSkip);

            // But we skipped it, so we don't have any task to do, so null reference for the task
            Assert.IsNull(p.GetNextPlannedTask());

            // Skip the planning until we reach the end to see if it will finish
        }

        [TestMethod()]
        public void GetWeekNumberTest()
        {
            var p = new MockPlanning();
            var dayToDecal = new Day();
            p.SetContent(new List<Week>
            {
                new Week
                (
                    new List<Day>
                    {
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        dayToDecal
                    }
                ),
                new Week
                (
                    new List<Day>
                    {
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day()
                    }
                )
            });

            Assert.AreEqual(1, p.GetWeekNumber(dayToDecal));
            p.DecalDay(dayToDecal); // We arrive at monday the next week
            Assert.AreEqual(2, p.GetWeekNumber(dayToDecal));
            for (int i = 0; i < 6; i++) { p.DecalDay(dayToDecal); };
            // We arrive at sunday the same week
            Assert.AreEqual(2, p.GetWeekNumber(dayToDecal));
            // We decal one day, we arrive at the monday next week
            p.DecalDay(dayToDecal);
            Assert.AreEqual(3, p.GetWeekNumber(dayToDecal));
        }

        [TestMethod()]
        public void SkipDaysUntilTaskTest()
        {
            var p = new MockPlanning();
            var firstDay = new Day();
            var dayToDecal = new Day(new List<Task>
            {
                new MockTask(),
                new MockTask()
            });
            var dayWithTasks = new Day(new List<Task>
            {
                new MockTask(),
                new MockTask()
            });
            p.SetContent(new List<Week>
            {
                new Week
                (
                    new List<Day>
                    {
                        firstDay,
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        new Day(),
                        dayToDecal // 6
                    }
                ),
                new Week
                (
                    new List<Day>
                    {
                        new Day(), //7
                        new Day(), //8
                        new Day(), //9
                        new Day(), //10
                        new Day(), //11
                        new Day(), //12
                        dayWithTasks //13
                    }
                )
            });
            Assert.AreEqual(0, p.currentDayIndex);
            p.SkipDaysUntilTask(firstDay); // should set the current day by jumping all the empty days
            Assert.AreEqual(6, p.currentDayIndex);
            p.SkipDaysUntilTask(dayToDecal);
            Assert.AreEqual(13, p.currentDayIndex);
        }


        [TestMethod()]
        public void GetNextPlannedTaskTest()
        {
            var p = new MockPlanning();
            var firstTask = new MockTask();
            var secondTask = new MockTask();
            var lastTask = new MockTask();
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
                new Day( new List<Task> { lastTask } ),
                })
            });
            var curDay = p.GetCurrentDay();
            var resultFirstTask = p.GetNextPlannedTask();
            Assert.AreEqual(firstTask, resultFirstTask);
            firstTask.IsFinished = true;
            var resultSecondTask = p.GetNextPlannedTask();
            Assert.AreEqual(secondTask, resultSecondTask);
            // Verify that the current task has no difference with the current day
            Assert.AreEqual(curDay, p.GetCurrentDay());
            Assert.AreEqual(secondTask, p.GetNextPlannedTask());
            p.currentDayIndex = 10;
            Assert.AreEqual(lastTask, p.GetNextPlannedTask());
        }

        [TestMethod()]
        public void GetWeekCountTest()
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

            Assert.AreEqual(2, p.GetWeekCount());
            p.AddDay(new Day());
            Assert.AreEqual(15, p.GetDays().Count);
            Assert.AreEqual(3, p.GetWeekCount());
        }

        [TestMethod()]
        public void PrintRoadmapTest()
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
            p.currentDayIndex = 13;
            rm.PrintRoadmap();
        }

        [TestMethod()]
        public void PrintGetRoadmapTest()
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
                new Day( new List<Task> { secondTask, new MockTask() } ),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day(),
                new Day( new List<Task> { new MockTask()} ),
                })
            });
            p.currentDayIndex = 13;
            Roadmap r = new Roadmap(p);
            var text = r.GetRoadmapText();
            Assert.IsNotNull(text);
            Console.WriteLine(text);
        }

    }
}