using System;
using System.Data;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an Name Value Pair Detail.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MWorkFlows : MNameValuePairDetail
{

    #region Constructors
        /// <summary>
        /// Provides a new account profile with the default vaules
        /// </summary>
        /// <remarks></remarks>
        public MWorkFlows()
        {
            this.SetupClass();
        }

        /// <summary>
        /// Will populate values based on the contents of the data row.
        /// </summary>
        /// <param name="dataRow">Datarow containing base values</param>
        /// <remarks>
        /// Class should be inherited to extend to your project specific properties
        /// </remarks>
        public MWorkFlows(DataRow dataRow)
        {
            this.SetupClass();
            this.Initialize(dataRow);
        }
    #endregion

    /// <summary>
    /// Initializes the specified dr.
    /// </summary>
    /// <param name="dataRow">The dr.</param>
    protected new void Initialize(DataRow dataRow)
    {
        base.Initialize(dataRow);
    }

    /// <summary>
    /// Sets up the common and specific class properties
    /// </summary>
    protected new void SetupClass()
    {
        base.SetupClass(); // setup common properties
        base.SetTableName("ZGWCoreWeb", "Work_Flows");
        // this.Id = -1;
        // base.m_ForeignKeyName = "NOT_USED";
    }

}
