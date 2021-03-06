﻿Public Class GroupControl
    Inherits System.Web.UI.UserControl

    Public AllGroups As ArrayList
    Public SelectedViewGroups As Array
    Public SelectedAddGroups As Array
    Public SelectedEditGroups As Array
    Public SelectedDeleteGroups As Array

    Public ReadOnly Property ViewGroups() As String
        Get
            Return ctlViewGroups.SelectedState
        End Get
    End Property

    Public ReadOnly Property ViewGroupsChanged() As Boolean
        Get
            Return ctlViewGroups.Changed
        End Get
    End Property

    Public ReadOnly Property AddGroups() As String
        Get
            Return ctlAddGroups.SelectedState
        End Get
    End Property

    Public ReadOnly Property AddGroupsChanged() As Boolean
        Get
            Return ctlAddGroups.Changed
        End Get
    End Property

    Public ReadOnly Property EditGroups() As String
        Get
            Return ctlEditGroups.SelectedState
        End Get
    End Property

    Public ReadOnly Property EditGroupsChanged() As Boolean
        Get
            Return ctlEditGroups.Changed
        End Get
    End Property

    Public ReadOnly Property DeleteGroups() As String
        Get
            Return ctlDeleteGroups.SelectedState
        End Get
    End Property

    Public ReadOnly Property DeleteGroupsChanged() As Boolean
        Get
            Return ctlDeleteGroups.Changed
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ctlViewGroups.DataSource = AllGroups
        ctlAddGroups.DataSource = AllGroups
        ctlEditGroups.DataSource = AllGroups
        ctlDeleteGroups.DataSource = AllGroups
        If Not SelectedViewGroups Is Nothing Then ctlViewGroups.SelectedItems = SelectedViewGroups
        If Not SelectedAddGroups Is Nothing Then ctlAddGroups.SelectedItems = SelectedAddGroups
        If Not SelectedEditGroups Is Nothing Then ctlEditGroups.SelectedItems = SelectedEditGroups
        If Not SelectedDeleteGroups Is Nothing Then ctlDeleteGroups.SelectedItems = SelectedDeleteGroups
        DataBind()
    End Sub

End Class