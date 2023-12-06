namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Class MSearchCriteria
    /// </summary>
    public class MSearchCriteria
    {
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public string Columns { get; set; }

        /// <summary>
        /// Gets or sets the order by colums and the direction - [column1] ASC, [column2] DESC.
        /// </summary>
        /// <value>The order by clause.</value>
        public string OrderByClause { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the selected page.
        /// </summary>
        /// <value>The selected page.</value>
        public int SelectedPage { get; set; }

        /// <summary>
        /// Gets or sets the table or view.
        /// </summary>
        /// <value>The table or view.</value>
        public string TableOrView { get; set; }

        /// <summary>
        /// Gets or sets the where clause.
        /// </summary>
        /// <value>The where clause.</value>
        public string WhereClause { get; set; }
    }
}
