using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels.Tutorials
{
    public abstract class TutorialViewModel : MvxViewModel
    {
        private MvxCommand _closeCommand;

        public MvxCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this))); }
        }
    }
}