using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    /// <summary>
    /// Process business logic for messages
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// MSecurityEntityProfile can be found in the GrowthWare.Framework.ModelObjects namespace.  
    /// 
    /// The following properties are necessary for correct business logic operation.
    /// .ConnectionString
    /// .DALName
    /// .DALNameSpace
    /// ]]>
    /// </remarks>
    /// <example> This sample shows how to create an instance of the class.
    /// <code language="VB.NET">BMessages
    /// <![CDATA[
    /// Dim myBll as new BMessages(mySecurityEntityProfile)
    /// ]]>
    /// </code>
    /// </example>
    public class BMessages : BaseBusinessLogic
    {
        private IDMessages m_DMessages;
        private MSecurityEntityProfile m_SecurityEntityProfile;

        /// <summary>
        /// Private BMessages() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BMessages()
        {
        }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired data store.
        /// </summary>
        /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        /// <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
        /// <remarks></remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="VB.NET">
        /// <![CDATA[
        /// MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
        /// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BMessages mBMessages = BMessages = New BMessages(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// Dim mSecurityEntityProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
        /// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL)
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL)
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// Dim mBMessages As BMessages = New BMessages(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BMessages(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile and not be null!");
            }
            if (centralManagement)
            {
                if (m_DMessages == null)
                {
                    m_DMessages = (IDMessages)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages");
                }
            }
            else
            {
                m_DMessages = (IDMessages)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages");
            }
            m_SecurityEntityProfile = securityEntityProfile;
            m_DMessages.ConnectionString = securityEntityProfile.ConnectionString;
            //m_DMessages.SecurityEntitySeqID = SecurityEntityProfile.Id;
        }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <param name="securityEntitySeqId">The security entity ID.</param>
        /// <returns>Collection{MMessageProfile}.</returns>
        public Collection<MMessageProfile> GetMessages(int securityEntitySeqId)
        {
            Collection<MMessageProfile> mRetList = new Collection<MMessageProfile>();
            DataTable mDataTable = null;
            if (isDataBaseOnline()) 
            {
                try
                {
                    m_DMessages.Profile.SecurityEntitySeqId = securityEntitySeqId;
                    mDataTable = m_DMessages.Messages();
                    foreach (DataRow item in mDataTable.Rows)
                    {
                        mRetList.Add(new MMessageProfile(item));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (mDataTable != null)
                    {
                        mDataTable.Dispose();
                    }
                }            
            }
            return mRetList;
        }

        /// <summary>
        /// Purpose is to return data for a specific message.
        /// </summary>
        /// <param name="messageSeqId">int of the desired message profile object</param>
        /// <returns>DataRow</returns>
        /// <remarks></remarks>
        public DataRow GetMessage(int messageSeqId)
        {
            DataRow mRetVal = null;
            if (isDataBaseOnline()) mRetVal = m_DMessages.Message(messageSeqId);
            return mRetVal;
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="profile">The message profile.</param>
        /// <returns>System.Int32.</returns>
        public void Save(MMessageProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile can not be null or Nothing in VB.Net!");
            if (isDataBaseOnline()) 
            {
                m_DMessages.Profile = profile;
                m_DMessages.Save();
            }
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public DataTable Search(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria can not be null (Nothing in VB) or empty!");
            DataTable mRetVal = null;
            if (string.IsNullOrEmpty(searchCriteria.WhereClause))
            {
                searchCriteria.WhereClause = " Security_Entity_SeqID = " + m_SecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                searchCriteria.WhereClause += " AND Security_Entity_SeqID = " + m_SecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture);
            }
            if (isDataBaseOnline()) mRetVal = m_DMessages.Search(searchCriteria);
            return mRetVal;
        }
    }
}
