using System;
using System.Data;
using System.Globalization;

namespace GrowthWare.Framework
{
    /// <summary>
    /// Class SortTable
    /// </summary>
    public class SortTable
    {
        private DateTime m_StartTime = DateTime.Now;

        private DateTime m_StopTime = DateTime.Now;

        /// <summary>
        /// Gets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime
        {
            get { return m_StartTime; }
        }

        /// <summary>
        /// Gets the stop time.
        /// </summary>
        /// <value>The stop time.</value>
        public DateTime StopTime
        {
            get { return m_StopTime; }
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
        /// <param name="dataTable">The data table.</param>
        /// <param name="dataColumn">The column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public void Sort(DataTable dataTable, DataColumn dataColumn, string sortDirection)
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable", "dataTable cannot be a null reference (Nothing in Visual Basic)!");
            if (dataColumn == null) throw new ArgumentNullException("dataColumn", "dataColumn cannot be a null reference (Nothing in Visual Basic)!");
            m_StartTime = DateTime.Now;
            int rowCount = dataTable.Rows.Count - 1;
            string[] sortValues = new string[rowCount + 1];
            string[] sortIndex = new string[rowCount + 1];
            for (int i = 0; i <= rowCount; i++)
            {
                sortIndex[i] = i.ToString(CultureInfo.InvariantCulture);
                sortValues[i] = dataTable.Rows[i][dataColumn].ToString();
            }
            if (sortDirection == "ASC")
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOption.None));
            }
            else
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOption.None, NaturalComparerDirections.Descending));
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dataTable.ImportRow(dataTable.Rows[int.Parse(sortIndex[i].ToString(), CultureInfo.InvariantCulture)]);
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dataTable.Rows.RemoveAt(0);
            }
            m_StopTime = DateTime.Now;
        }

        /// <summary>
        /// Sorts the specified data table.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="dataColumn">The column.</param>
        /// <remarks>Calls Sort passing "ASC" as sortDirection</remarks>
        public void Sort(DataTable dataTable, DataColumn dataColumn)
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable", "dataTable cannot be a null reference (Nothing in Visual Basic)!");
            if (dataColumn == null) throw new ArgumentNullException("dataColumn", "dataColumn cannot be a null reference (Nothing in Visual Basic)!");
            Sort(dataTable,dataColumn,"ASC");
        }

        /// <summary>
        /// Sorts the specified data table.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnName">The column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public void Sort(DataTable dataTable, string columnName, string sortDirection)
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable", "dataTable cannot be a null reference (Nothing in Visual Basic)!");
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName", "columnName cannot be a null reference (Nothing in Visual Basic)!");
            m_StartTime = DateTime.Now;
            int rowCount = dataTable.Rows.Count - 1;
            string[] sortValues = new string[rowCount + 1];
            string[] sortIndex = new string[rowCount + 1];
            for (int i = 0; i <= rowCount; i++)
            {
                sortIndex[i] = i.ToString(CultureInfo.InvariantCulture);
                sortValues[i] = dataTable.Rows[i][columnName].ToString();
            }
            if (sortDirection == "ASC")
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOption.None));
            }
            else
            {
                Array.Sort(sortValues, sortIndex, new NaturalComparer(NaturalComparerOption.None, NaturalComparerDirections.Descending));
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dataTable.ImportRow(dataTable.Rows[int.Parse(sortIndex[i].ToString(), CultureInfo.InvariantCulture)]);
            }
            for (int i = 0; i <= sortIndex.GetUpperBound(0); i++)
            {
                dataTable.Rows.RemoveAt(0);
            }
            m_StopTime = DateTime.Now;
        }

        /// <summary>
        /// Sorts the specified data table.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnName">The column.</param>
        /// <remarks>Calls Sort passing sortDirection of "ASC"</remarks>
        public void Sort(DataTable dataTable, string columnName) 
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable", "dataTable cannot be a null reference (Nothing in Visual Basic)!");
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName", "columnName cannot be a null reference (Nothing in Visual Basic)!");
            Sort(dataTable, columnName, "ASC");
        }
    }
}
