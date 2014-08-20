using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChGK.Core.ViewModels.Tutorials
{
    public class FirstTimeSeenViewModelsFactory
    {
        public Type CreateViewModel(Type viewModelType)
        {
            if (viewModelType ==  typeof(LastAddedTournamentsViewModel))
            {
                return typeof(FirstTimeSeenLastAddedTournamentsViewModel);
            }
            else if (viewModelType == typeof(QuestionsViewModel))
            {
                return typeof(FirstTimeSeenQuestionsViewModel);
            }
            else if (viewModelType == typeof(TeamsViewModel))
            {
                return typeof(FirstTimeSeenTeamsViewModel);
            }

            return null;
        }
    }
}
