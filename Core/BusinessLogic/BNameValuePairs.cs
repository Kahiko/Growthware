using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace GrowthWare.BusinessLogic
{
    /// <summary>
    /// Class BNameValuePairs
    /// </summary>
    public class BNameValuePairs : AbstractBusinessLogic
    {
        private INameValuePairs m_DNameValuePairs;

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
        public BNameValuePairs(MSecurityEntity securityEntityProfile)
        {
            if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile can not be null (Nothing in Visual Basic)");
            if (!ConfigSettings.CentralManagement)
            {
                if (m_DNameValuePairs == null)
                {
                    m_DNameValuePairs = (INameValuePairs)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DNameValuePairs");
                    m_DNameValuePairs.ConnectionString = securityEntityProfile.ConnectionString;
                }
            }
            else
            {
                m_DNameValuePairs = (INameValuePairs)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DNameValuePairs");
                m_DNameValuePairs.ConnectionString = securityEntityProfile.ConnectionString;
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
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.DeleteNVPDetail(detailProfile);
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
            m_DNameValuePairs.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityID;
            // for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePairs.AccountId = -1;
            m_DNameValuePairs.NameValuePairProfile.Id = -1;
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.GetAllNVP();
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
            m_DNameValuePairs.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityID;
            // for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePairs.AccountId = accountId;
            m_DNameValuePairs.NameValuePairProfile.Id = -1;
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.GetAllNVP();
            return mRetVal;
        }

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <param name="nameValuePairSeqDetailId">The NVP seq detail ID.</param>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        public MNameValuePairDetail GetNameValuePairDetail(int nameValuePairSeqId, int nameValuePairSeqDetailId)
         {
            DataRow mDataRow = null;
            MNameValuePairDetail mRetVal = new MNameValuePairDetail();
            m_DNameValuePairs.DetailProfile.Id = nameValuePairSeqDetailId;
            m_DNameValuePairs.DetailProfile.NameValuePairSeqId = nameValuePairSeqId;
            if (DatabaseIsOnline()) mDataRow = m_DNameValuePairs.NameValuePairDetail();
            if (mDataRow != null) mRetVal = new MNameValuePairDetail(mDataRow);
            return mRetVal;
        }

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <returns>DataTable.</returns>
        public DataTable GetAllNameValuePairDetail()
        {
            DataTable mRetVal = null;
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.AllNameValuePairDetail();
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
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.GetAllNVPDetail(nameValuePairSeqId);
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
            m_DNameValuePairs.NameValuePairProfile.Id = nameValuePairSeqId;
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.NameValuePair();
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
            m_DNameValuePairs.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityID;
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.GetRoles(nameValuePairSeqId);
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
            m_DNameValuePairs.SecurityEntitySeqId = ConfigSettings.DefaultSecurityEntityID;
            if (DatabaseIsOnline()) mRetVal = m_DNameValuePairs.GetGroups(nameValuePairSeqId);
            return mRetVal;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="nameValuePairProfile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public MNameValuePair Save(MNameValuePair nameValuePairProfile)
        {
            MNameValuePair mRetVal = null;
            m_DNameValuePairs.NameValuePairProfile = nameValuePairProfile;
            if (DatabaseIsOnline()) mRetVal = new MNameValuePair(m_DNameValuePairs.Save());
            return mRetVal;
        }

        /// <summary>
        /// Saves the NVP detail.
        /// </summary>
        /// <param name="nameValuePairDetailProfile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public void SaveNameValuePairDetail(MNameValuePairDetail nameValuePairDetailProfile)
        {
            m_DNameValuePairs.DetailProfile = nameValuePairDetailProfile;
            if (DatabaseIsOnline()) m_DNameValuePairs.SaveNVPDetail(nameValuePairDetailProfile);
        }

        /// <summary>
        /// Updates the groups.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="commaSeparatedGroups">The comma separated groups.</param>
        /// <param name="nameValuePairProfile">MNameValuePair.</param>
        public void UpdateGroups(int nameValuePairId, int SecurityEntityID, string commaSeparatedGroups, MNameValuePair nameValuePairProfile)
        {
            if (DatabaseIsOnline()) m_DNameValuePairs.UpdateGroups(nameValuePairId, SecurityEntityID, commaSeparatedGroups, nameValuePairProfile);
        }

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="commaSeparatedRoles">The comma separated roles.</param>
        /// <param name="nameValuePairProfile">MNameValuePair.</param>
        public void UpdateRoles(int nameValuePairId, int SecurityEntityID, string commaSeparatedRoles, MNameValuePair nameValuePairProfile)
        {
            if (DatabaseIsOnline()) m_DNameValuePairs.UpdateRoles(nameValuePairId, SecurityEntityID, commaSeparatedRoles, nameValuePairProfile);
        }
    }
}
