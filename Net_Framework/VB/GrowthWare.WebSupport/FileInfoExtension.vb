Imports System.Runtime.CompilerServices

Module FileInfoExtension

    <Extension()>
    Public Function ToFileSize(ByVal source As Integer) As String
        Return ToFileSize(Convert.ToInt64(source))
    End Function

    <Extension()>
    Public Function ToFileSize(ByVal source As Long) As String
        Const byteConversion As Integer = 1024
        Dim bytes As Double = Convert.ToDouble(source)
        If bytes >= Math.Pow(byteConversion, 3) Then 'GB Range
            Return String.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB")
        ElseIf bytes >= Math.Pow(byteConversion, 2) Then 'MB Range
            Return String.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB")
        ElseIf bytes >= byteConversion Then 'KB Range
            Return String.Concat(Math.Round(bytes / byteConversion, 2), " KB")
        Else 'Bytes
            Return String.Concat(bytes, " Bytes")
        End If
    End Function
End Module
