Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Profiles
Imports System.Data.SqlClient

Namespace DataAccessLayer.SQLServer.V2008
    Public Class DDBInformation
        Inherits DDBInteraction
        Implements IDBInformation

        Private m_Profile As MDBInformation = Nothing

        Public Function GetProfile() As DataRow Implements IDBInformation.GetProfile
            Dim mStoredProcedure As String = "ZGWSystem.Get_Database_Information"
            Return MyBase.GetDataRow(mStoredProcedure)
        End Function

        Public Property Profile As MDBInformation Implements IDBInformation.Profile
            Get
                Return m_Profile
            End Get
            Set(ByVal value As MDBInformation)
                m_Profile = value
            End Set
        End Property

        Public Function UpdateProfile() As Boolean Implements IDBInformation.UpdateProfile
            Dim mStoredProcedure As String = "ZGWSystem.Set_DataBase_Information"
            Dim mParameters() As SqlParameter = {
                New SqlParameter("@P_Database_Information_SeqID", m_Profile.InformationSeqId),
                New SqlParameter("@P_Version", m_Profile.Version),
                New SqlParameter("@P_Enable_Inheritance", m_Profile.EnableInheritance),
                New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile)),
                GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.InputOutput)
             }
            Try
                MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
                Return True
            Catch ex As Exception
                Throw ex
            End Try

        End Function
    End Class

End Namespace
