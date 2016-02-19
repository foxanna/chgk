using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    public class FullImageViewModel : MvxViewModel
    {
        private MvxCommand _closeCommand;
        public string Picture { get; set; }

        public MvxCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand =
                    new MvxCommand(() => Close(this)));
            }
        }

        public void Init(string image)
        {
            Picture = image;
        }
    }
}