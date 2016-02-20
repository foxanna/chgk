using System.Windows.Input;

namespace ChGK.Core.Services
{
    public enum DialogType
    {
        AddTeamDialog
    }

    public interface IDialogManager
    {
        void ShowDialog(DialogType type, ICommand yesAction);
    }
}