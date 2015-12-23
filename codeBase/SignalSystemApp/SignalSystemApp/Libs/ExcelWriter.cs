using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalSystemApp.Libs
{
    public class ExcelWriter
    {
        public static void ExcelGenerator<T>(List<T> filteredComplainsList)
        {
            DateTime dt = DateTime.Now;
            DateTime dDate = DateTime.Now;
            string[] sDate = dDate.ToString().Split(' ');
            string time = dt.ToString("hh:mm");


            GridView aGridView = new GridView();
            aGridView.DataSource = filteredComplainsList;
            aGridView.DataBind();


            foreach (TableCell cell in aGridView.HeaderRow.Cells)
            {
                cell.BackColor = Color.Cornsilk;
            }

            foreach (GridViewRow row in aGridView.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = Color.Gainsboro;
                    }
                    else
                    {
                        cell.BackColor = Color.GhostWhite;
                    }
                    cell.CssClass = "textmode";
                }
            }

            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=PendingTelephoneComplain_" + sDate[0] + "_" + sDate[1] + sDate[2] + ".xls");
            System.Web.HttpContext.Current.Response.ContentType = "application/excel";
            StringWriter swr = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(swr);
            aGridView.RenderControl(tw);
            System.Web.HttpContext.Current.Response.Write(swr.ToString());

            System.Web.HttpContext.Current.Response.End();
           // return null;
        }
       

    }
}