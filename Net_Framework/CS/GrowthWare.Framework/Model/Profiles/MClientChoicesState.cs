using System;
using System.Collections;
using System.Data;

namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Class MClientChoicesState
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MClientChoicesState
    {
        Hashtable m_ClientChoices = new Hashtable(StringComparer.OrdinalIgnoreCase);

        string m_AccountName;

        bool m_IsDirty = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MClientChoicesState" /> class.
        /// </summary>
        /// <param name="clientChoicesData">The client choices data.</param>
        public MClientChoicesState(DataRow clientChoicesData)
            : base()
        {
            m_AccountName = AccountName;
            if (clientChoicesData != null) 
            {
                try
                {
                    DataTable myTable = clientChoicesData.Table.Copy();
                    DataRow Row = myTable.Rows[0];
                    int i = 0;
                    for (i = 0; i <= myTable.Columns.Count - 1; i++)
                    {
                        object Value = Row[myTable.Columns[i]];
                        if (myTable.Columns[i].ToString() == "ACCT") m_AccountName = Value.ToString();
                        m_ClientChoices[myTable.Columns[i].ToString()] = Value.ToString();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }

        /// <summary>
        /// Gets the choices hashtable.
        /// </summary>
        /// <value>The choices hashtable.</value>
        public Hashtable ChoicesHashtable
        {
            get { return m_ClientChoices; }
        }

        /// <summary>
        /// Gets or sets the <see cref="String" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>[ERROR: invalid expression ReturnTypeName.FullName].</returns>
        public String this[string key]
        {
            get { return (string)m_ClientChoices[key]; }
            set
            {
                m_ClientChoices[key] = value;
                m_IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public bool IsDirty
        {
            get { return m_IsDirty; }
            set { m_IsDirty = value; }
        }
    }
}
