using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchQuestionsResultsViewModel : MenuItemViewModel
    {
        readonly IChGKWebService _service;

        public SearchParams _searchParams;

        public DataLoader DataLoader { get; private set; }

        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public SearchQuestionsResultsViewModel(IChGKWebService service)
        {
            Title = StringResources.SearchResults;

            _service = service;

            DataLoader = new DataLoader();
        }

        public async void Init(string searchParams)
        {
            _searchParams = JsonConvert.DeserializeObject<SearchParams>(searchParams);

            await DataLoader.LoadItemsAsync(LoadItems);
        }

        async Task LoadItems()
        {
            int oldCount = Questions != null ? Questions.Count : 0;

          // Questions = null;

            var questions = await _service.SearchQuestions(_searchParams, _cancellationTokenSource.Token);

            CanLoadMore = questions.Count > oldCount && questions.Count % _searchParams.Limit == 0;

            Questions = questions;
        }
        
        ICommand _loadMoreCommand;

        public ICommand LoadMoreCommand
        {
            get
            {
                return _loadMoreCommand ?? (_loadMoreCommand = new MvxCommand<int>(async (currentAmount) => {
                    _searchParams.Page++;
                    await DataLoader.LoadItemsAsync(LoadItems);
                }, (currentAmount) => CanLoadMore && !DataLoader.IsLoading));
            }
        }

        bool CanLoadMore;
        
        List<ISearchQuestionsResult> _questions;

        public List<ISearchQuestionsResult> Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
				RaisePropertyChanged(() => HasNoResults);
            }
        }
        
        public bool HasNoResults
        {
            get
            {
				return DataLoader.HasData && Questions != null && Questions.Count == 0;
            }
        }        

        MvxCommand<ISearchQuestionsResult> _showQuestionCommand;

        public MvxCommand<ISearchQuestionsResult> ShowQuestionCommand
        {
            get
            {
                _showQuestionCommand = _showQuestionCommand ?? new MvxCommand<ISearchQuestionsResult>(ShowQuestion);
                return _showQuestionCommand;
            }
        }

        void ShowQuestion(ISearchQuestionsResult question)
        {
            ShowViewModel<SearchQuestionSingleResultViewModel>(new { json = JsonConvert.SerializeObject(question)});        
        }
    }
}
