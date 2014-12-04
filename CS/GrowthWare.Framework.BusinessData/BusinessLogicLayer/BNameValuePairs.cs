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
            if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile can not be null (Nothing in Visual Basic)");
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
        /// <param name="detailProfile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool DeleteNameValuePairDetail(MNameValuePairDetail detailProfile)
        {
            bool mRetVal = false;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.DeleteNVPDetail(detailProfile);
            return mRetVal;
        }

        /// <summary>
        /// Retrieves all of the NVPs from the database or from cache
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAllNameValuePair()
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
        /// <param name="accountId">NVP's for a given account</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAllNameValuePair(int accountId)
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            // for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePair.AccountId = accountId;
            m_DNameValuePair.NameValuePairProfile.Id = -1;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetAllNVP();
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <param name="nameValuePairSeqDetailId">The NVP seq detail ID.</param>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        public DataRow GetNameValuePairDetail(int nameValuePairSeqDetailId, int nameValuePairSeqId)
         {
            DataRow mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.NameValuePairDetails(nameValuePairSeqDetailId, nameValuePairSeqId);
            return mRetVal;
        }

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <returns>DataTable.</returns>
        public DataTable GetAllNameValuePairDetail()
        {
            DataTable mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.AllNameValuePairDetail();
            return mRetVal;
        }

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetAllNameValuePairDetail(int nameValuePairSeqId)
        {
            DataTable mRetVal = null;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetAllNVPDetail(nameValuePairSeqId);
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        public DataRow GetNameValuePair(int nameValuePairSeqId)
        {
            DataRow mRetVal = null;
            m_DNameValuePair.NameValuePairProfile.Id = nameValuePairSeqId;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.NameValuePair();
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP roles.
        /// </summary>
        /// <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetNameValuePairRoles(int nameValuePairSeqId)
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetRoles(nameValuePairSeqId);
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP groups.
        /// </summary>
        /// <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetNameValuePairGroups(int nameValuePairSeqId)
        {
            DataTable mRetVal = null;
            m_DNameValuePair.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityId;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.GetGroups(nameValuePairSeqId);
            return mRetVal;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="nameValuePairProfile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public int Save(MNameValuePair nameValuePairProfile)
        {
            int mRetVal = -1;
            m_DNameValuePair.NameValuePairProfile = nameValuePairProfile;
            if (IsDatabaseOnline()) mRetVal = m_DNameValuePair.Save();
            return mRetVal;
        }

        /// <summary>
        /// Saves the NVP detail.
        /// </summary>
        /// <param name="nameValuePairDetailProfile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public void SaveNameValuePairDetail(MNameValuePairDetail nameValuePairDetailProfile)
        {
            m_DNameValuePair.DetailProfile = nameValuePairDetailProfile;
            if (IsDatabaseOnline()) m_DNameValuePair.SaveNVPDetail(nameValuePairDetailProfile);
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
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <param name="commaSeparatedGroups">The comma separated groups.</param>
        /// <param name="nameValuePairProfile">MNameValuePair.</param>
        public void UpdateGroups(int nameValuePairId, int securityEntityId, string commaSeparatedGroups, MNameValuePair nameValuePairProfile)
        {
            if (IsDatabaseOnline()) m_DNameValuePair.UpdateGroups(nameValuePairId, securityEntityId, commaSeparatedGroups, nameValuePairProfile);
        }

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="securityEntityId">The security entity ID.</param>
        /// <param name="commaSeparatedRoles">The comma separated roles.</param>
        /// <param name="nameValuePairProfile">MNameValuePair.</param>
        public void UpdateRoles(int nameValuePairId, int securityEntityId, string commaSeparatedRoles, MNameValuePair nameValuePairProfile)
        {
            if (IsDatabaseOnline()) m_DNameValuePair.UpdateRoles(nameValuePairId, securityEntityId, commaSeparatedRoles, nameValuePairProfile);
        }
    }
}
