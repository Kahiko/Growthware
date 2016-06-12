Imports GrowthWare.Framework.ModelObjects.Base
Imports GrowthWare.Framework.Enumerations

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public Class MStateProfile
		Inherits MProfile

#Region "Private Methods"
		Private Shadows Sub Init(ByVal dr As DataRow)
			MyBase.Init(dr)
			mState = dr("STATE")
			mDescription = dr("DESCRIPTION")
			mSTATUS_SEQ_ID = dr("STATUS_SEQ_ID")
		End Sub
#End Region

#Region "Private Properties"
		Private mState As String = "NEG1"
		Private mDescription As String = String.Empty
		Private mSTATUS_SEQ_ID As Integer = SystemStatus.Inactive
#End Region

#Region "Public Properties"
		Public Property State() As String
			Get
				Return mState
			End Get
			Set(ByVal value As String)
				mState = value.Trim
			End Set
		End Property

		Public Property Description() As String
			Get
				Return mDescription
			End Get
			Set(ByVal value As String)
				mDescription = value.Trim
			End Set
		End Property

		Public Property STATUS_SEQ_ID() As Integer
			Get
				Return mSTATUS_SEQ_ID
			End Get
			Set(ByVal value As Integer)
				mSTATUS_SEQ_ID = value
			End Set
		End Property
#End Region

#Region "Public Methods"
		''' <summary>
		''' Will return an instance populated with default values.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		''' <summary>
		''' Will return an instance populated with information for the data row provided.
		''' </summary>
		''' <param name="dr"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal dr As DataRow)
			Init(dr)
		End Sub
#End Region

	End Class
End Namespace