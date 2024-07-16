using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Class MRegistrationInformation  
/// </summary>
/// <seealso cref="GrowthWare.Framework.Models.MProfile" />
[Serializable(), CLSCompliant(true)]
public class MRegistrationInformation : AbstractBaseModel
{
#region "Member Properties"
    private string m_AccountChoices = string.Empty;
    private int m_AddAccount = -1;
    private string m_Groups = string.Empty;
    private string m_Roles = string.Empty;
    private int m_SecurityEntitySeqId_Owner = -1;
#endregion

#region "Protected Methods"

    /// <summary>
    /// Initializes with the specified DataRow.
    /// </summary>
    /// <param name="dataRow">The DataRow.</param>
    protected new void Initialize(DataRow dataRow) 
    { 
        Initialize();
        base.Initialize(dataRow);
        m_AccountChoices = base.GetString(dataRow, "AccountChoices");
        m_AddAccount = base.GetInt(dataRow, "AddAccount");
        m_Groups = base.GetString(dataRow, "Groups");
        m_Roles = base.GetString(dataRow, "Roles");
        m_SecurityEntitySeqId_Owner = base.GetInt(dataRow, "SecurityEntitySeqId_Owner");
    }

    private void Initialize()
    {
        base.NameColumnName = string.Empty;
        base.IdColumnName = "SecurityEntitySeqId";
    }
#endregion

#region "Public Methods"
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
        Initialize(dataRow);
    }
#endregion

#region Public Properties
    public string AccountChoices
	{
		get { return m_AccountChoices; }
		set { m_AccountChoices = value; }
	}

    public int AddAccount
	{
		get { return m_AddAccount; }
		set { m_AddAccount = value; }
	}
    public string Groups
	{
		get { return m_Groups; }
		set { m_Groups = value; }
	}
    public string Roles
	{
		get { return m_Roles; }
		set { m_Roles = value; }
	}
    public int SecurityEntitySeqIdOwner
	{
		get { return m_SecurityEntitySeqId_Owner; }
		set { m_SecurityEntitySeqId_Owner = value; }
	}

#endregion
}