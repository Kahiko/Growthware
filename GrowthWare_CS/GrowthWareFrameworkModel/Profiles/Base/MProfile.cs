using System;
using System.Data;
using GrowthWare.Framework.Model.Profiles.Base.Interfaces;
using System.Reflection;

namespace GrowthWare.Framework.Model.Profiles.Base
{
	/// <summary>
	/// Base class for profile objects.
	/// </summary>
	[Serializable()]
	public abstract class MProfile: IMProfile
	{

		int m_Id = -1;

#region Public Properties
		/// <summary>
		/// Account ID used to add
		/// </summary>
		public int AddedBy{ get; set; }
		
		/// <summary>
		/// Date the row was added.
		/// </summary>
		public DateTime AddedDate { get; set; }
		
		/// <summary>
		/// Unique numeric identifier
		/// </summary>
		public int Id {
			get { return m_Id; }
			set { m_Id = value; }
		}
		
		/// <summary>
		/// String representation normaly unique
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Account ID used to update
		/// </summary>
		public int UpdatedBy { get; set; }
		
		/// <summary>
		/// The date lasted updated
		/// </summary>
		public DateTime UpdatedDate { get; set; }
#endregion

#region Protected Methods
		/// <summary>
		/// Initializes values given a DataRow
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		/// <remarks>
		/// Does not set ID or Name .. ColumnName should be unique to 
		/// each inheriting class.
		/// </remarks>
		protected virtual void Initialize(ref DataRow Datarow)
		{
			this.AddedBy = GetInt(ref Datarow, "Added_By");
			this.AddedDate = GetDateTime(ref Datarow, "Added_Date", DateTime.Now);
			this.UpdatedBy = GetInt(ref Datarow, "Updated_By");
			this.UpdatedDate = GetDateTime(ref Datarow, "Updated_Date", DateTime.Now);
		}

		///// <summary>
		///// Returns a boolean from a datarow.
		///// </summary>
		///// <param name="YourObject">bool</param>
		///// <param name="ColumnName">Name of the column to retrieve from the data row</param>
		///// <param name="DR">DataRow</param>
		//protected void setBool(Boolean YourObject, String ColumnName, ref DataRow DR)
		//{
		//    if (DR != null && DR.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(DR[ColumnName])))
		//    {
		//        try { YourObject = Boolean.Parse(DR[ColumnName].ToString()); }
		//        catch
		//        {
		//            if (int.Parse(DR[ColumnName].ToString()) == 1) { YourObject = true; }
		//        }
		//    }
		//}

		///// <summary>
		///// Returns a date time object from a datarow.
		///// </summary>
		///// <param name="YourObject">DateTime</param>
		///// <param name="ColumnName">Name of the column to retrieve from the data row</param>
		///// <param name="DR">DataRow</param>
		///// <param name="DefaultDateTime">Date time object used as default</param>
		//protected void setDate(DateTime YourObject, String ColumnName, ref DataRow DR, DateTime DefaultDateTime)
		//{
		//    YourObject = DefaultDateTime;
		//    if (DR != null && DR.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(DR[ColumnName])))
		//    {
		//        YourObject = DateTime.Parse(DR[ColumnName].ToString());
		//    }
		//}

		///// <summary>
		///// Returns an integer from a datarow.
		///// </summary>
		///// <param name="YourObject">int</param>
		///// <param name="ColumnName">Name of the column to retrieve from the data row</param>
		///// <param name="DR">DataRow</param>
		//protected void setInteger(int YourObject, String ColumnName, ref DataRow DR)
		//{
		//    if (DR != null && DR.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(DR[ColumnName])))
		//    {
		//        YourObject = int.Parse(DR[ColumnName].ToString());
		//    }
		//}

		///// <summary>
		///// Returns a string from a datarow.
		///// </summary>
		///// <param name="YourObject">String</param>
		///// <param name="ColumnName">Name of the column to retrieve from the data row</param>
		///// <param name="DR">DataRow</param>
		//protected void setString(ref string YourObject, String ColumnName, ref DataRow DR)
		//{
		//    YourObject = string.Empty;
		//    if (DR != null && DR.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(DR[ColumnName])))
		//    {
		//        YourObject = DR[ColumnName].ToString().Trim();
		//    }
		//}

		/// <summary>
		/// Returns a boolean given the a DataRow and Column name.
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		/// <param name="ColumnName">String</param>
		/// <returns>Boolean</returns>
		/// <remarks></remarks>
		protected Boolean GetBool(ref DataRow Datarow, String ColumnName)
		{
			bool mRetVal = false;
			if(Datarow != null && Datarow.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(Datarow[ColumnName])))
			{
				if (Datarow[ColumnName].ToString() != "0")
				{
					mRetVal = true;
				}
			}
			return mRetVal;
		}

		/// <summary>
		/// Returns a DateTime given the a DataRow and Column name and the defaul value.
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		/// <param name="ColumnName">String</param>
		/// <param name="DefaultDateTime">DateTime</param>
		/// <returns>DateTime</returns>
		/// <remarks></remarks>
		protected DateTime GetDateTime(ref DataRow Datarow, String ColumnName, DateTime DefaultDateTime)
		{
			DateTime mRetVal = DefaultDateTime;
			if(Datarow != null && Datarow.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(Datarow[ColumnName])))
			{
				mRetVal = DateTime.Parse(Datarow[ColumnName].ToString());
			}
			return mRetVal;
		}

		/// <summary>
		/// Returns a int given the a DataRow and Column name.
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		/// <param name="ColumnName">String</param>
		/// <returns>int</returns>
		/// <remarks></remarks>
		protected Int32 GetInt(ref DataRow Datarow, String ColumnName)
		{
			int mRetVal = -1;
			if(Datarow != null && Datarow.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(Datarow[ColumnName])))
			{
				mRetVal = int.Parse(Datarow[ColumnName].ToString());
			}
			return mRetVal;
		}

		/// <summary>
		/// Returns a String given the a DataRow and Column name.
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		/// <param name="ColumnName">String</param>
		/// <returns>String</returns>
		/// <remarks></remarks>
		protected String GetString(ref DataRow Datarow, String ColumnName)
		{
			String mRetVal = string.Empty;
			if(Datarow != null && Datarow.Table.Columns.Contains(ColumnName) && !(Convert.IsDBNull(Datarow[ColumnName])))
			{
				mRetVal = Datarow[ColumnName].ToString().Trim();
			}
			return mRetVal;
		}

		/// <summary>
		/// Returns all properties encapsulated by angle brackets seporated by the Seporator parameter
		/// </summary>
		/// <param name="Seporator">string</param>
		/// <returns>string</returns>
		public string GetTags(string Seporator)
		{
			string retVal = string.Empty;
			PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
			foreach(PropertyInfo mPropertyItem in mPropertyInfo)
			{
				retVal = retVal + "<" + mPropertyItem.Name + ">" + Seporator;
			}
			return retVal;
		}

#endregion
	}
}
