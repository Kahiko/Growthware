using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;
public class MRefreshToken : AbstractDatabaseFunctions
{
    m_ForeignKeyIsNumber = true;
    m_ForeignKeyName = "[AccountSeqId]";
    m_PrimaryKeyName = "[RefreshTokenId]";
    m_TableName = "[ZGWSecurity].[RefreshTokens]";

    public MRefreshToken()
    {
    }
    
    public MRefreshToken(DataRow dataRow)
    {
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

    [Key]
	public int RefreshTokenId { get; set; }
    public int AccountSeqId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public string ReasonRevoked { get; set; }
    public bool IsExpired() { return DateTime.UtcNow >= Expires; }
    public bool IsRevoked() { return Revoked != null; }
    public bool IsActive() { return Revoked == null && !IsExpired(); }
}