using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MotivatorEngine.Tasks
{
    public abstract class WPFTask : AbstractTask
    {
        protected WPFTask()
        {
        }

        public override void ShowMessageBoxSync(string msg)
        {
            MessageBox.Show(msg);
        }

        public override void RunTaskSync()
        {
            var t = new Thread(() =>
            {
                Console.WriteLine("Starting GUI window");
                window = GetWindow();
                window.Closed += Window_Closed;
                // Application.Current.Get is null
                window.Show();
                try
                {
                    Dispatcher.Run();
                }
                catch (Exception) { }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

    }
}
