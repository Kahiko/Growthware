Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports System.Collections.ObjectModel
Imports GrowthWare.WebSupport

Namespace Controllers
    Public Class SecurityEntitiesController
        Inherits ApiController

        ' GET api/SecurityEntities/GetAvalibleParrents?Action=xx&id=1
        <HttpGet>
        Public Function GetAvalibleParrents(<FromUri> ByVal id As Integer) As IHttpActionResult
            Dim mRetVal As New Collection(Of MSecurityEntityProfile)
            Dim mProfiles As Collection(Of MSecurityEntityProfile) = SecurityEntityUtility.Profiles()
            For Each mProfile As MSecurityEntityProfile In mProfiles
                If mProfile.Id <> id Then
                    mRetVal.Add(mProfile)
                End If
            Next
            Return Ok(mRetVal)
        End Function

        ' GET api/SecurityEntities/GetAvalibleStyles?Action=xx
        <HttpGet>
        Public Function GetAvalibleStyles() As IHttpActionResult
            Dim mDirectoryInfo As New MDirectoryProfile
            Dim mDataView As DataView = FileUtility.GetDirectoryTableData(GWWebHelper.StylePath, mDirectoryInfo, False).DefaultView
            mDataView.RowFilter = "[Name] like '%.css'"
            Dim mRetVal As New List(Of String)
            For Each mRow In mDataView.Table.Rows
                mRetVal.Add(mRow("ShortFileName"))
            Next
            Return Ok(mRetVal)
        End Function

        ' GET api/SecurityEntities/GetAvalibleSkins?Action=xx
        <HttpGet>
        Public Function GetAvalibleSkins() As IHttpActionResult
            Dim mDirectoryInfo As New MDirectoryProfile
            Dim mDataView As DataView = FileUtility.GetDirectoryTableData(GWWebHelper.SkinPath, mDirectoryInfo, False).DefaultView
            mDataView.RowFilter = "Type = 'folder'"
            Dim mRetVal As New List(Of String)
            For Each mRow In mDataView.Table.Rows
                mRetVal.Add(mRow("Name"))
            Next
            Return Ok(mRetVal)
        End Function

        ' GET api/SecurityEntities/GetSecurityEntity?Action=xx&id=xx
        <HttpGet>
        Public Function GetSecurityEntity(<FromUri> ByVal id As Integer) As IHttpActionResult
            Return Ok(Me.GetProfile(id))
        End Function

        ' GET api/SecurityEntities/GetSecurityEntityForEdit?Action=xx&id=xx
        Public Function GetSecurityEntityForEdit(<FromUri> ByVal id As Integer) As IHttpActionResult
            Dim mAccountProfile = AccountUtility.CurrentProfile()
            HttpContext.Current.Session("EditId") = mAccountProfile.Account + "_" + id.ToString()
            Return Ok(Me.GetProfile(id))
        End Function

        Private Function GetProfile(ByVal id As Integer) As MUISecurityEntityProfile
            Dim mRetVal As New MUISecurityEntityProfile()
            Dim mProfile As New MSecurityEntityProfile()
            If id > -1 Then
                mProfile = SecurityEntityUtility.GetProfile(id)
            End If
            With mRetVal
                .ConnectionString = mProfile.ConnectionString
                .DAL = mProfile.DataAccessLayer
                .DALAssemblyName = mProfile.DataAccessLayerAssemblyName
                .DALNamespace = mProfile.DataAccessLayerNamespace
                .Description = mProfile.Description
                .EncryptionType = mProfile.EncryptionType
                .Id = mProfile.Id
                .Name = mProfile.Name
                .ParentSeqId = mProfile.ParentSeqId
                .Skin = mProfile.Skin
                .StatusSeqId = mProfile.StatusSeqId
                .Style = mProfile.Style
                .Url = mProfile.Url
            End With
            Return mRetVal
        End Function

        ' POST api/SecurityEntities/GetSearchResults?Action=xx - searchCriteria
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

        ' GET api/SecurityEntities/GetValidSecurityEntities?Action=xx
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

        ' POST api/SecurityEntities/Save?Action=xx - uiProfile
        <HttpPost>
        Public Function Save(ByVal uiProfile As MUISecurityEntityProfile) As IHttpActionResult
            If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As String = "false"
            Dim mLog As Logger = Logger.Instance()
            If Not String.IsNullOrEmpty(uiProfile.Name) Then
                If HttpContext.Current.Session("EditId") IsNot Nothing Then
                    Dim mRawEditId As String() = HttpContext.Current.Session("EditId").ToString().Split("_")
                    Dim mEditId = Integer.Parse(mRawEditId(1))
                    If mEditId = uiProfile.Id Then
                        Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditSecurityEntity", True)), AccountUtility.CurrentProfile())
                        If mSecurityInfo IsNot Nothing Then
                            If mEditId <> -1 Then
                                If mSecurityInfo.MayEdit Then
                                    Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(mEditId)
                                    mSecurityEntityProfile = populateSecurityEntity(uiProfile, mSecurityEntityProfile)
                                    mSecurityEntityProfile.Id = uiProfile.Id
                                    SecurityEntityUtility.Save(mSecurityEntityProfile)
                                    mRetVal = "true"
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
                                    mRetVal = "true"
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

    End Class
End Namespace
