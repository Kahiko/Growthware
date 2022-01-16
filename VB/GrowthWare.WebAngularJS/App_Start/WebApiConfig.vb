Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports System.Web.Routing

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        ' Web API configuration and services

        ' Web API routes
        config.MapHttpAttributeRoutes()

        RouteTable.Routes.MapHttpRoute(
            name:="GrowthwareApi",
            routeTemplate:="gw/api/{controller}/{action}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        ).RouteHandler = New SessionStateRouteHandler()
    End Sub
End Module
