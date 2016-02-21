using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Utils;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchQuestionSingleResultViewModel : MenuItemViewModel
    {
        private Command _openImageCommand, _openTourCommand, _openTournamentCommand;

        public ISearchQuestionsResult Question { get; set; }

        public string Picture { get; set; }

        public string TourName { get; set; }

        public string TournamentName { get; set; }

        public bool HasComments { get; set; }

        public bool HasAuthor { get; set; }

        public bool HasSource { get; set; }

        public bool HasPicture { get; set; }

        public bool HasPassCriteria { get; set; }

        public ICommand OpenImageCommand => _openImageCommand ?? (_openImageCommand =
            new Command(() => ShowViewModel<FullImageViewModel>(new {image = Picture})));

        public ICommand OpenTourCommand => _openTourCommand ?? (_openTourCommand = new Command(
            () => ShowViewModel<TourViewModel>(new {name = Question.TourName, path = Question.TourFileName})));

        public ICommand OpenTournamentCommand => _openTournamentCommand ?? (_openTournamentCommand =
            new Command(() => ShowViewModel<SingleTournamentViewModel>(new {filename = Question.TournamentFileName})));

        public void Init(string json)
        {
            Question = JsonConvert.DeserializeObject<SearchQuestionsResult>(json);

            HasPicture = !string.IsNullOrEmpty(Question.Picture);
            Picture = "http://db.chgk.info/images/db/" + Question.Picture;

            HasComments = !string.IsNullOrEmpty(Question.Comment);
            HasAuthor = !string.IsNullOrEmpty(Question.Author);
            HasSource = !string.IsNullOrEmpty(Question.Source);
            HasPassCriteria = !string.IsNullOrEmpty(Question.PassCriteria);

            TourName = $"<u>{Question.TourName}</u>";
            TournamentName = $"<u>{Question.TournamentName}</u>";

            Title = StringResources.Question + " " + Question.Number;
        }
    }
}