Imports ApplicationBase.Model

Namespace Modules
	'*******************************************
	' ModuleProfileInfo class represents
	' all the profile information for a given module
	'*******************************************
	<CLSCompliant(True)> _
	   Public Class MModuleProfileInfo
		Private _recordsPerPage As Integer = 5
		Private _MODULE_SEQ_ID As Integer = -1
		Private _Name As String = String.Empty
		Private _Description As String = String.Empty
		Private _Source As String = String.Empty
		Private _EnableViewState As Boolean = False
		Private _IS_NAV As Boolean = False
		Private _NAV_TYPE_SEQ_ID As Integer = 2
		Private _ParentID As Integer = 0
		Private _Action As String = String.Empty
		Private _inheritTransformations As Boolean
		Private _viewRoles() As String
		Private _addRoles() As String
		Private _editRoles() As String
		Private _deleteRoles() As String
		Private _viewGroups() As String
		Private _addGroups() As String
		Private _editGroups() As String
		Private _deleteGroups() As String
		Private _transformations As String

		'*********************************************************************
		'
		' MODULE_SEQ_ID Property
		' Specifies the sequence id of the module.
		' Also used as in the hierarchal menu placement as
		' the modules MenuID.
		'
		'*********************************************************************
		Public Property MODULE_SEQ_ID() As Integer
			Get
				Return _MODULE_SEQ_ID
			End Get
			Set(ByVal Value As Integer)
				_MODULE_SEQ_ID = Value
			End Set
		End Property

		'*********************************************************************
		'
		' ParentID Property
		' Specifies the parent id for a hierarchal menu placement.
		' ParentID should be the MODULE_SEQ_ID of the
		' menu item this will fall under or 0 as a root element
		'
		'*********************************************************************
		Public Property ParentID() As Integer
			Get
				Return _ParentID
			End Get
			Set(ByVal Value As Integer)
				_ParentID = Value
			End Set
		End Property

		'*********************************************************************
		'
		' Name Property
		' Specifies the name of the module.
		'
		'*********************************************************************
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal Value As String)
				_Name = Value.Trim
			End Set
		End Property

		'*********************************************************************
		'
		' Description Property
		'
		' A description of the module used as the "alt" for mouse overs.
		'
		'*********************************************************************

		Public Property Description() As String
			Get
				Return _Description
			End Get
			Set(ByVal Value As String)
				_Description = Value.Trim
			End Set
		End Property

		'*********************************************************************
		'
		' Source Property
		' The relitive location to the modules source.
		' EX:   Modules\System\LeftHand\Logon\Logon.ascx
		' Used by the NavControler to load the requested module
		'
		'*********************************************************************
		Public Property Source() As String
			Get
				Return _Source
			End Get
			Set(ByVal Value As String)
				_Source = Value.Trim
			End Set
		End Property

		'*********************************************************************
		' EnableViewState Property
		' Used to enable or disable the view state.
		'*********************************************************************
		Public Property EnableViewState() As Boolean
			Get
				Return _EnableViewState
			End Get
			Set(ByVal Value As Boolean)
				_EnableViewState = Value
			End Set
		End Property

		'*********************************************************************
		'
		' IS_NAV Property
		' Indicates if the module is avalible for navigation
		'
		'*********************************************************************
		Public Property IS_NAV() As Boolean
			Get
				Return _IS_NAV
			End Get
			Set(ByVal Value As Boolean)
				_IS_NAV = Value
			End Set
		End Property

		'*********************************************************************
		'
		' NAV_TYPE_SEQ_ID Property
		'
		' Determines the navigation location:
		'
		'*********************************************************************
		Public Property NAV_TYPE_SEQ_ID() As Integer
			Get
				Return _NAV_TYPE_SEQ_ID
			End Get
			Set(ByVal Value As Integer)
				_NAV_TYPE_SEQ_ID = Value
			End Set
		End Property

		'*********************************************************************
		'
		' Action Property
		'
		' Used by the navcontroler to retieve Source
		'
		'*********************************************************************
		Public Property Action() As String
			Get
				Return _Action
			End Get
			Set(ByVal Value As String)
				_Action = Value.Trim
			End Set
		End Property

		'*********************************************************************
		'
		' ViewRoles Property
		' Represents the roles of users who can view pages in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property ViewRoles() As String()
			Get
				Return _viewRoles
			End Get
		End Property

		'*********************************************************************
		'
		' AddRoles Property
		' Represents the roles of users who can add pages to this section. 
		'
		'*********************************************************************
		Public ReadOnly Property AddRoles() As String()
			Get
				Return _addRoles
			End Get
		End Property

		'*********************************************************************
		'
		' EditRoles Property
		' Represents the roles of users who can edit pages in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property EditRoles() As String()
			Get
				Return _editRoles
			End Get
		End Property

		'*********************************************************************
		'
		' DeleteRoles Property
		' Represents the roles of users who can delete pages in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property DeleteRoles() As String()
			Get
				Return _deleteRoles
			End Get
		End Property

		'*********************************************************************
		'
		' ViewGroups Property
		' Represents the Groups of users who can view pages in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property ViewGroups() As String()
			Get
				Return _viewGroups
			End Get
		End Property

		'*********************************************************************
		'
		' AddGroups Property
		' Represents the Groups of users who can add pages to this section. 
		'
		'*********************************************************************
		Public ReadOnly Property AddGroups() As String()
			Get
				Return _addGroups
			End Get
		End Property

		'*********************************************************************
		'
		' EditGroups Property
		' Represents the Groups of users who can edit pages in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property EditGroups() As String()
			Get
				Return _editGroups
			End Get
		End Property

		'*********************************************************************
		'
		' DeleteGroups Property
		' Represents the Groups of users who can delete pages in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property DeleteGroups() As String()
			Get
				Return _deleteGroups
			End Get
		End Property


		'*********************************************************************
		'
		' InheritTransformations Property
		' Indicates whether transformations are inherited from the parent
		' section. 
		'
		'*********************************************************************
		Public ReadOnly Property InheritTransformations() As Boolean
			Get
				Return _inheritTransformations
			End Get
		End Property

		'*********************************************************************
		'
		' Transformations Property
		' Represents the transformations that are applied to text
		' in this section. 
		'
		'*********************************************************************
		Public ReadOnly Property Transformations() As String
			Get
				Return _transformations
			End Get
		End Property

		'*********************************************************************
		'
		' RecordsPerPage Property
		' Represents the Records Per Page that are shown for a module
		' that uses the ContentList control. 
		'*********************************************************************
		Public Property RecordsPerPage() As Integer
			Get
				Return _recordsPerPage
			End Get
			Set(ByVal Value As Integer)
				_recordsPerPage = Value
			End Set
		End Property
		'*********************************************************************
		'
		' ModuleProfileInfo Constructor
		' Creates the ModuleProfileInfo object with default information. 
		'
		'*********************************************************************
		Public Sub New()

		End Sub


		'*********************************************************************
		'
		' ModuleProfileInfo Constructor
		' Initializes the ModuleProfileInfo object with a DataRow. 
		'
		'*********************************************************************
		Public Sub New(ByVal drowModule As DataRow, ByVal drowModuleSecurity() As DataRow)
			_MODULE_SEQ_ID = CInt(drowModule("MODULE_SEQ_ID"))
			_Name = CStr(drowModule("Name"))
			_Description = CStr(drowModule("Description"))
			_Source = CStr(drowModule("Source"))
			_EnableViewState = CStr(drowModule("Enable_View_State"))
			_IS_NAV = CStr(drowModule("IS_NAV"))
			_NAV_TYPE_SEQ_ID = CInt(drowModule("NAV_TYPE_SEQ_ID"))
			_ParentID = CInt(drowModule("PARENT_MODULE_SEQ_ID"))
			_Action = Trim(CStr(drowModule("module_Action")))
			_viewRoles = SplitRoles(drowModuleSecurity, MRoleType.value.ViewRole)
			_addRoles = SplitRoles(drowModuleSecurity, MRoleType.value.AddRole)
			_editRoles = SplitRoles(drowModuleSecurity, MRoleType.value.EditRole)
			_deleteRoles = SplitRoles(drowModuleSecurity, MRoleType.value.DeleteRole)
		End Sub		  'New

		'*********************************************************************
		'
		' SplitRoles Method
		' Splits the roles for a module.
		'
		'*********************************************************************
		Private Function SplitRoles(ByVal allRoles() As DataRow, ByVal moduleRoleType As MRoleType.value) As String()
			Dim colRoles As New ArrayList
			Dim row As DataRow
			For Each row In allRoles
				If CType(row("PERMISSIONS_SEQ_ID"), MRoleType.value) = moduleRoleType Then
					colRoles.Add(row("ROLE"))
				End If
			Next row
			Return CType(colRoles.ToArray(GetType(String)), String())
		End Function		  'SplitRoles
	End Class	' ModuleProfileInfo
End Namespace