using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;
public class MRefreshToken : ADatabaseTable
{

#region Public Properties
    public int AccountSeqId { get; set; }

    public DateTime Created { get; set; }

    public string CreatedByIp { get; set; }

    public DateTime Expires { get; set; }

    [DBIgnoreProperty]
    public override string ForeignKeyName => "[AccountSeqId]";

    [DBIgnoreProperty]
    public override bool IsForeignKeyNumeric => true;

    public string ReasonRevoked { get; set; }

    [DBPrimaryKey]
	public int RefreshTokenId { get; set; }

    public string ReplacedByToken { get; set; }

    public DateTime? Revoked { get; set; }

    public string RevokedByIp { get; set; }

    [DBIgnoreProperty]
    public override string TableName => "[ZGWSecurity].[RefreshTokens]";

    public string Token { get; set; }

    [DBIgnoreProperty]
    public bool IsExpired { get {return DateTime.UtcNow >= Expires; } }

    [DBIgnoreProperty]
    public bool IsRevoked { get { return Revoked != null; } }

    [DBIgnoreProperty]
    public bool IsActive { get { return Revoked == null && !IsExpired; } }
#endregion

#region Constructors
    public MRefreshToken()
    {
        this.setDefaults();
    }
    
    public MRefreshToken(DataRow dataRow)
    {
        this.setDefaults();
        DateTime mDateTime = DateTime.Now;
        this.RefreshTokenId = base.GetInt(dataRow, "RefreshTokenId");
        this.AccountSeqId = base.GetInt(dataRow, "AccountSeqId");
        this.Token = base.GetString(dataRow, "Token");
        this.Expires = base.GetDateTime(dataRow, "Expires", mDateTime);
        this.Created = base.GetDateTime(dataRow, "Created", mDateTime);
        this.CreatedByIp = base.GetString(dataRow, "CreatedByIp");
        this.Revoked = base.GetDateTime(dataRow, "Revoked", mDateTime);
        if(this.Revoked == mDateTime)
        {
           this.Revoked = null; 
        }
        this.RevokedByIp = base.GetString(dataRow, "RevokedByIp");
        this.ReplacedByToken = base.GetString(dataRow, "ReplacedByToken");
        this.ReasonRevoked = base.GetString(dataRow, "ReasonRevoked");

    }
#endregion

    protected override void setDefaults()
    {
        this.RefreshTokenId = -1;
    }

}