using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.Tasks
{
    public class MockMotivatorWindow : IMotivatorWindow
    {
        private string title;
        public event EventHandler Closed;


        public MockMotivatorWindow(string title)
        {
            this.title = title;
        }


        public string GetTitle()
        {
            return this.title;
        }

        public void Show()
        {
            Console.WriteLine("\t>> Mock window is now visible");
            Close();
        }

        public void Close()
        {
            Console.WriteLine("\t>> Closed the window (Task is finished)");
            Closed?.Invoke(this, null);
        }
    }
}
