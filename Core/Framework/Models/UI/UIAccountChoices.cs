using System.Globalization;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models.UI;
/// <summary>
/// Class MUIAccountChoices
/// </summary>
public class UIAccountChoices : AbstractBaseModel
{

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAccountChoices"/> class.
    /// </summary>

    public UIAccountChoices()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAccountChoices"/> class.
    /// </summary>
    /// <param name="clientChoicesState">State of the client choices.</param>
    public UIAccountChoices(MClientChoicesState clientChoicesState)
    {
        if (clientChoicesState[MClientChoices.AccountName] != null)
            AccountName = clientChoicesState[MClientChoices.AccountName].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.Action] != null)
            Action = clientChoicesState[MClientChoices.Action].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.BackColor] != null)
            BackColor = clientChoicesState[MClientChoices.BackColor].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.ColorScheme] != null)
            ColorScheme = clientChoicesState[MClientChoices.ColorScheme].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.HeadColor] != null)
            HeadColor = clientChoicesState[MClientChoices.HeadColor].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.LeftColor] != null)
            LeftColor = clientChoicesState[MClientChoices.LeftColor].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.RecordsPerPage] != null)
            RecordsPerPage = int.Parse(clientChoicesState[MClientChoices.RecordsPerPage].ToString(CultureInfo.InvariantCulture));
        if (clientChoicesState[MClientChoices.SecurityEntityID] != null)
            SecurityEntityID = int.Parse(clientChoicesState[MClientChoices.SecurityEntityID].ToString(CultureInfo.InvariantCulture));
        if (clientChoicesState[MClientChoices.SecurityEntityName] != null)
            SecurityEntityName = clientChoicesState[MClientChoices.SecurityEntityName].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.SubheadColor] != null)
            SubheadColor = clientChoicesState[MClientChoices.SubheadColor].ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets or sets the name of the account.
    /// </summary>
    /// <value>The name of the account.</value>
    public string AccountName { get; set; }

    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    /// <value>The action.</value>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets the color of the back.
    /// </summary>
    /// <value>The color of the back.</value>
    public string BackColor { get; set; }

    /// <summary>
    /// Gets or sets the color scheme.
    /// </summary>
    /// <value>The color scheme.</value>
    public string ColorScheme { get; set; }

    /// <summary>
    /// Gets or sets the environment.
    /// </summary>
    /// <value>The environment.</value>
    public string Environment { get; set; }

    /// <summary>
    /// Gets or sets the color of the head.
    /// </summary>
    /// <value>The color of the head.</value>
    public string HeadColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the header foreground color.
    /// </summary>
    /// <value>The color of the header foreground color.</value>
    public string HeaderForeColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the left.
    /// </summary>
    /// <value>The color of the left.</value>
    public string LeftColor { get; set; }

    /// <summary>
    /// Gets or sets the records per page.
    /// </summary>
    /// <value>The records per page.</value>
    public int RecordsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the account.
    /// </summary>
    /// <value>The account.</value>
    public string Account { get; set; }

    /// <summary>
    /// Gets or sets the security entity ID.
    /// </summary>
    /// <value>The security entity ID.</value>
    public int SecurityEntityID { get; set; }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the framework version.
    /// </summary>
    /// <value>The version.</value>
    public string FrameWorkVersion { get; set; }
    /// <summary>
    /// Gets or sets the name of the security entity.
    /// </summary>
    /// <value>The name of the security entity.</value>
    public string SecurityEntityName { get; set; }

    /// <summary>
    /// Gets or sets the color of the subhead.
    /// </summary>
    /// <value>The color of the subhead.</value>
    public string SubheadColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the row background color.
    /// </summary>
    /// <value>The color of the row background color.</value>
    public string RowBackColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the alternating row background color.
    /// </summary>
    /// <value>The color of the alternating row background color.</value>
    public string AlternatingRowBackColor { get; set; }
}
