
namespace GrowthWare.Framework.Model.Profiles.Interfaces
{
    /// <summary>
    /// Interface IMessageProfile
    /// </summary>
    public interface IMessageProfile
    {
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [format as HTML].
        /// </summary>
        /// <value><c>true</c> if [format as HTML]; otherwise, <c>false</c>.</value>
        bool FormatAsHtml { get; set; }

        /// <summary>
        /// Formats the body.
        /// </summary>
        void FormatBody();

        /// <summary>
        /// Tagses the specified seporator.
        /// </summary>
        /// <param name="separator">The seporator.</param>
        /// <returns>System.String.</returns>
        string GetTags(string separator);
    }
}
