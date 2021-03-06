﻿Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Net.Http
Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace Controllers
    Public Class SecurityEntitiesController
        Inherits ApiController

        <HttpPost>
        Public Function Save(ByVal uiProfile As MUISecurityEntityProfile) As IHttpActionResult
            If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As Boolean = False
            Dim mLog As Logger = Logger.Instance()
            If Not String.IsNullOrEmpty(uiProfile.Name) Then
                If Not HttpContext.Current.Session("EditId") Is Nothing Then
                    Dim mEditId = Integer.Parse(HttpContext.Current.Session("EditId").ToString())
                    If mEditId = uiProfile.Id Then
                        Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditSecurityEntity", True)), AccountUtility.CurrentProfile())
                        If Not mSecurityInfo Is Nothing Then
                            If mEditId <> -1 Then
                                If mSecurityInfo.MayEdit Then
                                    Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(mEditId)
                                    mSecurityEntityProfile = populateSecurityEntity(uiProfile, mSecurityEntityProfile)
                                    mSecurityEntityProfile.Id = uiProfile.Id
                                    SecurityEntityUtility.Save(mSecurityEntityProfile)
                                    mRetVal = True
                                Else
                                    Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                                    mLog.Error(mError)
                                    Return Me.InternalServerError(mError)
                                End If
                            Else
                                If mSecurityInfo.MayAdd Then
                                    Dim mSecurityEntityProfile As MSecurityEntityProfile = populateSecurityEntity(uiProfile, Nothing)
                                    mSecurityEntityProfile.Id = -1
                                    mSecurityEntityProfile.AddedBy = AccountUtility.CurrentProfile().Id
                                    mSecurityEntityProfile.AddedDate = Now
                                    mSecurityEntityProfile.UpdatedBy = mSecurityEntityProfile.AddedBy
                                    mSecurityEntityProfile.UpdatedDate = mSecurityEntityProfile.AddedDate
                                    SecurityEntityUtility.Save(mSecurityEntityProfile)
                                    mRetVal = True
                                Else
                                    Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                                    mLog.Error(mError)
                                    Return Me.InternalServerError(mError)
                                End If
                            End If
                        Else
                            Dim mError As Exception = New Exception("Security Info is not in context nothing has been saved!!!!")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                    Else
                        Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                End If
            Else
                Dim mError As ArgumentNullException = New ArgumentNullException("uiProfile", "uiProfile.Name  cannot be a null reference (Nothing in Visual Basic)!")
                Return Me.InternalServerError(mError)
            End If
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                mDataTable = SecurityEntityUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        <HttpGet>
        Public Function GetValidSecurityEntities() As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            'ClientChoicesState(MClientChoices.SecurityEntityId)
            Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Try
                mDataTable = SecurityEntityUtility.GetValidSecurityEntities(mAccountProfile.Account, 1, mAccountProfile.IsSystemAdmin).ToTable()
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        Private Function populateSecurityEntity(ByVal uiProfile As MUISecurityEntityProfile, securityEntityProfile As MSecurityEntityProfile) As MSecurityEntityProfile
            If securityEntityProfile Is Nothing Then securityEntityProfile = New MSecurityEntityProfile
            securityEntityProfile.UpdatedBy = AccountUtility.CurrentProfile.Id
            securityEntityProfile.UpdatedDate = Now
            securityEntityProfile.ConnectionString = uiProfile.ConnectionString
            securityEntityProfile.DataAccessLayer = uiProfile.DAL
            securityEntityProfile.DataAccessLayerAssemblyName = uiProfile.DALAssemblyName
            securityEntityProfile.DataAccessLayerNamespace = uiProfile.DALNamespace
            securityEntityProfile.Description = uiProfile.Description
            securityEntityProfile.EncryptionType = uiProfile.EncryptionType
            securityEntityProfile.Name = uiProfile.Name
            securityEntityProfile.ParentSeqId = uiProfile.ParentSeqId
            securityEntityProfile.Skin = uiProfile.Skin
            securityEntityProfile.StatusSeqId = uiProfile.StatusSeqId
            securityEntityProfile.Style = uiProfile.Style
            securityEntityProfile.Url = uiProfile.Url
            Return securityEntityProfile
        End Function

    End Class
End Namespace
