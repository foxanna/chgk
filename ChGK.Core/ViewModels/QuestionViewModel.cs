using System;
using System.Windows.Input;
using ChGK.Core.Models;
using ChGK.Core.Services;
using ChGK.Core.Services.WebBrowser;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class QuestionViewModel : BaseViewModel
    {
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly IGAService _gaService;

        private readonly ChGKTimer _timer = new ChGKTimer();
        private readonly IWebBrowserService _webBrowserService;

        private bool _isAnswerShown;
        private bool _isTimerStarted;

        private ICommand _openImageCommand, _showAnswerCommand;
        private ICommand _openQuestionInWebCommand;
        private IQuestion _question;
        private TimeSpan _timeSpan;

        public QuestionViewModel(IGAService gaService,
            IAudioPlayerService audioPlayerService,
            IWebBrowserService webBrowserService)
        {
            _gaService = gaService;
            _audioPlayerService = audioPlayerService;
            _webBrowserService = webBrowserService;
        }

        public IQuestion Question
        {
            get { return _question; }
            set
            {
                _question = value;

                RaisePropertyChanged(() => Id);
                RaisePropertyChanged(() => Text);
                RaisePropertyChanged(() => Answer);
                RaisePropertyChanged(() => PassCriteria);
                RaisePropertyChanged(() => Comment);
                RaisePropertyChanged(() => Author);
                RaisePropertyChanged(() => Gearbox);
                RaisePropertyChanged(() => Source);
                RaisePropertyChanged(() => Picture);

                RaisePropertyChanged(() => HasComments);
                RaisePropertyChanged(() => HasAuthor);
                RaisePropertyChanged(() => HasSource);
                RaisePropertyChanged(() => HasPicture);
                RaisePropertyChanged(() => HasPassCriteria);
                RaisePropertyChanged(() => HasGearbox);
            }
        }

        public string Id => Question?.Id;

        public string Text => Question?.Text;

        public string Answer => Question?.Answer;

        public string PassCriteria => Question?.PassCriteria;

        public string Comment => Question?.Comment;

        public string Author => Question?.Author;

        public string Gearbox => Question?.Gearbox;

        public string Source => Question?.Source;

        public string Picture => Question?.Picture;

        public bool HasComments => !string.IsNullOrEmpty(Comment);

        public bool HasAuthor => !string.IsNullOrEmpty(Author);

        public bool HasSource => !string.IsNullOrEmpty(Source);

        public bool HasPicture => !string.IsNullOrEmpty(Picture);

        public bool HasPassCriteria => !string.IsNullOrEmpty(PassCriteria);

        public bool HasGearbox => !string.IsNullOrEmpty(Gearbox);

        public bool IsAnswerShown
        {
            get { return _isAnswerShown; }
            set
            {
                _isAnswerShown = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ShowAnswerCommand =>
            _showAnswerCommand ?? (_showAnswerCommand = new Command(ShowAnswer));

        public ICommand OpenQuestionInWebCommand =>
            _openQuestionInWebCommand ?? (_openQuestionInWebCommand = new Command(OpenQuestionInWeb));

        public TimeSpan Time
        {
            get { return _timeSpan; }
            set
            {
                _timeSpan = value;
                RaisePropertyChanged(() => Time);
            }
        }

        public bool IsTimerStarted
        {
            get { return _isTimerStarted; }
            set
            {
                _isTimerStarted = value;

                RaisePropertyChanged(() => IsTimerStarted);
                RaisePropertyChanged(() => IsTimerStopped);
            }
        }

        public bool IsTimerStopped => !IsTimerStarted;

        public ICommand OpenImageCommand => _openImageCommand ?? (_openImageCommand =
            new Command(() => ShowViewModel<FullImageViewModel>(new {image = Picture})));

        private void OpenQuestionInWeb()
        {
            _webBrowserService.OpenInWebBrowser(Question.Url);
        }

        public override void OnViewDestroying()
        {
            PauseTimer();

            base.OnViewDestroying();
        }

        private void ShowAnswer()
        {
            IsAnswerShown = true;
            PauseTimer();

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Click, "answer shown");
        }

        private void OnTimerOneSecond(object sender, TimerEventArgs e)
        {
            Time = e.Seconds;

            if (e.Seconds == new TimeSpan(0, 0, 50))
            {
                _audioPlayerService.PlayShort();
            }
            else if (e.Seconds == new TimeSpan(0, 1, 0))
            {
                PauseTimer();

                _audioPlayerService.PlayLong();
            }
        }

        public void StartTimer()
        {
            _timer.OneSecond += OnTimerOneSecond;

            _timer.Resume();
            IsTimerStarted = true;

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Timer, "start");
        }

        public void PauseTimer()
        {
            IsTimerStarted = false;
            _timer.Pause();

            _timer.OneSecond -= OnTimerOneSecond;

            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Timer, "stop");
        }

        public void EnterResults()
        {
            _gaService.ReportEvent(GACategory.PlayQuestion, GAAction.Click, "enter results dialog opened");

            ShowViewModel<EnterResultsViewModel>(new {questionId = Id});
        }
    }
}