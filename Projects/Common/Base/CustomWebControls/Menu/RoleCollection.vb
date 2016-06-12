Imports System.Collections.Specialized
Imports System.Web.UI

'/ <summary>
   '/ Provides a collection of roles.
   '/ </summary>
   
   Public Class RoleCollection
		Implements IStateManager
      ' private member variables
		Private _roles As New StringDictionary
		Private _isTrackingViewState As Boolean = False
      
      #Region "IStateManager Interface"
      
      '/ <summary>
      '/ Indicates that changes to the view state should be tracked.
      '/ </summary>
		Sub TrackViewState() Implements IStateManager.TrackViewState
			Me._isTrackingViewState = True
		End Sub		  'IStateManager.TrackViewState


		'/ <summary>
		'/ Returns an array of objects, where each object is a string in the collection.
		'/ </summary>
		'/ <returns>An object array.</returns>
		Function SaveViewState() As Object Implements IStateManager.SaveViewState
			If _roles.Count = 0 OrElse Me._isTrackingViewState = False Then
				Return Nothing
			End If
			'Dim state(Roles.Keys.Count) As Object
			'Roles.Keys.CopyTo(state, 0)
			Dim state(Roles().Length) As Object
			Roles().CopyTo(state, 0)

			Return state
		End Function		  'IStateManager.SaveViewState


		'/ <summary>
		'/ Iterate through the object array passed in.  For each element in the object array
		'/ passed-in, a new role is added to the collection.
		'/ </summary>
		'/ <param name="savedState">The object array returned by the SaveViewState() method in
		'/ the previous page visit.</param>
		Sub LoadViewState(ByVal savedState As Object) Implements IStateManager.LoadViewState
			If Not (savedState Is Nothing) Then
				Dim state As Object() = CType(savedState, Object())

				_roles.Clear()

				Dim i As Integer
				For i = 0 To state.Length - 1
					_roles.Add(CStr(state(i)), String.Empty)
				Next i
			End If
		End Sub		  'IStateManager.LoadViewState
#End Region
      
      #Region "Collection-Related Methods"
      
      '/ <summary>
      '/ Adds a new role to the collection.
      '/ </summary>
      '/ <param name="role">The name of the role to add.</param>
      Public Overridable Sub Add(role As String)
			_roles.Add(role, String.Empty)
      End Sub 'Add
      
      
      '/ <summary>
      '/ Adds a range of roles to the collection.
      '/ </summary>
      '/ <param name="roles">A string array of roles to add.</param>
      Public Overridable Sub AddRange(roles() As String)
         Dim i As Integer
			For i = 0 To roles.Length - 1
				Me.Add(roles(i))
			Next i
      End Sub 'AddRange
       
      '/ <summary>
      '/ Removes a role from the collection.
      '/ </summary>
      '/ <param name="role">The name of the role to remove.</param>
      Public Overridable Sub Remove(role As String)
			_roles.Remove(role)
      End Sub 'Remove
      
      
      '/ <summary>
      '/ Clears out the roles collection.
      '/ </summary>
      Public Overridable Sub Clear()
			_roles.Clear()
      End Sub 'Clear
      
      
      '/ <summary>
      '/ Returns a Boolean indicating if the passed-in role name exists in the role collection.
      '/ </summary>
      '/ <param name="role">A role.</param>
      '/ <returns>Returns <b>true</b> if <b>role</b> is contained in the collection, <b>false</b> if it is not.</returns>
      Public Overridable Function Contains(role As String) As Boolean
			Return _roles.ContainsKey(role)
      End Function 'Contains
      
      
      '/ <summary>
      '/ Returns true if this role collection and the passed in role collection are disjoint (share no elements in
      '/ common).
      '/ </summary>
      '/ <param name="roles">A role collection.</param>
      '/ <returns><b>true</b> if the collections are disjoint, <b>false</b> otherwise.</returns>
      Public Overridable Function Disjoint(roles As RoleCollection) As Boolean
         Dim role As String
			For Each role In Me._roles.Keys
				If roles.Contains(role) Then
					Return False
				End If
			Next role		  ' no common roles, return true
			Return True
      End Function 'Disjoint
      #End Region
      
      #Region "Public Properties"
      '/ <summary>
      '/ Returns the number of roles in the collection.
      '/ </summary>
      '/ <value>An integer value greater than or equal to zero indicating the number of roles in the role collection.</value>
      
      Public Overridable ReadOnly Property Count() As Integer
         Get
				Return _roles.Count
         End Get
      End Property
      
      '/ <summary>
      '/ Returns the list of roles as a string array.
      '/ </summary>
      '/ <remarks>The RoleCollection class only contains methods to add, remove, and check to see if a role exists in
      '/ the collection.  It does not provide a means to enumerate through the roles.  In order to enumerate through
      '/ the roles, use this property to retrieve the roles as a string array.</remarks>
      
      Public Overridable ReadOnly Property Roles() As String()
			Get
				'Dim rArray(Roles().Keys.Count) As String
				Dim rArray(Roles().Length) As String
				Me._roles.Keys.CopyTo(rArray, 0)
				Return rArray
			End Get
		End Property
      
      '/ <summary>
      '/ Specifies if the RoleCollection is tracking ViewState.  Required, since RoleCollection
      '/ implements the IStateManager interface.
      '/ </summary>
      
		ReadOnly Property IsTrackingViewState() As Boolean Implements IStateManager.IsTrackingViewState
			Get
				Return Me._isTrackingViewState
			End Get
		End Property
#End Region
   End Class 'RoleCollection