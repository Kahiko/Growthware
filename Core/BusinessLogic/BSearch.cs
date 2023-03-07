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
    public class BSearch : AbstractBusinessLogic {

        private ISearch m_DSearch;

        private BSearch(){}

        private string m_DB_ClassName = "DSearch";

        public BSearch(MSecurityEntity securityEntityProfile){
            if (securityEntityProfile == null)
            {
                throw new ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
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
        public DataTable GetSearchResults(MSearchCriteria searchCriteria) {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!");
            return m_DSearch.GetSearchResults(searchCriteria);
        }
    }