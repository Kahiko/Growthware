using System;
using System.Data;
using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Base.Interfaces;

namespace GrowthWare.Framework.Model.Profiles
{
	/// <summary>
	/// Represents the properties necessary to interact with a servers directory(ies)
	/// </summary>
	[Serializable(), CLSCompliant(true)]
	public sealed class MDirectoryProfile : MProfile, IMProfile
	{

#region Constructors
		/// <summary>
		/// Will return a directory profile with the default vaules
		/// </summary>
		/// <remarks></remarks>
		public MDirectoryProfile()
		{
		}

		/// <summary>
		/// Will return a directory profile with the values from the data row
		/// </summary>
		/// <param name="Datarow">DataRow</param>
		public MDirectoryProfile(DataRow Datarow)
		{
			base.Initialize(ref Datarow);
			m_Function_Seq_ID = base.GetInt(ref Datarow, "FUNCTION_SEQ_ID");
			m_Directory = base.GetString(ref Datarow, "Directory");
			m_Impersonate = base.GetBool(ref Datarow, "Impersonate");
			m_Impersonate_Account = base.GetString(ref Datarow, "Impersonate_Account");
			m_Impersonate_PWD = base.GetString(ref Datarow, "Impersonate_PWD");
			base.Id = m_Function_Seq_ID;
			base.Name = m_Directory.ToString();
		}
#endregion

#region Field Objects
		private int m_Function_Seq_ID;
		private string m_Directory = string.Empty;
		private bool m_Impersonate = false;
		private string m_Impersonate_Account = string.Empty;
		private string m_Impersonate_PWD = string.Empty;
#endregion

#region Public Properties
		/// <summary>
		/// Is the primary key
		/// </summary>
		public int Function_Seq_ID
		{
			get
			{
				return m_Function_Seq_ID;
			}
			set
			{
				m_Function_Seq_ID = value;
			}
		}

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
				m_Directory = value.Trim();
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
		public string Impersonate_Account
		{
			get
			{
				return m_Impersonate_Account;
			}
			set
			{
				m_Impersonate_Account = value.Trim();
			}
		}

		/// <summary>
		/// Is the password associated with the Impersonate_Account property
		/// </summary>
		/// <value>String</value>
		/// <returns>String</returns>
		public string Impersonate_PWD
		{
			get
			{
				return m_Impersonate_PWD;
			}
			set
			{
				m_Impersonate_PWD = value.Trim();
			}
		}

#endregion
	}
}
