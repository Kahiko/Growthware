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
        if (clientChoicesState[MClientChoices.Account] != null) 
            Account = clientChoicesState[MClientChoices.Account].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.SecurityEntityId] != null)
            SecurityEntityId = int.Parse(clientChoicesState[MClientChoices.SecurityEntityId].ToString(CultureInfo.InvariantCulture));

        if (clientChoicesState[MClientChoices.SecurityEntityName] != null)
            SecurityEntityName = clientChoicesState[MClientChoices.SecurityEntityName].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.Action] != null)
            Action = clientChoicesState[MClientChoices.Action].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.RecordsPerPage] != null)
            RecordsPerPage = int.Parse(clientChoicesState[MClientChoices.RecordsPerPage].ToString(CultureInfo.InvariantCulture));

        if (clientChoicesState[MClientChoices.ColorScheme] != null)
            ColorScheme = clientChoicesState[MClientChoices.ColorScheme].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.EvenRow] != null)
            EvenRow = clientChoicesState[MClientChoices.EvenRow].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.EvenFont] != null)
            EvenFont = clientChoicesState[MClientChoices.EvenFont].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.OddRow] != null)
            OddRow = clientChoicesState[MClientChoices.OddRow].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.OddFont] != null)
            OddFont = clientChoicesState[MClientChoices.OddFont].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.Background] != null)
            Background = clientChoicesState[MClientChoices.Background].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.HeaderRow] != null)
            HeaderRow = clientChoicesState[MClientChoices.HeaderRow].ToString(CultureInfo.InvariantCulture);
        if (clientChoicesState[MClientChoices.HeaderFont] != null)
            HeaderFont = clientChoicesState[MClientChoices.HeaderFont].ToString(CultureInfo.InvariantCulture);

        if (clientChoicesState[MClientChoices.Background] != null)
            Background = clientChoicesState[MClientChoices.Background].ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets or sets the account.
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// Gets or sets the security entity identifier.
    /// </summary>
    public int SecurityEntityId { get; set; }

    /// <summary>
    /// Gets or sets the security entity name.
    /// </summary>
    public string SecurityEntityName { get; set; }
    
    /// <summary>
    /// Gets or sets the favorite action.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets the records per page.
    /// </summary>
    public int RecordsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the color scheme.
    /// </summary>
    public string ColorScheme { get; set; }

    /// <summary>
    /// Gets or sets the even row background color.
    /// </summary>
    public string EvenRow { get; set; }

    /// <summary>
    /// Gets or sets the even row font color.
    /// </summary>
    public string EvenFont { get; set; }

    /// <summary>
    /// Gets or sets the odd row background color.
    /// </summary>
    public string OddRow { get; set; }

    /// <summary>
    /// Gets or sets the odd row font color.
    /// </summary>
    public string OddFont { get; set; }

    /// <summary>
    /// Gets or sets the header row background color.
    /// </summary>
    public string HeaderRow { get; set; }

    /// <summary>
    /// Gets or sets the header row font color.
    /// </summary>
    public string HeaderFont { get; set; }

    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    public string Background { get; set; }
}
