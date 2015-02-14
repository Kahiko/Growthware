Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces

Namespace BusinessLogicLayer
    Public Class BDBInformation
        Inherits BaseBusinessLogic

        Private m_DDBInformation As IDBInformation

        ''' <summary>
        ''' Prevents a default instance of the <see cref="BDBInformation"/> class from being created.
        ''' </summary>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Parameters are need to pass along to the factory for correct connection to the desired data store.
        ''' </summary>
        ''' <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        ''' <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
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
        ''' BDBInformation mBAccount = BDBInformation = New BDBInformation(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        ''' Dim mBDBInformation As BDBInformation = New BDBInformation(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentException("securityEntityProfile can not be null or empty!")
            End If
            If Not centralManagement Then
                If m_DDBInformation Is Nothing Then
                    m_DDBInformation = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation")
                End If
            Else
                m_DDBInformation = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation")
            End If
            m_DDBInformation.ConnectionString = securityEntityProfile.ConnectionString
        End Sub

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <returns>MDBInformation.</returns>
        Public Function GetProfile() As MDBInformation
            Return New MDBInformation(m_DDBInformation.GetProfile())
        End Function

        ''' <summary>
        ''' Updates the profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Public Function UpdateProfile(ByVal profile As MDBInformation) As Boolean
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic)")
            Dim mRetVal As Boolean = False
            m_DDBInformation.Profile = profile
            If DatabaseIsOnline() Then
                mRetVal = m_DDBInformation.UpdateProfile()
            End If
            Return mRetVal
        End Function
    End Class

End Namespace
