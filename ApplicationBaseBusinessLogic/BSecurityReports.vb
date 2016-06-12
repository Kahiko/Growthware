Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase.Interfaces
Imports ApplicationBase.Model.States

Public Class BSecurityReports
    Private Shared iBaseDAL As ISecurityReports = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DSecurityReports")

    Public Shared Function SecurityByRole(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet
        Return iBaseDAL.SecurityByRole(Business_Unit_SEQ_ID, ENVIRONMENT)
    End Function

    Public Shared Function Security4Module(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet
        Return iBaseDAL.Security4Module(Business_Unit_SEQ_ID, ENVIRONMENT)
    End Function
End Class