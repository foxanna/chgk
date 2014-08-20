using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.Services
{
    public interface IFirstViewStartInfoProvider
    {
        bool IsSeenForTheFirstTime(Type type);

        void SetSeen(Type type);
    }
}
