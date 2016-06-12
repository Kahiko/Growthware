Imports GrowthWare.Framework.Web
Imports GrowthWare.Framework.Web.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports System.Xml
Imports System.IO

Public Class DisplayError
	Inherits System.Web.UI.UserControl

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			'NotifyCheckBox.Visible = AccountUtility.GetCurrentAccountProfile.IS_SYSTEM_ADMIN
			'WriteExceptionLog(WebHelper.ExceptionError)
		End If
	End Sub

	Private Sub WriteExceptionLog(ByVal poExec As Exception)
		Dim LastError As Exception
		Dim doc As New XmlDocument
		Dim root As XmlNode
		Dim lsStr As String
		Dim ErrorNode As XmlElement
		Dim ErrorChild As XmlNode
		Dim psFileNm As String
		Dim firstXMLLine As String = "<?xml version='1.0'?><?xml-stylesheet type='text/xsl' href='" & WebConfigSettings.RootSite & "Public/XSLT/UnexpecedError.xslt'?><ROOT></ROOT>"
		If WebConfigSettings.LogPath.Length > 0 Then
			psFileNm = WebConfigSettings.LogPath & Format(Now, "yyyy_MM_dd") & ".xml"
		Else
			psFileNm = HttpContext.Current.Server.MapPath("~\Logs\") & Format(Now, "yyyy_MM_dd") & ".xml"
		End If

		'Check if the Log file has crossed its threshold limit of 3MB
		Dim psInfo As FileInfo = New FileInfo(psFileNm)
		Dim filesizeinMB As Long

		If Not psInfo.Exists Then
			Dim fs As FileStream = File.Create(psFileNm)
			Dim sw As StreamWriter = New StreamWriter(fs)
			sw.Write(firstXMLLine)
			sw.Close()
			fs.Close()
		Else
			filesizeinMB = CType((psInfo.Length / 1024 / 1024), Long)
		End If
		'CType(ConfigurationSettings.AppSettings.Get("MaxLogSizeMB"), Long) Then
		If filesizeinMB > 10 Then
			Dim Bkpdate As String = Date.Now.Month & "_" & Date.Now.Day & "_" & Date.Now.Year & "_" & Date.Now.Hour & "_" & Date.Now.Minute

			psInfo.MoveTo(WebConfigSettings.LogPath & "Error_" & Bkpdate & ".xml")

			Dim fs As FileStream = File.Create(psFileNm)
			Dim sw As StreamWriter = New StreamWriter(fs)
			sw.Write(firstXMLLine)
			sw.Close()
			fs.Close()
		End If

		'Loads the current XML Document for appending

		Try
			Dim functionProfile As New MFunctionProfile
			If Not Request.QueryString("ReturnURL") Is Nothing Then
				'functionProfile = FunctionUtility.GetFunctionInfoByAction(Request.QueryString("ReturnURL"))
			End If
			doc.Load(psFileNm)

			'Gets the root node of the xml document
			root = doc.DocumentElement

			'Create the Error node 
			ErrorNode = doc.CreateElement("ERROR")

			ErrorChild = doc.CreateElement("DATE")
			ErrorChild.InnerText = Date.Now.ToString
			ErrorNode.AppendChild(ErrorChild)

			ErrorChild = doc.CreateElement("Login")
			If Not IsNothing(lblAccount.Text) Then
				ErrorChild.InnerText = lblAccount.Text
			Else
				ErrorChild.InnerText = ""
			End If
			'ErrorChild.InnerText = AccountUtility.GetCurrentAccountProfile.ACCOUNT
			ErrorNode.AppendChild(ErrorChild)

			ErrorChild = doc.CreateElement("Security_Entity")
			'ErrorChild.InnerText = ClientChoicesState(MClientChoices.SecurityEntityName)
			ErrorNode.AppendChild(ErrorChild)

			ErrorChild = doc.CreateElement("CLIPADDR")
			ErrorChild.InnerText = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
			ErrorNode.AppendChild(ErrorChild)

			ErrorChild = doc.CreateElement("CLBRTYPE")
			ErrorChild.InnerText = Request.Browser.Type
			ErrorNode.AppendChild(ErrorChild)

			If (Not poExec Is Nothing) Then

				If Not IsNothing(poExec.InnerException) Then

					LastError = poExec.InnerException

				Else
					LastError = poExec

				End If

				lbltoday.Text = Date.Now.ToString
				'lblBUName.Text = ClientChoicesState(MClientChoices.SecurityEntityName)
				'lblAccount.Text = AccountUtility.GetCurrentAccountProfile.ACCOUNT
				lblComputer.Text = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
				lblOpSystem.Text = Request.Browser.Platform
				lblBrowser.Text = Request.Browser.Type
				lblErrSource.Text = Request.QueryString("ReturnURL")
				lblErrMsg.Text = poExec.Message
				lblErrMsg1.Text = LastError.StackTrace
				lblErrorlog.Text = psFileNm.ToString


				ErrorChild = doc.CreateElement("STKTRA")
				ErrorChild.InnerText = LastError.StackTrace
				ErrorNode.AppendChild(ErrorChild)

				ErrorChild = doc.CreateElement("EXTYP")
				ErrorChild.InnerText = LastError.GetType.ToString
				ErrorNode.AppendChild(ErrorChild)

				ErrorChild = doc.CreateElement("ERRDESC")
				ErrorChild.InnerText = LastError.Message
				ErrorNode.AppendChild(ErrorChild)

				ErrorChild = doc.CreateElement("ACTION")
				ErrorChild.InnerText = lblErrSource.Text
				ErrorNode.AppendChild(ErrorChild)

			End If

			root.AppendChild(ErrorNode)
			lsStr = doc.InnerXml

			doc.Save(psFileNm)
			'Dim mMessage As New MMessageProfile
			'MessageUtility.GetMessageProfileByName(mMessage, "UnhandledException")
			'mMessage.BODY = doc.InnerText
			'NotifyUtility.SendNotifications(mMessage, SecurityEntityUtility.GetCurrentSeProfile.ID, FunctionUtility.GetCurrentFunction.ID)
		Catch lobjEx As Exception
		End Try
	End Sub

End Class