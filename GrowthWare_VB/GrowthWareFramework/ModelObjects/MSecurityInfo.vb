Imports GrowthWare.Framework.ModelObjects.Base.Interfaces
Imports GrowthWare.Framework.ModelObjects
Imports System.Web
Imports System.Collections.ObjectModel

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public Class MSecurityInfo

		Private m_MayView As Boolean = False
		Private m_MayAdd As Boolean = False
		Private m_MayEdit As Boolean = False
		Private m_MayDelete As Boolean = False

		''' <summary>
		''' MayView()--
		''' This property is calculated relative to the current object that 
		''' implements ISecurityInfo.  
		''' When true, user can view the module.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property MayView() As Boolean
			Get
				Return m_MayView
			End Get
		End Property

		''' <summary>
		''' MayAdd()--
		''' This property is calculated relative to the current object that 
		''' implements ISecurityInfo.  
		''' When true, user can view the module.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property MayAdd() As Boolean
			Get
				Return m_MayAdd
			End Get
		End Property

		''' <summary>
		''' MayEdit()--
		''' This property is calculated relative to the current object that 
		''' implements ISecurityInfo.  
		''' When true, user can view the module.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property MayEdit() As Boolean
			Get
				Return m_MayEdit
			End Get
		End Property

		''' <summary>
		''' MayDelete()--
		''' This property is calculated relative to the current object that 
		''' implements ISecurityInfo.  
		''' When true, user can view the module.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property MayDelete() As Boolean
			Get
				Return m_MayDelete
			End Get
		End Property

		Public Sub New()

		End Sub

		''' <summary>
		''' Initializes a new HSecurityInfo object given an object that implements ISecurityInfo.
		'''  All client permissions are calculated relative to the object and the client roles.
		''' </summary>
		''' <param name="SecurityInfoObject"></param>
		''' <param name="AccountRoles"></param>
		''' <remarks></remarks>
		Public Sub New(ByRef SecurityInfoObject As ISecurityInfo, ByRef AccountRoles As Collection(Of String))
			' Check View Permissions
			m_MayView = CheckAuthenticatedPermission(SecurityInfoObject.ViewRoles, AccountRoles)
			' Check Add Permissions
			m_MayAdd = CheckAuthenticatedPermission(SecurityInfoObject.AddRoles, AccountRoles)
			' Check Edit Permissions
			m_MayEdit = CheckAuthenticatedPermission(SecurityInfoObject.EditRoles, AccountRoles)
			' Check Delete Permissions
			m_MayDelete = CheckAuthenticatedPermission(SecurityInfoObject.DeleteRoles, AccountRoles)
		End Sub	'New

		''' <summary>
		''' Checks whether an account is in the necessary role for the 4 permissions given an objects roles
		''' </summary>
		''' <param name="ObjRoles">String array</param>
		''' <returns>True/False</returns>
		''' <remarks></remarks>
		Protected Function CheckAuthenticatedPermission(ByVal objRoles As Collection(Of String), ByRef AccountRoles As Collection(Of String)) As Boolean
			Dim role As String
			Dim retVal As Boolean = False
			'If objRoles contains the role "Anonymous" the don't bother running the rest of code just return true
			If objRoles.Contains("Anonymous") Then Return True
			If objRoles.Contains("SysAdmin") Then Return True
			For Each role In objRoles
				If AccountRoles.Contains(role) Then
					retVal = True
					Exit For
				End If
			Next role	 ' Nope, doesn't have permissions
			Return retVal
		End Function

	End Class
End Namespace