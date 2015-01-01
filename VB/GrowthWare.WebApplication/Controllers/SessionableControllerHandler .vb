Imports System.Web.Http.WebHost
Imports System.Web.Routing

Public Class SessionableControllerHandler
    Inherits HttpControllerHandler
    Implements IRequiresSessionState

    Public Sub New(ByVal routeData As RouteData)
        MyBase.New(routeData)
    End Sub

End Class
