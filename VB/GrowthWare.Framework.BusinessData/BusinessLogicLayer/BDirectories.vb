Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Collections.ObjectModel

Namespace BusinessLogicLayer
    ''' <summary>
    ''' Process business logic for Directories
    ''' </summary>
    ''' <remarks>
    ''' <![CDATA[
    ''' MSecurityEntityProfile can be found in the GrowthWare.Framework.Model.Profiles namespace.  
    ''' 
    ''' The following properties are necessary for correct business logic operation.
    ''' .ConnectionString
    ''' .DALName
    ''' .DALNameSpace
    ''' ]]>
    ''' </remarks>
    Public Class BDirectories
        Inherits BaseBusinessLogic

        Private m_DDirectories As IDDirectories

        ''' <summary>
        ''' Private sub New() to ensure only new instances with passed parameters is used.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Parameters are need to pass along to the factory for correct connection to the desired datastore.
        ''' </summary>
        ''' <param name="SecurityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        ''' <param name="CentralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
        ''' <remarks></remarks>
        ''' <example> This sample shows how to create an instance of the class.
        ''' <code language="VB.NET">
        ''' <![CDATA[
        ''' MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
        ''' mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
        ''' mSecurityEntityProfile.DAL = ConfigSettings.DAL;
        ''' mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
        ''' mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
        ''' mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
        ''' 
        ''' BDirectories mBDirectories = BDirectories = New BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        ''' ]]>
        ''' </code>
        ''' <code language="C#">
        ''' <![CDATA[
        ''' Dim mSecurityEntityProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
        ''' mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID
        ''' mSecurityEntityProfile.DAL = ConfigSettings.DAL
        ''' mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL)
        ''' mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL)
        ''' mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString
        ''' 
        ''' Dim mBDirectories As BDirectories = New BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentException("securityEntityProfile can not be null or empty!")
            End If
            If Not centralManagement Then
                If m_DDirectories Is Nothing Then
                    m_DDirectories = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDirectories")
                End If
            Else
                m_DDirectories = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDirectories")
            End If
            m_DDirectories.ConnectionString = securityEntityProfile.ConnectionString
            m_DDirectories.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        Public Function Directories() As Collection(Of MDirectoryProfile)
            Dim mRetVal As Collection(Of MDirectoryProfile) = New Collection(Of MDirectoryProfile)
            If DatabaseIsOnline() Then
                Dim mDataTable As DataTable = m_DDirectories.Directories()
                If Not mDataTable Is Nothing Then
                    For Each mDataRow In mDataTable.Rows
                        Dim mProfile As MDirectoryProfile = New MDirectoryProfile(mDataRow)
                        mRetVal.Add(mProfile)
                    Next
                End If
            End If
            Return mRetVal
        End Function

        Public Sub Save(ByVal profile As MDirectoryProfile)
            If DatabaseIsOnline() Then m_DDirectories.Save(profile)
        End Sub
    End Class
End Namespace