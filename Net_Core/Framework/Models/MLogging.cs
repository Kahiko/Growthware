using GrowthWare.Framework.Enumerations;
using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;
/// <summary>
/// Class MLoggingProfile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MLoggingProfile : ADatabaseTable
{
    #region Member Objects
        string m_Account = "System";
    #endregion

    #region Public Properties
        /// <summary>
        /// Indicates the account used when the log entry was made.
        /// </summary>
        /// <remarks>
        /// For areas where account is unknown or does not apply
        ///  do not set and the default value will be used
        /// </remarks>
        public string Account
        {
            get { return this.m_Account; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.m_Account = value;
                }
            }
        }

        /// <summary>
        /// Indicates what class the log event occurred.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Indicates what component the log event occurred.
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// Indicates where the log event occurred.
        /// </summary>
        [DBIgnoreProperty]
        public LogDestination[] Destination { get; set; }

        /// <summary>
        /// Used as the level of the log entry
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Represents the date the log entry was made.
        /// </summary>
        /// <remarks>This is a unique value</remarks>
        public DateTime LogDate { get; set; }

        [DBPrimaryKey]
        public int LogSeqId { get; set; }

        /// <summary>
        /// Indicates what method of the class the log event occurred.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The message for the log event 
        /// </summary>
        public string Msg { get; set; }

    #endregion

    #region Constructors
        /// <summary>
        /// Will return a Function profile with the default values
        /// </summary>
        public MLoggingProfile()
        {
            this.SetupClass();
            LogSeqId = -1;
        }

        /// <summary>
        /// Will return a fully populated logging profile.
        /// </summary>
        /// <param name="dataRow">A data row containing the log entry</param>
        /// <remarks></remarks>
        public MLoggingProfile(DataRow dataRow)
        {
            this.SetupClass();
            this.Account = base.GetString(dataRow, "Account");
            this.ClassName = base.GetString(dataRow, "ClassName");
            this.Component = base.GetString(dataRow, "Component");
            this.Level = base.GetString(dataRow, "Level");
            this.LogDate = base.GetDateTime(dataRow, "LogDate", DateTime.Now);
            this.LogSeqId = base.GetInt(dataRow, "LogSeqId");
            this.MethodName = base.GetString(dataRow, "MethodName");
            this.Msg = base.GetString(dataRow, "Msg");
        }
    #endregion

    protected override void SetupClass()
    {
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSystem].[Logging]";
    }
}
