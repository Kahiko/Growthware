Namespace Model.Enumerations
    ''' <summary>
    ''' Enumeration of system status
    ''' </summary>
    ''' <remarks>
    ''' Values match ZGWSystem.Statuses in the database
    ''' </remarks>
    Public Enum SystemStatus
        ''' <summary>
        ''' The active
        ''' </summary>
        Active = 1
        ''' <summary>
        ''' The change password
        ''' </summary>
        ChangePassword = 4
        ''' <summary>
        ''' The disabled
        ''' </summary>
        Disabled = 3
        ''' <summary>
        ''' The inactive
        ''' </summary>
        Inactive = 2
        ''' <summary>
        ''' Not used
        ''' </summary>
        None = 0
        ''' <summary>
        ''' Need to set the account details normaly used when auto creating account
        ''' </summary>
        SetAccountDetails = 5
    End Enum
End Namespace
