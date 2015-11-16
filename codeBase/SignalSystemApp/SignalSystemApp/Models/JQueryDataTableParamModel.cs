﻿

using System.Collections.Generic;

namespace SignalSystemApp.Models
{
    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class JQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho{ get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch{ get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength{ get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart{ get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns{ get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols{ get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns{ get; set; }

        public int iSortCol_0 { get; set; } // the first (and usually only) column to be sorted by
        public string sSortDir_0 { get; set; } // the direction of the first column sort (asc/desc)
        public string[] yadcf_data_2 { set; get; }

    }
}