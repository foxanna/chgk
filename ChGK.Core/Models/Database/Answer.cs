namespace ChGK.Core.Models.Database
{
    internal class Answer : DatabaseModel
    {
        public string QuestionId { get; set; }

        public int TeamId { get; set; }
    }
}