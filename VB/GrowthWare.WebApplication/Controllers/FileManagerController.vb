Imports System.Net
Imports System.Web.Http
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class FileManagerController
        Inherits ApiController

        <HttpPost>
        Public Function GetDirectoryLinks(ByVal requestDirectoryInfo As RequestDirectoryLinksInfo) As IHttpActionResult
            Dim mRetVal As String = String.Empty
            mRetVal = FileUtility.GetDirectoryLinks(requestDirectoryInfo.CurrentDirectoryString, requestDirectoryInfo.FunctionSeqId)
            Return Me.Ok(mRetVal)
        End Function
    End Class

    Public Class RequestDirectoryLinksInfo
        Public Property CurrentDirectoryString() As String
        Public Property FunctionSeqId() As Integer
    End Class
End Namespace