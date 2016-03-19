using System;

namespace ChGK.Core.Utils
{
    public class UndoBarRequestEventArgs : EventArgs
    {
        public UndoBarRequestEventArgs(string title, Action onApply, Action onUndo)
        {
            Title = title;
            OnApply = onApply;
            OnUndo = onUndo;
        }

        public string Title { get; }
        public Action OnApply { get; }
        public Action OnUndo { get; }
    }
}