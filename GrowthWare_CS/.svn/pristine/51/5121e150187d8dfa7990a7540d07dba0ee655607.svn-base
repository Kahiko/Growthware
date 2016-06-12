using System;
using System.Data;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles.Base;

namespace GrowthWare.Framework.Model.Profiles
{
	/// <summary>
	/// Represents all of the prperties associated with a Security Entity.
	/// </summary>
	[Serializable(), CLSCompliant(true)]
	public class MSecurityEntityProfile : MProfile
	{

#region Protected Methods
		/// <summary>
		/// Initializes all of the properties given a data row.
		/// </summary>
		/// <param name="DetailRow">DataRow</param>
		protected override void Initialize(ref DataRow DetailRow)
		{
			base.Initialize(ref DetailRow);
			base.Id = base.GetInt(ref DetailRow, "SE_SEQ_ID");
			base.Name = base.GetString(ref DetailRow, "Name");
			this.Description = base.GetString(ref DetailRow, "Description");
			this.Url = base.GetString(ref DetailRow, "URL");
			this.Skin = base.GetString(ref DetailRow, "Skin");
			this.Style = base.GetString(ref DetailRow, "Style");
			this.ParentSeqId = base.GetInt(ref DetailRow, "PARENT_SE_SEQ_ID");
			this.StatusSeqId = base.GetInt(ref DetailRow, "STATUS_SEQ_ID");
			this.DAL = base.GetString(ref DetailRow, "DAL");
			this.DALAssemblyName = base.GetString(ref DetailRow, "DAL_NAME");
			this.DALNamespace = base.GetString(ref DetailRow, "DAL_NAME_SPACE");
			this.ConnectionString = base.GetString(ref DetailRow, "DAL_STRING");
			EncryptionType = (EncryptionTypes)base.GetInt(ref DetailRow, "ENCRYPTION_TYPE");
		}
#endregion

#region Public Methods
		/// <summary>
		/// Will return a account profile with the default vaules
		/// </summary>
		/// <remarks></remarks>
		public MSecurityEntityProfile()
		{
		}

		/// <summary>
		/// Will populate values based on the contents of the data row.
		/// </summary>
		/// <param name="dr">Datarow containing base values</param>
		/// <remarks>
		/// Class should be inherited to extend to your project specific properties
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public MSecurityEntityProfile(DataRow dr)
		{
			Initialize(ref dr);
		}
#endregion

#region Public Properties
		/// <summary>
		/// Represents the connection string
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Represents the descript property.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Represents the Data Access Layer.  
		/// SQLServer or Oracle or MySQL are examples of a data access.
		/// </summary>
		public string DAL { get; set; }

		/// <summary>
		/// Represents the Data Access Layer's Assembly or DLL name.  
		/// GrowthWareFramework for example.
		/// </summary>
		public string DALAssemblyName { get; set; }

		/// <summary>
		/// Represents the Data Access Layer's Namespace.
		/// GrowthWare.Framework.DataAccessLayer.SQLServer.V2000 or 
		/// GrowthWare.Framework.DataAccessLayer.SQLServer.V2008 are examples.
		/// </summary>
		public string DALNamespace { get; set; }

		/// <summary>
		/// Represends the Encrytion used by the security entity.
		/// </summary>
		public EncryptionTypes EncryptionType { get; set; }

		/// <summary>
		/// Security Entities have a hierarchical relationship to each other and this represents the parent of this Security Entity.
		/// </summary>
		public int ParentSeqId{	get; set; }

		/// <summary>
		/// Represents the "Skin" associated with this Security Entity
		/// </summary>
		public string Skin { get; set; }

		/// <summary>
		/// Represents the CSS file associated with this Security Entity
		/// </summary>
		public string Style { get; set; }

		/// <summary>
		/// Represends the status
		/// </summary>
		public int StatusSeqId { get; set; }

		/// <summary>
		/// Represents the URL associated with the Security Entity.  
		/// The intended use was to all a way to retrieve a profile based on the URL
		/// </summary>
		public string Url { get; set; }
#endregion
	}
}
