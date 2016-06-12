Imports System.Data.SqlClient
Imports System.Xml

Namespace SharedSQLServer
    ' The SqlHelper class is intended to encapsulate high performance, scalable best practices for 
    ' common uses of SqlClient.
    Public NotInheritable Class SqlHelper

#Region "private utility methods & constructors"

        'Since this class provides only static methods, make the default constructor private to prevent 
        'instances from being created with "new SqlHelper()".
        Private Sub New()
        End Sub 'New

        ' This method is used to attach array of SqlParameters to a SqlCommand.
        ' This method will assign a value of DbNull to any parameter with a direction of
        ' InputOutput and a value of null.  
        ' This behavior will prevent default values from being used, but
        ' this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ' where the user provided no input value.
        ' Parameters:
        ' -command - The command to which the parameters will be added
        ' -commandParameters - an array of SqlParameters tho be added to command
        Private Shared Sub AttachParameters(ByVal command As SqlCommand, ByVal commandParameters() As SqlParameter)
            Dim p As SqlParameter
            For Each p In commandParameters
                'check for derived output value with no value assigned
                If p.Direction = ParameterDirection.InputOutput And p.Value Is Nothing Then
                    p.Value = Nothing
                End If
                command.Parameters.Add(p)
            Next p
        End Sub 'AttachParameters

        'This method assigns dataRow column values to an array of SqlParameters.
        'Parameters:
        '-commandParameters: array of SqlParameters to be assigned values
        '-dataRow: the dataRow used to hold the stored procedure's parameter values
        Private Overloads Shared Sub AssignParameterValues(ByVal commandParameters() As SqlParameter, ByVal dataRow As DataRow)

            If commandParameters Is Nothing Or dataRow Is Nothing Then
                'do nothing if we get no data    
                Exit Sub
            End If

            'set the parameters values
            Dim commandParameter As SqlParameter
            For Each commandParameter In commandParameters

                If dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) <> -1 Then
                    commandParameter.Value = dataRow(commandParameter.ParameterName.Substring(1))
                End If
            Next
        End Sub

        ' This method assigns an array of values to an array of SqlParameters.
        ' Parameters:
        ' -commandParameters - array of SqlParameters to be assigned values
        ' -array of objects holding the values to be assigned
        Private Overloads Shared Sub AssignParameterValues(ByVal commandParameters() As SqlParameter, ByVal parameterValues() As Object)

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
        End Sub 'AssignParameterValues

        ' This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        ' to the provided command.
        ' Parameters:
        ' -command - the SqlCommand to be prepared
        ' -connection - a valid SqlConnection, on which to execute this command
        ' -transaction - a valid SqlTransaction, or 'null'
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' -commandParameters - an array of SqlParameters to be associated with the command or 'null' if no parameters are required
        Private Shared Sub PrepareCommand(ByVal command As SqlCommand, _
         ByVal connection As SqlConnection, _
         ByVal transaction As SqlTransaction, _
         ByVal commandType As CommandType, _
         ByVal commandText As String, _
         ByVal commandParameters() As SqlParameter)

            'if the provided connection is not open, we will open it
            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If

            'associate the connection with the command
            command.Connection = connection

            'set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText

            'if we were provided a transaction, assign it.
            If Not (transaction Is Nothing) Then
                command.Transaction = transaction
            End If

            'set the command type
            command.CommandType = commandType

            'attach the command parameters if they are provided
            If Not (commandParameters Is Nothing) Then
                AttachParameters(command, commandParameters)
            End If

            Return
        End Sub 'PrepareCommand

#End Region

