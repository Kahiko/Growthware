Imports GrowthWare.Framework.Model.Profiles.Interfaces
Namespace Model.Profiles.Base
    ''' <summary>
    ''' Contains all profile base properties
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), CLSCompliant(True)> _
    Public MustInherit Class MProfile
        Inherits MDatabaseFunctions
        Implements IMProfile

        Public Property AddedBy As Integer Implements IMProfile.AddedBy

        Public Property AddedDate As Date Implements IMProfile.AddedDate

        Public Property Id As Integer = -1 Implements IMProfile.Id

        Public Property IdColumnName As String Implements IMProfile.IdColumnName

        Public Property Name As String Implements IMProfile.Name

        Public Property NameColumnName As String Implements IMProfile.NameColumnName

        Public Property UpdatedBy As Integer Implements IMProfile.UpdatedBy

        Public Property UpdatedDate As Date Implements IMProfile.UpdatedDate

        ''' <summary>
        ''' Initializes values given a DataRow
        ''' </summary>
        ''' <param name="Datarow">datarow</param>
        ''' <remarks>Only sets Id and Name if IdColumnName or NameColumnName is not null</remarks>
        Friend Overridable Sub Initialize(ByVal dataRow As DataRow)
            Me.AddedBy = Me.GetInt(dataRow, "Added_By")
            Me.AddedDate = Me.GetDateTime(dataRow, "Added_Date", DateTime.Now)
            Me.UpdatedBy = Me.GetInt(dataRow, "Updated_By")
            Me.UpdatedDate = Me.GetDateTime(dataRow, "Updated_Date", DateTime.Now)
            If Not String.IsNullOrEmpty(Me.IdColumnName) Then
                Me.Id = Me.GetInt(dataRow, Me.IdColumnName)
            End If
            If Not String.IsNullOrEmpty(Me.NameColumnName) Then
                Me.Name = Me.GetString(dataRow, Me.NameColumnName)
            End If
        End Sub
    End Class
End Namespace
