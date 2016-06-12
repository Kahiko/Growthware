using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.Enumerations.Base
{
	/// <summary>
	/// Was exploring replacing enum to reduce warnings for using
	/// an enum without a zero value.
	/// Decided to mark the clase with
	/// [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")
	/// This code was derived from http://www.codeproject.com/KB/cs/EnhancedEnums.aspx
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class EnumBaseType<T> where T : EnumBaseType<T>
	{
		protected static List<T> enumValues = new List<T>();

		public readonly int Key;
		public readonly string Value;

		public EnumBaseType(int key, string value)
		{
			Key = key;
			Value = value;
			enumValues.Add((T)this);
		}

		protected static ReadOnlyCollection<T> GetBaseValues()
		{
			return enumValues.AsReadOnly();
		}

		protected static T GetBaseByKey(int key)
		{
			foreach(T t in enumValues)
			{
				if(t.Key == key)
					return t;
			}
			return null;
		}

		public override string ToString()
		{
			return Value;
		}	
	}
}
