using System;
using System.Collections.ObjectModel;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

public class BStates: AbstractBusinessLogic
{
    private IState m_DStates;

    /// <summary>
    /// Private constructor to ensure only new instances with passed parameters is used.
    /// </summary>
    /// <remarks></remarks>
    private BStates() { }

    /// <summary>
    /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
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
    /// Dim mBClientChoices As BClientChoices = New BClientChoices(MSecurityEntity, ConfigSettings.CentralManagement)
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
    /// BClientChoices mBClientChoices = new BClientChoices(MSecurityEntity, ConfigSettings.CentralManagement);
    /// ]]>
    /// </code>
    /// </example>
    public BStates(MSecurityEntity securityEntityProfile, bool centralManagement)
    {
        if (securityEntityProfile == null)
        {
            throw new ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
        }
        if (centralManagement)
        {
            if (m_DStates == null)
            {
                m_DStates = (IState)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DState");
            }
        }
        else
        {
            m_DStates = (IState)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DState");
        }

        m_DStates.ConnectionString = securityEntityProfile.ConnectionString;
        m_DStates.SecurityEntitySeqId = securityEntityProfile.Id;
    }

    public Collection<MState> GetStates()
    {
        Collection<MState> mRetVal = new Collection<MState>();
        MState mState = new MState();
        mState.State = "-1";
        m_DStates.Profile = mState;
        DataTable mDataTable = m_DStates.GetStates;
        foreach (DataRow item in mDataTable.Rows)
        {
            mRetVal.Add(new MState(item));
        }
        return mRetVal;
    }

    public void Save(MState state)
    {
        m_DStates.Profile = state;
        m_DStates.Save();
    }
}