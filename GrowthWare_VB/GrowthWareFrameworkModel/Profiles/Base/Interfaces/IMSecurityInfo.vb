Imports System.Collections.ObjectModel

Namespace Profiles.Base.Interfaces
	''' <summary>
	''' IMSecurityInfo sets the contract for all
	''' classing inheriting fromm MSecurity.vb
	''' </summary>
	Public Interface IMSecurityInfo
		''' <summary>
		''' A collectionn of assigned add roles
		''' </summary>
		ReadOnly Property AssignedAddRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of assigned delete roles
		''' </summary>
		ReadOnly Property AssignedDeleteRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of assigned edit roles
		''' </summary>
		ReadOnly Property AssignedEditRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of assigned view roles
		''' </summary>
		ReadOnly Property AssignedViewRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of roles that may add
		''' </summary>
		ReadOnly Property DerivedAddRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of roles that may delete
		''' </summary>
		ReadOnly Property DerivedDeleteRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of roles that may edit
		''' </summary>
		ReadOnly Property DerivedEditRoles() As Collection(Of String)

		''' <summary>
		''' A collectionn of roles that may view
		''' </summary>
		ReadOnly Property DerivedViewRoles() As Collection(Of String)
	End Interface
End Namespace