#Region "ExecuteNonQuery"

        ' Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        ' the connection string. 
        ' e.g.:  
        '  Dim result As Integer =  ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders")
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' Returns: an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQuery(ByVal connectionString As String, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As Integer
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteNonQuery(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteNonQuery

        ' Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim result As Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' -commandParameters - an array of SqlParamters used to execute the command
        ' Returns: an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQuery(ByVal connectionString As String, _
         ByVal commandType As CommandType, _
         ByVal commandText As String, _
         ByVal ParamArray commandParameters() As SqlParameter) As Integer
            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As New SqlConnection(connectionString)
            Try
                connection.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteNonQuery(connection, commandType, commandText, commandParameters)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Function 'ExecuteNonQuery

        ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        '  Dim result As Integer = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQuery(ByVal connectionString As String, _
         ByVal spName As String, _
         ByVal ParamArray parameterValues() As Object) As Integer

            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)

                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteNonQuery

        ' Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
        ' e.g.:  
        ' Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        ' Parameters:
        ' -connection - a valid SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, _
         ByVal commandType As CommandType, _
         ByVal commandText As String) As Integer
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteNonQuery(connection, commandType, commandText, CType(Nothing, SqlParameter()))

        End Function 'ExecuteNonQuery

        ' Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        ' using the provided parameters.
        ' e.g.:  
        '  Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an int representing the number of rows affected by the command 
        Public Overloads Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, _
         ByVal commandType As CommandType, _
         ByVal commandText As String, _
         ByVal ParamArray commandParameters() As SqlParameter) As Integer

            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim retval As Integer

                PrepareCommand(cmd, connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters)

                'finally, execute the command.
                retval = cmd.ExecuteNonQuery()

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                Return retval
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try
        End Function 'ExecuteNonQuery

        ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        '  Dim result As integer = ExecuteNonQuery(conn, "PublishOrders", 24, 36)
        ' Parameters:
        ' -connection - a valid SqlConnection
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: an int representing the number of rows affected by the command 
        Public Overloads Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, _
         ByVal spName As String, _
         ByVal ParamArray parameterValues() As Object) As Integer
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName)
            End If

        End Function 'ExecuteNonQuery

        ' Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction.
        ' e.g.:  
        '  Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        ' Parameters:
        ' -transaction - a valid SqlTransaction associated with the connection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: an int representing the number of rows affected by the command 
        Public Overloads Shared Function ExecuteNonQuery(ByVal transaction As SqlTransaction, _
         ByVal commandType As CommandType, _
         ByVal commandText As String) As Integer
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteNonQuery(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteNonQuery

        ' Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
        ' using the provided parameters.
        ' e.g.:  
        ' Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an int representing the number of rows affected by the command 
        Public Overloads Shared Function ExecuteNonQuery(ByVal transaction As SqlTransaction, _
         ByVal commandType As CommandType, _
         ByVal commandText As String, _
         ByVal ParamArray commandParameters() As SqlParameter) As Integer
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim retval As Integer

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

                'finally, execute the command.
                retval = cmd.ExecuteNonQuery()

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                Return retval
            Finally
                ' can not close the connection ... needs to be left open
                ' for subsequent transactions
                ' call code must close the connection!!!!!
                'cmd.Connection.Close()
                cmd.Dispose()
            End Try


        End Function 'ExecuteNonQuery

        ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlTransaction 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim result As Integer = SqlHelper.ExecuteNonQuery(trans, "PublishOrders", 24, 36)
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: an int representing the number of rows affected by the command 
        Public Overloads Shared Function ExecuteNonQuery(ByVal transaction As SqlTransaction, _
         ByVal spName As String, _
         ByVal ParamArray parameterValues() As Object) As Integer
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteNonQuery

#End Region

#Region "ExecuteDataset"

        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        ' the connection string. 
        ' e.g.:  
        ' Dim ds As DataSet = SqlHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connectionString As String, _
           ByVal commandType As CommandType, _
           ByVal commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteDataset

        ' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' -commandParameters - an array of SqlParamters used to execute the command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connectionString As String, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As SqlParameter) As DataSet
            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As New SqlConnection(connectionString)
            Try
                connection.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteDataset(connection, commandType, commandText, commandParameters)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Function 'ExecuteDataset

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim ds As Dataset= ExecuteDataset(connString, "GetOrders", 24, 36)
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connectionString As String, _
           ByVal spName As String, _
           ByVal ParamArray parameterValues() As Object) As DataSet

            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteDataset

        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connection - a valid SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connection As SqlConnection, _
           ByVal commandType As CommandType, _
           ByVal commandText As String) As DataSet

            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(connection, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteDataset

        ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connection - a valid SqlConnection
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' -commandParameters - an array of SqlParamters used to execute the command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connection As SqlConnection, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As SqlParameter) As DataSet

            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim ds As New DataSet
                Dim da As SqlDataAdapter

                PrepareCommand(cmd, connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                da = New SqlDataAdapter(cmd)

                'fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds)

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                'return the dataset
                Return ds
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try



        End Function 'ExecuteDataset

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(conn, "GetOrders", 24, 36)
        ' Parameters:
        ' -connection - a valid SqlConnection
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal connection As SqlConnection, _
           ByVal spName As String, _
           ByVal ParamArray parameterValues() As Object) As DataSet
            'Return ExecuteDataset(connection, spName, parameterValues)
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteDataset(connection, CommandType.StoredProcedure, spName)
            End If

        End Function 'ExecuteDataset


        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        ' Parameters
        ' -transaction - a valid SqlTransaction
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal transaction As SqlTransaction, _
           ByVal commandType As CommandType, _
           ByVal commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteDataset

        ' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ' using the provided parameters.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters
        ' -transaction - a valid SqlTransaction 
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command
        ' -commandParameters - an array of SqlParamters used to execute the command
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal transaction As SqlTransaction, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As SqlParameter) As DataSet
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim ds As New DataSet
                Dim da As SqlDataAdapter

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                da = New SqlDataAdapter(cmd)

                'fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds)

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                'return the dataset
                Return ds
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function 'ExecuteDataset

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        ' SqlTransaction using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim ds As Dataset = ExecuteDataset(trans, "GetOrders", 24, 36)
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataset(ByVal transaction As SqlTransaction, _
           ByVal spName As String, _
           ByVal ParamArray parameterValues() As Object) As DataSet
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteDataset(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteDataset

#End Region

#Region "ExecuteReader"
        ' this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        ' we can set the appropriate CommandBehavior when calling ExecuteReader()
        Private Enum SqlConnectionOwnership
            'Connection is owned and managed by SqlHelper
            Internal
            'Connection is owned and managed by the caller
            [External]
        End Enum 'SqlConnectionOwnership

        ' Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        ' If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        ' If the caller provided the connection, we want to leave it to them to manage.
        ' Parameters:
        ' -connection - a valid SqlConnection, on which to execute this command 
        ' -transaction - a valid SqlTransaction, or 'null' 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParameters to be associated with the command or 'null' if no parameters are required 
        ' -connectionOwnership - indicates whether the connection parameter was provided by the caller, or created by SqlHelper 
        ' Returns: SqlDataReader containing the results of the command 
        Private Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
           ByVal transaction As SqlTransaction, _
           ByVal commandType As CommandType, _
           ByVal commandText As String, _
           ByVal commandParameters() As SqlParameter, _
           ByVal connectionOwnership As SqlConnectionOwnership) As SqlDataReader
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand
            'create a reader
            Dim dr As SqlDataReader = Nothing
            Try

                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters)

                ' call ExecuteReader with the appropriate CommandBehavior
                If connectionOwnership = SqlConnectionOwnership.External Then
                    dr = cmd.ExecuteReader()
                Else
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                End If

                'detach the SqlParameters from the command object, so they can be used again
                Dim canClear As Boolean = True
                Dim commandParameter As SqlParameter
                For Each commandParameter In cmd.Parameters
                    If commandParameter.Direction <> ParameterDirection.Input Then
                        canClear = False
                    End If
                Next

                If (canClear) Then cmd.Parameters.Clear()

            Catch ex As Exception
            Finally
                If cmd.Connection.State <> ConnectionState.Closed Then
                    cmd.Dispose()
                End If
            End Try

            Return dr
        End Function 'ExecuteReader

        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        ' the connection string. 
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal connectionString As String, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As SqlDataReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteReader(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteReader

        ' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal connectionString As String, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
            'create & open a SqlConnection
            Dim connection As New SqlConnection(connectionString)
            connection.Open()

            Try
                'call the private overload that takes an internally owned connection in place of the connection string
                Return ExecuteReader(connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, SqlConnectionOwnership.Internal)
            Catch
                'if we fail to return the SqlDatReader, we need to close the connection ourselves
                connection.Close()
                connection.Dispose()
                Throw
            End Try
        End Function 'ExecuteReader

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(connString, "GetOrders", 24, 36)
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal connectionString As String, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As SqlDataReader
            ' use this to get the para from cache
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteReader(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteReader

        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As SqlDataReader

            Return ExecuteReader(connection, commandType, commandText, CType(Nothing, SqlParameter()))

        End Function 'ExecuteReader

        ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
            'pass through the call to private overload using a null transaction value
            Return ExecuteReader(connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, SqlConnectionOwnership.External)

        End Function 'ExecuteReader

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(conn, "GetOrders", 24, 36)
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal connection As SqlConnection, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As SqlDataReader
            'pass through the call using a null transaction value
            'Return ExecuteReader(connection, CType(Nothing, SqlTransaction), spName, parameterValues)

            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                AssignParameterValues(commandParameters, parameterValues)

                Return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteReader(connection, CommandType.StoredProcedure, spName)
            End If

        End Function 'ExecuteReader

        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -transaction - a valid SqlTransaction  
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal transaction As SqlTransaction, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As SqlDataReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteReader(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteReader

        ' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ' using the provided parameters.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -commandType - the CommandType (stored procedure, text, etc.)
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: a SqlDataReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteReader(ByVal transaction As SqlTransaction, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
            'pass through to private overload, indicating that the connection is owned by the caller
            Return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External)
        End Function 'ExecuteReader

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim dr As SqlDataReader = ExecuteReader(trans, "GetOrders", 24, 36)
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns: a SqlDataReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteReader(ByVal transaction As SqlTransaction, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As SqlDataReader
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                AssignParameterValues(commandParameters, parameterValues)

                Return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteReader(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteReader

#End Region

#Region "ExecuteScalar"

        ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        ' the connection string. 
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command
        Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As Object
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteScalar(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteScalar

        ' Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim orderCount As Integer = Cint(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As Object
            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As New SqlConnection(connectionString)
            Try
                connection.Open()

                'call the overload that takes a connection in place of the connection string
                Return ExecuteScalar(connection, commandType, commandText, commandParameters)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Function 'ExecuteScalar

        ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(connString, "GetOrderCount", 24, 36))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As Object
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteScalar

        ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"))
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As Object
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteScalar(connection, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteScalar

        ' Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As Object
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim retval As Object

                PrepareCommand(cmd, connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters)

                'execute the command & return the results
                retval = cmd.ExecuteScalar()

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                Return retval
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function 'ExecuteScalar

        ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(conn, "GetOrderCount", 24, 36))
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As Object

            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteScalar(connection, CommandType.StoredProcedure, spName)
            End If

        End Function 'ExecuteScalar

        ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction.
        ' e.g.:  
        ' Dim orderCount As Integer  = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"))
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal transaction As SqlTransaction, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As Object
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteScalar(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteScalar

        ' Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        ' using the provided parameters.
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        ' Parameters:
        ' -transaction - a valid SqlTransaction  
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal transaction As SqlTransaction, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As Object
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim retval As Object

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

                'execute the command & return the results
                retval = cmd.ExecuteScalar()

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                Return retval
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function 'ExecuteScalar

        ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(trans, "GetOrderCount", 24, 36))
        ' Parameters:
        ' -transaction - a valid SqlTransaction 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: an object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal transaction As SqlTransaction, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As Object
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteScalar(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteScalar

#End Region

#Region "ExecuteXmlReader"

        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ' e.g.:  
        ' Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        ' Returns: an XmlReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteXmlReader(ByVal connection As SqlConnection, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As XmlReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteXmlReader(connection, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteXmlReader

        ' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ' using the provided parameters.
        ' e.g.:  
        ' Dim r As XmlReader = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an XmlReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteXmlReader(ByVal connection As SqlConnection, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As XmlReader
            'pass through the call using a null transaction value
            'Return ExecuteXmlReader(connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters)
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim retval As XmlReader

                PrepareCommand(cmd, connection, CType(Nothing, SqlTransaction), commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                retval = cmd.ExecuteXmlReader()

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                Return retval
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function 'ExecuteXmlReader

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim r As XmlReader = ExecuteXmlReader(conn, "GetOrders", 24, 36)
        ' Parameters:
        ' -connection - a valid SqlConnection 
        ' -spName - the name of the stored procedure using "FOR XML AUTO" 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: an XmlReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteXmlReader(ByVal connection As SqlConnection, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As XmlReader
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteXmlReader


        ' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction
        ' e.g.:  
        ' Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders")
        ' Parameters:
        ' -transaction - a valid SqlTransaction
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        ' Returns: an XmlReader containing the resultset generated by the command 
        Public Overloads Shared Function ExecuteXmlReader(ByVal transaction As SqlTransaction, _
          ByVal commandType As CommandType, _
          ByVal commandText As String) As XmlReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteXmlReader(transaction, commandType, commandText, CType(Nothing, SqlParameter()))
        End Function 'ExecuteXmlReader

        ' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ' using the provided parameters.
        ' e.g.:  
        ' Dim r As XmlReader = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        ' Parameters:
        ' -transaction - a valid SqlTransaction
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        ' -commandParameters - an array of SqlParamters used to execute the command 
        ' Returns: an XmlReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteXmlReader(ByVal transaction As SqlTransaction, _
          ByVal commandType As CommandType, _
          ByVal commandText As String, _
          ByVal ParamArray commandParameters() As SqlParameter) As XmlReader
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand

            Try
                Dim retval As XmlReader

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                retval = cmd.ExecuteXmlReader()

                'detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear()

                Return retval
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function 'ExecuteXmlReader

        ' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        ' using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure's return value parameter.
        ' e.g.:  
        ' Dim r As XmlReader = ExecuteXmlReader(trans, "GetOrders", 24, 36)
        ' Parameters:
        ' -transaction - a valid SqlTransaction
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteXmlReader(ByVal transaction As SqlTransaction, _
          ByVal spName As String, _
          ByVal ParamArray parameterValues() As Object) As XmlReader
            Dim commandParameters As SqlParameter()

            'if we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
                'otherwise we can just call the SP without params
            Else
                Return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function 'ExecuteXmlReader

#End Region

#Region "FillDataset"
        'Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        'the connection string. 
        'e.g.:  
        '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        'Parameters:    
        '-connectionString: a valid connection string for a SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '               by a user defined name (probably the actual table name)
        Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal commandType As CommandType, ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String)

            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)

                connection.Open()

                'call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Sub

        'Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        'using the provided parameters.
        'e.g.:  
        '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() = {"orders"}, new SqlParameter("@prodid", 24))
        'Parameters:    
        '-connectionString: a valid connection string for a SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '               by a user defined name (probably the actual table name)
        '-commandParameters: an array of SqlParamters used to execute the command
        Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal commandType As CommandType, ByVal commandText As String, ByVal dataSet As DataSet, _
        ByVal tableNames() As String, ByVal ParamArray commandParameters() As SqlParameter)

            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)

                connection.Open()

                'call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Sub

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        'the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'This method provides no access to output parameters or the stored procedure's return value parameter.
        'e.g.:  
        '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, 24)
        'Parameters:
        '-connectionString: a valid connection string for a SqlConnection
        '-spName: the name of the stored procedure
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '             by a user defined name (probably the actual table name)
        '-parameterValues: an array of objects to be assigned As the input values of the stored procedure
        Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal spName As String, _
        ByVal dataSet As DataSet, ByVal tableNames As String(), ByVal ParamArray parameterValues() As Object)

            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)

                connection.Open()

                'call the overload that takes a connection in place of the connection string
                FillDataset(connection, spName, dataSet, tableNames, parameterValues)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Sub

        'Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        'e.g.:  
        '   FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        'Parameters:
        '-connection: a valid SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        'by a user defined name (probably the actual table name)
        Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames As String())

            FillDataset(connection, commandType, commandText, dataSet, tableNames, Nothing)

        End Sub

        'Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the provided parameters.
        'e.g.:  
        '   FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new SqlParameter("@prodid", 24))
        'Parameters:
        '-connection: a valid SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        'by a user defined name (probably the actual table name)
        '-commandParameters: an array of SqlParamters used to execute the command
        Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames As String(), _
         ByVal ParamArray commandParameters() As SqlParameter)

            FillDataset(connection, Nothing, commandType, commandText, dataSet, tableNames, commandParameters)

        End Sub

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the provided parameter values.  This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'This method provides no access to output parameters or the stored procedure's return value parameter.
        'e.g.:  
        ' FillDataset (conn, "GetOrders", ds, new string() {"orders"}, 24, 36)
        'Parameters:
        '-connection: a valid SqlConnection
        '-spName: the name of the stored procedure
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '             by a user defined name (probably the actual table name)
        '-parameterValues: an array of objects to be assigned as the input values of the stored procedure
        Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataSet As DataSet, _
        ByVal tableNames() As String, ByVal ParamArray parameterValues() As Object)

            'if we receive parameter values, we need to figure out where they go
            If Not parameterValues Is Nothing AndAlso parameterValues.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters)
            Else
                'otherwise we can just call the SP without params
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames)
            End If

        End Sub

        'Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        'e.g.:  
        '   FillDataset (trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"})
        'Parameters:
        '-transaction: a valid SqlTransaction
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '             by a user defined name (probably the actual table name)
        Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String)

            FillDataset(transaction, commandType, commandText, dataSet, tableNames, Nothing)
        End Sub

        'Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        'using the provided parameters.
        'e.g.:  
        '   FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"}, new SqlParameter("@prodid", 24))
        'Parameters:
        '-transaction: a valid SqlTransaction
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        'by a user defined name (probably the actual table name)
        '-commandParameters: an array of SqlParamters used to execute the command
        Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String, _
         ByVal ParamArray commandParameters() As SqlParameter)

            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters)

        End Sub

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        'SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'This method provides no access to output parameters or the stored procedure's return value parameter.
        'e.g.:  
        '   FillDataset(trans, "GetOrders", ds, new String(){"orders"}, 24, 36)
        'Parameters:
        '-transaction: a valid SqlTransaction
        '-spName: the name of the stored procedure
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '             by a user defined name (probably the actual table name)
        '-parameterValues: an array of objects to be assigned as the input values of the stored procedure
        Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal spName As String, _
        ByVal dataSet As DataSet, ByVal tableNames() As String, ByVal ParamArray parameterValues() As Object)

            'if we receive parameter values, we need to figure out where they go
            If Not parameterValues Is Nothing AndAlso parameterValues.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters)
            Else

                'otherwise we can just call the SP without params
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames)
            End If
        End Sub

        'Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and SqlConnection
        'using the provided parameters.
        'e.g.:  
        '   FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new SqlParameter("@prodid", 24))
        'Parameters:
        '-connection: a valid SqlConnection
        '-transaction: a valid SqlTransaction
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        '             by a user defined name (probably the actual table name)
        '-commandParameters: an array of SqlParamters used to execute the command
        Private Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableNames() As String, _
         ByVal ParamArray commandParameters() As SqlParameter)

            'create a command and prepare it for execution
            Dim command As New SqlCommand

            Try
                PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(command)

                'add the table mappings specified by the user
                If Not tableNames Is Nothing AndAlso tableNames.Length > 0 Then

                    Dim tableName As String = "Table"
                    Dim index As Integer

                    For index = 0 To tableNames.Length - 1
                        dataAdapter.TableMappings.Add(tableName, tableNames(index))
                        tableName = tableName & (index + 1).ToString()
                    Next
                End If

                'fill the DataSet using default values for DataTable names, etc.
                dataAdapter.Fill(dataSet)

                'detach the SqlParameters from the command object, so they can be used again.
                command.Parameters.Clear()
            Finally
                command.Connection.Close()
                command.Dispose()
            End Try

        End Sub

        'Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        'the connection string. 
        'e.g.:  
        '   FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, "Orders", 0, 10)
        'Parameters:    
        '-connectionString: a valid connection string for a SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal commandType As CommandType, ByVal commandText As String, ByVal dataSet As DataSet, _
        ByVal tableName As String, ByVal from As Integer, ByVal count As Integer)

            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)

                connection.Open()

                'call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableName, from, count)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Sub

        'Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        'using the provided parameters.
        'e.g.:  
        '   FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10, new SqlParameter("@prodid", 24))
        'Parameters:    
        '-connectionString: a valid connection string for a SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-commandParameters: an array of SqlParamters used to execute the command
        Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal commandType As CommandType, ByVal commandText As String, ByVal dataSet As DataSet, _
        ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, ByVal ParamArray commandParameters() As SqlParameter)

            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)

                connection.Open()

                'call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableName, from, count, commandParameters)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Sub

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        'the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'This method provides no access to output parameters or the stored procedure's return value parameter.
        'e.g.:  
        '   FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10, 24)
        'Parameters:
        '-connectionString: a valid connection string for a SqlConnection
        '-spName: the name of the stored procedure
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-parameterValues: an array of objects to be assigned as the input values of the stored procedure
        Public Overloads Shared Sub FillDataset(ByVal connectionString As String, ByVal spName As String, _
        ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, _
         ByVal ParamArray parameterValues() As Object)

            'create & open a SqlConnection, and dispose of it after we are done.
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)

                connection.Open()

                'call the overload that takes a connection in place of the connection string
                FillDataset(connection, spName, dataSet, tableName, from, count, parameterValues)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Sub

        'Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        'e.g.:  
        '   FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10)
        'Parameters:
        '-connection: a valid SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer)

            FillDataset(connection, commandType, commandText, dataSet, tableName, from, count, Nothing)

        End Sub

        'Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the provided parameters.
        'e.g.:  
        '   FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10, new SqlParameter("@prodid", 24))
        'Parameters:
        '-connection: a valid SqlConnection
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-commandParameters: an array of SqlParamters used to execute the command
        Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, _
         ByVal ParamArray commandParameters() As SqlParameter)

            FillDataset(connection, Nothing, commandType, commandText, dataSet, tableName, from, count, commandParameters)

        End Sub

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the provided parameter values.  This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'This method provides no access to output parameters or the stored procedure's return value parameter.
        'e.g.:  
        '   FillDataset(conn, "GetOrders", ds, "orders", 0, 10, 24, 36)
        'Parameters:
        '-connection: a valid SqlConnection
        '-spName: the name of the stored procedure
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-parameterValues: an array of objects to be assigned as the input values of the stored procedure
        Public Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataSet As DataSet, _
        ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, ByVal ParamArray parameterValues() As Object)

            'if we receive parameter values, we need to figure out where they go
            If Not parameterValues Is Nothing AndAlso parameterValues.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableName, from, count, commandParameters)
            Else
                'otherwise we can just call the SP without params
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableName, from, count)
            End If

        End Sub

        'Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        'e.g.:  
        '   FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10)
        'Parameters:
        '-transaction: a valid SqlTransaction
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer)

            FillDataset(transaction, commandType, commandText, dataSet, tableName, from, count, Nothing)
        End Sub

        'Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        'using the provided parameters.
        'e.g.:  
        '   FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10, new SqlParameter("@prodid", 24))
        'Parameters:
        '-transaction: a valid SqlTransaction
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-commandParameters: an array of SqlParamters used to execute the command
        Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, _
         ByVal ParamArray commandParameters() As SqlParameter)

            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableName, from, count, commandParameters)

        End Sub

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        'SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'This method provides no access to output parameters or the stored procedure's return value parameter.
        'e.g.:  
        '   FillDataset(trans, "GetOrders", ds, "orders", 0, 10, 24, 36)
        'Parameters:
        '-transaction: a valid SqlTransaction
        '-spName: the name of the stored procedure
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-parameterValues: an array of objects to be assigned as the input values of the stored procedure
        Public Overloads Shared Sub FillDataset(ByVal transaction As SqlTransaction, ByVal spName As String, _
        ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, _
         ByVal ParamArray parameterValues() As Object)

            'if we receive parameter values, we need to figure out where they go
            If Not parameterValues Is Nothing AndAlso parameterValues.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableName, from, count, commandParameters)
            Else

                'otherwise we can just call the SP without params
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableName, from, count)
            End If
        End Sub

        'Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and SqlConnection
        'using the provided parameters.
        'e.g.:  
        '   FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, "orders", 0, 10, new SqlParameter("@prodid", 24))
        'Parameters:
        '-connection: a valid SqlConnection
        '-transaction: a valid SqlTransaction
        '-commandType: the CommandType (stored procedure, text, etc.)
        '-commandText: the stored procedure name or T-SQL command
        '-dataSet: a dataset wich will contain the resultset generated by the command
        '-tableName: Used to create a table mappings allowing the DataTables to be referenced
        '            by a user defined name (probably the actual table name)
        '-from: The zero-based record number to start with
        '-count: The maxium number of records to retrieve
        '-commandParameters: an array of SqlParamters used to execute the command
        Private Overloads Shared Sub FillDataset(ByVal connection As SqlConnection, ByVal transaction As SqlTransaction, ByVal commandType As CommandType, _
        ByVal commandText As String, ByVal dataSet As DataSet, ByVal tableName As String, ByVal from As Integer, ByVal count As Integer, _
         ByVal ParamArray commandParameters() As SqlParameter)

            'create a command and prepare it for execution
            Dim command As New SqlCommand

            Try
                PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters)

                'create the DataAdapter & DataSet
                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(command)

                'fill the DataSet using default values for DataTable names, etc.
                dataAdapter.Fill(dataSet, from, count, tableName)

                'detach the SqlParameters from the command object, so they can be used again.
                command.Parameters.Clear()
            Finally
                command.Connection.Close()
                command.Dispose()
            End Try

        End Sub

