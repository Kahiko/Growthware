Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports Newtonsoft.Json.Serialization
Imports GrowthWare.Framework.Common

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        Dim mLog As Logger = Logger.Instance()
        mLog.Debug("WebApiConfig.Register start")
        ' Web API configuration and services

        ' Web API routes
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )
        mLog.Debug("WebApiConfig.Register done")

    End Sub
End Module
