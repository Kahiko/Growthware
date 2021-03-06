﻿using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Represents all of the prperties associated with a Security Entity.
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MSecurityEntityProfile : MProfile
    {

        #region Protected Methods
        /// <summary>
        /// Initializes all of the properties given a data row.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        protected new void Initialize(DataRow dataRow)
        {
            base.NameColumnName = "Name";
            base.IdColumnName = "SE_SEQ_ID";
            base.Initialize(dataRow);
            this.Description = base.GetString(dataRow, "Description");
            this.Url = base.GetString(dataRow, "URL");
            this.Skin = base.GetString(dataRow, "Skin");
            this.Style = base.GetString(dataRow, "Style");
            this.ParentSeqId = base.GetInt(dataRow, "PARENT_SE_SEQ_ID");
            this.StatusSeqId = base.GetInt(dataRow, "STATUS_SEQ_ID");
            this.DataAccessLayer = base.GetString(dataRow, "DAL");
            this.DataAccessLayerAssemblyName = base.GetString(dataRow, "DAL_NAME");
            this.DataAccessLayerNamespace = base.GetString(dataRow, "DAL_NAME_SPACE");
            this.ConnectionString = base.GetString(dataRow, "DAL_STRING");
            EncryptionType = (EncryptionType)base.GetInt(dataRow, "ENCRYPTION_TYPE");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Will return a account profile with the default vaules
        /// </summary>
        /// <remarks></remarks>
        public MSecurityEntityProfile()
        {
        }

        /// <summary>
        /// Will populate values based on the contents of the data row.
        /// </summary>
        /// <param name="dataRow">Datarow containing base values</param>
        /// <remarks>
        /// Class should be inherited to extend to your project specific properties
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MSecurityEntityProfile(DataRow dataRow)
        {
            Initialize(dataRow);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Represents the connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Represents the descript property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the Data Access Layer.  
        /// SQLServer or Oracle or MySQL are examples of a data access.
        /// </summary>
        public string DataAccessLayer { get; set; }

        /// <summary>
        /// Represents the Data Access Layer's Assembly or DLL name.  
        /// GrowthWareFramework for example.
        /// </summary>
        public string DataAccessLayerAssemblyName { get; set; }

        /// <summary>
        /// Represents the Data Access Layer's Namespace.
        /// GrowthWare.Framework.DataAccessLayer.SQLServer.V2000 or 
        /// GrowthWare.Framework.DataAccessLayer.SQLServer.V2008 are examples.
        /// </summary>
        public string DataAccessLayerNamespace { get; set; }

        /// <summary>
        /// Represends the Encrytion used by the security entity.
        /// </summary>
        public EncryptionType EncryptionType { get; set; }

        /// <summary>
        /// Security Entities have a hierarchical relationship to each other and this represents the parent of this Security Entity.
        /// </summary>
        public int ParentSeqId { get; set; }

        /// <summary>
        /// Represents the "Skin" associated with this Security Entity
        /// </summary>
        public string Skin { get; set; }

        /// <summary>
        /// Represents the CSS file associated with this Security Entity
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Represends the status
        /// </summary>
        public int StatusSeqId { get; set; }

        /// <summary>
        /// Represents the URL associated with the Security Entity.  
        /// The intended use was to all a way to retrieve a profile based on the URL
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string Url { get; set; }
        #endregion
    }
}
