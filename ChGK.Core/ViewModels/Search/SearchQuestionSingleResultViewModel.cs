using ChGK.Core.Models;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchQuestionSingleResultViewModel : MenuItemViewModel
    {
        public ISearchQuestionsResult Question { get; set; }

        public void Init(string json)
        {
            Question = JsonConvert.DeserializeObject<SearchQuestionsResult>(json);

            HasPicture = !string.IsNullOrEmpty(Question.Picture);
            Picture = "http://db.chgk.info/images/db/" + Question.Picture;

            HasComments = !string.IsNullOrEmpty(Question.Comment);
            HasAuthor = !string.IsNullOrEmpty(Question.Author);
            HasSource = !string.IsNullOrEmpty(Question.Source);
            HasPassCriteria = !string.IsNullOrEmpty(Question.PassCriteria);

            TourName = string.Format("<u>{0}</u>", Question.TourName);
            TournamentName = string.Format("<u>{0}</u>", Question.TournamentName);

            Title = StringResources.Question + " " + Question.Number;
        }
        
        public string Picture { get; set; }

        public string TourName{ get; set; }

        public string TournamentName { get; set; }

        public bool HasComments { get; set; }

        public bool HasAuthor { get; set; }

        public bool HasSource { get; set; }

        public bool HasPicture { get; set; }

        public bool HasPassCriteria { get; set; }

        MvxCommand _openImageCommand;

        public MvxCommand OpenImageCommand
        {
            get
            {
                return _openImageCommand ?? (_openImageCommand =
                    new MvxCommand(() => ShowViewModel<FullImageViewModel>(new { image = Picture })));
            }
        }

       MvxCommand _openTourCommand;
       public MvxCommand  OpenTourCommand
        {
            get
            {
                return _openTourCommand ?? (_openTourCommand = new MvxCommand(() => ShowViewModel<TourViewModel>(new { name = Question.TourName, filename = Question.TourFileName })));
            }
        }

       MvxCommand _openTournamentCommand;
       public MvxCommand OpenTournamentCommand
       {
           get
            {
                return _openTournamentCommand ?? (_openTournamentCommand = new MvxCommand(() => { }));
            }
       }
    }
}
