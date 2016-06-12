Imports GrowthWare.Framework.Model.Profiles.Base.Interfaces
Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Profiles
	<Serializable(), CLSCompliant(True)> _
	Public Class MFunctionTypeProfile
		Inherits MProfile
		Implements IMProfile

#Region "Member Properties"
		Private mFunction_Type_Seq_ID = -1
		Private mName As String = String.Empty
		Private mDescription As String = String.Empty
		Private _TEMPLATE As String = String.Empty
		Private _IS_CONTENT As Boolean
		Private _ADDED_BY As Integer = -1
		Private _ADDED_DATE As DateTime = Now
		Private _UPDATED_BY As Integer = -1
		Private _UPDATED_DATE As DateTime = Now
#End Region

		''' <summary>
		''' Will return a Function profile with the default vaules
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		''' <summary>
		''' Will return a fully populated Function type profile.
		''' </summary>
		''' <param name="drowProfile">A data row containing the Function type information</param>
		''' <remarks></remarks>
		Public Sub New(ByVal drowProfile As DataRow)
			Init(drowProfile)
			MyBase.Initialize(drowProfile)
		End Sub

		Protected Overloads Sub Init(ByVal dr As DataRow)
			On Error Resume Next
			mFunction_Type_Seq_ID = CInt(dr("Function_Type_Seq_ID"))
			MyBase.Id = mFunction_Type_Seq_ID
			mName = CStr(dr("NAME"))
			MyBase.Name = mName.ToLower
			mDescription = CStr(dr("DESCRIPTION"))
			_TEMPLATE = CStr(dr("TEMPLATE"))
			_IS_CONTENT = CStr(dr("IS_CONTENT"))
		End Sub

		Public Property Function_Type_Seq_ID() As Integer
			Get
				Return mFunction_Type_Seq_ID
			End Get
			Set(ByVal value As Integer)
				mFunction_Type_Seq_ID = value
			End Set
		End Property

		Public Shadows Property Name() As String
			Get
				Return mName
			End Get
			Set(ByVal Value As String)
				mName = Value.Trim
			End Set
		End Property

		Public Property Description() As String
			Get
				Return mDescription
			End Get
			Set(ByVal Value As String)
				mDescription = Value.Trim
			End Set
		End Property

		Public Property TEMPLATE() As String
			Get
				Return _TEMPLATE
			End Get
			Set(ByVal value As String)
				_TEMPLATE = value.Trim
			End Set
		End Property

		Public Property IS_CONTENT() As Boolean
			Get
				Return _IS_CONTENT
			End Get
			Set(ByVal value As Boolean)
				_IS_CONTENT = value
			End Set
		End Property
	End Class
End Namespace