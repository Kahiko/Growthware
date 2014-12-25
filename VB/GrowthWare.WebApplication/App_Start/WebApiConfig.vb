Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports GrowthWare.WebSupport
Imports Newtonsoft.Json.Serialization
Imports System.Web.Routing

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        '' Web API configuration and services
        '' Configure Web API to use only bearer token authentication.
        'config.SuppressDefaultHostAuthentication()
        'config.Filters.Add(New HostAuthenticationFilter(OAuthDefaults.AuthenticationType))

        ' Use camel case for JSON data.
        'config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = New CamelCasePropertyNamesContractResolver()

        ' Web API routes
        config.MapHttpAttributeRoutes()

        RouteTable.Routes.MapHttpRoute(
            name:="GrowthwareApi",
            routeTemplate:="gw/api/{controller}/{action}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        ).RouteHandler = New SessionStateRouteHandler()

        ' Attach simple post variable binding
        'GlobalConfiguration.Configuration.ParameterBindingRules().Insert(0, param >= New SimplePostVariableParameterBinding.HookupParameterBinding(param))
        'config.ParameterBindingRules.Insert(0, Function(descriptor) New SimplePostVariableParameterBinding(descriptor))

    End Sub
End Module
