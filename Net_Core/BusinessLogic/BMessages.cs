using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace GrowthWare.BusinessLogic;

/// <summary>
/// Process business logic for messages
/// </summary>
/// <remarks>
/// <![CDATA[
/// MSecurityEntity can be found in the GrowthWare.Framework.Model.Profiles namespace.  
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
public class BMessages : AbstractBusinessLogic
{

    #region Member Fields
    private IMessages m_DMessages;
    #endregion

    #region Constructors
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
    /// <code language="C#">
    /// <![CDATA[
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BMessages mBMessages = BMessages = New BMessages(MSecurityEntity);
    /// ]]>
    /// </code>
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
    /// MSecurityEntity.DAL = ConfigSettings.DAL
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
    /// 
    /// Dim mBMessages As BMessages = New BMessages(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BMessages(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_DMessages == null)
        {
            this.m_DMessages = (IMessages)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages", securityEntityProfile.ConnectionString);
            if (this.m_DMessages == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DMessages.");
            }
        }
    }
    #endregion

    /// <summary>
    /// Gets all messages for the requested security entity (securityEntitySeqId).
    /// </summary>
    /// <param name="securityEntitySeqId">The security entity ID.</param>
    /// <returns>Collection{MMessage}.</returns>
    public async Task<Collection<MMessage>> GetMessages(int securityEntitySeqId)
    {
        Collection<MMessage> mRetList = new Collection<MMessage>();
        DataTable mDataTable = null;
        if (DatabaseIsOnline())
        {
            try
            {
                m_DMessages.Profile.SecurityEntitySeqId = securityEntitySeqId;
                /**
                  * m_DMessages.Messages() will create new entries for the message if they does not exist
                  * for the given securityEntitySeqId.  So it should always return data.
                  * There is a way that no data can be returned and that is if the securityEntitySeqId
                  * does not exist in the database.  There is a bigger problem if the securityEntitySeqId
                  * is not valid.
                  */
                mDataTable = await m_DMessages.Messages();
                foreach (DataRow item in mDataTable.Rows)
                {
                    mRetList.Add(new MMessage(item));
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
    public async Task<DataRow> GetMessage(int messageSeqId)
    {
        DataRow mRetVal = null;
        if (DatabaseIsOnline()) mRetVal = await m_DMessages.Message(messageSeqId);
        return mRetVal;
    }

    /// <summary>
    /// Adds the message.
    /// </summary>
    /// <param name="profile">The message profile.</param>
    /// <returns>System.Int32.</returns>
    public async Task<int> Save(MMessage profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        int mRetVal = -1;
        if (DatabaseIsOnline())
        {
            m_DMessages.Profile = profile;
            mRetVal = await m_DMessages.Save();
        }
        return mRetVal;
    }
}
