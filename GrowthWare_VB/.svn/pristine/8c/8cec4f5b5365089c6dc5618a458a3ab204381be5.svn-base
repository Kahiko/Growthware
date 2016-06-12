Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Base.Interfaces

Namespace Profiles
	''' <summary>
	''' Represents all of the prperties associated with a Security Entity.
	''' </summary>
	<Serializable(), CLSCompliant(True)> _
	Public Class MSecurityEntityProfile
		Inherits MProfile
		Implements IMProfile
#Region "Member Properties"
		'Private m_Description As String = String.Empty
		'Private m_URL As String = String.Empty
		'Private m_Skin As String = "Default"
		'Private m_Style As String = "Default"
		'Private m_Parent_SECURITY_ENTITY_SEQ_ID As Integer = -1
		'Private m_STATUS_SEQ_ID As Integer = 3
		'Private m_DAL As String = String.Empty
		'Private m_DAL_NAME As String = String.Empty
		'Private m_DAL_NAME_SPACE As String = String.Empty
		'Private m_ConnectionString As String = String.Empty
		'Private m_EncryptedConnection As String = String.Empty
		'Private m_EncryptionType As Integer = 1
#End Region

#Region "Protected Methods"
		''' <summary>
		''' Initializes all of the properties given a data row.
		''' </summary>
		''' <param name="DetailRow">DataRow</param>
		Protected Overrides Sub Initialize(ByRef DetailRow As DataRow)
			MyBase.Initialize(DetailRow)
			MyBase.Id = MyBase.GetString(DetailRow, "SE_SEQ_ID")
			MyBase.Name = MyBase.GetString(DetailRow, "Name")
			Description = MyBase.GetString(DetailRow, "Description")
			Url = MyBase.GetString(DetailRow, "URL")
			Skin = MyBase.GetString(DetailRow, "Skin")
			Style = MyBase.GetString(DetailRow, "Style")
			ParentSeqId = MyBase.GetInt(DetailRow, "PARENT_SE_SEQ_ID")
			StatusSeqId = MyBase.GetInt(DetailRow, "STATUS_SEQ_ID")
			DAL = MyBase.GetString(DetailRow, "DAL")
			DALAssemblyName = MyBase.GetString(DetailRow, "DAL_NAME")
			DALNamespace = MyBase.GetString(DetailRow, "DAL_NAME_SPACE")
			ConnectionString = MyBase.GetString(DetailRow, "DAL_STRING")
			Name = MyBase.GetString(DetailRow, "Name")
			EncryptionType = MyBase.GetInt(DetailRow, "ENCRYPTION_TYPE")
		End Sub
#End Region

#Region "Public Methods"
		''' <summary>
		''' Will return a account profile with the default vaules
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		''' <summary>
		''' Will populate values based on the contents of the data row.
		''' </summary>
		''' <param name="dr">Datarow containing base values</param>
		''' <remarks>
		''' Class should be inherited to extend to your project specific properties
		''' </remarks>
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")>
		Public Sub New(ByVal dr As DataRow)
			Me.Initialize(dr)
		End Sub
#End Region

#Region "Public Properties"
		''' <summary>
		''' Represents the Descriptionn
		''' </summary>
		Public Property Description() As String

		''' <summary>
		''' Represents the URL associated with the Security Entity.  
		''' The intended use was to all a way to retrieve a profile based on the URL
		''' </summary>
		Public Property Url() As String

		''' <summary>
		''' Represents the "Skin" associated with this Security Entity
		''' </summary>
		Public Property Skin() As String

		''' <summary>
		''' Represents the CSS file associated with this Security Entity
		''' </summary>
		Public Property Style() As String

		''' <summary>
		''' Security Entities have a hierarchical relationship to each other and this represents the parent of this Security Entity.
		''' </summary>
		Public Property ParentSeqId() As Integer

		''' <summary>
		''' Represents the connection string for the given security entity.
		''' </summary>
		Public Property ConnectionString() As String

		''' <summary>
		''' Represends the Encrytion used by the security entity.
		''' </summary>
		Public Property EncryptionType() As Integer

		''' <summary>
		''' Represends the status
		''' </summary>
		Public Property StatusSeqId() As Integer

		''' <summary>
		''' Represents the Data Access Layer.  
		''' SQLServer or Oracle or MySQL are examples of a data access.
		''' </summary>
		Public Property DAL() As String

		''' <summary>
		''' Represents the Data Access Layer's Assembly or DLL name.  
		''' GrowthWareFramework for example.
		''' </summary>
		Public Property DALAssemblyName() As String

		''' <summary>
		''' Represents the Data Access Layer's Namespace.
		''' GrowthWare.Framework.DataAccessLayer.SQLServer.V2000 or 
		''' GrowthWare.Framework.DataAccessLayer.SQLServer.V2008 are examples.
		''' </summary>
		Public Property DALNamespace() As String
#End Region
	End Class
End Namespace