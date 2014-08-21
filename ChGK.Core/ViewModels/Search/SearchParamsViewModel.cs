using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchParamsViewModel : MenuItemViewModel
    {
        public SearchParams SearchParams { get; set; }

        public SearchParamsViewModel()
		{
            Title = StringResources.Search;

            SearchParams = new SearchParams();

            HasQuestionTitle = StringResources.HasQuestionTitle;
            HasAnswerTitle = StringResources.HasAnswerTitle;
            HasPassCriteriaTitle  = StringResources.HasPassCriteriaTitle;
            HasCommentTitle  = StringResources.HasCommentTitle;
            HasSourseTitle  = StringResources.HasSourseTitle;
            HasAuthorsTitle  = StringResources.HasAuthorsTitle;

            AnyWordTitle  = StringResources.AnyWordTitle;
            AllWordsTitle = StringResources.AllWordsTitle;
		}

        ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new MvxCommand(() => ShowViewModel<SearchResultsViewModel>(
                    new { searchParams = JsonConvert.SerializeObject(SearchParams) })));
            }
        }

        public string HasQuestionTitle { get; set; }
        public string HasAnswerTitle { get; set; }
        public string HasPassCriteriaTitle { get; set; }
        public string HasCommentTitle { get; set; }
        public string HasSourseTitle { get; set; }
        public string HasAuthorsTitle { get; set; }

        public string AnyWordTitle { get; set; }
        public string AllWordsTitle { get; set; }
    }
}
