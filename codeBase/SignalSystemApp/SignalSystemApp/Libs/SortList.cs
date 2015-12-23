using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalSystemApp.Models;
using SortDirection = System.Web.Helpers.SortDirection;

namespace SignalSystem.Libs
{
    public class SortList
    {
        public static void GetSortedList<T>(JQueryDataTableParamModel aModel, List<T> filterList, List<string> columnlist)
        {
            if (aModel.sSortDir_0 == "asc")
            {
                ListSort(filterList, columnlist[aModel.iSortCol_0], SortDirection.Ascending);
            }
            else
            {
                ListSort(filterList, columnlist[aModel.iSortCol_0], SortDirection.Descending);
            }
        }

        private static void ListSort<T>(List<T> list, string columnName, SortDirection direction)
        {
            var property = typeof(T).GetProperty(columnName);
            var multiplier = direction == SortDirection.Descending ? -1 : 1;
            list.Sort((t1, t2) =>
            {
                var col1 = property.GetValue(t1);
                var col2 = property.GetValue(t2);
                return multiplier * Comparer<object>.Default.Compare(col1, col2);
            });
        }
    }
}