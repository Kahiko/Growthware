using System;
using System.Collections;
using System.Data;

namespace GrowthWare.Framework.Model.Profiles
{
	[Serializable(), CLSCompliant(true)]
	public class MClientChoicesState
	{
		Hashtable m_ClientChoices = new Hashtable();
		string m_AccountName;
		bool m_IsDirty = false;

		public MClientChoicesState(DataRow clientChoicesData) : base()
		{
			m_AccountName = AccountName;
			try {
				DataTable myTable = clientChoicesData.Table.Copy();
				DataRow Row = myTable.Rows[0];
				int i = 0;
				for (i = 0; i <= myTable.Columns.Count - 1; i++) {
					object Value = Row[myTable.Columns[i]];
                    m_ClientChoices[myTable.Columns[i].ToString()] = Value.ToString();
				}
			}
			catch (Exception) {
				throw;
			}
			finally {
			}
		}

		public string AccountName {
			get { return m_AccountName; }
			set { m_AccountName = value; }
		}

		public Hashtable ChoicesHashtable {
			get { return m_ClientChoices; }
		}

        public String this[string key]
        {
            get { return (string)m_ClientChoices[key]; }
            set
            {
                m_ClientChoices[key] = value;
                m_IsDirty = true;
            }
        }

		public bool isDirty {
			get { return m_IsDirty; }
			set { m_IsDirty = value; }
		}
	}
}
