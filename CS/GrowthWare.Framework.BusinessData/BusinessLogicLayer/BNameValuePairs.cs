using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Data;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    /// <summary>
    /// Class BNameValuePairs
    /// </summary>
    public class BNameValuePairs : BaseBusinessLogic
    {
        private IDNameValuePair m_DNameValuePair;

        /// <summary>
        /// Private sub new() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BNameValuePairs()
        {
        }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
        /// </summary>
        /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        /// <remarks></remarks>
        public BNameValuePairs(MSecurityEntityProfile securityEntityProfile)
        {
            if (!ConfigSettings.CentralManagement)
            {
                if (m_DNameValuePair == null)
                {
                    m_DNameValuePair = (IDNameValuePair)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DNameValuePair");
                    m_DNameValuePair.ConnectionString = securityEntityProfile.ConnectionString;
                }
            }
            else
            {
                m_DNameValuePair = (IDNameValuePair)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DNameValuePair");
                m_DNameValuePair.ConnectionString = securityEntityProfile.ConnectionString;
            }
        }

        /// <summary>
        /// Deletes the NVP detail.
        /// </summary>
        /// <param name="Profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool DeleteNVPDetail(MNameValuePairDetail Profile)
        {
            bool mRetVal = false;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.DeleteNVPDetail(Profile);
            return mRetVal;
        }

        /// <summary>
        /// Retrieves all of the NVPs from the database or from cache
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAllNVP()
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            // for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePair.AccountId = -1;
            m_DNameValuePair.NameValuePairProfile.Id = -1;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetAllNVP();
            return mRetVal;
        }

        /// <summary>
        /// Retrieves the NVPs from the DB for a given account
        /// </summary>
        /// <param name="AccountID">NVP's for a given account</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAllNVP(int AccountID)
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            // for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePair.AccountId = AccountID;
            m_DNameValuePair.NameValuePairProfile.Id = -1;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetAllNVP();
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <param name="NVPSeqDetID">The NVP seq detail ID.</param>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        public DataRow GetNVPDetail(int NVPSeqDetID, int NVPSeqID)
        {
            DataRow mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetNVPDetails(NVPSeqDetID, NVPSeqID);
            return mRetVal;
        }

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <returns>DataTable.</returns>
        public DataTable GetAllNVPDetail()
        {
            DataTable mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetAllNVPDetail();
            return mRetVal;
        }

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetAllNVPDetail(int NVPSeqID)
        {
            DataTable mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetAllNVPDetail(NVPSeqID);
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP.
        /// </summary>
        /// <param name="NVPSeqID">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        public DataRow GetNVP(int NVPSeqID)
        {
            DataRow mRetVal = null;
            m_DNameValuePair.NameValuePairProfile.Id = NVPSeqID;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetNVP();
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP roles.
        /// </summary>
        /// <param name="nameValuePairSeqID">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetNVPRoles(int nameValuePairSeqID)
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetRoles(nameValuePairSeqID);
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP groups.
        /// </summary>
        /// <param name="nameValuePairSeqID">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetNVPGroups(int nameValuePairSeqID)
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetGroups(nameValuePairSeqID);
            return mRetVal;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public int Save(MNameValuePair profile)
        {
            int mRetVal = -1;
            m_DNameValuePair.NameValuePairProfile = profile;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.Save();
            return mRetVal;
        }

        /// <summary>
        /// Saves the NVP detail.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public void SaveNVPDetail(MNameValuePairDetail profile)
        {
            m_DNameValuePair.DetailProfile = profile;
            if (IsDatabaseOnline()) m_DNameValuePair.SaveNVPDetail(profile);
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public DataTable Search(MSearchCriteria searchCriteria)
        {
            DataTable mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.Search(searchCriteria);
            return mRetVal;
        }

        /// <summary>
        /// Updates the groups.
        /// </summary>
        /// <param name="NVP_ID">The NV p_ ID.</param>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <param name="commaSeparatedGroups">The comma separated groups.</param>
        /// <param name="AddUpd_By">The add upd_ by.</param>
        public void UpdateGroups(int NVP_ID, int securityEntityId, string commaSeparatedGroups, MNameValuePair nvpProfile)
        {
            if (IsDatabaseOnline()) m_DNameValuePair.UpdateGroups(NVP_ID, securityEntityId, commaSeparatedGroups, nvpProfile);
        }

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="NVP_ID">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="CommaSeparatedRoles">The comma separated roles.</param>
        /// <param name="AddUpd_By">The add upd_ by.</param>
        public void UpdateRoles(int NVP_ID, int SecurityEntityID, string CommaSeparatedRoles, MNameValuePair nvpProfile)
        {
            if (IsDatabaseOnline()) m_DNameValuePair.UpdateRoles(NVP_ID, SecurityEntityID, CommaSeparatedRoles, nvpProfile);
        }
    }
}
