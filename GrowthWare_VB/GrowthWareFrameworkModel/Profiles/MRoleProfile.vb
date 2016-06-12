Imports GrowthWare.Framework.ModelObjects.Base.Interfaces
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public Class MRoleProfile
		Inherits MProfile
		Implements IProfile

#Region "Member Properties"
		Private mDESCRIPTION As String = String.Empty
		Private mIsSystem As Integer = 0
		Private mIsSystemOnly As Integer = 0
		Private mErrorCode As Integer = -1
		Private mSE_SEQ_ID As Integer = 1
#End Region

#Region "Protected Methods"
		Protected Overloads Sub Init(ByVal dr As DataRow)
			On Error Resume Next
			MyBase.Init(dr)
			mDESCRIPTION = CStr(dr("DESCRIPTION")).Trim
			mIsSystem = CInt(dr("IS_SYSTEM"))
			mIsSystemOnly = CInt(dr("IS_SYSTEM_ONLY"))
			m_ID = CInt(dr("ROLE_SEQ_ID"))
			m_Name = CStr(dr("NAME")).Trim
		End Sub
#End Region

#Region "Public Methods"
		''' <summary>
		''' Will return a message profile with the default vaules
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		Public Sub New(ByVal dr As DataRow)
			Init(dr)
		End Sub
#End Region

#Region "Public Properties"
		Public Property SecurityEntityID() As Integer
			Get
				Return mSE_SEQ_ID
			End Get
			Set(ByVal value As Integer)
				mSE_SEQ_ID = value
			End Set
		End Property
		Public Property Description() As String
			Get
				Return mDESCRIPTION
			End Get
			Set(ByVal value As String)
				mDESCRIPTION = value.Trim
			End Set
		End Property

		Public Property IsSystem() As Integer
			Get
				Return mIsSystem
			End Get
			Set(ByVal value As Integer)
				mIsSystem = value
			End Set
		End Property

		Public Property IsSystemOnly() As Integer
			Get
				Return mIsSystemOnly
			End Get
			Set(ByVal value As Integer)
				mIsSystemOnly = value
			End Set
		End Property
#End Region
	End Class
End Namespace