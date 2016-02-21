using System;
using MvvmCross.Platform.Platform;

namespace ChGK.Droid.Helpers
{
    public class DebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Trace(level, tag, message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Trace(level, tag, message, null);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
            }
            catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1} {2}", level, message);
            }
        }
    }
}