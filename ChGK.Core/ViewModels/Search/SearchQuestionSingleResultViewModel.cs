using ChGK.Core.Models;
using ChGK.Core.Services;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;

namespace ChGK.Core.ViewModels.Search
{
    public class SearchQuestionSingleResultViewModel : MenuItemViewModel
    {
        private readonly IGAService _gaService;

        private MvxCommand _openImageCommand;

        private MvxCommand _openTourCommand;

        private MvxCommand _openTournamentCommand;

        public SearchQuestionSingleResultViewModel(IGAService gaService)
        {
            _gaService = gaService;
        }

        public ISearchQuestionsResult Question { get; set; }

        public string Picture { get; set; }

        public string TourName { get; set; }

        public string TournamentName { get; set; }

        public bool HasComments { get; set; }

        public bool HasAuthor { get; set; }

        public bool HasSource { get; set; }

        public bool HasPicture { get; set; }

        public bool HasPassCriteria { get; set; }

        public MvxCommand OpenImageCommand
        {
            get
            {
                return _openImageCommand ?? (_openImageCommand =
                    new MvxCommand(() => ShowViewModel<FullImageViewModel>(new {image = Picture})));
            }
        }

        public MvxCommand OpenTourCommand
        {
            get
            {
                return _openTourCommand ??
                       (_openTourCommand =
                           new MvxCommand(
                               () =>
                                   ShowViewModel<TourViewModel>(
                                       new {name = Question.TourName, filename = Question.TourFileName})));
            }
        }

        public MvxCommand OpenTournamentCommand
        {
            get
            {
                return _openTournamentCommand ??
                       (_openTournamentCommand =
                           new MvxCommand(
                               () =>
                                   ShowViewModel<SingleTournamentViewModel>(new {filename = Question.TournamentFileName})));
            }
        }

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
    }
}