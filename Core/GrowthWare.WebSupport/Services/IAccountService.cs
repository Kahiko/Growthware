using System;
using GrowthWare.Framework.Models;


namespace GrowthWare.WebSupport.Services;

public interface IAccountService
{
    public MAccountProfile Authenticate(string account, string password, string ipAddress);

    public void Delete(int accountSeqId);

    public MAccountProfile GetAccount(String account);

    public bool RefreshTokenExists(string refreshToken);

    public MAccountProfile Save(MAccountProfile accountProfile, bool saveRefreshTokens, bool saveRoles, bool saveGroups);
}