using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChGK.Core.ViewModels
{
    public class FirstTimeSeenViewModel : MvxViewModel
    {
        public class HintViewMetaData 
        {

        }

        public FirstTimeSeenViewModel()
        {
            _hintViews = new Dictionary<string, List<HintViewMetaData>>();
        }

        public List<HintViewMetaData> Views { get; private set; }

        Dictionary<string, List<HintViewMetaData>> _hintViews;

        public void Init(string type)
        {
           // Views = _hintViews[type];
        }
        
        MvxCommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this)));
            }
        }
    }
}
