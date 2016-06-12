Imports Common.Security.BaseSecurity
Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports DALModel.Special.Accounts
Imports System.Runtime.InteropServices

Namespace Special
    Public Class BAccount
		'Private Shared iBaseDAL As IAccount = FAccount.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IAccount = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DAccount")

        Public Shared Function AddAccount(ByVal profile As MAccountProfileInfo, Optional ByVal ClientChoicesAccount As String = "DEFAULT") As Integer
			Return iBaseDAL.AddAccount(profile, ClientChoicesAccount)
        End Function 'AddAccount

        Public Shared Function GetAccountsByLetter(ByVal dsAccounts As DataSet, ByVal AccountType As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
			Return iBaseDAL.GetAccountsByLetter(dsAccounts, AccountType, BUSINESS_UNIT_SEQ_ID)
        End Function 'GetAccountsByLetter

        Public Shared Function GetProfile(ByVal Account As String) As MAccountProfileInfo
			Return iBaseDAL.GetProfile(Account)
        End Function 'GetProfile

        Public Shared Function GetRolesFromDB(ByVal ACCOUNT_SEQ_ID As Integer, Optional ByVal BusinessUnitId As Integer = 0) As String()
			Return iBaseDAL.GetRolesFromDB(ACCOUNT_SEQ_ID, BusinessUnitId)
        End Function 'GetRolesFromDB

        Public Shared Function GetGroupsFromDB(ByVal ACCOUNT_SEQ_ID As Integer, Optional ByVal BusinessUnitId As Integer = 0) As String()
			Return iBaseDAL.GetGroupsFromDB(ACCOUNT_SEQ_ID, BusinessUnitId)
        End Function    'GetRolesFromDB

        Public Shared Function GetRolesFromDBByBusinessUnitID(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String()
			Return iBaseDAL.GetRolesFromDBByBusinessUnit(ACCOUNT_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
        End Function    'GetRolesFromDBByBusinessUnitID

        Public Shared Function GetGroupsFromDBByBusinessUnitID(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String()
			Return iBaseDAL.GetGroupsFromDBByBusinessUnit(ACCOUNT_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
        End Function    'GetRolesFromDBByBusinessUnitID

        Public Shared Function LoginClient(ByRef clientProfileInfo As MAccountProfileInfo, ByVal AccountName As String, ByVal Password As String, ByVal AuthenticationType As String, ByVal LDAPServer As String, ByVal LDAPDomain As String) As Boolean
            If AccountName.Trim.Length = 0 OrElse Password.Trim.Length = 0 Then
                Dim ex As New ApplicationException("You must supply both an account and password")
                Throw ex
            End If
            Dim retVal As Boolean = False
            Dim myCryptoUtil As New CryptoUtil
            Select Case AuthenticationType.ToLower    ' determine what type of authentication to envoke
                Case "internal"    ' envoke database authentication
                    If myCryptoUtil.EncryptTripleDES(Password) = clientProfileInfo.PWD Then
                        retVal = True
                    ElseIf Password = clientProfileInfo.PWD Then
                        retVal = True
                    End If
                Case "ldap"    ' envoke ldap authentication
                    Dim objLDAPUtility As New LDAPUtility
                    retVal = objLDAPUtility.IsAuthenticated(LDAPServer, LDAPDomain, AccountName, Password)
            End Select
            Return retVal
        End Function    'LoginClient

        Public Shared Function UpdateProfile(ByVal profile As MAccountProfileInfo) As Boolean
			Return iBaseDAL.UpdateProfile(profile)
        End Function    'UpdateProfile

        Public Shared Sub UpdateRoles(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BusinessUnitID As Integer, ByVal roles() As String)
			iBaseDAL.UpdateRoles(ACCOUNT_SEQ_ID, BusinessUnitID, roles)
        End Sub    'UpdateRoles

        Public Shared Sub UpdateGroups(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BusinessUnitID As Integer, ByVal groups() As String)
			iBaseDAL.UpdateGroups(ACCOUNT_SEQ_ID, BusinessUnitID, groups)
        End Sub    'UpdateRoles
    End Class
End Namespace