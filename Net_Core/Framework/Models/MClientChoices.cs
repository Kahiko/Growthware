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
            get { return "RecordsPerPage"; }
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
        public static string Account
        {
            get { return "Account"; }
        }

        /// <summary>
        /// Gets the security entity ID.
        /// </summary>
        /// <value>The security entity ID.</value>
        public static string SecurityEntityId
        {
            get { return "SecurityEntityId"; }
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
        public static string Background
        {
            get { return "Background"; }
        }

        /// <summary>
        /// Gets the color of the head.
        /// </summary>
        /// <value>The color of the head.</value>
        public static string HeaderRow
        {
            get { return "HeaderRow"; }
        }

        /// <summary>
        /// Gets the color of the header fore.
        /// </summary>
        /// <value>The color of the header fore.</value>
        public static string HeaderFont
        {
            get { return "HeaderFont"; }
        }

        /// <summary>
        /// Gets the color of the row back.
        /// </summary>
        /// <value>The color of the row back.</value>
        public static string EvenRow
        {
            get { return "EvenRow"; }
        }

        /// <summary>
        /// Gets the color of the row back.
        /// </summary>
        /// <value>The color of the row back.</value>
        public static string EvenFont
        {
            get { return "EvenFont"; }
        }

        /// <summary>
        /// Gets the color of the alternating row back.
        /// </summary>
        /// <value>The color of the alternating row back.</value>
        public static string OddRow
        {
            get { return "OddRow"; }
        }

        /// <summary>
        /// Gets the color of the alternating row back.
        /// </summary>
        /// <value>The color of the alternating row back.</value>
        public static string OddFont
        {
            get { return "OddFont"; }
        }

        /// <summary>
        /// Gets the color scheme.
        /// </summary>
        /// <value>The color scheme.</value>
        public static string ColorScheme
        {
            get { return "ColorScheme"; }
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public static string Action
        {
            get { return "FavoriteAction"; }
        }
    }

}