#End Region

#Region "UpdateDataset"
        'Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        'e.g.:  
        '   UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order")
        'Parameters:
        '-insertCommand: a valid transact-SQL statement or stored procedure to insert new records into the data source
        '-deleteCommand: a valid transact-SQL statement or stored procedure to delete records from the data source
        '-updateCommand: a valid transact-SQL statement or stored procedure used to update records in the data source
        '-dataSet: the DataSet used to update the data source
        '-tableName: the DataTable used to update the data source
        Public Overloads Shared Sub UpdateDataset(ByVal insertCommand As SqlCommand, ByVal deleteCommand As SqlCommand, ByVal updateCommand As SqlCommand, ByVal dataSet As DataSet, ByVal tableName As String)

            'create a SqlDataAdapter, and dispose of it after we are done.
            Dim dataAdapter As New SqlDataAdapter
            Try
                'Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand
                dataAdapter.InsertCommand = insertCommand
                dataAdapter.DeleteCommand = deleteCommand

                'Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName)

                'Commit all the changes made to the DataSet
                dataSet.AcceptChanges()
            Finally
                dataAdapter.Dispose()
            End Try
        End Sub
#End Region

#Region "CreateCommand"
        'Simplify the creation of a Sql command object by allowing
        'a stored procedure and optional parameters to be provided
        'e.g.:  
        'Dim command As SqlCommand = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName")
        'Parameters:
        '-connection: a valid SqlConnection object
        '-spName: the name of the stored procedure
        '-sourceColumns: an array of string to be assigned as the source columns of the stored procedure parameters
        'Returns:
        ' a valid SqlCommand object
        Public Overloads Shared Function CreateCommand(ByVal connection As SqlConnection, ByVal spName As String, ByVal ParamArray sourceColumns() As String) As SqlCommand

            'create a SqlCommand
            Dim cmd As New SqlCommand(spName, connection)
            cmd.CommandType = CommandType.StoredProcedure

            'if we receive parameter values, we need to figure out where they go
            If Not sourceColumns Is Nothing AndAlso sourceColumns.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'assign the provided source columns to these parameters based on parameter order
                Dim index As Integer
                For index = 0 To sourceColumns.Length - 1
                    commandParameters(index).SourceColumn = sourceColumns(index)
                Next

                'attach the discovered parameters to the SqlCommand object
                AttachParameters(cmd, commandParameters)
            End If

            CreateCommand = cmd
        End Function
