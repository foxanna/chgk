using Cirrious.MvvmCross.ViewModels;
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
            Limit = 50;
            Type = "types1";
            EndDate = DateTime.Today;
            StartDate = new DateTime(1990, 1, 1);
        }

        public SearchParams(SearchParams copy)
        {
            SearchQuery = copy.SearchQuery;
            HasQuestion = copy.HasQuestion;
            HasAnswer = copy.HasAnswer;
            HasPassCriteria = copy.HasPassCriteria;
            HasComment = copy.HasComment;
            HasSourse = copy.HasSourse;
            HasAuthors = copy.HasAuthors;
            AllWords = copy.AllWords;
            AnyWord = copy.AnyWord;
            Limit = copy.Limit;
            Type = copy.Type;
            EndDate = copy.EndDate;
            StartDate = copy.StartDate;
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

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Limit { get; set; }

        public int Page { get; set; }

        public string Type { get; set; }

        public bool Equals(SearchParams other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(SearchQuery, other.SearchQuery)
                && string.Equals(Type, other.Type)
                && HasQuestion == other.HasQuestion
                && HasAnswer == other.HasAnswer
                && HasPassCriteria == other.HasPassCriteria
                && HasComment == other.HasComment
                && HasSourse == other.HasSourse
                && HasAuthors == other.HasAuthors
                && AnyWord == other.AnyWord
                && AllWords == other.AllWords;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SearchParams)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SearchQuery != null ? SearchQuery.GetHashCode() : 0) * 397) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }
    }
}
