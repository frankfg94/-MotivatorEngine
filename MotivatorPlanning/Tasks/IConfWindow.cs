namespace MotivatorEngine
{
    public interface IConfWindow
    {
        bool ShowAndWaitForChoices();
        void Close();
        string Title { get; set; }

        IMotivatorWindow GetWindow();
    }
}