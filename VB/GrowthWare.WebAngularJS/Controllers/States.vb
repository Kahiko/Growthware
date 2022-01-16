Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Web.Http
Public Class StatesController
    Inherits ApiController

    <HttpPost>
    Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
        Dim mDataTable As DataTable = Nothing
        Try
            mDataTable = StatesUtility.Search(searchCriteria)
        Catch ex As Exception
            ' Only going to log message
            Dim mLog As Logger = Logger.Instance()
            mLog.Error(ex)
        End Try
        Return Ok(mDataTable)
    End Function

    <HttpGet>
    Public Function GetProfile(<FromUri()> ByVal state As String) As MUIState
        Dim mRetVal As MUIState = New MUIState()
        Dim mProfile As MStateProfile = StatesUtility.GetProfile(state)
        With mRetVal
            .Description = mProfile.Description
            .State = mProfile.State
            .Status_SeqID = mProfile.Status_SeqID
        End With
        Return mRetVal
    End Function

    <HttpPost>
    Public Function SaveProfile(<FromBody> ByVal profile As MUIState) As Boolean
        Dim mRetVal As Boolean = False
        Dim mProfile As MStateProfile = StatesUtility.GetProfile(profile.State)
        ' Only allowed to change the following
        With mProfile
            .Description = profile.Description
            .Status_SeqID = profile.Status_SeqID
            .UpdatedBy = AccountUtility.CurrentProfile().Id
        End With
        mRetVal = StatesUtility.Save(mProfile)
        Return mRetVal
    End Function
End Class
