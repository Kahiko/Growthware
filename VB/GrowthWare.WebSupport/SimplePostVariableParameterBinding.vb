Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Web.Http.Metadata
Imports System.Linq
Imports System.Globalization

''' <summary>
''' Allows passing multipule simple POST values to ASP.NET Web API
''' </summary>
''' <remarks>From article http://weblog.west-wind.com/posts/2012/Sep/11/Passing-multiple-simple-POST-Values-to-ASPNET-Web-API</remarks>
Public Class SimplePostVariableParameterBinding
    Inherits HttpParameterBinding
    Private Const MultipleBodyParameters As String = "MultipleBodyParameters"

    Public Sub New(descriptor As HttpParameterDescriptor)
        MyBase.New(descriptor)
    End Sub

    ''' <summary>
    ''' Check for simple binding parameters in POST data. Bind POST
    ''' data as well as query string data
    ''' </summary>
    ''' <param name="metadataProvider"></param>
    ''' <param name="actionContext"></param>
    ''' <param name="cancellationToken"></param>
    ''' <returns></returns>
    Public Overrides Function ExecuteBindingAsync(metadataProvider As ModelMetadataProvider, actionContext As HttpActionContext, cancellationToken As CancellationToken) As Task
        Dim stringValue As String = Nothing

        Dim col As NameValueCollection = TryReadBody(actionContext.Request)
        If col IsNot Nothing Then
            stringValue = col(Descriptor.ParameterName)
        End If

        ' try reading query string if we have no POST/PUT match
        If stringValue Is Nothing Then
            Dim query = actionContext.Request.GetQueryNameValuePairs()
            If query IsNot Nothing Then
                Dim matches = query.Where(Function(kv) kv.Key.ToLower() = Descriptor.ParameterName.ToLower())
                If matches.Count() > 0 Then
                    stringValue = matches.First().Value
                End If
            End If
        End If

        Dim value As Object = StringToType(stringValue)

        ' Set the binding result here
        SetValue(actionContext, value)

        ' now, we can return a completed task with no result
        Dim tcs As New TaskCompletionSource(Of AsyncVoid)()
        tcs.SetResult(Nothing)
        Return tcs.Task
    End Function


    ''' <summary>
    ''' Method that implements parameter binding hookup to the global configuration object's
    ''' ParameterBindingRules collection delegate.
    ''' 
    ''' This routine filters based on POST/PUT method status and simple parameter
    ''' types.
    ''' </summary>
    ''' <example>
    ''' GlobalConfiguration.Configuration.
    '''       .ParameterBindingRules
    '''       .Insert(0,SimplePostVariableParameterBinding.HookupParameterBinding);
    ''' </example>    
    ''' <param name="descriptor"></param>
    ''' <returns></returns>
    Public Shared Function HookupParameterBinding(descriptor As HttpParameterDescriptor) As HttpParameterBinding
        Dim supportedMethods = descriptor.ActionDescriptor.SupportedHttpMethods

        ' Only apply this binder on POST and PUT operations
        If supportedMethods.Contains(HttpMethod.Post) OrElse supportedMethods.Contains(HttpMethod.Put) Then
            Dim supportedTypes = New Type() {GetType(String), GetType(Integer), GetType(Decimal), GetType(Double), GetType(Boolean), GetType(DateTime), _
                GetType(Byte())}

            If supportedTypes.Where(Function(typ) typ = descriptor.ParameterType).Count() > 0 Then
                Return New SimplePostVariableParameterBinding(descriptor)
            End If
        End If

        Return Nothing
    End Function


    Private Function StringToType(stringValue As String) As Object
        Dim value As Object = Nothing

        If stringValue Is Nothing Then
            value = Nothing
        ElseIf Descriptor.ParameterType = GetType(String) Then
            value = stringValue
        ElseIf Descriptor.ParameterType = GetType(Integer) Then
            value = Integer.Parse(stringValue, CultureInfo.CurrentCulture)
        ElseIf Descriptor.ParameterType = GetType(Int32) Then
            value = Int32.Parse(stringValue, CultureInfo.CurrentCulture)
        ElseIf Descriptor.ParameterType = GetType(Int64) Then
            value = Int64.Parse(stringValue, CultureInfo.CurrentCulture)
        ElseIf Descriptor.ParameterType = GetType(Decimal) Then
            value = Decimal.Parse(stringValue, CultureInfo.CurrentCulture)
        ElseIf Descriptor.ParameterType = GetType(Double) Then
            value = Double.Parse(stringValue, CultureInfo.CurrentCulture)
        ElseIf Descriptor.ParameterType = GetType(DateTime) Then
            value = DateTime.Parse(stringValue, CultureInfo.CurrentCulture)
        ElseIf Descriptor.ParameterType = GetType(Boolean) Then
            value = False
            If stringValue = "true" OrElse stringValue = "on" OrElse stringValue = "1" Then
                value = True
            End If
        Else
            value = stringValue
        End If

        Return value
    End Function

    ''' <summary>
    ''' Read and cache the request body
    ''' </summary>
    ''' <param name="request"></param>
    ''' <returns></returns>
    Private Function TryReadBody(request As HttpRequestMessage) As NameValueCollection
        Dim result As Object = Nothing

        ' try to read out of cache first
        If Not request.Properties.TryGetValue(MultipleBodyParameters, result) Then
            Dim contentType = request.Content.Headers.ContentType

            ' only read if there's content and it's form data
            If contentType Is Nothing OrElse contentType.MediaType <> "application/x-www-form-urlencoded" Then
                ' Nope no data
                result = Nothing
            Else
                ' parsing the string like firstname=Hongmei&lastname=ASDASD            
                result = request.Content.ReadAsFormDataAsync().Result
            End If

            request.Properties.Add(MultipleBodyParameters, result)
        End If

        Return TryCast(result, NameValueCollection)
    End Function

    Private Structure AsyncVoid
    End Structure
End Class
