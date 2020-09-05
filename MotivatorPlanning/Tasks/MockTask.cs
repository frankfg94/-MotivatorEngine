using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace MotivatorEngine.Tasks
{
    public class MockTask : Task
    {
        public override TimeSpan EstimatedDuration { get; set; } = TimeSpan.FromSeconds(5);
        public override int EstimatedDifficulty { get; set; } = 1;

        protected override TimeSpan SecurityDuration => TimeSpan.FromSeconds(120);

        public override TaskInfos Infos { get; set; } = new TaskInfos
        {
            description = "Task used for testing purposes, does nothing",
            difficultyLvl = 1,
            durationLvl = 1,
            illustrationImagePath = "",
            name = "Mock Task",
            shortDescription = "Task used for testing purposes, does nothing"
        };

        protected override IConfWindow GetConfWindow()
        {
            return new MockConfWindow
            {
                Title = "Mock configuration window"
            };
        }


        protected override IMotivatorWindow _GetWindow()
        {
            return new MockMotivatorWindow("Mock window obtained directly");
        }

        protected override void Window_Closed(object sender, EventArgs e)
        {
            base.Window_Closed(sender, e);

            // To avoid letting the program finish
            Thread.Sleep(500);
        }
    }
}
