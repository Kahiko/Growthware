using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.BusinessLogic;

public class BStates: AbstractBusinessLogic
{
    
#region Member Fields
    private IState m_DStates;
#endregion

#region Constructors
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
    /// Dim mBClientChoices As BClientChoices = New BClientChoices(MSecurityEntity)
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
    /// BClientChoices mBClientChoices = new BClientChoices(MSecurityEntity);
    /// ]]>
    /// </code>
    /// </example>
    public BStates(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if(m_DStates == null || ConfigSettings.CentralManagement)
        {
            m_DStates = (IState)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DState", securityEntityProfile.ConnectionString, securityEntityProfile.Id);
            if (this.m_DStates == null) 
            {
                throw new InvalidOperationException("Failed to create an instance of DState.");
            }
        }
    }
#endregion

    public async Task<Collection<MState>> GetStates()
    {
        Collection<MState> mRetVal = new Collection<MState>();
        MState mState = new MState();
        mState.State = "-1";
        m_DStates.Profile = mState;
        DataTable mDataTable = await m_DStates.GetStates();
        foreach (DataRow item in mDataTable.Rows)
        {
            mRetVal.Add(new MState(item));
        }
        return mRetVal;
    }

    public async Task Save(MState state)
    {
        m_DStates.Profile = state;
        await m_DStates.Save();
    }
}