#End Region

#Region "ExecuteNonQueryTypedParams"
        'Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        'the connection string using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        'Parameters:
        '-connectionString: a valid connection string for a SqlConnection
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values
        'Returns:
        'an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQueryTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As Integer

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteNonQueryTypedParams = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else

                ExecuteNonQueryTypedParams = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        'using the dataRow column values as the stored procedure's parameters values.  
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        'Parameters:
        '-connection:a valid SqlConnection object
        '-spName: the name of the stored procedure
        '-dataRow:The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQueryTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As Integer

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteNonQueryTypedParams = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else

                ExecuteNonQueryTypedParams = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified
        'SqlTransaction using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        'Parameters:
        '-transaction:a valid SqlTransaction object
        '-spName:the name of the stored procedure
        '-dataRow:The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'an int representing the number of rows affected by the command
        Public Overloads Shared Function ExecuteNonQueryTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As Integer

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteNonQueryTypedParams = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteNonQueryTypedParams = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function
#End Region

#Region "ExecuteDataSetTypedParams"
        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        'the connection string using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        'Parameters:
        '-connectionString: a valid connection string for a SqlConnection
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataSetTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As DataSet

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteDataSetTypedParams = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteDataSetTypedParams = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the dataRow column values as the store procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        'Parameters:
        '-connection: a valid SqlConnection object
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataSetTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As DataSet

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteDataSetTypedParams = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteDataSetTypedParams = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        'Parameters:
        '-transaction: a valid SqlTransaction object
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'a dataset containing the resultset generated by the command
        Public Overloads Shared Function ExecuteDataSetTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As DataSet

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteDataSetTypedParams = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteDataSetTypedParams = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function
#End Region

