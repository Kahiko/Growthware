using System;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.WebSupport.Services;

public interface IAccountService
{
    public MAccountProfile Authenticate(string account, string password, string ipAddress);

    public void Delete(int accountSeqId);

    public string ChangePassword(UIChangePassword changePassword);

    public MAccountProfile GetAccount(String account);

    public AuthenticationResponse RefreshToken(string token, string ipAddress);

    public bool RefreshTokenExists(string refreshToken);

    public MAccountProfile Save(MAccountProfile accountProfile, bool saveRefreshTokens, bool saveRoles, bool saveGroups);
}