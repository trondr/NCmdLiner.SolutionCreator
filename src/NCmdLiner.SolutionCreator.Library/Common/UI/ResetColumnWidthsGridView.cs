using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NCmdLiner.SolutionCreator.Library.Common.UI
{
    public static class ResetColumnWidthsGridView
    {
        public static void ResetColumnWidts(this GridView gridView)
        {
            foreach (var column in gridView.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = column.ActualWidth;
                }
                column.Width = double.NaN;
            }
        }
    }
}