#Region "ExecuteReaderTypedParams"
        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        'the connection string using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-connectionString: a valid connection string for a SqlConnection
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        ' a SqlDataReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteReaderTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As SqlDataReader

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteReaderTypedParams = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteReaderTypedParams = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-connectiona valid SqlConnection object
        '-spNamethe name of the stored procedure
        '-dataRowThe dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'a SqlDataReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteReaderTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As SqlDataReader

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteReaderTypedParams = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteReaderTypedParams = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-transactiona valid SqlTransaction object
        '-spNamethe name of the stored procedure
        '-dataRowThe dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'a SqlDataReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteReaderTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As SqlDataReader

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteReaderTypedParams = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteReaderTypedParams = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function
#End Region

#Region "ExecuteScalarTypedParams"
        'Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        'the connection string using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-connectionString: a valid connection string for a SqlConnection
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns:
        'an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Overloads Shared Function ExecuteScalarTypedParams(ByVal connectionString As String, ByVal spName As String, ByVal dataRow As DataRow) As Object

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteScalarTypedParams = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteScalarTypedParams = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-connection: a valid SqlConnection object
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns: 
        ' an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Overloads Shared Function ExecuteScalarTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As Object

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteScalarTypedParams = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteScalarTypedParams = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-transaction: a valid SqlTransaction object
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns: 
        ' an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Overloads Shared Function ExecuteScalarTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As Object

            'If the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteScalarTypedParams = SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteScalarTypedParams = SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function
