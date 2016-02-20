namespace ChGK.Core.Models
{
    public interface ISearchQuestionsResult
    {
        string Id { get; }

        int Number { get; }

        string Text { get; }

        string Answer { get; }

        string Comment { get; }

        string Author { get; }

        string Source { get; }

        string Picture { get; }

        string PassCriteria { get; }

        string TourName { get; }

        string TourFileName { get; }

        string TournamentName { get; }

        string TournamentFileName { get; }
    }

    internal class SearchQuestionsResult : ISearchQuestionsResult
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public string Text { get; set; }

        public string Answer { get; set; }

        public string Comment { get; set; }

        public string Author { get; set; }

        public string Source { get; set; }

        public string Picture { get; set; }

        public string PassCriteria { get; set; }

        public string TourName { get; set; }

        public string TourFileName { get; set; }

        public string TournamentName { get; set; }

        public string TournamentFileName { get; set; }
    }
}