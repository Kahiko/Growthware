Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    ''' <summary>
    ''' Process business logic for functions
    ''' </summary>
    ''' <remarks>
    ''' <![CDATA[
    ''' MSecurityEntityProfile can be found in the GrowthWare.Framework.ModelObjects namespace.  
    ''' 
    ''' The following properties are necessary for correct business logic operation.
    ''' .ConnctionString
    ''' .DALName
    ''' .DALNameSpace
    ''' ]]>
    ''' </remarks>
    ''' <example> This sample shows how to create an instance of the class.
    ''' <code language="VB.NET">
    ''' <![CDATA[
    ''' Dim myBll as new BClientChoices(mySecurityEntityProfile, ConfigSettings.CentralManagement)
    ''' ]]>
    ''' </code>
    ''' </example>
    Public Class BClientChoices
        Inherits BaseBusinessLogic

        Private m_DClientChoices As IDClientChoices

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
        ''' Dim mBClientChoices As BClientChoices = New BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement)
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
        ''' BClientChoices mBClientChoices = new BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentException("securityEntityProfile can not be null or empty!")
            End If
            If Not centralManagement Then
                If m_DClientChoices Is Nothing Then
                    m_DClientChoices = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DClientChoices")
                End If
            Else
                m_DClientChoices = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DClientChoices")
            End If
            m_DClientChoices.ConnectionString = securityEntityProfile.ConnectionString
        End Sub

        ''' <summary>
        ''' Retrieves a data row from the data store and populates a MClientChoicesState object.
        ''' </summary>
        ''' <param name="account">The desired account in which to base the MClientChoicesState model object</param>
        ''' <returns>A populated MClientChoicesState</returns>
        ''' <remarks>None.</remarks>
        Public Function GetClientChoicesState(ByVal account As String) As MClientChoicesState
            Dim mRetVal As MClientChoicesState = Nothing
            Try
                If IsDatabaseOnline() Then
                    mRetVal = New MClientChoicesState(m_DClientChoices.GetChoices(account))
                End If
            Catch ex As Exception
                Throw New BusinessLogicLayerException("Could not retrieve the client choices state", ex)
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Saves the choices a client may have made during usage of the application.
        ''' </summary>
        ''' <param name="clientChoicesState">A populated MClientChoicesState object.</param>
        ''' <remarks>MClientChoicesState can be found in the GrowthWare.Framework.ModelObjects namespace.</remarks>
        Public Sub Save(ByVal clientChoicesState As MClientChoicesState)
            If Not clientChoicesState Is Nothing Then
                If IsDatabaseOnline() Then
                    m_DClientChoices.Save(clientChoicesState.ChoicesHashtable)
                End If
            Else
                Throw New ArgumentNullException("clientChoicesState", "clientChoicesState cannot be a null reference (Nothing in Visual Basic)!")
            End If
        End Sub
    End Class
End Namespace

