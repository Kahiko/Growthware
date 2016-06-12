using System;
using System.Collections;
using System.Runtime.Serialization;
using GrowthWare.Framework.ModelObjects.Base.Interfaces;

namespace GrowthWare.Framework.ModelObjects.Base
{
	[Serializable()]
	public abstract class MProfileCollection : Hashtable
	{
		protected Hashtable m_ByID = new Hashtable();
		protected Hashtable m_ByName = new Hashtable();

		public Object this[string key] {
			// Don't allow duplicate values
			get { return base[key]; }
			set {
				if (Contains(key)) return;
 
				MProfile mProfile = (MProfile)value;
				if (!(mProfile.ID == -1)) m_ByID.Add(mProfile.ID, value); 
				if (!(mProfile.Name.Trim().Length == 0)) m_ByName.Add(mProfile.Name.ToString(), value); 
				base[key] = value;
			}
		}

		public override void Add(Object key, Object value)
		{
			// Don't allow duplicate values
			if (Contains(key)) return;
 
			MProfile _Profile = (MProfile)value;
			m_ByID.Add(_Profile.ID, value);
			m_ByName.Add(_Profile.Name.ToString().Trim().ToUpper(), value);
			base.Add(key, value);
		}

		public IProfile GetByID(int SEQ_ID)
		{
            return (IProfile)m_ByID[SEQ_ID];
		}

		public IProfile GetByString(string yourString)
		{
			return (IProfile)m_ByName[yourString.Trim().ToUpper()];
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info,context);
		}

		protected MProfileCollection() : base()
		{
			// do nothing
		}
		protected MProfileCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// do nothing
		}
	}
}
