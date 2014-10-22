using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrowthWare.Framework.Common;

namespace GrowthWare.WebApplication.Functions.System
{
    public partial class TestNaturalSort : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String mSortDirection = "ASC";
            if (Request.QueryString["SortDirection"] == null)
            {
                dropSortDirection.SelectedIndex = 0;
            }
            else
            {
                mSortDirection = Request.QueryString["SortDirection"].ToString();
                if (mSortDirection == "ASC")
                {
                    dropSortDirection.SelectedIndex = 0;
                }
                else
                {
                    dropSortDirection.SelectedIndex = 1;
                }
            }
            BindData(mSortDirection);
        }

        private void BindData(String sortDirection) 
        {
            Logger mLog = Logger.Instance();

            DataTable oTable = new DataTable("MyTable");
            oTable.Columns.Add("COL1", Type.GetType("System.String"));
            oTable.Columns.Add("COL2", Type.GetType("System.String"));
            DataRow oRow = oTable.NewRow();
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter(10)";
            oRow["COL2"] = "Chapter(10)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter 2 Ep 2-3";
            oRow["COL2"] = "Chapter 2 Ep 2-3";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter 2 Ep 1-2";
            oRow["COL2"] = "Chapter 2 Ep 1-2";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "";
            oRow["COL2"] = "";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Rocky(IV)";
            oRow["COL2"] = "Rocky(IV)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter(1)";
            oRow["COL2"] = "Chapter(1)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter(11)";
            oRow["COL2"] = "Chapter(11)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Rocky(I)";
            oRow["COL2"] = "Rocky(I)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Rocky(II)";
            oRow["COL2"] = "Rocky(II)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Rocky(IX)";
            oRow["COL2"] = "Rocky(IX)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Rocky(X)";
            oRow["COL2"] = "Rocky(X)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter(2)";
            oRow["COL2"] = "Chapter(2)";
            oTable.Rows.Add(oRow);
            oRow = oTable.NewRow();
            oRow["COL1"] = "Chapter 1 Ep 2-3";
            oRow["COL2"] = "Chapter 1 Ep 2-3";
            oTable.Rows.Add(oRow);
            DataView myDV = oTable.DefaultView;
            Framework.Common.SortTable mySorter = new Framework.Common.SortTable();
            String mColName = "COL1";
            mySorter.Sort(oTable, mColName, sortDirection);
            StartTime.Text = mySorter.StartTime.ToString();
            StopTime.Text = mySorter.StopTime.ToString();
            TimeSpan ts = mySorter.StopTime.Subtract(mySorter.StartTime);
            mLog.Debug(ts.TotalMilliseconds.ToString());
            lblTotalTime.Text = ts.TotalMilliseconds.ToString();
            GridView2.DataSource = oTable;
            GridView2.DataBind();
            DropDownList2.DataSource = oTable;
            DropDownList2.DataTextField = "COL1";
            DropDownList2.DataBind();
            string mySort = "COL1 " + sortDirection;
            myDV.Sort = mySort;
            DropDownList3.DataSource = myDV;
            DropDownList3.DataTextField = "COL1";
            DropDownList3.DataBind();
            GridView3.DataSource = myDV;
            GridView3.DataBind();
            oTable.Dispose();
            myDV.Dispose();        
        }
    }
}