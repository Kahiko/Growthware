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
        Private s_CachedAnonymousChoicesState As String = "AnonymousClientChoicesState"

        Public Function GetClientChoicesState(ByVal account As String) As MClientChoicesState
            Return GetClientChoicesState(account, False)
        End Function

        Public Function GetClientChoicesState(ByVal account As String, ByVal fromDB As Boolean) As MClientChoicesState
            If String.IsNullOrEmpty(account) Then Throw New ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As MClientChoicesState = Nothing
            If HttpContext.Current.Cache IsNot Nothing Then
                mRetVal = CType(HttpContext.Current.Cache(MClientChoices.SessionName), MClientChoicesState)
            End If
            Dim mBClientChoices As BClientChoices = New BClientChoices(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement)
            If fromDB Then Return mBClientChoices.GetClientChoicesState(account)
            If mRetVal Is Nothing Then
                If account.Trim().ToLower(CultureInfo.CurrentCulture) = "anonymous" Then
                    mRetVal = CType(HttpContext.Current.Cache(ClientChoicesUtility.s_CachedAnonymousChoicesState), MClientChoicesState)
                    If mRetVal Is Nothing Then
                        mRetVal = mBClientChoices.GetClientChoicesState(account)
                        CacheController.AddToCacheDependency(ClientChoicesUtility.s_CachedAnonymousChoicesState, mRetVal)
                    End If
                Else
                    mRetVal = mBClientChoices.GetClientChoicesState(account)
                End If
            Else
                If mRetVal.AccountName <> account Then
                    mRetVal = mBClientChoices.GetClientChoicesState(account)
                End If
            End If
            If HttpContext.Current.Cache IsNot Nothing And mRetVal IsNot Nothing Then
                HttpContext.Current.Cache(MClientChoices.SessionName) = mRetVal
            End If
            Return mRetVal
        End Function

        Public Function SelectedSecurityEntity() As Integer
            Dim myClientChoicesState As MClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.SessionName), MClientChoicesState)
            Dim result As Integer
            If myClientChoicesState IsNot Nothing Then
                result = Integer.Parse(myClientChoicesState(MClientChoices.SecurityEntityId), CultureInfo.InvariantCulture)
            Else
                result = ConfigSettings.DefaultSecurityEntityId
            End If
            Return result
        End Function

        Public Sub Save(ByVal clientChoicesState As MClientChoicesState)
            If clientChoicesState Is Nothing Then Throw New ArgumentNullException("clientChoicesState", "clientChoicesState cannot be a null reference (Nothing in Visual Basic)!")
            Dim mBClientChoices As BClientChoices = New BClientChoices(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement)
            mBClientChoices.Save(clientChoicesState)
            If HttpContext.Current.Cache IsNot Nothing Then
                HttpContext.Current.Cache(MClientChoices.SessionName) = clientChoicesState
            End If
        End Sub

    End Module
End Namespace

