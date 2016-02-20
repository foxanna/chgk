namespace ChGK.Core.Models
{
    internal class Answer : DatabaseModel
    {
        public string QuestionId { get; set; }

        public int TeamId { get; set; }
    }
}