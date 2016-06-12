Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.OracleClient
Imports System.Collections

Namespace OracleDAL

	'/ <summary>
	'/ A helper class used to execute queries against an Oracle database
	'/ </summary>

	Public MustInherit Class OraHelper

		'Create a hashtable for the parameter cached
		Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable)


		'/ <summary>
		'/ Execute a database query which does not include a select
		'/ </summary>
		'/ <param name="connString">Connection string to database</param>
		'/ <param name="cmdType">Command type either stored procedure or SQL</param>
		'/ <param name="cmdText">Acutall SQL Command</param>
		'/ <param name="cmdParms">Parameters to bind to the command</param>
		'/ <returns></returns>
		Public Overloads Shared Function ExecuteNonQuery(ByVal connString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Integer

			' Create a new Oracle command
			Dim cmd As New OracleCommand

			'Create a connection
			Dim conn As New OracleConnection(connString)
			Try

				'Prepare the command
				PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)

				'Execute the command
				Dim val As Integer = cmd.ExecuteNonQuery()
				cmd.Parameters.Clear()
				Return val
			Finally
				conn.Dispose()
			End Try
		End Function		  'ExecuteNonQuery


		'/ <summary>
		'/ Execute an OracleCommand (that returns no resultset) against an existing database transaction 
		'/ using the provided parameters.
		'/ </summary>
		'/ <remarks>
		'/ e.g.:  
		'/  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
		'/ </remarks>
		'/ <param name="trans">an existing database transaction</param>
		'/ <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		'/ <param name="commandText">the stored procedure name or PL/SQL command</param>
		'/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
		'/ <returns>an int representing the number of rows affected by the command</returns>
		Public Overloads Shared Function ExecuteNonQuery(ByVal trans As OracleTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Integer
			Dim cmd As New OracleCommand
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms)
			Dim val As Integer = cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()
			Return val
		End Function		  'ExecuteNonQuery


		'/ <summary>
		'/ Execute an OracleCommand (that returns no resultset) against an existing database connection 
		'/ using the provided parameters.
		'/ </summary>
		'/ <remarks>
		'/ e.g.:  
		'/  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
		'/ </remarks>
		'/ <param name="conn">an existing database connection</param>
		'/ <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		'/ <param name="commandText">the stored procedure name or PL/SQL command</param>
		'/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
		'/ <returns>an int representing the number of rows affected by the command</returns>
		Public Overloads Shared Function ExecuteNonQuery(ByVal conn As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Integer

			Dim cmd As New OracleCommand

			PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)
			Dim val As Integer = cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()
			Return val
		End Function		  'ExecuteNonQuery


		'/ <summary>
		'/ Execute a select query that will return a result set
		'/ </summary>
		'/ <param name="connString">Connection string</param>
		'// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		'/ <param name="commandText">the stored procedure name or PL/SQL command</param>
		'/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
		'/ <returns></returns>
		Public Shared Function ExecuteReader(ByVal connString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As OracleDataReader

			'Create the command and connection
			Dim cmd As New OracleCommand
			Dim conn As New OracleConnection(connString)

			Try
				'Prepare the command to execute
				PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)

				'Execute the query, stating that the connection should close when the resulting datareader has been read
				Dim rdr As OracleDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
				cmd.Parameters.Clear()
				Return rdr

			Catch e As Exception

				'If an error occurs close the connection as the reader will not be used and we expect it to close the connection
				conn.Close()
				Throw e
			End Try
		End Function		  'ExecuteReader


		'/ <summary>
		'/ Execute an OracleCommand that returns the first column of the first record against the database specified in the connection string 
		'/ using the provided parameters.
		'/ </summary>
		'/ <remarks>
		'/ e.g.:  
		'/  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
		'/ </remarks>
		'/ <param name="connectionString">a valid connection string for a SqlConnection</param>
		'/ <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		'/ <param name="commandText">the stored procedure name or PL/SQL command</param>
		'/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
		'/ <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
		Public Overloads Shared Function ExecuteScalar(ByVal connString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Object
			Dim cmd As New OracleCommand

			Dim conn As New OracleConnection(connString)
			Try
				PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)
				Dim val As Object = cmd.ExecuteScalar()
				cmd.Parameters.Clear()
				Return val
			Finally
				conn.Dispose()
			End Try
		End Function		  'ExecuteScalar


		'/ <summary>
		'/ Execute an OracleCommand that returns the first column of the first record against an existing database connection 
		'/ using the provided parameters.
		'/ </summary>
		'/ <remarks>
		'/ e.g.:  
		'/  Object obj = ExecuteScalar(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
		'/ </remarks>
		'/ <param name="conn">an existing database connection</param>
		'/ <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		'/ <param name="commandText">the stored procedure name or PL/SQL command</param>
		'/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
		'/ <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
		Public Overloads Shared Function ExecuteScalar(ByVal conn As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Object

			Dim cmd As New OracleCommand

			PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)
			Dim val As Object = cmd.ExecuteScalar()
			cmd.Parameters.Clear()
			Return val
		End Function		  'ExecuteScalar


		'/ <summary>
		'/ Add a set of parameters to the cached
		'/ </summary>
		'/ <param name="cacheKey">Key value to look up the parameters</param>
		'/ <param name="cmdParms">Actual parameters to cached</param>
		Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray cmdParms() As OracleParameter)
			parmCache(cacheKey) = cmdParms
		End Sub		  'CacheParameters


		'/ <summary>
		'/ Fetch parameters from the cache
		'/ </summary>
		'/ <param name="cacheKey">Key to look up the parameters</param>
		'/ <returns></returns>
		Public Shared Function GetCachedParameters(ByVal cacheKey As String) As OracleParameter()
			Dim cachedParms As OracleParameter() = CType(parmCache(cacheKey), OracleParameter())

			If cachedParms Is Nothing Then
				Return Nothing
			End If
			' If the parameters are in the cache
			Dim clonedParms(cachedParms.Length) As OracleParameter
			Dim i As Integer = 0
			Dim j As Integer = cachedParms.Length
			For i = 0 To j
				clonedParms(i) = CType(cachedParms(1), OracleClient.OracleParameter)
			Next

			' return a copy of the parameters

			Return clonedParms
		End Function		  'GetCachedParameters


		'/ <summary>
		'/ Internal function to prepare a command for execution by the database
		'/ </summary>
		'/ <param name="cmd">Existing command object</param>
		'/ <param name="conn">Database connection object</param>
		'/ <param name="trans">Optional transaction object</param>
		'/ <param name="cmdType">Command type, e.g. stored procedure</param>
		'/ <param name="cmdText">Command test</param>
		'/ <param name="cmdParms">Parameters for the command</param>
		Private Shared Sub PrepareCommand(ByVal cmd As OracleCommand, ByVal conn As OracleConnection, ByVal trans As OracleTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms() As OracleParameter)

			'Open the connection if required
			If conn.State <> ConnectionState.Open Then
				conn.Open()
			End If
			'Set up the command
			cmd.Connection = conn
			cmd.CommandText = cmdText
			cmd.CommandType = cmdType

			'Bind it to the transaction if it exists
			If Not (trans Is Nothing) Then
				cmd.Transaction = trans
			End If
			' Bind the parameters passed in
			If Not (cmdParms Is Nothing) Then
				Dim parm As OracleParameter
				For Each parm In cmdParms
					cmd.Parameters.Add(parm)
				Next parm
			End If
		End Sub		  'PrepareCommand
	End Class	'OraHelper
End Namespace 'UMASS