#End Region

#Region "ExecuteXmlReaderTypedParams"
        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-connection: a valid SqlConnection object
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns: 
        ' an XmlReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteXmlReaderTypedParams(ByVal connection As SqlConnection, ByVal spName As String, ByVal dataRow As DataRow) As XmlReader

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteXmlReaderTypedParams = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteXmlReaderTypedParams = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        'Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        'using the dataRow column values as the stored procedure's parameters values.
        'This method will query the database to discover the parameters for the 
        'stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        'Parameters:
        '-transaction: a valid SqlTransaction object
        '-spName: the name of the stored procedure
        '-dataRow: The dataRow used to hold the stored procedure's parameter values.
        'Returns: 
        ' an XmlReader containing the resultset generated by the command
        Public Overloads Shared Function ExecuteXmlReaderTypedParams(ByVal transaction As SqlTransaction, ByVal spName As String, ByVal dataRow As DataRow) As XmlReader

            'if the row has values, the store procedure parameters must be initialized
            If dataRow.ItemArray.Length > 0 Then

                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters() As SqlParameter = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName)

                'set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                ExecuteXmlReaderTypedParams = SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                ExecuteXmlReaderTypedParams = SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function
#End Region
    End Class 'SqlHelper

    ' SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
    ' ability to discover parameters for stored procedures at run-time.
    Public NotInheritable Class SqlHelperParameterCache

