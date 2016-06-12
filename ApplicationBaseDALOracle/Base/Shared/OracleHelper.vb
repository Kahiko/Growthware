Imports System
Imports System.Configuration
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Collections

'/ <summary>
'/ A helper class used to execute queries against an Oracle database
'/ </summary>
Namespace SharedOracle
    Public MustInherit Class OracleHelper

        'Create a hashtable for the parameter cached
        Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable)

        'This method assigns dataRow column values to an array of OracleParameters.
        'Parameters:
        '-commandParameters: array of OracleParameters to be assigned values
        '-dataRow: the dataRow used to hold the stored procedure's parameter values
        Private Overloads Shared Sub AssignParameterValues(ByVal commandParameters() As OracleParameter, ByVal dataRow As DataRow)

            If commandParameters Is Nothing Or dataRow Is Nothing Then
                'do nothing if we get no data    
                Exit Sub
            End If

            'set the parameters values
            Dim commandParameter As OracleParameter
            For Each commandParameter In commandParameters

                If dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) <> -1 Then
                    commandParameter.Value = dataRow(commandParameter.ParameterName.Substring(1))
                End If
            Next
        End Sub

        ' This method assigns an array of values to an array of OracleParameters.
        ' Parameters:
        ' -commandParameters - array of OracleParameters to be assigned values
        ' -array of objects holding the values to be assigned
        Private Overloads Shared Sub AssignParameterValues(ByVal commandParameters() As OracleParameter, ByVal parameterValues() As Object)

            Dim i As Integer
            Dim j As Integer

            If (commandParameters Is Nothing) And (parameterValues Is Nothing) Then
                'do nothing if we get no data
                Return
            End If

            ' we must have the same number of values as we pave parameters to put them in
            If commandParameters.Length <> parameterValues.Length Then
                Throw New ArgumentException("Parameter count does not match Parameter Value count.")
            End If

            'value array
            j = commandParameters.Length - 1
            For i = 0 To j
                'if the current array value derives from IDbDataParameter, then assign its Value property
                If TypeOf parameterValues(i) Is IDbDataParameter Then
                    commandParameters(i).Value = CType(parameterValues(i), IDbDataParameter).Value
                Else
                    commandParameters(i).Value = parameterValues(i)
                End If
            Next
        End Sub       'AssignParameterValues

        '/ <summary>
        '/ Execute a database query which does not include a select
        '/ </summary>
        '/ <param name="connString">Connection string to database</param>
        '/ <param name="cmdType">Command type either stored procedure or Oracle</param>
        '/ <param name="cmdText">Acutall Oracle Command</param>
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
        End Function          'ExecuteNonQuery


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
        '/ <param name="commandText">the stored procedure name or PL/Oracle command</param>
        '/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        '/ <returns>an int representing the number of rows affected by the command</returns>
        Public Overloads Shared Function ExecuteNonQuery(ByVal trans As OracleTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Integer
            Dim cmd As New OracleCommand
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Function          'ExecuteNonQuery


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
        '/ <param name="commandText">the stored procedure name or PL/Oracle command</param>
        '/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        '/ <returns>an int representing the number of rows affected by the command</returns>
        Public Overloads Shared Function ExecuteNonQuery(ByVal conn As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Integer

            Dim cmd As New OracleCommand

            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Function          'ExecuteNonQuery


        '/ <summary>
        '/ Execute a select query that will return a result set
        '/ </summary>
        '/ <param name="connString">Connection string</param>
        '// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        '/ <param name="commandText">the stored procedure name or PL/Oracle command</param>
        '/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        '/ <returns></returns>
        Public Shared Function ExecuteReader(ByVal connString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As OracleDataReader

            'Create the command and connection
            Dim cmd As New OracleCommand
            Dim conn As New OracleConnection(connString)
            Dim myRefCursor As New OracleParameter

            With myRefCursor
                .Direction = ParameterDirection.Output
                .ParameterName = "cur1"
                .OracleDbType = OracleDbType.RefCursor
            End With

            Try
                'Prepare the command to execute
                PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)
                cmd.Parameters.Add(myRefCursor)

                'Execute the query, stating that the connection should close when the resulting datareader has been read
                Dim rdr As OracleDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                cmd.Parameters.Clear()
                Return rdr

            Catch e As Exception

                'If an error occurs close the connection as the reader will not be used and we expect it to close the connection
                conn.Close()
                Throw e
            End Try
        End Function          'ExecuteReader


        '/ <summary>
        '/ Execute an OracleCommand that returns the first column of the first record against the database specified in the connection string 
        '/ using the provided parameters.
        '/ </summary>
        '/ <remarks>
        '/ e.g.:  
        '/  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        '/ </remarks>
        '/ <param name="connectionString">a valid connection string for a OracleConnection</param>
        '/ <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        '/ <param name="commandText">the stored procedure name or PL/Oracle command</param>
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
        End Function          'ExecuteScalar


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
        '/ <param name="commandText">the stored procedure name or PL/Oracle command</param>
        '/ <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        '/ <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        Public Overloads Shared Function ExecuteScalar(ByVal conn As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray cmdParms() As OracleParameter) As Object

            Dim cmd As New OracleCommand

            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, cmdParms)
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        End Function          'ExecuteScalar


        '/ <summary>
        '/ Add a set of parameters to the cached
        '/ </summary>
        '/ <param name="cacheKey">Key value to look up the parameters</param>
        '/ <param name="cmdParms">Actual parameters to cached</param>
        Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray cmdParms() As OracleParameter)
            parmCache(cacheKey) = cmdParms
        End Sub       'CacheParameters


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
            Dim j = cachedParms.Length
            For i = 0 To j
                clonedParms(i) = CType(cachedParms(1), OracleParameter)
            Next

            ' return a copy of the parameters

            Return clonedParms
        End Function          'GetCachedParameters


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
            'If Not (trans Is Nothing) Then
            '    cmd.Transaction = trans
            'End If
            ' Bind the parameters passed in
            If Not (cmdParms Is Nothing) Then
                Dim parm As OracleParameter
                For Each parm In cmdParms
                    cmd.Parameters.Add(parm)
                Next parm
            End If
        End Sub       'PrepareCommand

