using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.Models
{
    public class SearchParams
    {
        public SearchParams()
        {
            HasQuestion = true;
            HasAnswer = true;
            HasPassCriteria = true;
            HasComment = true;
            AllWords = true;
        }

        public string SearchQuery { get; set; }

        public bool HasQuestion { get; set; }

        public bool HasAnswer { get; set; }

        public bool HasPassCriteria { get; set; }

        public bool HasComment { get; set; }

        public bool HasSourse { get; set; }

        public bool HasAuthors { get; set; }

        public bool AnyWord { get; set; }

        public bool AllWords { get; set; }

        public int Limit { get; set; }

        public int Page { get; set; }  
    }
}
