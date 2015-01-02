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
    Public Class ClientChoicesUtility
        Private Shared s_CachedAnonymousChoicesState As String = "AnonymousClientChoicesState"

        ''' <summary>
        ''' Returns the client choices given the account
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <returns>MClientChoicesState</returns>
        Public Shared Function GetClientChoicesState(ByVal account As String) As MClientChoicesState
            Return GetClientChoicesState(account, False)
        End Function

        ''' <summary>
        ''' Gets the state of the client choices.
        ''' </summary>
        ''' <param name="account">The account.</param>
        ''' <param name="fromDB">if set to <c>true</c> [from database].</param>
        ''' <returns>MClientChoicesState.</returns>
        Public Shared Function GetClientChoicesState(ByVal account As String, ByVal fromDB As Boolean) As MClientChoicesState
            If String.IsNullOrEmpty(account) Then Throw New ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As MClientChoicesState = Nothing
            Dim mBClientChoices As BClientChoices = New BClientChoices(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement)
            If fromDB Then Return mBClientChoices.GetClientChoicesState(account)
            If account.Trim().ToLower(CultureInfo.CurrentCulture) <> "anonymous" Then
                mRetVal = CType(HttpContext.Current.Cache(MClientChoices.SessionName), MClientChoicesState)
                If mRetVal Is Nothing Then
                    mRetVal = mBClientChoices.GetClientChoicesState(account)
                    HttpContext.Current.Cache(MClientChoices.SessionName) = mRetVal
                End If
            Else
                mRetVal = CType(HttpContext.Current.Cache(ClientChoicesUtility.s_CachedAnonymousChoicesState), MClientChoicesState)
                If mRetVal Is Nothing Then
                    mRetVal = mBClientChoices.GetClientChoicesState(account)
                    CacheController.AddToCacheDependency(ClientChoicesUtility.s_CachedAnonymousChoicesState, mRetVal)
                End If
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the selected security entity.
        ''' </summary>
        ''' <returns>System.Int32.</returns>
        Public Shared Function SelectedSecurityEntity() As Integer
            Dim myClientChoicesState As MClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.SessionName), MClientChoicesState)
            Dim result As Integer
            If myClientChoicesState IsNot Nothing Then
                result = Integer.Parse(myClientChoicesState(MClientChoices.SecurityEntityId), CultureInfo.InvariantCulture)
            Else
                result = ConfigSettings.DefaultSecurityEntityId
            End If
            Return result
        End Function

        ''' <summary>
        ''' Save the client choices to the database.
        ''' </summary>
        ''' <param name="clientChoicesState">MClientChoicesState</param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal clientChoicesState As MClientChoicesState)
            Save(clientChoicesState, True)
        End Sub

        ''' <summary>
        ''' Save the client choices to the database.
        ''' </summary>
        ''' <param name="clientChoicesState">MClientChoicesState</param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal clientChoicesState As MClientChoicesState, ByVal updateContext As Boolean)
            If clientChoicesState Is Nothing Then Throw New ArgumentNullException("clientChoicesState", "clientChoicesState cannot be a null reference (Nothing in Visual Basic)!")
            Dim mBClientChoices As BClientChoices = New BClientChoices(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement)
            mBClientChoices.Save(clientChoicesState)
            If (updateContext) Then
                If HttpContext.Current.Cache IsNot Nothing Then
                    HttpContext.Current.Cache(MClientChoices.SessionName) = clientChoicesState
                End If
            End If
        End Sub

    End Class
End Namespace

