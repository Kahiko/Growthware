Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySQL.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports MySql.Data.MySqlClient

Namespace DataAccessLayer.MySQL.V5_6_21
    ''' <summary>
    ''' DClientChoices provides all database interaction to SQL Server 2000 to 2005
    ''' </summary>
    ''' <remarks>
    '''		The Profile and SecurityEntitySeqID properties must be set
    '''		before using any methods.
    '''		Properties where chosen instead of parameters because all
    '''		methods will need one or both to perform their work.
    '''	</remarks>
    Public Class DClientChoices
        Inherits DDBInteraction
        Implements IDClientChoices

#Region "Public Methods:"
        Public Function GetChoices(ByVal account As String) As DataRow Implements IDClientChoices.GetChoices
            If String.IsNullOrEmpty(account) Then Throw New ArgumentException("Must set the Account property", "account")
            Dim myParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_ACCOUNT", account),
              New MySqlParameter("@P_Debug", 0)
            }
            Return MyBase.GetDataRow("ZGWCoreWeb.Get_Account_Choice", myParameters)
        End Function

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId:="0")>
        Public Sub Save(ByVal clientChoicesStateHashtable As Hashtable) Implements IDClientChoices.Save
            If clientChoicesStateHashtable Is Nothing Or clientChoicesStateHashtable.Count = 0 Then Throw New ArgumentNullException("clientChoicesStateHashtable", "Must set the clientChoicesStateHashTable property")
            Dim HashKeyEnum As IEnumerator = CType(clientChoicesStateHashtable.Keys, IEnumerable).GetEnumerator()
            Dim HashValEnum As IEnumerator = CType(clientChoicesStateHashtable.Values, IEnumerable).GetEnumerator()
            Dim commandParameters(clientChoicesStateHashtable.Count - 1) As MySqlParameter
            Dim x As Integer = 0
            Do While (HashKeyEnum.MoveNext() And HashValEnum.MoveNext())
                Dim myParameter As MySqlParameter = New MySqlParameter("@P_" + HashKeyEnum.Current.ToString(), SqlDbType.NVarChar, 1000)
                myParameter.Value = HashValEnum.Current.ToString()
                commandParameters.SetValue(myParameter, x)
                x = x + 1
            Loop
            MyBase.ExecuteNonQuery("ZGWCoreWeb.Set_Account_Choices", commandParameters)
        End Sub
#End Region
    End Class
End Namespace

