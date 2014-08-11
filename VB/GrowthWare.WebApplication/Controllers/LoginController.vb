Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles

Namespace Controllers
    Public Class LoginController
        Inherits ApiController

        Private m_Routes As MRoutes() = New MRoutes() _
        {
            New MRoutes() With {.Name = "", .RouteTemplate = ""},
            New MRoutes() With {.Name = "", .RouteTemplate = ""},
            New MRoutes() With {.Name = "", .RouteTemplate = ""}
        }

        Public Function GetRoutes() As IEnumerable(Of MRoutes)
            Return m_Routes
        End Function
    End Class
End Namespace