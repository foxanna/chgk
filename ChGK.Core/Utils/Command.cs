using System;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.Utils
{
    public class Command : MvxCommand
    {
        public Command(Action execute) : base(execute)
        {
        }

        public Command(Action execute, Func<bool> canExecute) : base(execute, canExecute)
        {
        }
    }

    public class Command<T> : MvxCommand<T>
    {
        public Command(Action<T> execute) : base(execute)
        {
        }

        public Command(Action<T> execute, Func<T, bool> canExecute) : base(execute, canExecute)
        {
        }
    }
}