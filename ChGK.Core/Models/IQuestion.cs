namespace ChGK.Core.Models
{
    public interface IQuestion
    {
        string Id { get; }

        string Text { get; }

        string Answer { get; }

        string Comment { get; }

        string Author { get; }

        string Source { get; }

        string Picture { get; }

        string PassCriteria { get; }

        string Gearbox { get; }
    }

    internal class Question : IQuestion
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string Answer { get; set; }

        public string Comment { get; set; }

        public string Author { get; set; }

        public string Source { get; set; }

        public string Picture { get; set; }

        public string PassCriteria { get; set; }

        public string Gearbox { get; set; }

        public override bool Equals(object obj)
        {
            var question = obj as Question;
            return Id.Equals(question?.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}