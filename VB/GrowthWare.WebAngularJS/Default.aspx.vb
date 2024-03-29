﻿Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities
Imports System.IO

Public Class _Default
    Inherits BaseWebpage


    Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        MyBase.Page_PreInit(sender, e)
        If ConfigSettings.ForceHttps And Not HttpContext.Current.Request.Url.Scheme.ToLowerInvariant() = "https" Then
            Response.Redirect(ConfigSettings.RootSite)
        End If
        Dim mSecProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
        Dim mMasterPage As String = "Content/Skins/" + mSecProfile.Skin + "/" + mSecProfile.Skin + ".master"
        Dim fileName As String = String.Empty
        fileName = HttpContext.Current.Server.MapPath("~\") + mMasterPage
        If Not File.Exists(fileName) Then
            mMasterPage = "Content/Skins/Default/Default.Master"
        End If
        Me.MasterPageFile = mMasterPage
    End Sub

End Class