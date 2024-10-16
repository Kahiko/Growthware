using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Represents all of the prperties associated with a Security Entity.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MRegistrationInformation : AbstractBaseModel
{

#region Protected Methods
    /// <summary>
    /// Initializes all of the properties given a data row.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.AccountChoices = base.GetString(dataRow, "AccountChoices");
        this.AddAccount = base.GetInt(dataRow, "AddAccount");
        this.Groups = base.GetString(dataRow, "Groups");
        this.Roles = base.GetString(dataRow, "Roles");
        this.SecurityEntitySeqIdOwner = base.GetInt(dataRow, "SecurityEntitySeqIdOwner");
    }
    #endregion

#region Private Methods
    #pragma warning disable IDE1006 // Naming Styles
    private void setNameandColumn()
#pragma warning restore IDE1006 // Naming Styles
    {
        base.NameColumnName = string.Empty;
        base.IdColumnName = "SecurityEntitySeqId";        
    }
#endregion

#region Public Methods
    /// <summary>
    /// Will return a account profile with the default vaules
    /// </summary>
    /// <remarks></remarks>
    public MRegistrationInformation()
    {
        this.setNameandColumn();       
        // populate with default values
        this.Id = -1;
    }

    /// <summary>
    /// Will populate values based on the contents of the data row.
    /// </summary>
    /// <param name="dataRow">Datarow containing base values</param>
    /// <remarks>
    /// Class should be inherited to extend to your project specific properties
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MRegistrationInformation(DataRow dataRow)
    {
        this.setNameandColumn();
        Initialize(dataRow);
    }
#endregion

#region Public Properties
    /// <summary>
    /// Used to copy properties from the [ZGWCoreWeb].[Account_Choices] table for the account being registered.
    /// </summary>
    public string AccountChoices { get; set; }

    /// <summary>
    /// Represents the Int value of the [ZGWSecurity].[Accounts] table of the acount associated with the new accounts "AddedBy".
    /// </summary>
    public int AddAccount { get; set; }

    /// <summary>
    /// A comma separated list of groups
    /// </summary>
    public string Groups { get; set; }

    /// <summary>
    /// A comma separated list of roles
    /// </summary>
    public string Roles { get; set; }

    /// <summary>
    /// The security entity id directly associated with the Roles and Groups
    /// </summary>
    public int SecurityEntitySeqIdOwner { get; set; }
#endregion
}
