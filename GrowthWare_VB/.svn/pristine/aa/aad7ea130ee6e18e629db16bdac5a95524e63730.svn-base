Imports System.Data.Common
Imports System.Data.SqlClient
Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.Model.Profiles.Base.Interfaces
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Namespace DataAccessLayer.SqlServer.Base
	''' <summary>
	''' Performs all data store interaction to SQL Server through the use of stored procedures only.
	''' </summary>
	''' <remarks>Uses Microsoft.Practices.EnterpriseLibrary.Data for underlying database access.</remarks>
	Public MustInherit Class DDBInteraction
		Implements IDDBInteraction, IDisposable

#Region "Private Fields"
		Private m_DisposedValue As Boolean ' To detect redundant calls
#End Region

#Region "Public Properties"
		''' <summary>
		''' Used for all methods to connect to the database.
		''' </summary>
		Property ConnectionString As String Implements IDDBInteraction.ConnectionString
#End Region

#Region "Private Methods"
		''' <summary>
		''' Formats a an error message containing the stored procedure name and the sqlparameter/values.
		''' </summary>
		''' <param name="parameters">SqlParameter</param>
		''' <param name="storedProcedure">String</param>
		''' <param name="yourExMSG">String</param>
		''' <returns>String</returns>
		''' <remarks>Used when a DB error occures</remarks>
		Private Function formatError(ByVal parameters() As SqlParameter, ByVal storedProcedure As String, ByVal yourExMSG As String) As String
			Dim mMessage As String = vbCrLf + "Error executing '" + storedProcedure + "' :: " + vbCrLf
			Dim testParameter As SqlParameter
			mMessage += "Parameters are as follows:" + vbCrLf
			For Each testParameter In parameters
				mMessage += testParameter.ParameterName.ToString() + " = "
				If (Not testParameter.Value Is Nothing) Then
					mMessage += testParameter.Value.ToString() + vbCrLf
				End If
			Next
			mMessage += "Connection string : " + ConnectionString + vbCrLf
			mMessage += "Originating error :: " + vbCrLf
			mMessage += yourExMSG + vbCrLf
			Return mMessage
		End Function

		''' <summary>
		''' Ensures ConnectionString has a value.
		''' </summary>
		''' <remarks>Throws ArgumentException</remarks>
		Protected Overridable Sub IsValid()
			If ConnectionString Is Nothing Or ConnectionString.Length = 0 Then
				Throw New ArgumentException("ConnectionString cannot be blank!")
			End If
		End Sub
#End Region

#Region "IDDBInteraction Members"
		''' <summary>
		''' Executes a non Query given the store procedure and sql parameters
		''' </summary>
		''' <param name="storedProcedure">String</param>
		''' <param name="parameters">SqlParmeter</param>
		''' <remarks></remarks>
		Protected Overridable Sub ExecuteNonQuery(ByRef storedProcedure As String, Optional ByRef parameters() As SqlParameter = Nothing)
			Me.IsValid()
			Dim myParameter As SqlParameter = Nothing
			Try
				Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
				Dim dbCommand As DbCommand = db.GetStoredProcCommand(storedProcedure)
				If Not (parameters Is Nothing) Then
					If parameters.Length > 0 Then
						For Each myParameter_loopVariable As SqlParameter In parameters
							myParameter = myParameter_loopVariable
							dbCommand.Parameters.Add(myParameter)
						Next
						db.ExecuteNonQuery(dbCommand)
					Else
						db.ExecuteNonQuery(dbCommand)
					End If
				Else
					db.ExecuteNonQuery(CommandType.StoredProcedure, storedProcedure)
				End If
			Catch ex As SqlException
				If ex.Message.ToUpper.StartsWith("CANNOT OPEN DATABASE") Then
					Throw
				Else
					Throw New DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString), ex)
				End If
			Catch ex As Exception
				Throw New DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString), ex)
			End Try
		End Sub

		''' <summary>
		''' Returns a DataSet given the store procedure and sql parameters
		''' </summary>
		''' <param name="storedProcedure">String</param>
		''' <param name="parameters">SqlParmeter</param>
		''' <returns>DataSet</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetDataSet(ByRef storedProcedure As String, Optional ByRef parameters() As SqlParameter = Nothing) As DataSet
			Me.IsValid()
			Dim mRetDataSet As DataSet = Nothing
			Dim mParameter As SqlParameter = Nothing
			Try
				If Not parameters Is Nothing Then
					If parameters.Length > 0 Then
						Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
						Dim dbCommand As System.Data.Common.DbCommand = db.GetStoredProcCommand(storedProcedure)
						For Each mParameter In parameters
							dbCommand.Parameters.Add(mParameter)
						Next
						mRetDataSet = db.ExecuteDataSet(dbCommand)
					Else
						Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
						mRetDataSet = db.ExecuteDataSet(CommandType.StoredProcedure, storedProcedure)
					End If
				Else
					Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
					mRetDataSet = db.ExecuteDataSet(CommandType.StoredProcedure, storedProcedure)
				End If
				If mRetDataSet Is Nothing Then
					Dim myMessage As String = vbCrLf & "Store procedure '" & storedProcedure & "' executed and no data was found." & vbCrLf
					Dim testParameter As SqlParameter
					myMessage &= "Parameters are as follows:" & vbCrLf
					For Each testParameter In parameters
						myMessage += testParameter.ParameterName & " = " & testParameter.Value & vbCrLf
					Next
					Throw New DataAccessLayerException(myMessage)
				End If
			Catch ex As SqlException
				If ex.Message.ToUpper.StartsWith("CANNOT OPEN DATABASE") Then
					Throw
				Else
					Throw New DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString), ex)
				End If
			Catch ex As Exception
				Throw New DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString), ex)
			End Try
			Return mRetDataSet
		End Function

		''' <summary>
		''' Returns a DataTable given the stored procedure and sql parameters
		''' </summary>
		''' <param name="storedProcedure">String</param>
		''' <param name="parameters">SqlParameter</param>
		''' <returns>DataTable</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetDataTable(ByRef storedProcedure As String, Optional ByRef parameters() As SqlParameter = Nothing) As DataTable
			Me.IsValid()
			Dim mRetDataTable As New DataTable
			Dim mReader As IDataReader = Nothing
			Dim mParameter As SqlParameter = Nothing
			Try
				If Not parameters Is Nothing Then
					If parameters.Length > 0 Then
						Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
						Dim dbCommand As System.Data.Common.DbCommand = db.GetStoredProcCommand(storedProcedure)
						For Each mParameter In parameters
							dbCommand.Parameters.Add(mParameter)
						Next
						mReader = db.ExecuteReader(dbCommand)
					Else
						Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
						mReader = db.ExecuteReader(CommandType.StoredProcedure, storedProcedure)
					End If
				Else
					Dim db As SqlDatabase = New SqlDatabase(ConnectionString)
					mReader = db.ExecuteReader(CommandType.StoredProcedure, storedProcedure)
				End If
				If Not mReader Is Nothing Then
					mRetDataTable.Load(mReader)
				Else
					Dim myMessage As String = vbCrLf & "Store procedure '" & storedProcedure & "' executed and no data was found." & vbCrLf
					Dim testParameter As SqlParameter
					myMessage &= "Parameters are as follows:" & vbCrLf
					For Each testParameter In parameters
						myMessage += testParameter.ParameterName & " = " & testParameter.Value & vbCrLf
					Next
					Throw New DataAccessLayerException(myMessage)
				End If
			Catch ex As SqlException
				mRetDataTable.Dispose()
				If ex.Message.ToUpper.StartsWith("CANNOT OPEN DATABASE") Then
					Throw
				Else
					Throw New DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString), ex)
				End If
			Catch ex As Exception
				mRetDataTable.Dispose()
					Throw New DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString), ex)
			Finally
				If Not (mReader Is Nothing) Then
					mReader.Close()
					mReader = Nothing
				End If
			End Try
			Return mRetDataTable
		End Function

		''' <summary>
		''' Returns a DataRow given the store procedure and sql parameters
		''' </summary>
		''' <param name="storedProcedure">Stirng</param>
		''' <param name="parameters">SqlParameter</param>
		''' <returns>DataRow</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetDataRow(ByRef storedProcedure As String, ByRef parameters() As SqlParameter) As DataRow
			Me.IsValid()
			Return Me.GetDataTable(storedProcedure, parameters).Rows(0)
		End Function

		''' <summary>
		''' Returns a DataRow given the store procedure
		''' </summary>
		''' <param name="storedProcedure">Stirng</param>
		''' <returns>DataRow</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetDataRow(ByRef storedProcedure As String) As DataRow
			Dim mParameters As SqlParameter() = Nothing
			Return Me.GetDataTable(storedProcedure, mParameters).Rows(0)
		End Function

		''' <summary>
		''' Returns the value of an output parameter given the parameter name and an array of parameters
		''' </summary>
		''' <param name="parameterName">ParameterName</param>
		''' <param name="parameters">SqlParameter</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function GetParameterValue(ByVal parameterName As String, ByRef parameters() As SqlParameter) As String
			Dim mRetVal As String = String.Empty
			For Each parameter In parameters
				If parameter.ParameterName = parameterName Then
					If Not IsDBNull(parameter.Value) Then
						mRetVal = parameter.Value
					End If
					Exit For
				End If
			Next
			Return mRetVal
		End Function

		''' <summary>
		''' Retruns a SqlParameter given the ParameterName, ParameterValue and Direction.
		''' </summary>
		''' <param name="parameterName">String</param>
		''' <param name="parameterValue">Object</param>
		''' <param name="direction">ParameterDirection</param>
		''' <returns>SqlParameter</returns>
		Protected Function GetSqlParameter(ByVal parameterName As String, ByVal parameterValue As Object, ByVal direction As ParameterDirection) As SqlParameter
			Dim mRetVal As SqlParameter = New SqlParameter(parameterName, parameterValue)
			Select Case direction
				Case ParameterDirection.Input
					mRetVal.Direction = ParameterDirection.Input
				Case ParameterDirection.InputOutput
					mRetVal.Direction = ParameterDirection.InputOutput
				Case ParameterDirection.Output
					mRetVal.Direction = ParameterDirection.Output
				Case ParameterDirection.ReturnValue
					mRetVal.Direction = ParameterDirection.ReturnValue
				Case Else
					mRetVal.Direction = ParameterDirection.Input
			End Select
			Return mRetVal
		End Function

		''' <summary>
		''' Returns the correct integer for added or updated by
		''' </summary>
		''' <param name="profile">Object implementing IProfile</param>
		''' <returns>int</returns>
		Protected Function GetAddedUpdatedBy(ByRef profile As IMProfile) As Integer
			Dim mAdded_Updated_By As Integer = profile.Id
			If profile.Id = -1 Then
				mAdded_Updated_By = profile.AddedBy
			End If
			Return mAdded_Updated_By
		End Function

#End Region

#Region "IDisposable Members"
		''' <summary>
		''' Implements IDispose
		''' </summary>
		''' <param name="disposing">Boolean</param>
		''' <remarks></remarks>
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.m_DisposedValue Then
				If disposing Then
					' TODO: dispose managed state (managed objects).
				End If

				' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
				' TODO: set large fields to null.
			End If
			Me.m_DisposedValue = True
		End Sub

		' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
		'Protected Overrides Sub Finalize()
		'	' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
		'	Dispose(False)
		'	MyBase.Finalize()
		'End Sub

		' This code added by Visual Basic to correctly implement the disposable pattern.

		''' <summary>
		''' Implements Dispose
		''' </summary>
		''' <remarks></remarks>
		Public Sub Dispose() Implements IDisposable.Dispose
			' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

	End Class

End Namespace
