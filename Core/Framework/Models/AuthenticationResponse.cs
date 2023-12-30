using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace GrowthWare.Framework.Models;

[Serializable(), CLSCompliant(true)]
public class AuthenticationResponse
{
    public AuthenticationResponse(MAccountProfile accountProfile)
    {
        this.Account = accountProfile.Account;
        this.Created = accountProfile.AddedDate;
        this.Email = accountProfile.Email;
        this.FirstName = accountProfile.FirstName;
        this.Id = accountProfile.Id;
        this.IsSystemAdmin = accountProfile.IsSystemAdmin;
        this.IsVerified = false; // Unknown as of yet
        this.JwtToken = accountProfile.Token;
        this.LastName = accountProfile.LastName;
        this.Location = accountProfile.Location;
        this.MiddleName = accountProfile.MiddleName;
        this.PreferredName = accountProfile.PreferredName;
        this.RefreshToken = string.Empty;
        if(accountProfile.RefreshTokens != null && accountProfile.RefreshTokens.Count > 0)
        {
            this.RefreshToken = accountProfile.RefreshTokens
                .OrderByDescending(obj => obj.Created)
                .FirstOrDefault().Token;
        }
        this.Status = accountProfile.Status;
        this.TimeZone = accountProfile.TimeZone;
        this.Updated = accountProfile.UpdatedDate;
    }

    public string Account { get; set; }
    public DateTime Created { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public int Id { get; set; }
    public bool IsSystemAdmin { get; set; }
    public bool IsVerified { get; set; }
    public string JwtToken { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }
    public string MiddleName { get; set; }
    public String PreferredName { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
    public int Status { get; set; }
    public int TimeZone { get; set; }
    public DateTime? Updated { get; set; }
}