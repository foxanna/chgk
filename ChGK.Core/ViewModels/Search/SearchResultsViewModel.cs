using ChGK.Core.Models;
using ChGK.Core.Services;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchResultsViewModel : MenuItemViewModel
    {
        readonly IChGKWebService _service;

        public SearchParams _searchParams;

        public SearchResultsViewModel(IChGKWebService service)
        {
            Title = StringResources.SearchResults;

            _service = service;
        }

        public void Init(string searchParams)
        {
            _searchParams = JsonConvert.DeserializeObject<SearchParams>(searchParams);
            _searchParams.Page = 0;
        }

        public override Task Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
