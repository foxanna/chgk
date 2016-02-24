using System.Windows.Input;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
    public class FullImageViewModel : BaseViewModel
    {
        private ICommand _closeCommand;

        public string Picture { get; set; }

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand =
            new Command(() => Close(this)));

        public void Init(string image)
        {
            Picture = image;
        }
    }
}