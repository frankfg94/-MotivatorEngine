using System;

namespace MotivatorEngine
{
    public interface IMotivatorWindow
    {

        void Show();
        void Close();

        string GetTitle();
        event EventHandler Closed;

    }
}