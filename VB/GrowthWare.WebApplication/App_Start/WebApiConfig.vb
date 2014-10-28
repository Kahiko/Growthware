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

        config.Routes.MapHttpRoute(
            name:="DefaultApi1",
            routeTemplate:="api/{controller}/{action}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )


        ' Attach simple post variable binding
        'GlobalConfiguration.Configuration.ParameterBindingRules().Insert(0, param >= New SimplePostVariableParameterBinding.HookupParameterBinding(param))
        'config.ParameterBindingRules.Insert(0, Function(descriptor) New SimplePostVariableParameterBinding(descriptor))

    End Sub

    Sub RegisterRoutes(routes As RouteCollection)
        Dim sessionRoute As Route = Nothing
        Dim sessionUrlPattern As String

        sessionUrlPattern = "api/{controller}/{action}/{id}"
        sessionRoute = New Route(sessionUrlPattern, New SessionStateRouteHandler())
        sessionRoute.Defaults = New RouteValueDictionary(New With {.id = RouteParameter.Optional})
        routes.Add("DefaultApi", sessionRoute)

    End Sub
End Module
