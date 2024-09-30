using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Data;

namespace GrowthWare.BusinessLogic;
/// <summary>
/// Process business logic for accounts
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
/// Dim myBll as new BGroups(mySecurityEntityProfile)
/// ]]>
/// </code>
/// </example>
public class BSearch : AbstractBusinessLogic
{

    private ISearch m_DSearch;

    private BSearch() { }

    private string m_DB_ClassName = "DSearch";

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
    /// BSearch mBSearch = BSearch = New BSearch(MSecurityEntity);
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
    /// Dim mBSearch As BSearch = New BSearch(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BSearch(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null)
        {
            throw new ArgumentNullException(nameof(securityEntityProfile), "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
        }
        if (!ConfigSettings.CentralManagement)
        {
            if (m_DSearch == null)
            {
                m_DSearch = (ISearch)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, m_DB_ClassName);
            }
        }
        else
        {
            m_DSearch = (ISearch)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, m_DB_ClassName);
        }

        m_DSearch.ConnectionString = securityEntityProfile.ConnectionString;
        m_DSearch.SecurityEntitySeqID = securityEntityProfile.Id;
    }
    public DataTable GetSearchResults(MSearchCriteria searchCriteria)
    {
        if (searchCriteria == null) throw new ArgumentNullException(nameof(searchCriteria), "searchCriteria cannot be a null reference (Nothing in Visual Basic)!");
        return m_DSearch.GetSearchResults(searchCriteria);
    }
}