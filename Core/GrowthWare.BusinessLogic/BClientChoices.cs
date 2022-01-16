using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections;

namespace GrowthWare.BusinessLogic
{
    /// <summary>
    /// Process business logic for functions
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
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim myBll as new BClientChoices(mySecurityEntityProfile, ConfigSettings.CentralManagement)
    /// ]]>
    /// </code>
    /// </example>
    public class BClientChoices : BaseBusinessLogic
    {
        private IClientChoices m_DClientChoices;

        /// <summary>
        /// Private BClientChoices() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BClientChoices()
        {
        }

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
        /// BClientChoices mBClientChoices = New BClientChoices(MSecurityEntity, ConfigSettings.CentralManagement);
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
        /// Dim mBClientChoices As BClientChoices = New BClientChoices(MSecurityEntity, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BClientChoices(MSecurityEntity securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile and not be null!");
            }
            if (centralManagement)
            {
                if (m_DClientChoices == null)
                {
                    m_DClientChoices = (IClientChoices)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DClientChoices");
                }
            }
            else
            {
                m_DClientChoices = (IClientChoices)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DClientChoices");
            }

            m_DClientChoices.ConnectionString = securityEntityProfile.ConnectionString;
        }

        /// <summary>
        /// Retrieves a data row from the data store and populates a MClientChoicesState object.
        /// </summary>
        /// <param name="account">The desired account in which to base the MClientChoicesState model object</param>
        /// <returns>A populated MClientChoicesState</returns>
        /// <remarks>None.</remarks>
        public MClientChoicesState GetClientChoicesState(string account)
        {
            MClientChoicesState mRetVal = null;
            try
            {
                if (base.DatabaseIsOnline()) 
                { 
                    mRetVal = new MClientChoicesState(m_DClientChoices.GetChoices(account));
                }
            }
            catch (Exception ex)
            {
                
                throw new BusinessLogicLayerException("Could not retrieve the client choices state", ex);
            }
            return mRetVal;
        }

        /// <summary>
        /// Saves the choices a client may have made during usage of the application.
        /// </summary>
        /// <param name="clientChoicesState">A populated MClientChoicesState object.</param>
        /// <remarks>MClientChoicesState can be found in the GrowthWare.Framework.Model.Profiles namespace.</remarks>
        public void Save(MClientChoicesState clientChoicesState)
        {
            if (clientChoicesState != null)
            {
                Hashtable mChoices = clientChoicesState.ChoicesHashtable;
                m_DClientChoices.Save(mChoices);
            }
            else 
            {
                throw new ArgumentNullException("clientChoicesState", "clientChoicesState cannot be a null reference (Nothing in Visual Basic)!");
            }
        }
    }
}
