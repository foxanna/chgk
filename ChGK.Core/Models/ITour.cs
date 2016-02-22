using System.Collections.Generic;

namespace ChGK.Core.Models
{
    public interface ITour
    {
        string Id { get; }

        string Name { get; }

        string Editors { get; }

        List<IQuestion> Questions { get; }
    }

    internal class Tour : ITour
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Editors { get; set; }

        public List<IQuestion> Questions { get; set; }
    }
}