Namespace DataAccessLayer.Interfaces.Base
    ''' <summary>
    ''' The base interface for Database interaction code
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="DDB")>
    Public Interface IDDBInteraction
        ''' <summary>
        ''' Sets or Gets the connection string information.
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>Can not be blank</remarks>
        Property ConnectionString As String
    End Interface
End Namespace
