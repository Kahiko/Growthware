using System;
using System.Data;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Properties for an Name Value Pair Detail.
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MNavigationType : MNameValuePairDetail
{

    #region Constructors
        /// <summary>
        /// Provides a new account profile with the default vaules
        /// </summary>
        /// <remarks></remarks>
        public MNavigationType()
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
        public MNavigationType(DataRow dataRow)
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
    private void SetupClass()
    {
        base.setDefaults(); // setup common properties
        base.SetTableName("ZGWSecurity", "Navigation_Types");
        // this.Id = -1;
        // base.m_ForeignKeyName = "NOT_USED";
    }

}
