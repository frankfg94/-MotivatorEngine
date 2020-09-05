using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MotivatorEngine.Tasks
{
    class MockConfWindow : IConfWindow
    {
        public string Title { get; set; }

        public void Close()
        {
            Console.WriteLine("Mock conf window closed");
        }

        public void ForceStart()
        {
            Console.WriteLine("Mock conf window started");
        }

        public IMotivatorWindow GetWindow()
        {
            return new MockMotivatorWindow("Mock Window obtained with configuration");
        }

        public bool ShowAndWaitForChoices()
        {
            Console.WriteLine("Mock conf window is now shown");
            return true;
        }
    }
}