#Region "ExecuteDataset"

        ' Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        ' the connection string. 
        ' e.g.:  
        ' Dim ds As DataSet = OracleHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connectionString - a valid connection string for a OracleConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-Oracle command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connectionString As String, _
           ByVal commandType As CommandType, _
           ByVal commandText As String) As DataSet
            'pass through the call providing null for the set of OracleParameters
            Return ExecuteDataset(connectionString, commandType, commandText, CType(Nothing, OracleParameter()))
        End Function          'ExecuteDataset

        ' Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        ' Parameters:
        ' -connectionString - a valid connection string for a OracleConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-Oracle command
        ' -commandParameters - an array of OracleParamters used to execute the command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connectionString As String, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As OracleParameter) As DataSet
            'create & open a OracleConnection, and dispose of it after we are done.
            Dim connection As New OracleConnection(connectionString)
            Try
                connection.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteDataset(connection, commandType, commandText, commandParameters)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Function          'ExecuteDataset

        ' Execute a stored procedure via a OracleCommand (that returns a resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim ds As Dataset= ExecuteDataset(connString, "GetOrders", 24, 36)
        ' Parameters:
        ' -connectionString - a valid connection string for a OracleConnection
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connectionString As String, _
           ByVal spName As String, _
           ByVal ParamArray parameterValues() As Object) As DataSet

            Dim commandParameters As OracleParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = OracleHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of OracleParameters
                Return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function          'ExecuteDataset

        ' Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connection - a valid OracleConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-Oracle command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connection As OracleConnection, _
           ByVal commandType As CommandType, _
           ByVal commandText As String) As DataSet
            'pass through the call providing null for the set of OracleParameters
            Return ExecuteDataset(connection, commandType, commandText, CType(Nothing, OracleParameter()))
        End Function          'ExecuteDataset

        ' Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        ' Parameters:
        ' -connection - a valid OracleConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-Oracle command
        ' -commandParameters - an array of OracleParamters used to execute the command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connection As OracleConnection, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As OracleParameter) As DataSet

            'create a command and prepare it for execution
            Dim cmd As New OracleCommand
            Dim myRefCursor As New OracleParameter

            With myRefCursor
                .Direction = ParameterDirection.Output
                .ParameterName = "cur1"
                .OracleDbType = OracleDbType.RefCursor
            End With

            Try
                Dim ds As New DataSet
                Dim da As OracleDataAdapter

                PrepareCommand(cmd, connection, CType(Nothing, OracleTransaction), commandType, commandText, commandParameters)
                cmd.Parameters.Add(myRefCursor)
                'create the DataAdapter & DataSet
                da = New OracleDataAdapter(cmd)

                'fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds)

                'detach the OracleParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                'return the dataset
                Return ds
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try



        End Function          'ExecuteDataset

        ' Execute a stored procedure via a OracleCommand (that returns a resultset) against the specified OracleConnection 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(conn, "GetOrders", 24, 36)
        ' Parameters:
        ' -connection - a valid OracleConnection
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connection As OracleConnection, _
           ByVal spName As String, _
           ByVal ParamArray parameterValues() As Object) As DataSet
            'Return ExecuteDataset(connection, spName, parameterValues)
            Dim commandParameters As OracleParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = OracleHelperParameterCache.GetSpParameterSet(connection, spName)
                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of OracleParameters
                Return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteDataset(connection, CommandType.StoredProcedure, spName)
            End If

        End Function          'ExecuteDataset


        ' Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction. 
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        ' Parameters
        ' -transaction - a valid OracleTransaction
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-Oracle command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal transaction As OracleTransaction, _
           ByVal commandType As CommandType, _
           ByVal commandText As String) As DataSet
            'pass through the call providing null for the set of OracleParameters
            Return ExecuteDataset(transaction, commandType, commandText, CType(Nothing, OracleParameter()))
        End Function          'ExecuteDataset

        ' Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        ' using the provided parameters.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        ' Parameters
        ' -transaction - a valid OracleTransaction 
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-Oracle command
        ' -commandParameters - an array of OracleParamters used to execute the command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal transaction As OracleTransaction, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As OracleParameter) As DataSet
            'create a command and prepare it for execution
            Dim cmd As New OracleCommand

            Try
                Dim ds As New DataSet
                Dim da As OracleDataAdapter

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                da = New OracleDataAdapter(cmd)

                'fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds)

                'detach the OracleParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                'return the dataset
                Return ds
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function          'ExecuteDataset

        ' Execute a stored procedure via a OracleCommand (that returns a resultset) against the specified
        ' OracleTransaction using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(trans, "GetOrders", 24, 36)
        ' Parameters:
        ' -transaction - a valid OracleTransaction 
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal transaction As OracleTransaction, _
           ByVal spName As String, _
           ByVal ParamArray parameterValues() As Object) As DataSet
            Dim commandParameters As OracleParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = OracleHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of OracleParameters
                Return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteDataset(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function          'ExecuteDataset

#End Region

    End Class   'OraHelper

    Public NotInheritable Class OracleHelperParameterCache

#Region "private methods, variables, and constructors"


        'Since this class provides only static methods, make the default constructor private to prevent 
        'instances from being created with "new OracleHelperParameterCache()".
        Private Sub New()
        End Sub       'New 

        Private Shared paramCache As Hashtable = Hashtable.Synchronized(New Hashtable)

        ' resolve at run time the appropriate set of OracleParameters for a stored procedure
        ' Parameters:
        ' - connectionString - a valid connection string for a OracleConnection
        ' - spName - the name of the stored procedure
        ' - includeReturnValueParameter - whether or not to include their return value parameter>
        ' Returns: OracleParameter()
        Private Shared Function DiscoverSpParameterSet(ByVal connection As OracleConnection, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean, _
           ByVal ParamArray parameterValues() As Object) As OracleParameter()

            Dim cmd As New OracleCommand(spName, connection)

            Try
                Dim discoveredParameters() As OracleParameter

                cmd.CommandType = CommandType.StoredProcedure
                OracleCommandBuilder.DeriveParameters(cmd)
                If Not includeReturnValueParameter Then
                    cmd.Parameters.RemoveAt(0)
                End If

                discoveredParameters = New OracleParameter(cmd.Parameters.Count - 1) {}
                cmd.Parameters.CopyTo(discoveredParameters, 0)

                'Init the parameters with a DBNull value
                Dim discoveredParameter As OracleParameter
                For Each discoveredParameter In discoveredParameters
                    discoveredParameter.Value = DBNull.Value
                Next

                Return discoveredParameters
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function          'DiscoverSpParameterSet

        'deep copy of cached OracleParameter array
        Private Shared Function CloneParameters(ByVal originalParameters() As OracleParameter) As OracleParameter()

            Dim i As Integer
            Dim j As Integer = originalParameters.Length - 1
            Dim clonedParameters(j) As OracleParameter

            For i = 0 To j
                clonedParameters(i) = CType(CType(originalParameters(i), ICloneable).Clone, OracleParameter)
            Next

            Return clonedParameters
        End Function          'CloneParameters

#End Region

#Region "caching functions"

        ' add parameter array to the cache
        ' Parameters
        ' -connectionString - a valid connection string for a OracleConnection 
        ' -commandText - the stored procedure name or T-Oracle command 
        ' -commandParameters - an array of OracleParamters to be cached 
        Public Shared Sub CacheParameterSet(ByVal connectionString As String, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As OracleParameter)
            Dim hashKey As String = connectionString + ":" + commandText

            paramCache(hashKey) = commandParameters
        End Sub       'CacheParameterSet

        ' retrieve a parameter array from the cache
        ' Parameters:
        ' -connectionString - a valid connection string for a OracleConnection 
        ' -commandText - the stored procedure name or T-Oracle command 
        ' Returns: an array of OracleParamters 
        Public Shared Function GetCachedParameterSet(ByVal connectionString As String, ByVal commandText As String) As OracleParameter()
            Dim hashKey As String = connectionString + ":" + commandText
            Dim cachedParameters As OracleParameter() = CType(paramCache(hashKey), OracleParameter())

            If cachedParameters Is Nothing Then
                Return Nothing
            Else
                Return CloneParameters(cachedParameters)
            End If
        End Function          'GetCachedParameterSet

#End Region

#Region "Parameter Discovery Functions"
        ' Retrieves the set of OracleParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connectionString - a valid connection string for a OracleConnection 
        ' -spName - the name of the stored procedure 
        ' Returns: an array of OracleParameters
        Public Overloads Shared Function GetSpParameterSet(ByVal connectionString As String, ByVal spName As String) As OracleParameter()
            Return GetSpParameterSet(connectionString, spName, False)
        End Function          'GetSpParameterSet 

        ' Retrieves the set of OracleParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connectionString - a valid connection string for a OracleConnection
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of OracleParameters 
        Public Overloads Shared Function GetSpParameterSet(ByVal connectionString As String, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean) As OracleParameter()
            Dim connection As New OracleConnection
            Try
                connection = New OracleConnection(connectionString)
                connection.Open()

                GetSpParameterSet = GetSpParameterSetInternal(connection, spName, includeReturnValueParameter)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Function          'GetSpParameterSet

        ' Retrieves the set of OracleParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connection - a valid OracleConnection object
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of OracleParameters 
        Public Overloads Shared Function GetSpParameterSet(ByVal connection As OracleConnection, _
           ByVal spName As String) As OracleParameter()

            GetSpParameterSet = GetSpParameterSet(connection, spName, False)
        End Function          'GetSpParameterSet

        ' Retrieves the set of OracleParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connection - a valid OracleConnection object
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of OracleParameters 
        Public Overloads Shared Function GetSpParameterSet(ByVal connection As OracleConnection, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean) As OracleParameter()

            Dim clonedConnection As New OracleConnection
            Try
                clonedConnection = CType((CType(connection, ICloneable).Clone), OracleConnection)
                clonedConnection.Open()

                GetSpParameterSet = GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter)
            Finally
                clonedConnection.Close()
                clonedConnection.Dispose()
            End Try
        End Function          'GetSpParameterSet

        ' Retrieves the set of OracleParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connection - a valid OracleConnection object
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of OracleParameters 
        Private Overloads Shared Function GetSpParameterSetInternal(ByVal connection As OracleConnection, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean) As OracleParameter()

            Dim cachedParameters() As OracleParameter
            Dim hashKey As String

            hashKey = connection.ConnectionString + ":" + spName + IIf(includeReturnValueParameter = True, ":include ReturnValue Parameter", "").ToString

            cachedParameters = CType(paramCache(hashKey), OracleParameter())

            If (cachedParameters Is Nothing) Then
                Dim spParameters() As OracleParameter = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter)
                paramCache(hashKey) = spParameters
                cachedParameters = spParameters

            End If

            Return CloneParameters(cachedParameters)

        End Function          'GetSpParameterSet
#End Region

    End Class   'OracleHelperParameterCache 

End Namespace