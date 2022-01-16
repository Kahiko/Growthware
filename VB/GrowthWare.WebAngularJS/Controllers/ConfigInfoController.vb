Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports System.Globalization
Imports System.Web.Http

Namespace Controllers
    Public Class ConfigInfoController
        Inherits ApiController

        ' POST api/ConfigInfo/Decrypt?textValue=
        <HttpGet>
        Public Function Decrypt(<FromUri()> textValue) As String
            Dim mRetVal As String = "Not authroized"
            Dim mSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_Encryption_Helper", True)), AccountUtility.CurrentProfile())
            If mSecurityInfo.MayView Then
                mRetVal = CryptoUtility.Decrypt(textValue.Trim, SecurityEntityUtility.CurrentProfile().EncryptionType, ConfigSettings.EncryptionSaltExpression)
            End If
            Return mRetVal
        End Function

        ' POST api/ConfigInfo/Decrypt?textValue=
        <HttpGet>
        Public Function Encrypt(<FromUri()> ByVal textValue) As String
            Dim mRetVal As String = "Not authroized"
            Dim mSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_Encryption_Helper", True)), AccountUtility.CurrentProfile())
            If mSecurityInfo.MayView Then
                mRetVal = CryptoUtility.Encrypt(textValue.Trim, SecurityEntityUtility.CurrentProfile().EncryptionType, ConfigSettings.EncryptionSaltExpression)
            End If
            Return mRetVal
        End Function

        ' GET api/ConfigInfo/GetConfigInfo
        <HttpGet>
        Public Function GetConfigInfo() As MUIConfigInfo
            Dim mRetVal As MUIConfigInfo = New MUIConfigInfo()
            With mRetVal
                .ApplicationName = ConfigSettings.AppDisplayedName
                .Environment = GWWebHelper.DisplayEnvironment
                .FrameworkVersion = GWWebHelper.FrameworkVersion
                .LogLevel = GWWebHelper.GetLogLevel(ConfigSettings.LogPriority)
                .SecurityEntityTranslation = ConfigSettings.SecurityEntityTranslation
                .Version = GWWebHelper.Version
            End With
            Return mRetVal
        End Function

        ' GET api/ConfigInfo/GetDBInformation
        <HttpGet>
        Public Function GetDBInformation() As MDBInformation
            Dim mProfile As MDBInformation = DBInformationUtility.DBInformation()
            Return mProfile
        End Function

        ' GET api/ConfigInfo/GetGUID
        <HttpGet>
        Public Function GetGUID() As String
            Dim mRetVal As String = String.Empty
            mRetVal = GWWebHelper.GetNewGuid()
            Return mRetVal
        End Function

        ' GET api/ConfigInfo/GetLogLevel
        <HttpGet>
        Public Function GetLogLevel() As Integer
            Dim mLog As Logger = Logger.Instance()
            Return mLog.CurrentLogLevel
        End Function

        <HttpGet>
        Public Function GetRandomNumbers(<FromUri()> ByVal amountOfNumbers As Integer, <FromUri()> ByVal maxNumber As Integer, <FromUri()> ByVal minNumber As Integer) As String
            Dim mRetVal As String = String.Empty
            Dim X As Integer = 1
            For X = 1 To amountOfNumbers
                mRetVal += GWWebHelper.GetRandomNumber(maxNumber, minNumber) + ", "
            Next
            Return mRetVal
        End Function

        ' POST api/ConfigInfo/SaveDBInformation?enableInheritance=1
        <HttpPost>
        Public Function SaveDBInformation(<FromUri()> ByVal enableInheritance As Integer) As Boolean
            Dim mProfile As MDBInformation = DBInformationUtility.DBInformation()
            mProfile.EnableInheritance = enableInheritance
            Dim mRetVal As Boolean = DBInformationUtility.UpdateProfile(mProfile)
            If mRetVal Then FunctionUtility.RemoveCachedFunctions()
            Return mRetVal
        End Function

        ' GET api/ConfigInfo/SetLogLevel
        <HttpPost>
        Public Function SetLogLevel(<FromUri()> ByVal logLevel As Integer) As String
            Dim mRetVal As String = "The log level has been temporarily set to "
            Dim mLog As Logger = Logger.Instance()
            Select Case logLevel
                Case 0
                    mLog.SetThreshold(LogPriority.Debug)
                    mRetVal += "'Debug'"
                Case 1
                    mLog.SetThreshold(LogPriority.Info)
                    mRetVal += "'Information'"
                Case 2
                    mLog.SetThreshold(LogPriority.Warn)
                    mRetVal += "'Warning'"
                Case 3
                    mLog.SetThreshold(LogPriority.Error)
                    mRetVal += "'Error'"
                Case 4
                    mLog.SetThreshold(LogPriority.Fatal)
                    mRetVal += "'Fatal'"
                Case Else
                    mLog.SetThreshold(LogPriority.Error)
                    mRetVal += "'Error by default'"
            End Select
            Return mRetVal
        End Function
    End Class
End Namespace