namespace ChGK.Core.Services
{
    public enum GACategory
    {
        PlayQuestion,
        QuestionsList
    }

    public enum GAAction
    {
        Click,
        Open,
        Timer
    }

    public interface IGAService
    {
        void ReportScreenEnter(string name);

        void ReportEvent(GACategory category, GAAction action, string label);
    }
}