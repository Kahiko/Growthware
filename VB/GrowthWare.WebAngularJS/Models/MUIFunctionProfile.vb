Imports System.Collections.ObjectModel

Public Class MUIFunctionProfile
    Public Action As String

    Public DerivedAddRoles As Collection(Of String)
    Public DerivedDeleteRoles As Collection(Of String)
    Public DerivedEditRoles As Collection(Of String)
    Public DerivedViewRoles As Collection(Of String)

    Public Description As String
    Public DirectoryData As MUIDirectoryProfile
    Public EnableNotifications As Boolean
    Public EnableViewState As Boolean
    Public FunctionTypeSeqID As Integer
    Public Id As Integer
    Public IsNav As Boolean
    Public LinkBehavior As Integer
    Public MetaKeyWords As String
    Public Name As String
    Public NavigationTypeSeqId As Integer
    Public NoUI As Boolean
    Public Notes As String
    Public ParentID As Integer
    Public RedirectOnTimeout As Boolean
    Public RolesAndGroups As MUIFunctionRolesGroups
    Public Source As String
    Public Controller As String
End Class
