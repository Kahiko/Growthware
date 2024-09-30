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
    /// Dim myBll as new BFunctions(MSecurityEntity, ConfigSettings.CentralManagement)
    /// ]]>
    /// </code>
    /// </example>
    public class BFunctions : AbstractBusinessLogic
    {
        private IFunction m_DFunctions;

        /// <summary>
        /// Private BFunctions() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BFunctions()
        {
        }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
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
        /// BFunctions mBFunction = New BFunctions(MSecurityEntity);
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
        /// Dim mBFunction As BFunctions = New BFunctions(MSecurityEntity)
        /// ]]>
        /// </code>
        /// </example>
        public BFunctions(MSecurityEntity securityEntityProfile)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile can not be null!");
            }
            m_DFunctions = (IFunction)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DFunctions");
            m_DFunctions.ConnectionString = securityEntityProfile.ConnectionString;
            m_DFunctions.SecurityEntitySeqId = securityEntityProfile.Id;
        }

        /// <summary>
        /// Gets the function types.
        /// </summary>
        /// <returns>DataTable.</returns>
        public DataTable FunctionTypes()
        {
            return m_DFunctions.FunctionTypes();
        }

        /// <summary>
        /// Returns a collection of MFunctionProfile objects for the given
        /// security entity.
        /// </summary>
        /// <param name="securityEntitySeqId">Integer</param>
        /// <returns>Collection(of MFunctionProfile)</returns>
        /// <remarks></remarks>
        public Collection<MFunctionProfile> GetFunctions(int securityEntitySeqId)
        {
            Collection<MFunctionProfile> mRetVal = new Collection<MFunctionProfile>();
            DataSet mDSFunctions = null;
            if (DatabaseIsOnline()) 
            {
                try
                {
                    m_DFunctions.Profile = new MFunctionProfile();
                    m_DFunctions.SecurityEntitySeqId = securityEntitySeqId;
                    mDSFunctions = m_DFunctions.GetFunctions;
                    bool mHasAssignedRoles = false;
                    bool mHasGroups = false;
                    if (mDSFunctions.Tables[1].Rows.Count > 0) mHasAssignedRoles = true;
                    if (mDSFunctions.Tables[2].Rows.Count > 0) mHasGroups = true;
                    DataRow[] mGroups = null;
                    DataRow[] mAssignedRoles = null;
                    DataRow[] mDerivedRoles = null;

                    foreach (DataRow item in mDSFunctions.Tables["Functions"].Rows)
                    {
                        mDerivedRoles = item.GetChildRows("DerivedRoles");
                        mAssignedRoles = null;
                        if (mHasAssignedRoles) mAssignedRoles = item.GetChildRows("AssignedRoles");
                        mGroups = null;
                        if (mHasGroups) mGroups = item.GetChildRows("Groups");
                        MFunctionProfile mProfile = new MFunctionProfile(item, mDerivedRoles, mAssignedRoles, mGroups);
                        mRetVal.Add(mProfile);
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the menu order.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetMenuOrder(MFunctionProfile profile)
        {
            DataTable mRetVal = null;
            if (DatabaseIsOnline()) mRetVal = m_DFunctions.GetMenuOrder(profile);
            return mRetVal;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="saveGroups">if set to <c>true</c> [save groups].</param>
        /// <param name="saveRoles">if set to <c>true</c> [save roles].</param>
        /// <returns>System.Int32.</returns>
        public int Save(MFunctionProfile profile, bool saveGroups, bool saveRoles)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
            if (DatabaseIsOnline()) 
            {
                m_DFunctions.Profile = profile;
                profile.Id = m_DFunctions.Save();
                m_DFunctions.Profile = profile;
                if (saveGroups)
                {
                    m_DFunctions.SaveGroups(PermissionType.Add);
                    m_DFunctions.SaveGroups(PermissionType.Delete);
                    m_DFunctions.SaveGroups(PermissionType.Edit);
                    m_DFunctions.SaveGroups(PermissionType.View);
                }
                if (saveRoles)
                {
                    m_DFunctions.SaveRoles(PermissionType.Add);
                    m_DFunctions.SaveRoles(PermissionType.Delete);
                    m_DFunctions.SaveRoles(PermissionType.Edit);
                    m_DFunctions.SaveRoles(PermissionType.View);
                }            
            }
            return profile.Id;

        }

        public void CopyFunctionSecurity(int source, int target, int added_Updated_By)
        {
            if (DatabaseIsOnline()) m_DFunctions.CopyFunctionSecurity(source, target, added_Updated_By);
        }

        /// <summary>
        /// Deletes the specified function seq id.
        /// </summary>
        /// <param name="functionSeqId">The function seq id.</param>
        public void Delete(int functionSeqId)
        {
            if (DatabaseIsOnline()) m_DFunctions.Delete(functionSeqId);
        }
        
        public DataTable MenuTypes()
        {
            return m_DFunctions.MenuTypes();
        }

        /// <summary>
        /// Updates the sort order fot the given functions in the commaSeparated_Ids.
        /// </summary>
        /// <param name="commaSeparated_Ids">A comma separated list of ids</param>
        /// <param name="profile">The profile.</param>
        public void UpdateMenuOrder(string commaseparated_Ids, MFunctionProfile profile)
        {
            if (DatabaseIsOnline()) m_DFunctions.UpdateMenuOrder(commaseparated_Ids, profile);
        }
    }
}
