Imports System.Web
Imports GrowthWare.Framework.Model.Profiles
Imports System.Globalization
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer

Namespace Utilities
    ''' <summary>
    ''' ClientChoicesUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework
    ''' with regards to ClientChoices.
    ''' </summary>
    Public Module ClientChoicesUtility
        Private m_CachedAnonymousChoicesState As String = "AnonymousClientChoicesState"

        Public Function GetClientChoicesState(ByVal account As String) As MClientChoicesState
            Return GetClientChoicesState(account, False)
        End Function

        Public Function GetClientChoicesState(ByVal account As String, ByVal fromDB As Boolean) As MClientChoicesState
            Dim mRetVal As MClientChoicesState = Nothing
            If HttpContext.Current.Cache IsNot Nothing Then
                mRetVal = CType(HttpContext.Current.Cache(MClientChoices.SessionName), MClientChoicesState)
            End If
            Dim mBClientChoices As BClientChoices = New BClientChoices(SecurityEntityUtility.GetDefaultProfile(), ConfigSettings.CentralManagement)
            If fromDB Then Return mBClientChoices.GetClientChoicesState(account)
            If mRetVal Is Nothing Then
                If account.Trim().ToLower(CultureInfo.CurrentCulture) = "anonymous" Then
                    mRetVal = CType(HttpContext.Current.Cache(ClientChoicesUtility.m_CachedAnonymousChoicesState), MClientChoicesState)
                    If mRetVal Is Nothing Then
                        mRetVal = mBClientChoices.GetClientChoicesState(account)
                        CacheController.AddToCacheDependency(ClientChoicesUtility.m_CachedAnonymousChoicesState, mRetVal)
                    End If
                Else
                    mRetVal = mBClientChoices.GetClientChoicesState(account)
                End If
            Else
                If mRetVal.AccountName <> account Then
                    mRetVal = mBClientChoices.GetClientChoicesState(account)
                End If
            End If
            If HttpContext.Current.Cache IsNot Nothing Then
                HttpContext.Current.Cache(MClientChoices.SessionName) = mRetVal
            End If
            Return mRetVal
        End Function

        Public Function GetSelectedSecurityEntity() As Integer
            Dim myClientChoicesState As MClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.SessionName), MClientChoicesState)
            Dim result As Integer
            If myClientChoicesState IsNot Nothing Then
                result = Integer.Parse(myClientChoicesState(MClientChoices.SecurityEntityId))
            Else
                result = ConfigSettings.DefaultSecurityEntityId
            End If
            Return result
        End Function

        Public Sub Save(ByRef clientChoicesState As MClientChoicesState)
            Dim mBClientChoices As BClientChoices = New BClientChoices(SecurityEntityUtility.GetDefaultProfile(), ConfigSettings.CentralManagement)
            mBClientChoices.Save(clientChoicesState)
            If HttpContext.Current.Cache IsNot Nothing Then
                HttpContext.Current.Cache(MClientChoices.SessionName) = clientChoicesState
            End If
        End Sub

    End Module
End Namespace

