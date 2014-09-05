using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Utils;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchQuestionsResultsViewModel : MenuItemViewModel
    {
        readonly IChGKWebService _service;

        public SearchParams _searchParams;

        public DataLoader DataLoader { get; private set; }

        LoadMoreHelper<ISearchQuestionsResult> _loadMoreHelper;

        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public SearchQuestionsResultsViewModel(IChGKWebService service)
        {
            Title = StringResources.SearchResults;

            _service = service;

            DataLoader = new DataLoader();
            _loadMoreHelper = new LoadMoreHelper<ISearchQuestionsResult>() { OnLastItemShown = SearchQuestionsResultsViewModel_Showing };
        }

        public async void Init(string searchParams)
        {
            _searchParams = JsonConvert.DeserializeObject<SearchParams>(searchParams);

            DataLoader.IsLoadingForTheFirstTime = true;
            await DataLoader.LoadItemsAsync(LoadItems);
        }
        
        async Task LoadItems()
        {   
            var questions = await _service.SearchQuestions(_searchParams, _cancellationTokenSource.Token);

            CheckIfThereIsMoreDataToLoad(questions.Count, Questions != null ? Questions.Count : 0);
            UpdatePageAccordingToNewdata(questions.Count);

            Questions = questions.Select(t => new LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult>() { Item = t }).ToList();

            DisplayErrorIfNoData();
            _loadMoreHelper.Subscribe(Questions);         
        }
        
        void DisplayErrorIfNoData()
        {
            if (Questions.Count == 0)
            {
                DataLoader.Error = StringResources.NothingFound;
            }
        }

        void CheckIfThereIsMoreDataToLoad(int newCount, int oldCount)
        {
            CanLoadMore = newCount > oldCount && newCount % _searchParams.Limit == 0;
        }

        void UpdatePageAccordingToNewdata(int newCount)
        {
            _searchParams.Page = newCount / (_searchParams.Limit + 1);
        }

        async void SearchQuestionsResultsViewModel_Showing()
        {
            if (CanLoadMore && !DataLoader.IsLoading)
            {
                _searchParams.Page++;

                DataLoader.IsLoadingForTheFirstTime = false;
                DataLoader.IsLoadingMoreData = true;
                
                await DataLoader.LoadItemsAsync(LoadItems);
            }
        }

        bool CanLoadMore;

        List<LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult>> _questions;

        public List<LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult>> Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        MvxCommand<LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult>> _showQuestionCommand;

        public MvxCommand<LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult>> ShowQuestionCommand
        {
            get
            {
                return _showQuestionCommand ?? (_showQuestionCommand = new MvxCommand<LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult>>(ShowQuestion));
            }
        }

        void ShowQuestion(LoadMoreOnScrollListViewItemViewModel<ISearchQuestionsResult> question)
        {
            ShowViewModel<SearchQuestionSingleResultViewModel>(new { json = JsonConvert.SerializeObject(question.Item)});        
        }

        public override void OnViewDestroying()
        {
            _cancellationTokenSource.Cancel();

            base.OnViewDestroying();
        }
    }
}
