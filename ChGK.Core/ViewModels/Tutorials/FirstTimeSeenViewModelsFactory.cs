using System;

namespace ChGK.Core.ViewModels.Tutorials
{
    public class FirstTimeSeenViewModelsFactory
    {
        public Type CreateViewModel(Type viewModelType)
        {
            if (viewModelType == typeof (LastAddedTournamentsViewModel))
            {
                return typeof (FirstTimeSeenLastAddedTournamentsViewModel);
            }
            if (viewModelType == typeof (QuestionsViewModel))
            {
                return typeof (FirstTimeSeenQuestionsViewModel);
            }
            if (viewModelType == typeof (TeamsViewModel))
            {
                return typeof (FirstTimeSeenTeamsViewModel);
            }

            return null;
        }
    }
}