Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Model.Profiles
    <Serializable(), CLSCompliant(True)> _
Public Class MSecurityEntityProfile
        Inherits MProfile


#Region "Constructors"
        ''' <summary>
        ''' Will return a account profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.IdColumnName = "SE_SEQ_ID"
            MyBase.NameColumnName = "Name"
            Me.Id = -1
            Me.Name = " "
            Description = " "
            Url = " "
            Skin = "Default"
            Style = "Default"
            ParentSeqId = 1
            StatusSeqId = 1
            DataAccessLayer = "SQLServer"
            DataAccessLayerAssemblyName = "GrowthWare.Framework.BusinessData"
            DataAccessLayerNamespace = "GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008"
            ConnectionString = " "
            Name = " "
            EncryptionType = 1
        End Sub

        ''' <summary>
        ''' Will populate values based on the contents of the data row.
        ''' </summary>
        ''' <param name="dataRow">Datarow containing base values</param>
        ''' <remarks>
        ''' Class should be inherited to extend to your project specific properties
        ''' </remarks>
        Public Sub New(ByVal dataRow As DataRow)
            Me.Initialize(dataRow)
        End Sub
#End Region

#Region "Protected Methods"
        ''' <summary>
        ''' Initializes all of the properties given a data row.
        ''' </summary>
        ''' <param name="dataRow">DataRow</param>
        Protected Shadows Sub Initialize(ByVal dataRow As DataRow)
            MyBase.IdColumnName = "SE_SEQ_ID"
            MyBase.NameColumnName = "Name"
            MyBase.Initialize(dataRow)
            Description = MyBase.GetString(dataRow, "Description")
            Url = MyBase.GetString(dataRow, "URL")
            Skin = MyBase.GetString(dataRow, "Skin")
            Style = MyBase.GetString(dataRow, "Style")
            ParentSeqId = MyBase.GetInt(dataRow, "PARENT_SE_SEQ_ID")
            StatusSeqId = MyBase.GetInt(dataRow, "STATUS_SEQ_ID")
            DataAccessLayer = MyBase.GetString(dataRow, "DAL")
            DataAccessLayerAssemblyName = MyBase.GetString(dataRow, "DAL_NAME")
            DataAccessLayerNamespace = MyBase.GetString(dataRow, "DAL_NAME_SPACE")
            ConnectionString = MyBase.GetString(dataRow, "DAL_STRING")
            Name = MyBase.GetString(dataRow, "Name")
            EncryptionType = MyBase.GetInt(dataRow, "ENCRYPTION_TYPE")
        End Sub
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Represents the Descriptionn
        ''' </summary>
        Public Property Description() As String

        ''' <summary>
        ''' Represents the URL associated with the Security Entity.  
        ''' The intended use was to all a way to retrieve a profile based on the URL
        ''' </summary>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")>
        Public Property Url() As String

        ''' <summary>
        ''' Represents the "Skin" associated with this Security Entity
        ''' </summary>
        Public Property Skin() As String

        ''' <summary>
        ''' Represents the CSS file associated with this Security Entity
        ''' </summary>
        Public Property Style() As String

        ''' <summary>
        ''' Security Entities have a hierarchical relationship to each other and this represents the parent of this Security Entity.
        ''' </summary>
        Public Property ParentSeqId() As Integer

        ''' <summary>
        ''' Represents the connection string for the given security entity.
        ''' </summary>
        Public Property ConnectionString() As String

        ''' <summary>
        ''' Represends the Encrytion used by the security entity.
        ''' </summary>
        Public Property EncryptionType() As Integer

        ''' <summary>
        ''' Represends the status
        ''' </summary>
        Public Property StatusSeqId() As Integer

        ''' <summary>
        ''' Represents the Data Access Layer.  
        ''' SQLServer or Oracle or MySQL are examples of a data access.
        ''' </summary>
        Public Property DataAccessLayer() As String

        ''' <summary>
        ''' Represents the Data Access Layer's Assembly or DLL name.  
        ''' GrowthWareFramework for example.
        ''' </summary>
        Public Property DataAccessLayerAssemblyName() As String

        ''' <summary>
        ''' Represents the Data Access Layer's Namespace.
        ''' GrowthWare.Framework.DataAccessLayer.SQLServer.V2000 or 
        ''' GrowthWare.Framework.DataAccessLayer.SQLServer.V2008 are examples.
        ''' </summary>
        Public Property DataAccessLayerNamespace() As String
#End Region
    End Class
End Namespace
