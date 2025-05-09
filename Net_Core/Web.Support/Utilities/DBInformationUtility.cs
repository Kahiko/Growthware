using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

/// <summary>
/// DBInformationUtility serves as the focal point for any web application needing to utilize the GrowthWare framework.
/// Web needs such as caching are handled here.
/// </summary>
public static class DBInformationUtility
{
    private static BDBInformation m_BDBInformation = null;

    /// <summary>
    /// New instance of the class
    /// </summary>
    /// <returns>MDBInformation</returns>
    public static async Task<MDBInformation> DBInformation()
    {
        return await(await getBusinessLogic()).GetProfile();
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static async Task<BDBInformation> getBusinessLogic()
    {
        if (m_BDBInformation == null || ConfigSettings.CentralManagement == true)
        {
            m_BDBInformation = new(await SecurityEntityUtility.CurrentProfile());
        }
        return m_BDBInformation;
    }

    /// <summary>
    /// Updated the information in the data store
    /// </summary>
    /// <param name="profile">MDBInformation</param>
    /// <returns>bool or exception</returns>
    public static async Task<bool> UpdateProfile(MDBInformation profile)
    {
        bool mRetVal = false;
        mRetVal = await (await getBusinessLogic()).UpdateProfile(profile);
        return mRetVal;
    }
}