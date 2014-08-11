Imports System.Net
Imports System.Web.Http

Namespace Controllers
    Public Class MenuController
        Inherits ApiController

        ' GET: api/Menu
        Public Function GetValues() As IEnumerable(Of String)
            Return New String() {"value1", "value2"}
        End Function

        ' GET: api/Menu/5
        Public Function GetValue(ByVal id As Integer) As String
            Return "value"
        End Function

        ' POST: api/Menu
        Public Sub PostValue(<FromBody()> ByVal value As String)

        End Sub
    End Class
End Namespace