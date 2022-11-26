
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.WebSupport.Utilities;

public static class SecurityEntityUtility
{

    private static Collection<MSecurityEntity> m_SecurityEntities = null;

    public static MSecurityEntity CurrentProfile()
    {
        MSecurityEntity mRetVal = DefaultProfile();
        return mRetVal;
    }

    public static MSecurityEntity DefaultProfile()
    {
        return new MSecurityEntity
        {
            ConnectionString = ConfigSettings.ConnectionString,
            DataAccessLayer = ConfigSettings.DataAccessLayer,
            DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName,
            DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace,
            Id = ConfigSettings.DefaultSecurityEntityID
        };
    }

    /// <summary>
    /// Get a single function given it's action.
    /// </summary>
    /// <param name="name">String</param>
    /// <returns>MSecurityEntityProfile</returns>
    public static MSecurityEntity GetProfile(string name)
    {
        MSecurityEntity mRetVal = new MSecurityEntity();
        var mResult = from mProfile in Profiles()
                      where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                      select mProfile;
        try
        {
            mRetVal = mResult.First();
        }
        catch (NullReferenceException)
        {
            mRetVal = null;
        }
        return mRetVal;

    }

    /// <summary>
    /// Get a single function given it's id.
    /// </summary>
    /// <param name="securityEntitySeqId">int or Integer</param>
    /// <returns>MSecurityEntityProfile</returns>
    public static MSecurityEntity GetProfile(int securityEntitySeqId)
    {
        MSecurityEntity mRetVal = new MSecurityEntity();
        var mResult = from mProfile in Profiles()
                      where mProfile.Id == securityEntitySeqId
                      select mProfile;
        try
        {
            mRetVal = mResult.First();
        }
        catch (NullReferenceException)
        {
            mRetVal = null;
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the valid security entities.
    /// </summary>
    /// <param name="account">The account.</param>
    /// <param name="securityEntityId">The security entity id.</param>
    /// <param name="isSystemAdmin">if set to <c>true</c> [is system admin].</param>
    /// <returns>DataView.</returns>
    public static DataView GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin)
    {
        BSecurityEntities mBSecurityEntities = new BSecurityEntities(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        return mBSecurityEntities.GetValidSecurityEntities(account, securityEntityId, isSystemAdmin).DefaultView;
    }

    public static Collection<MSecurityEntity> Profiles()
    {
        if (m_SecurityEntities == null)
        {
            BSecurityEntities mBSecurityEntities = new BSecurityEntities(DefaultProfile(), ConfigSettings.CentralManagement);
            m_SecurityEntities = mBSecurityEntities.SecurityEntities();
        }
        return m_SecurityEntities;
    }
}