Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    ''' <summary>
    ''' Process business logic for messages
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
    ''' Dim myBll as new DMessages(mySecurityEntityProfile, ConfigSettings.CentralManagement)
    ''' ]]>
    ''' </code>
    ''' </example>
    Public Class BMessages
        Inherits BaseBusinessLogic

        Private m_DMessages As IDMessages
        Private m_SecurityEntityProfile As MSecurityEntityProfile

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
        ''' BFunctions mBFunctions = BFunctions = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        ''' Dim mBFunctions As BFunctions = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentException("securityEntityProfile can not be null or empty!")
            End If
            If Not centralManagement Then
                If m_DMessages Is Nothing Then
                    m_DMessages = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages")
                End If
            Else
                m_DMessages = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DMessages")
            End If
            m_SecurityEntityProfile = securityEntityProfile
            m_DMessages.ConnectionString = securityEntityProfile.ConnectionString
            m_DMessages.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        ''' <summary>
        ''' Gets the messages.
        ''' </summary>
        ''' <param name="securityEntitySeqId">The security entity seq ID.</param>
        ''' <returns>Collection{MMessageProfile}.</returns>
        Public Function GetMessages(ByVal securityEntitySeqId As Integer) As Collection(Of MMessageProfile)
            Dim mRetVal As Collection(Of MMessageProfile) = New Collection(Of MMessageProfile)
            Dim mDataTable As DataTable = Nothing
            If IsDatabaseOnline() Then
                Try
                    m_DMessages.SecurityEntitySeqId = securityEntitySeqId
                    mDataTable = m_DMessages.Messages()
                    For Each item As DataRow In mDataTable.Rows
                        Dim mProfile As MMessageProfile = New MMessageProfile(item)
                        mRetVal.Add(mProfile)
                    Next
                Catch ex As Exception
                    Throw
                Finally
                    If Not mDataTable Is Nothing Then
                        mDataTable.Dispose()
                    End If
                End Try
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the messages.
        ''' </summary>
        ''' <param name="messageSeqId">The security entity seq ID.</param>
        ''' <returns>Collection{MMessageProfile}.</returns>
        Public Function GetMessage(ByVal messageSeqId As Integer) As MMessageProfile
            Dim mRetVal As MMessageProfile = Nothing
            If IsDatabaseOnline() Then
                mRetVal = New MMessageProfile(m_DMessages.GetMessage(messageSeqId))
            End If
            Return mRetVal
        End Function
        ''' <summary>
        ''' Save Function information to the database
        ''' </summary>
        ''' <param name="profile">MMessageProfile</param>
        Public Sub Save(ByVal profile As MMessageProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            m_DMessages.Profile = profile
            m_DMessages.Save()
        End Sub

        ''' <summary>
        ''' Returns a data table given the search criteria
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            If String.IsNullOrEmpty(searchCriteria.WhereClause) Then
                searchCriteria.WhereClause = " Security_Entity_SeqID = " + m_SecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture)
            Else
                searchCriteria.WhereClause += " AND Security_Entity_SeqID = " + m_SecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture)
            End If
            Return m_DMessages.Search(searchCriteria)
        End Function
    End Class
End Namespace
