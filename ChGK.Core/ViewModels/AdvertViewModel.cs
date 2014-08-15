using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels
{
    public class AdvertViewModel : MenuItemViewModel
    {
        public override Task Refresh()
        {
            throw new NotImplementedException();
        }

        public List<string> AsIds = new List<string>()
        {
            "ca-app-pub-6283431932505508/5322449877",
            "ca-app-pub-6283431932505508/8356641477",
        };
    }
}
