using System;
using System.Windows.Input;
using ChGK.Core.Models;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchParamsViewModel : MenuItemViewModel
    {
        private ICommand _searchCommand;

        public SearchParamsViewModel()
        {
            Title = StringResources.Search;

            SearchParams = new SearchParams();
        }

        private SearchParams SearchParams { get; }

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new MvxCommand(DoSearch)); }
        }

        public bool SearchAmongQuestions
        {
            get { return SearchParams.SearchAmongQuestions; }
            set
            {
                SearchParams.SearchAmongQuestions = value;
                if (value)
                {
                    SearchAmongTours = false;
                    SearchAmongUnsorted = false;
                }
                RaisePropertyChanged(() => SearchAmongQuestions);
            }
        }

        public bool SearchAmongTours
        {
            get { return SearchParams.SearchAmongTours; }
            set
            {
                SearchParams.SearchAmongTours = value;
                if (value)
                {
                    SearchAmongQuestions = false;
                    SearchAmongUnsorted = false;
                }
                RaisePropertyChanged(() => SearchAmongTours);
            }
        }

        public bool SearchAmongUnsorted
        {
            get { return SearchParams.SearchAmongUnsorted; }
            set
            {
                SearchParams.SearchAmongUnsorted = value;
                if (value)
                {
                    SearchAmongQuestions = false;
                    SearchAmongTours = false;
                }
                RaisePropertyChanged(() => SearchAmongUnsorted);
            }
        }

        public string SearchQuery
        {
            get { return SearchParams.SearchQuery; }
            set
            {
                SearchParams.SearchQuery = value;
                RaisePropertyChanged(() => SearchQuery);
            }
        }

        public bool HasQuestion
        {
            get { return SearchParams.HasQuestion; }
            set
            {
                SearchParams.HasQuestion = value;
                RaisePropertyChanged(() => HasQuestion);
            }
        }

        public bool HasAnswer
        {
            get { return SearchParams.HasAnswer; }
            set
            {
                SearchParams.HasAnswer = value;
                RaisePropertyChanged(() => HasAnswer);
            }
        }

        public bool HasPassCriteria
        {
            get { return SearchParams.HasPassCriteria; }
            set
            {
                SearchParams.HasPassCriteria = value;
                RaisePropertyChanged(() => HasPassCriteria);
            }
        }

        public bool HasComment
        {
            get { return SearchParams.HasComment; }
            set
            {
                SearchParams.HasComment = value;
                RaisePropertyChanged(() => HasComment);
            }
        }

        public bool HasSourse
        {
            get { return SearchParams.HasSourse; }
            set
            {
                SearchParams.HasSourse = value;
                RaisePropertyChanged(() => HasSourse);
            }
        }

        public bool HasAuthors
        {
            get { return SearchParams.HasAuthors; }
            set
            {
                SearchParams.HasAuthors = value;
                RaisePropertyChanged(() => HasAuthors);
            }
        }

        public DateTime StartDate
        {
            get { return SearchParams.StartDate; }
            set
            {
                SearchParams.StartDate = value;
                RaisePropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get { return SearchParams.EndDate; }
            set
            {
                SearchParams.EndDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }

        public bool AnyWord
        {
            get { return SearchParams.AnyWord; }
            set
            {
                SearchParams.AnyWord = value;
                if (value)
                {
                    AllWords = false;
                }
                RaisePropertyChanged(() => AnyWord);
            }
        }


        public bool AllWords
        {
            get { return SearchParams.AllWords; }
            set
            {
                SearchParams.AllWords = value;
                if (value)
                {
                    AnyWord = false;
                }
                RaisePropertyChanged(() => AllWords);
            }
        }

        private void DoSearch()
        {
            if (SearchAmongQuestions)
            {
                if (CanSearch())
                {
                    ShowViewModel<SearchQuestionsResultsViewModel>(
                        new {searchParams = JsonConvert.SerializeObject(SearchParams)});
                }
            }
        }

        public bool CanSearchWithThisParams()
        {
            return (SearchParams.HasAnswer || SearchParams.HasAuthors || SearchParams.HasComment ||
                    SearchParams.HasPassCriteria || SearchParams.HasQuestion || SearchParams.HasSourse);
        }

        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(SearchParams.SearchQuery) && CanSearchWithThisParams();
        }
    }
}