#Region "private methods, variables, and constructors"


        'Since this class provides only static methods, make the default constructor private to prevent 
        'instances from being created with "new SqlHelperParameterCache()".
        Private Sub New()
        End Sub 'New 

        Private Shared paramCache As Hashtable = Hashtable.Synchronized(New Hashtable)

        ' resolve at run time the appropriate set of SqlParameters for a stored procedure
        ' Parameters:
        ' - connectionString - a valid connection string for a SqlConnection
        ' - spName - the name of the stored procedure
        ' - includeReturnValueParameter - whether or not to include their return value parameter>
        ' Returns: SqlParameter()
        Private Shared Function DiscoverSpParameterSet(ByVal connection As SqlConnection, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean, _
           ByVal ParamArray parameterValues() As Object) As SqlParameter()

            Dim cmd As New SqlCommand(spName, connection)

            Try
                Dim discoveredParameters() As SqlParameter

                cmd.CommandType = CommandType.StoredProcedure
                SqlCommandBuilder.DeriveParameters(cmd)
                If Not includeReturnValueParameter Then
                    cmd.Parameters.RemoveAt(0)
                End If

                discoveredParameters = New SqlParameter(cmd.Parameters.Count - 1) {}
                cmd.Parameters.CopyTo(discoveredParameters, 0)

                'Init the parameters with a DBNull value
                Dim discoveredParameter As SqlParameter
                For Each discoveredParameter In discoveredParameters
                    discoveredParameter.Value = DBNull.Value
                Next

                Return discoveredParameters
            Finally
                cmd.Connection.Close()
                cmd.Dispose()
            End Try

        End Function 'DiscoverSpParameterSet

        'deep copy of cached SqlParameter array
        Private Shared Function CloneParameters(ByVal originalParameters() As SqlParameter) As SqlParameter()

            Dim i As Integer
            Dim j As Integer = originalParameters.Length - 1
            Dim clonedParameters(j) As SqlParameter

            For i = 0 To j
                clonedParameters(i) = CType(CType(originalParameters(i), ICloneable).Clone, SqlParameter)
            Next

            Return clonedParameters
        End Function 'CloneParameters

