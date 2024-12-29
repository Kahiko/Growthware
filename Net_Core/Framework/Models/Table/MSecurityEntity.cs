using System;
using System.Data;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Represents all of the prperties associated with a Security Entity.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MSecurityEntity : AbstractBaseModel
{

#region Public Properties
    /// <summary>
    /// Represents the connection string
    /// </summary>
    [DBColumnName("DAL_String")]
    public string ConnectionString { get; set; }

    /// <summary>
    /// Represents the descript property.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Represents the Data Access Layer.  
    /// SQLServer or Oracle or MySQL are examples of a data access.
    /// </summary>
    [DBColumnName("DAL")]
    public string DataAccessLayer { get; set; }

    /// <summary>
    /// Represents the Data Access Layer's Assembly or DLL name.  
    /// GrowthWareFramework for example.
    /// </summary>
    [DBColumnName("DAL_Name")]
    public string DataAccessLayerAssemblyName { get; set; }

    /// <summary>
    /// Represents the Data Access Layer's Namespace.
    /// GrowthWare.Framework.DataAccessLayer.SQLServer.V2000 or 
    /// GrowthWare.Framework.DataAccessLayer.SQLServer.V2008 are examples.
    /// </summary>
    [DBColumnName("DAL_Name_Space")]
    public string DataAccessLayerNamespace { get; set; }

    /// <summary>
    /// Represends the Encrytion used by the security entity.
    /// </summary>
    [DBColumnName("Encryption_Type")]
    public EncryptionType EncryptionType { get; set; }

    // // Commented out for now this should need to come back when we fix the base class
    // [DBPrimaryKey]
    // [DBColumnName("SecurityEntitySeqId")]
    // public int Id { get; set; }

    // // Commented out for now this should need to come back when we fix the base class
    // public string Name { get; set; }

    /// <summary>
    /// Security Entities have a hierarchical relationship to each other and this represents the parent of this Security Entity.
    /// </summary>
    [DBColumnName("ParentSecurityEntitySeqId")]
    public int ParentSeqId { get; set; }

    /// <summary>
    /// Represents the "Skin" associated with this Security Entity
    /// </summary>
    public string Skin { get; set; }

    /// <summary>
    /// Represends the status
    /// </summary>
    public int StatusSeqId { get; set; }

    /// <summary>
    /// Represents the CSS file associated with this Security Entity
    /// </summary>
    public string Style { get; set; }

    /// <summary>
    /// Represents the URL associated with the Security Entity.  
    /// The intended use was to all a way to retrieve a profile based on the URL
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
    public string Url { get; set; }
#endregion

#region Constructors
    /// <summary>
    /// Will return a account profile with the default vaules
    /// </summary>
    /// <remarks></remarks>
    public MSecurityEntity()
    {
        this.SetupClass();
        // populate with default values
        this.ConnectionString = ConfigSettings.ConnectionString;
        this.DataAccessLayer = ConfigSettings.DataAccessLayer;
        this.DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName;
        this.DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace;
        this.EncryptionType = ConfigSettings.EncryptionType;
        this.ParentSeqId = -1;
        this.Skin = "Default";
        this.StatusSeqId = 1;
        this.Style = "Default";
        this.Url = "no url";
    }

    /// <summary>
    /// Will populate values based on the contents of the data row.
    /// </summary>
    /// <param name="dataRow">Datarow containing base values</param>
    /// <remarks>
    /// Class should be inherited to extend to your project specific properties
    /// </remarks>
    public MSecurityEntity(DataRow dataRow)
    {
        this.SetupClass();
        this.Initialize(dataRow);
    }
#endregion

    /// <summary>
    /// Initializes all of the properties given a data row.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
        this.Description = base.GetString(dataRow, "Description");
        this.Url = base.GetString(dataRow, "URL");
        this.Skin = base.GetString(dataRow, "Skin");
        this.Style = base.GetString(dataRow, "Style");
        this.ParentSeqId = base.GetInt(dataRow, "PARENT_SecurityEntityID");
        this.StatusSeqId = base.GetInt(dataRow, "STATUS_SEQ_ID");
        this.DataAccessLayer = base.GetString(dataRow, "DAL");
        this.DataAccessLayerAssemblyName = base.GetString(dataRow, "DAL_NAME");
        this.DataAccessLayerNamespace = base.GetString(dataRow, "DAL_NAME_SPACE");
        this.ConnectionString = base.GetString(dataRow, "DAL_STRING");
        EncryptionType = (EncryptionType)base.GetInt(dataRow, "ENCRYPTION_TYPE");
        CryptoUtility.TryDecrypt(this.ConnectionString, out string mConnectionString, this.EncryptionType);
        this.ConnectionString = mConnectionString;
    }

    private void SetupClass()
    {
        base.NameColumnName = "Name";
        base.IdColumnName = "SecurityEntityID";
        
        // this.Id = -1;
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSecurity].[Security_Entities]";
    }

}
