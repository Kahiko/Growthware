﻿Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces
Imports System.Globalization

Namespace Model.Profiles
    ''' <summary>
    ''' Represents the properties necessary to interact with a servers directory(ies)
    ''' </summary>
    <Serializable(), CLSCompliant(True)> _
    Public NotInheritable Class MDirectoryProfile
        Inherits MProfile
        Implements IMProfile


#Region "Constructors"
        ''' <summary>
        ''' Will return a directory profile with the default values
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Will return a directory profile with the values from the data row
        ''' </summary>
        ''' <param name="dataRow">DataRow</param>
        Public Sub New(ByVal dataRow As DataRow)
            MyBase.Initialize(dataRow)
            mFunction_Seq_ID = MyBase.GetInt(dataRow, "FUNCTION_SEQ_ID")
            mDirectory = MyBase.GetString(dataRow, "Directory").ToUpper(CultureInfo.InvariantCulture)
            mImpersonate = MyBase.GetBool(dataRow, "Impersonate")
            mImpersonate_Account = MyBase.GetString(dataRow, "Impersonate_Account")
            mImpersonate_PWD = MyBase.GetString(dataRow, "Impersonate_PWD")
            MyBase.Id = mFunction_Seq_ID
            MyBase.Name = mDirectory
        End Sub
#End Region

#Region "Field Objects"
        Private mFunction_Seq_ID As Integer
        Private mDirectory As String = String.Empty
        Private mImpersonate As Boolean = False
        Private mImpersonate_Account As String = String.Empty
        Private mImpersonate_PWD As String = String.Empty
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Is the primary key
        ''' </summary>
        Public Property FunctionSeqId() As Integer
            Get
                Return mFunction_Seq_ID
            End Get
            Set(ByVal Value As Integer)
                mFunction_Seq_ID = Value
            End Set
        End Property

        ''' <summary>
        ''' Is the full local directory i.e. C:\temp
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>Can also be a network location \\mycomputer\c$\temp</remarks>
        Public Property Directory() As String
            Get
                Return mDirectory
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then mDirectory = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Indicates if impersonation is necessary
        ''' </summary>
        ''' <value>Boolean</value>
        ''' <returns>Boolean</returns>
        ''' <remarks>Works in conjunction with Impersonate_Account and Impersonate_PWD</remarks>
        Public Property Impersonate() As Boolean
            Get
                Return mImpersonate
            End Get
            Set(ByVal Value As Boolean)
                mImpersonate = Value
            End Set
        End Property

        ''' <summary>
        ''' Is the account used to impersonate when working with the directory
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>Must be a valid network account with access to the information supplied in the directory property</remarks>
        Public Property ImpersonateAccount() As String
            Get
                Return mImpersonate_Account
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then mImpersonate_Account = Value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Is the password associated with the Impersonate_Account property
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        Public Property ImpersonatePassword() As String
            Get
                Return mImpersonate_PWD
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then mImpersonate_PWD = Value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace
