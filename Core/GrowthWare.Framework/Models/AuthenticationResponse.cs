using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace GrowthWare.Framework.Models;

[Serializable(), CLSCompliant(true)]
public class AuthenticationResponse
{
    public AuthenticationResponse(){}

    public AuthenticationResponse(MAccountProfile accountProfile)
    {
        this.Account = accountProfile.Account;
        this.Email = accountProfile.Email;
        this.FirstName = accountProfile.FirstName;
        this.IsVerified = true;
        this.JwtToken = accountProfile.Token;
        this.LastName = accountProfile.LastName;
        this.MiddleName = accountProfile.MiddleName;
        this.PreferredName = accountProfile.PreferredName;
        this.m_DerivedRoles = accountProfile.DerivedRoles;
        if(accountProfile.RefreshTokens != null && accountProfile.RefreshTokens.Count > 0)
        {
            this.RefreshToken = accountProfile.RefreshTokens
                .OrderByDescending(obj => obj.Created)
                .FirstOrDefault().Token;
        }
    }

    private Collection<string> m_DerivedRoles = new Collection<string>();

    public String Account { get; set; }
    
    public Collection<String> DerivedRoles
    {
        get
        {
            return m_DerivedRoles;
        }
    }
    
    public String Email { get; set; }
    
    public String FirstName { get; set; }
    
    public String LastName { get; set; }

    public String MiddleName { get; set; }

    public String PreferredName { get; set; }

    public bool IsVerified { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
}