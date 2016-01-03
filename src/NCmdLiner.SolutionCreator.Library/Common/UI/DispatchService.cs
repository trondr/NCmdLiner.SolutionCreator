//Credits: http://blogs.msdn.com/b/davidrickard/archive/2010/04/01/using-the-dispatcher-with-mvvm.aspx

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace NCmdLiner.SolutionCreator.Library.Common.UI
{
    public static class DispatchService
    {
        public static void Invoke(Action action)
        {
            Dispatcher dispatcher = null;
            var currentApplication = Application.Current;
            if (currentApplication != null)
            {
                dispatcher = currentApplication.Dispatcher;
            }
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        public static void Invoke(Dispatcher dispatcher, Action action)
        {            
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }
    }
}
