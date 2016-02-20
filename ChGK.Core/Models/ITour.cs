using System.Collections.Generic;

namespace ChGK.Core.Models
{
    public interface ITour
    {
        string FileName { get; }

        string Name { get; }

        string Editors { get; }

        List<IQuestion> Questions { get; }
    }

    internal class Tour : ITour
    {
        public string FileName { get; set; }

        public string Name { get; set; }

        public string Editors { get; set; }

        public List<IQuestion> Questions { get; set; }
    }
}