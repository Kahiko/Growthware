using System;
using System.Collections.Generic;
using System.Data;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;

public interface IAccountUtility
{

    public string AnonymousAccount{ get; }
    public string SessionName { get; }

    public MAccountProfile Authenticate(string account, string password, string ipAddress);

    public void Delete(int accountSeqId);

    public string ChangePassword(UIChangePassword changePassword);

    public MAccountProfile GetAccount(String account, bool forceDb = false, bool updateSession = false);

    public MAccountProfile GetCurrentAccount();

    public IList<MMenuTree> GetMenuItems(string account, MenuType menuType);

    public AuthenticationResponse RefreshToken(string token, string ipAddress);

    public bool RefreshTokenExists(string refreshToken);

    public void RemmoveFromCacheOrSession(string name, string forAccount);

    public void RemoveMenusFromCacheOrSession(string forAccount);

    public MAccountProfile Save(MAccountProfile accountProfile, bool saveRefreshTokens, bool saveRoles, bool saveGroups);
}