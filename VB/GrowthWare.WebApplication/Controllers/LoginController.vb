Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports System.Globalization

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

        Public Function Logon(ByVal jsonData As LogonInfo) As IHttpActionResult
            If jsonData Is Nothing Then Throw New ArgumentNullException("logonInfo", "logonInfo can not be null or Nothing in VB.net")
            If String.IsNullOrEmpty(jsonData.Account) Then Throw New NullReferenceException("jsonData.Account can not be null or Nothing in VB.net")
            If String.IsNullOrEmpty(jsonData.Password) Then Throw New NullReferenceException("jsonData.Password can not be null or Nothing in VB.net")
            Dim mRetVal As String = "false"
            Dim mDomainPassed As Boolean = False
            If jsonData.Account.Contains("\") Then
                mDomainPassed = True
            End If
            If ConfigSettings.AuthenticationType.ToUpper() = "LDAP" And Not mDomainPassed Then
                jsonData.Account = ConfigSettings.LdapDomain + "\" + jsonData.Account
            End If
            If AccountUtility.Authenticated(jsonData.Account, jsonData.Password) Then
                Dim mAccountProfile As MAccountProfile = AccountUtility.GetProfile(jsonData.Account)
                AccountUtility.SetPrincipal(mAccountProfile)
                mRetVal = "true"
            Else
                Dim mAccountProfile As MAccountProfile = AccountUtility.GetProfile(jsonData.Account)
                If mAccountProfile IsNot Nothing Then
                    If mAccountProfile.Account.ToUpper(New CultureInfo("en-US", False)) = jsonData.Account.ToUpper(New CultureInfo("en-US", False)) Then
                        If ConfigSettings.AuthenticationType.ToUpper() = "INTERNAL" Then
                            mRetVal = "Request"
                        Else
                            Dim mMessageProfile As MMessageProfile = MessageUtility.GetProfile("Logon Error")
                            If mMessageProfile IsNot Nothing Then
                                mRetVal = mMessageProfile.Body
                            End If
                        End If
                    End If
                Else
                    Dim mMessageProfile As MMessageProfile = MessageUtility.GetProfile("Logon Error")
                    If mMessageProfile IsNot Nothing Then
                        mRetVal = mMessageProfile.Body
                    End If
                End If
            End If

            Return Ok(mRetVal)
        End Function
    End Class

    Public Class LogonInfo
        Public Property Account() As String
        Public Property Password() As String
    End Class
End Namespace