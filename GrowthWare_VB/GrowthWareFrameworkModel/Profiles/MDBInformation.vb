Namespace Profiles
	''' <summary>
	''' Base properties an DB Information Profile
	''' </summary>
	''' <remarks>
	''' Corresponds to table ZF_INFORMATION and 
	''' Store procedures: 
	''' ZFP_SET_INFORMATION, ZFP_GET_INFORMATION
	''' </remarks>
	<Serializable(), CLSCompliant(True)> _
	Public Class MDBInformation
#Region "Member Properties"
		Protected mInformation_SEQ_ID As Integer = 1
		Protected mVersion As String = String.Empty
		Protected mEnableInheritance As Integer = 1
		Protected mADDED_BY As Integer = -1
		Protected mADDED_DATE As DateTime = Now
		Protected mUPDATED_BY As Integer = -1
		Protected mUPDATED_DATE As DateTime = Now
#End Region

#Region "Public Properties"
		Public Property Information_SEQ_ID() As Integer
			Get
				Return mInformation_SEQ_ID
			End Get
			Set(ByVal value As Integer)
				mInformation_SEQ_ID = value
			End Set
		End Property

		Public Property Version() As String
			Get
				Return mVersion.Trim
			End Get
			Set(ByVal value As String)
				mVersion = value.Trim
			End Set
		End Property

		Public Property EnableInheritance() As Integer
			Get
				Return mEnableInheritance
			End Get
			Set(ByVal value As Integer)
				mEnableInheritance = value
			End Set
		End Property

		Public Property ADDED_BY() As Integer
			Get
				Return mADDED_BY
			End Get
			Set(ByVal value As Integer)
				mADDED_BY = value
			End Set
		End Property

		Public Property ADDED_DATE() As DateTime
			Get
				Return mADDED_DATE
			End Get
			Set(ByVal value As DateTime)
				mADDED_DATE = value
			End Set
		End Property

		Public Property UPDATED_BY() As Integer
			Get
				Return mUPDATED_BY
			End Get
			Set(ByVal value As Integer)
				mUPDATED_BY = value
			End Set
		End Property

		Public Property UPDATED_DATE() As DateTime
			Get
				Return mUPDATED_DATE
			End Get
			Set(ByVal value As DateTime)
				mUPDATED_DATE = value
			End Set
		End Property

#End Region

#Region "Protected Methods"
		Protected Sub init(ByVal dr As DataRow)
			mInformation_SEQ_ID = dr("Information_SEQ_ID")
			mVersion = dr("VERSION")
			mEnableInheritance = dr("ENABLE_INHERITANCE")
			mADDED_BY = dr("ADDED_BY")
			mADDED_DATE = dr("ADDED_DATE")
			mUPDATED_BY = dr("UPDATED_BY")
			mUPDATED_DATE = dr("UPDATED_DATE")
		End Sub

#End Region

#Region "Public Methods"
		Public Sub New()

		End Sub

		Public Sub New(ByVal dr As DataRow)
			init(dr)
		End Sub
#End Region
	End Class
End Namespace