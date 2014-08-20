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

        public override Task Refresh()
        {
            throw new NotImplementedException();
        }

        public string HasQuestionTitle { get; set; }
    }
}
