Imports System.Data.SqlClient

Namespace DataAccessLayer.SqlServer.V2008
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
		Inherits Base.DDBInteraction
		Implements IDClientChoices

#Region "Public Methods:"
		Public Function GetChoices(ByRef account As String) As DataRow Implements IDClientChoices.GetChoices
			If String.IsNullOrEmpty(account) Then Throw New ArgumentException("Must set the Account property", "account")
			Dim myParameters() As SqlParameter =
			{
			  New SqlParameter("@P_ACCOUNT", account),
			  New SqlParameter("@P_Debug", 0)
			}
			Return MyBase.GetDataRow("ZGWCoreWeb.Get_Account_Choice", myParameters)
		End Function

		Public Sub Save(ByRef clientChoicesStateHashTable As Hashtable) Implements IDClientChoices.Save
			If clientChoicesStateHashTable Is Nothing Or clientChoicesStateHashTable.Count = 0 Then Throw New ArgumentException("Must set the clientChoicesStateHashTable property", "clientChoicesStateHashTable")
			Dim HashKeyEnum As IEnumerator = CType(clientChoicesStateHashTable.Keys, IEnumerable).GetEnumerator()
			Dim HashValEnum As IEnumerator = CType(clientChoicesStateHashTable.Values, IEnumerable).GetEnumerator()
			Dim commandParameters(clientChoicesStateHashTable.Count - 1) As SqlParameter
			Dim x As Integer = 0
			Do While (HashKeyEnum.MoveNext() And HashValEnum.MoveNext())
				Dim myParameter As SqlParameter = New SqlParameter("@P_" + HashKeyEnum.Current.ToString(), SqlDbType.NVarChar, 1000)
				myParameter.Value = HashValEnum.Current.ToString()
				commandParameters.SetValue(myParameter, x)
				x = x + 1
			Loop
			MyBase.ExecuteNonQuery("ZGWCoreWeb.Account_Choices", commandParameters)
		End Sub
#End Region
	End Class
End Namespace
