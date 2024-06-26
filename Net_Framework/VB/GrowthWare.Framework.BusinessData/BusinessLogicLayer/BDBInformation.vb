﻿Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces

Namespace BusinessLogicLayer
    ''' <summary>
    ''' BDBInformation is the business implementation for the DB information.
    ''' </summary>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="BDB")> Public Class BDBInformation
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
                Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!")
            End If
            If Not centralManagement Then
                If Me.m_DDBInformation Is Nothing Then
                    Me.m_DDBInformation = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation")
                End If
            Else
                Me.m_DDBInformation = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation")
            End If
            Me.m_DDBInformation.ConnectionString = securityEntityProfile.ConnectionString
        End Sub

        ''' <summary>
        ''' Gets the profile.
        ''' </summary>
        ''' <returns>MDBInformation.</returns>
        Public ReadOnly Property GetProfile As MDBInformation
            Get
                Return New MDBInformation(Me.m_DDBInformation.GetProfileRow)
            End Get
        End Property

        ''' <summary>
        ''' Updates the profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Public Function UpdateProfile(ByVal profile As MDBInformation) As Boolean
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic)")
            Dim mRetVal As Boolean = False
            Me.m_DDBInformation.Profile = profile
            If DatabaseIsOnline() Then
                mRetVal = Me.m_DDBInformation.UpdateProfile()
            End If
            Return mRetVal
        End Function
    End Class

End Namespace
