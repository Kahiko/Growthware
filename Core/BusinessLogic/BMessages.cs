using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

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
    private IMessages m_DMessages;
    private MSecurityEntity m_SecurityEntityProfile;

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
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BMessages mBMessages = BMessages = New BMessages(MSecurityEntity, ConfigSettings.CentralManagement);
    /// ]]>
    /// </code>
    /// <code language="C#">
    /// <![CDATA[
    /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
    /// MSecurityEntity.DAL = ConfigSettings.DAL
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
    /// 
    /// Dim mBMessages As BMessages = New BMessages(MSecurityEntity, ConfigSettings.CentralManagement)
    /// ]]>
    /// </code>
    /// </example>
    public BMessages(MSecurityEntity securityEntityProfile, bool centralManagement)
    {
        if (securityEntityProfile == null)
        {
            throw new ArgumentException("The securityEntityProfile and not be null!");
        }
        if (centralManagement)
        {
            if (m_DMessages == null)
            {
                m_DMessages = (IMessages)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages");
            }
        }
        else
        {
            m_DMessages = (IMessages)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages");
        }
        m_SecurityEntityProfile = securityEntityProfile;
        m_DMessages.ConnectionString = securityEntityProfile.ConnectionString;
        //m_DMessages.SecurityEntitySeqID = SecurityEntityProfile.Id;
    }

    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <param name="securityEntitySeqId">The security entity ID.</param>
    /// <returns>Collection{MMessage}.</returns>
    public Collection<MMessage> GetMessages(int securityEntitySeqId)
    {
        Collection<MMessage> mRetList = new Collection<MMessage>();
        DataTable mDataTable = null;
        if (DatabaseIsOnline())
        {
            try
            {
                m_DMessages.Profile.SecurityEntitySeqId = securityEntitySeqId;
                mDataTable = m_DMessages.Messages();
                // the DB code is set to create entries for
                // the given security entity however the insert into the table
                // may not have commited before this code has finished executing
                // so an extra check is made here to ensure that
                // messages have been retruned... this is not necessary in the VB
                // code.
                // Basic assumption ... that has to be at least one message
                // because at DB design time messages were created!!!
                if (mDataTable == null || mDataTable.Rows.Count == 0) mDataTable = m_DMessages.Messages();
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
    public DataRow GetMessage(int messageSeqId)
    {
        DataRow mRetVal = null;
        if (DatabaseIsOnline()) mRetVal = m_DMessages.Message(messageSeqId);
        return mRetVal;
    }

    /// <summary>
    /// Adds the message.
    /// </summary>
    /// <param name="profile">The message profile.</param>
    /// <returns>System.Int32.</returns>
    public void Save(MMessage profile)
    {
        if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
        if (DatabaseIsOnline())
        {
            m_DMessages.Profile = profile;
            m_DMessages.Save();
        }
    }
}
