using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Represents the properties necessary to interact with a servers directory(ies)
/// </summary>
[Serializable(), CLSCompliant(true)]
public sealed class MDirectoryProfile : AAddedUpdated
{

    #region Constructors
        /// <summary>
        /// Will return a directory profile with the default vaules
        /// </summary>
        /// <remarks></remarks>
        public MDirectoryProfile()
        {
            SetupClass();
        }

        /// <summary>
        /// Will return a directory profile with the values from the data row
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        public MDirectoryProfile(DataRow dataRow)
        {
            SetupClass();
            base.Initialize(dataRow);
            Id = base.GetInt(dataRow, "FUNCTION_SEQ_ID");
            m_Directory = base.GetString(dataRow, "Directory");
            m_Impersonate = base.GetBool(dataRow, "Impersonate");
            m_Impersonate_Account = base.GetString(dataRow, "Impersonate_Account");
            m_Impersonate_PWD = base.GetString(dataRow, "Impersonate_PWD");
        }
    #endregion

    #region Field Objects
        private string m_Directory = string.Empty;
        private bool m_Impersonate = false;
        private string m_Impersonate_Account = string.Empty;
        private string m_Impersonate_PWD = string.Empty;
    #endregion

    #region Public Properties
        /// <summary>
        /// Is the primary key
        /// </summary>
        [DBPrimaryKey]
        [DBColumnName("FunctionSeqId")]
        public int Id { get; set; }

        /// <summary>
        /// Is the full local directory i.e. C:\temp
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks>Can also be a network location \\mycomputer\c$\temp</remarks>
        public string Directory
        {
            get
            {
                return m_Directory;
            }
            set
            {
                if (value != null) m_Directory = value.Trim();
            }
        }

        /// <summary>
        /// Indicates if impersonation is necessary
        /// </summary>
        /// <value>Boolean</value>
        /// <returns>Boolean</returns>
        /// <remarks>Works in conjunction with Impersonate_Account and Impersonate_PWD</remarks>
        public bool Impersonate
        {
            get
            {
                return m_Impersonate;
            }
            set
            {
                m_Impersonate = value;
            }
        }

        /// <summary>
        /// Is the account used to impersonate when working with the directory
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks>Must be a valid network account with access to the information supplied in the directory property</remarks>
        [DBColumnName("Impersonating_Account")]
        public string ImpersonateAccount
        {
            get
            {
                return m_Impersonate_Account;
            }
            set
            {
                if (!string.IsNullOrEmpty(value)) m_Impersonate_Account = value.Trim();
            }
        }

        /// <summary>
        /// Is the password associated with the Impersonate_Account property
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        [DBColumnName("Impersonating_Password")]
        public string ImpersonatePassword
        {
            get
            {
                return m_Impersonate_PWD;
            }
            set
            {
                value ??= string.Empty;
                m_Impersonate_PWD = value.Trim();
            }
        }
    #endregion

    protected override void SetupClass()
    {
        this.Id = -1;
        base.m_ForeignKeyName = "NOT_USED";
        base.m_IsForeignKeyNumeric = true;
        m_TableName = "[ZGWOptional].[Directories]";
    }
}
