Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Profiles
Imports System.Data.SqlClient

Namespace DataAccessLayer.SQLServer.V2008
    Public Class DDirectories
        Inherits DDBInteraction
        Implements IDDirectories

        Public Function Directories() As DataTable Implements IDDirectories.Directories
            Dim mStoredProcedure As String = "ZGWOptional.Get_Directory"
            Dim mParameters As SqlParameter() =
            {
             New SqlParameter("@P_Function_SeqID", -1)
            }
            Return MyBase.GetDataTable(mStoredProcedure, mParameters)
        End Function

        'Public Property Profile As MDirectoryProfile Implements IDDirectories.Profile

        Public Sub Save(ByVal profile As MDirectoryProfile) Implements IDDirectories.Save
            Dim mStoredProcedure As String = "ZGWOptional.Set_Directory"
            Dim mParameters As SqlParameter() =
            {
              New SqlParameter("@P_Function_SeqID", profile.FunctionSeqId),
              New SqlParameter("@P_Directory", profile.Directory),
              New SqlParameter("@P_Impersonate", profile.Impersonate),
              New SqlParameter("@P_Impersonating_Account", profile.ImpersonateAccount),
              New SqlParameter("@P_Impersonating_Password", profile.ImpersonatePassword),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile)),
              GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output)
            }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Property SecurityEntitySeqID As Integer Implements IDDirectories.SecurityEntitySeqID
    End Class
End Namespace