#End Region

#Region "caching functions"

        ' add parameter array to the cache
        ' Parameters
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandText - the stored procedure name or T-SQL command 
        ' -commandParameters - an array of SqlParamters to be cached 
        Public Shared Sub CacheParameterSet(ByVal connectionString As String, _
           ByVal commandText As String, _
           ByVal ParamArray commandParameters() As SqlParameter)
            Dim hashKey As String = connectionString + ":" + commandText

            paramCache(hashKey) = commandParameters
        End Sub 'CacheParameterSet

        ' retrieve a parameter array from the cache
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: an array of SqlParamters 
        Public Shared Function GetCachedParameterSet(ByVal connectionString As String, ByVal commandText As String) As SqlParameter()
            Dim hashKey As String = connectionString + ":" + commandText
            Dim cachedParameters As SqlParameter() = CType(paramCache(hashKey), SqlParameter())

            If cachedParameters Is Nothing Then
                Return Nothing
            Else
                Return CloneParameters(cachedParameters)
            End If
        End Function 'GetCachedParameterSet

#End Region

#Region "Parameter Discovery Functions"
        ' Retrieves the set of SqlParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -spName - the name of the stored procedure 
        ' Returns: an array of SqlParameters
        Public Overloads Shared Function GetSpParameterSet(ByVal connectionString As String, ByVal spName As String) As SqlParameter()
            Return GetSpParameterSet(connectionString, spName, False)
        End Function 'GetSpParameterSet 

        ' Retrieves the set of SqlParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of SqlParameters 
        Public Overloads Shared Function GetSpParameterSet(ByVal connectionString As String, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean) As SqlParameter()
            Dim connection As SqlConnection = Nothing
            Try
                connection = New SqlConnection(connectionString)
                connection.Open()

                GetSpParameterSet = GetSpParameterSetInternal(connection, spName, includeReturnValueParameter)
            Finally
                connection.Close()
                connection.Dispose()
            End Try
        End Function 'GetSpParameterSet

        ' Retrieves the set of SqlParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connection - a valid SqlConnection object
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of SqlParameters 
        Public Overloads Shared Function GetSpParameterSet(ByVal connection As SqlConnection, _
           ByVal spName As String) As SqlParameter()

            GetSpParameterSet = GetSpParameterSet(connection, spName, False)
        End Function 'GetSpParameterSet

        ' Retrieves the set of SqlParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connection - a valid SqlConnection object
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of SqlParameters 
        Public Overloads Shared Function GetSpParameterSet(ByVal connection As SqlConnection, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean) As SqlParameter()

            Dim clonedConnection As SqlConnection = Nothing
            Try
                clonedConnection = CType((CType(connection, ICloneable).Clone), SqlConnection)
                clonedConnection.Open()

                GetSpParameterSet = GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter)
            Finally
                clonedConnection.Close()
                clonedConnection.Dispose()
            End Try
        End Function 'GetSpParameterSet

        ' Retrieves the set of SqlParameters appropriate for the stored procedure
        ' 
        ' This method will query the database for this information, and then store it in a cache for future requests.
        ' Parameters:
        ' -connection - a valid SqlConnection object
        ' -spName - the name of the stored procedure 
        ' -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        ' Returns: an array of SqlParameters 
        Private Overloads Shared Function GetSpParameterSetInternal(ByVal connection As SqlConnection, _
           ByVal spName As String, _
           ByVal includeReturnValueParameter As Boolean) As SqlParameter()

            Dim cachedParameters() As SqlParameter
            Dim hashKey As String

            hashKey = connection.ConnectionString + ":" + spName + IIf(includeReturnValueParameter = True, ":include ReturnValue Parameter", "").ToString

            cachedParameters = CType(paramCache(hashKey), SqlParameter())

            If (cachedParameters Is Nothing) Then
                Dim spParameters() As SqlParameter = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter)
                paramCache(hashKey) = spParameters
                cachedParameters = spParameters

            End If

            Return CloneParameters(cachedParameters)

        End Function 'GetSpParameterSet
#End Region

    End Class 'SqlHelperParameterCache 
End Namespace
