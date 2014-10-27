Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Net.Http
Imports System.Globalization

Public Class SecurityEntitiesController
    Inherits ApiController

    <HttpPost>
    Public Function Save(ByVal uiProfile As MSecurityEntityProfile) As IHttpActionResult
        Dim mRetVal As Boolean = False
        Dim mResponse As HttpResponseMessage = Request.CreateResponse(mRetVal, Request)
        If Not String.IsNullOrEmpty(uiProfile.Name) Then
            SecurityEntityUtility.Save(uiProfile)
            mRetVal = True
        Else
            Dim mError As ArgumentNullException = New ArgumentNullException("uiProfile", "uiProfile.Name  cannot be a null reference (Nothing in Visual Basic)!")
            Return Me.InternalServerError(mError)
        End If
        Return Me.Ok(mRetVal)
    End Function
End Class
