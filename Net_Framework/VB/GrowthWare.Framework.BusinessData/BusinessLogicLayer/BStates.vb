Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    Public Class BStates
        Inherits BaseBusinessLogic

        Private m_DStates As IDState

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
        ''' Dim mBAccount As BStates = New BStates(mSecurityEntityProfile, ConfigSettings.CentralManagement)
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
        ''' BStates mBAccount = new BStates(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!")
            End If
            If Not centralManagement Then
                If m_DStates Is Nothing Then
                    m_DStates = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DStates")
                End If
            Else
                m_DStates = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DStates")
            End If
            m_DStates.ConnectionString = securityEntityProfile.ConnectionString
            m_DStates.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        Public Sub Delete(ByVal accountId As Integer)
            Throw New NotImplementedException()
        End Sub

        ''' <summary>
        ''' Returns Account model based given the acccount name 
        ''' </summary>
        ''' <param name="state">String</param>
        ''' <returns>MAccountProfile</returns>
        ''' <remarks></remarks>
        ''' <example> This sample shows how to create an instance of the class.
        ''' <code language="VB.NET">
        ''' <![CDATA[
        ''' Dim mBll as new BStates(mySecurityEntityProfile)
        ''' Dim mMAccountProfile as MAccountProfile = mbill.GetAccountProfile("Tester")
        ''' ]]>
        ''' </code>
        ''' <code language="C#">
        ''' <![CDATA[
        ''' BStates mBll = new BStates(mySecurityEntityProfile);
        ''' MAccountProfile mMAccountProfile = mbill.GetAccountProfile("Tester");
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Function GetProfile(ByVal state As String) As MStateProfile
            Dim mRetVal As MStateProfile = Nothing
            m_DStates.Profile = New MStateProfile()
            m_DStates.Profile.State = state
            If DatabaseIsOnline() Then
                mRetVal = New MStateProfile(m_DStates.GetState)
            End If
            Return mRetVal
        End Function

        Function Save(ByVal profile As MStateProfile) As Boolean
            Dim mRetVal As Boolean = False
            m_DStates.Profile = profile
            m_DStates.Save()
            ' if it didn't throw an error the we assume it worked
            mRetVal = True
            Return mRetVal
        End Function

        ''' <summary>
        ''' Performs the search
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then
                mRetVal = m_DStates.Search(searchCriteria)
            End If
            Return mRetVal
        End Function
    End Class
End Namespace