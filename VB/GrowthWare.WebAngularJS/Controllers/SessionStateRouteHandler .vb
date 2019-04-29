Imports System.Web.Routing
Imports System.Web.SessionState
Imports System.Web.Http.WebHost

Public NotInheritable Class SessionStateRouteHandler
    Implements IRouteHandler

    Public Function GetHttpHandler(requestContext As RequestContext) As IHttpHandler Implements IRouteHandler.GetHttpHandler
        Return New SessionableControllerHandler(requestContext.RouteData)
    End Function
End Class
