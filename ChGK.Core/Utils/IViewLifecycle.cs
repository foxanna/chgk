using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.Utils
{
    public interface IViewLifecycle
    {
        void OnViewDestroying();
    }
}
