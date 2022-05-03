using System;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Class MClientChoices
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public sealed class MClientChoices
    {

        private MClientChoices()
        {
        }

        /// <summary>
        /// Gets the records per page.
        /// </summary>
        /// <value>The records per page.</value>
        public static string RecordsPerPage
        {
            get { return "RECORDS_PER_PAGE"; }
        }

        /// <summary>
        /// Gets the state of the anonymous client choices.
        /// </summary>
        /// <value>The state of the anonymous client choices.</value>
        public static string AnonymousClientChoicesState
        {
            get { return "AnonymousClientChoicesState"; }
        }

        /// <summary>
        /// Gets the name of the session.
        /// </summary>
        /// <value>The name of the session.</value>
        public static string SessionName
        {
            get { return "ClientChoicesState"; }
        }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        public static string AccountName
        {
            get { return "ACCT"; }
        }

        /// <summary>
        /// Gets the security entity ID.
        /// </summary>
        /// <value>The security entity ID.</value>
        public static string SecurityEntityID
        {
            get { return "SecurityEntityID"; }
        }

        /// <summary>
        /// Gets the name of the security entity.
        /// </summary>
        /// <value>The name of the security entity.</value>
        public static string SecurityEntityName
        {
            get { return "SecurityEntityName"; }
        }

        /// <summary>
        /// Gets the color of the back.
        /// </summary>
        /// <value>The color of the back.</value>
        public static string BackColor
        {
            get { return "BackColor"; }
        }

        /// <summary>
        /// Gets the color of the left.
        /// </summary>
        /// <value>The color of the left.</value>
        public static string LeftColor
        {
            get { return "LeftColor"; }
        }

        /// <summary>
        /// Gets the color of the head.
        /// </summary>
        /// <value>The color of the head.</value>
        public static string HeadColor
        {
            get { return "HeadColor"; }
        }

        /// <summary>
        /// Gets the color of the header fore.
        /// </summary>
        /// <value>The color of the header fore.</value>
        public static string HeaderForeColor
        {
            get { return "Header_ForeColor"; }
        }

        /// <summary>
        /// Gets the color of the subhead.
        /// </summary>
        /// <value>The color of the subhead.</value>
        public static string SubheadColor
        {
            get { return "SUB_HeadColor"; }
        }

        /// <summary>
        /// Gets the color of the row back.
        /// </summary>
        /// <value>The color of the row back.</value>
        public static string RowBackColor
        {
            get { return "Row_BackColor"; }
        }

        /// <summary>
        /// Gets the color of the alternating row back.
        /// </summary>
        /// <value>The color of the alternating row back.</value>
        public static string AlternatingRowBackColor
        {
            get { return "AlternatingRow_BackColor"; }
        }

        /// <summary>
        /// Gets the color scheme.
        /// </summary>
        /// <value>The color scheme.</value>
        public static string ColorScheme
        {
            get { return "COLOR_SCHEME"; }
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public static string Action
        {
            get { return "Favorite_Action"; }
        }
    }

}
