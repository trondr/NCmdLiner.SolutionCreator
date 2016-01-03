using System.Reflection;
using System.Windows;

namespace NCmdLiner.SolutionCreator.Library.Common.UI
{
    public static class WindowsIsModal
    {
        /// <summary>
        /// Check if Windows has been started using ShowDialog().
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool IsModal(this Window window)
        {
            return System.Windows.Interop.ComponentDispatcher.IsThreadModal;
            //var memberInfo = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);
            //return memberInfo != null && (bool)memberInfo.GetValue(window);
        }
    }
}
