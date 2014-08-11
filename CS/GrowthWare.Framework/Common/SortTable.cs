using System;
using System.Data;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// Class SortTable
    /// </summary>
    public class SortTable
    {
        private DateTime mStartTime = DateTime.Now;

        private DateTime mStopTime = DateTime.Now;

        /// <summary>
        /// Gets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime
        {
            get { return mStartTime; }
        }

        /// <summary>
        /// Gets the stop time.
        /// </summary>
        /// <value>The stop time.</value>
        public DateTime StopTime
        {
            get { return mStopTime; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortTable" /> class.
        /// </summary>
        public SortTable()
        {
        }

        /// <summary>
        /// Sorts the specified data table.
        /// </summary>
        /// <param name="dt">The data table.</param>
        /// <param name="col">The column.</param>
        /// <param name="SortDirection">The sort direction.</param>
        public void Sort(DataTable dt, DataColumn col, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("ASC")]  string SortDirection)
        {
            mStartTime = DateTime.Now;
            int rowCount = dt.Rows.Count - 1;
            string[] sortValues = new string[rowCount + 1];
            string[] sortIndex = new string[rowCount + 1];
            for (int i = 0; i <= rowCount; i++)
            {
                sortIndex[i] = i.ToString();
                sortValues[i] = dt.Rows[i][col].ToString();
            }
            if (SortDirection == "ASC")
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOptions.None));
            }
            else
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOptions.None, NaturalComparerDirection.Descending));
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dt.ImportRow(dt.Rows[int.Parse(sortIndex[i].ToString())]);
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dt.Rows.RemoveAt(0);
            }
            mStopTime = DateTime.Now;
        }

        /// <summary>
        /// Sorts the specified data table.
        /// </summary>
        /// <param name="dt">The data table.</param>
        /// <param name="col">The column.</param>
        /// <param name="SortDirection">The sort direction.</param>
        public void Sort(DataTable dt, string col, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("ASC")]  string SortDirection)
        {
            mStartTime = DateTime.Now;
            int rowCount = dt.Rows.Count - 1;
            string[] sortValues = new string[rowCount + 1];
            string[] sortIndex = new string[rowCount + 1];
            for (int i = 0; i <= rowCount; i++)
            {
                sortIndex[i] = i.ToString();
                sortValues[i] = dt.Rows[i][col].ToString();
            }
            if (SortDirection == "ASC")
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOptions.None));
            }
            else
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOptions.None, NaturalComparerDirection.Descending));
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dt.ImportRow(dt.Rows[int.Parse(sortIndex[i].ToString())]);
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dt.Rows.RemoveAt(0);
            }
            mStopTime = DateTime.Now;
        }
    }
}
