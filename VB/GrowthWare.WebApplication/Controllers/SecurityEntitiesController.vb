Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Net.Http
Imports System.Globalization

Public Class SecurityEntitiesController
    Inherits ApiController

    <HttpPost>
    Public Function Save(ByVal uiProfile As MUISecurityEntityProfile) As IHttpActionResult
        If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
        If HttpContext.Current.Items("EditId") Is Nothing Then
            Dim mike As String = String.Empty
        End If
        Dim mRetVal As Boolean = False
        Dim mResponse As HttpResponseMessage = Request.CreateResponse(mRetVal, Request)
        If Not String.IsNullOrEmpty(uiProfile.Name) Then
            'SecurityEntityUtility.Save(uiProfile)
            mRetVal = True
        Else
            Dim mError As ArgumentNullException = New ArgumentNullException("uiProfile", "uiProfile.Name  cannot be a null reference (Nothing in Visual Basic)!")
            Return Me.InternalServerError(mError)
        End If
        Return Me.Ok(mRetVal)
    End Function
End Class


Public Class MUISecurityEntityProfile
    Public Property ConnectionString As String
    Public Property DAL As String
    Public Property DALAssemblyName As String
    Public Property DALNamespace As String
    Public Property Description As String
    Public Property EncryptionType As Integer
    Public Property Id As String
    Public Property Name As String
    Public Property ParentSeqId As Integer
    Public Property Skin As String
    Public Property StatusSeqId As Integer
    Public Property Style As String
    Public Property Url As String
End Class