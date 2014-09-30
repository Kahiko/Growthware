Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    Public MustInherit Class BaseBusinessLogic
        Protected Shared Function IsDatabaseOnline() As Boolean
            If ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) = "ONLINE" Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
