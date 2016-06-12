Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.BusinessUnits
Imports ApplicationBase.Model.Modules
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web

#Region " Notes "
' The BaseHelper class gives common access to application settings
' and other common routines
#End Region
Public Class BaseHelperOld
    Private Shared _ExceptionError As Exception = Nothing
    Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

    Public Shared Property ExceptionError() As Exception
        Get
            Return _ExceptionError
        End Get
        Set(ByVal Value As Exception)
            _ExceptionError = Value
        End Set
    End Property

    '*********************************************************************
    ' GetURL Method
    ' Returns the URL including all URL parameters.
    '*********************************************************************
    Public Shared Function GetURL() As String
        Dim context As HttpContext
        context = HttpContext.Current
        Dim item
        Dim myURL As String = "&"
        For Each item In context.Request.QueryString
            If item.tolower <> "action" And item.tolower <> "returnurl" And item.tolower <> "wfn" And item.tolower <> "requestedaction" Then
                myURL &= item & "=" & context.Server.UrlEncode(context.Request.QueryString(item)) & "&"
            End If
        Next
        myURL = myURL.Substring(0, Len(myURL) - 1)
        Return myURL
    End Function    ' GetURL

    Public Shared Function IsNumeric(ByVal strInteger As String) As Boolean
        Try
            Dim intTemp As Integer = Int32.Parse(strInteger)
            Return True
        Catch
            Return False
        End Try
    End Function 'IsNumeric

    '*********************************************************************
    '
    ' GetRandomString function
    '
    ' Returns a lowercase alpha charactor
    '
    '*********************************************************************
    Public Shared Function GetRandomPassword(Optional ByVal Low As Integer = 65, Optional ByVal High As Integer = 90) As String
        Dim retVal As String
        retVal = System.Guid.NewGuid().ToString()
        Return retVal
    End Function    ' GetRandomString

    '*********************************************************************
    '
    ' GetRandomNumber function
    '
    ' Returns a random number given the max and min - 0
    '
    '*********************************************************************
    Public Shared Function GetRandomNumber(ByVal MaxNumber As Integer, Optional ByVal MinNumber As Integer = 0) As Integer
        Dim retVal As Integer = 0
        'initialize random number generator
        Dim r As New Random(System.DateTime.Now.Millisecond)
        'if passed incorrect arguments, swap them
        'can also throw exception or return 0
        If MinNumber > MaxNumber Then
            Dim t As Integer = MinNumber
            MinNumber = MaxNumber
            MaxNumber = t
        End If
        retVal = r.Next(MinNumber, MaxNumber)
        Sleep(CType((System.DateTime.Now.Millisecond * (retVal / 100)), Long))
        Return retVal
    End Function    'GetRandomNumber


    '*********************************************************************
    '
    ' UI Property
    '
    ' Retrieves the UI path from the web.config.
    '
    '*********************************************************************

    Public Shared ReadOnly Property UI(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String
        Get
            Dim businessUnitProfileInfo As New MBusinessUnitProfileInfo
            Dim businessUnitProfileInfoCollection As New MBusinessUnitProfileInfoCollection
            BusinessUnitUtility.GetBusinessProfileCollection(businessUnitProfileInfoCollection)
            Dim retVal As String = "Default"
            businessUnitProfileInfo = businessUnitProfileInfoCollection.GetBusinessUnitByID(BUSINESS_UNIT_SEQ_ID)
            retVal = businessUnitProfileInfo.Skin
            Return retVal
        End Get
    End Property


    '*********************************************************************
    '
    ' SelectBusinessUnit method
    ' Navigates to the select a business unit module with a return url
    '
    '*********************************************************************
    Public Shared Sub SelectBusinessUnit()
        Dim myRedirect As String = String.Empty
        Dim moduleProfileInfo As MModuleProfileInfo
        moduleProfileInfo = AppModulesUtility.GetCurrentModule()
        myRedirect += "select a business unit"
        myRedirect += "&ReturnURL=" & moduleProfileInfo.Action
        HttpContext.Current.Session("clientMSG") = moduleProfileInfo.Name & "  is not valid with the " & BaseSettings.businessUnitTranslation & " you have selected.<br>Please choose an appropriate " & BaseSettings.businessUnitTranslation
        NavControler.NavTo(myRedirect)
    End Sub 'SelectBusinessUnit

    '*********************************************************************
    '
    ' SetDropSelection method
    ' Sets the selected value of a drop down list
    '
    '*********************************************************************
    Public Shared Sub SetDropSelection(ByRef theDropDown As WebControls.DropDownList, ByVal SelectedVale As String)
        Try
            Dim X As Integer
            For X = 0 To theDropDown.Items.Count - 1
                If theDropDown.Items(X).Value = SelectedVale Then
                    theDropDown.SelectedIndex = X
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub 'SetDropSelection

    Public Shared Sub ShowUnderConstruction()
        Try
            If BaseSettings.underConstruction Then
                HttpContext.Current.Response.Redirect(BaseSettings.rootSite & "Pages/System/UnderConstruction.aspx")
            End If
        Catch ex As Exception
            Dim mike As String = String.Empty
        End Try
    End Sub

    Public Shared Function StripTags(ByVal [text] As String) As String
        [text] = Regex.Replace([text], "&nbsp;", "", RegexOptions.IgnoreCase)
        Return Regex.Replace([text], "<.+?>", "", RegexOptions.Singleline)
    End Function 'StripTags     

    Public Shared Function Truncate(ByVal [text] As String, ByVal length As Integer) As String
        If [text].Length > length Then
            [text] = [text].Substring(0, length)
        End If
        Return [text]
    End Function 'Truncate

    Public Shared Function TruncateWithEllipsis(ByVal [text] As String, ByVal length As Integer) As String
        If [text].Length > length Then
            [text] = [text].Substring(0, length) + "..."
        End If
        Return [text]
    End Function 'TruncateWithEllipsis

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Binds a dataset to a report and the report to a crystal report viewer.
    ''' </summary>
    ''' <param name="CRViewer">Crystal Report Viewer object</param>
    ''' <param name="Report">Crystal report</param>
    ''' <param name="DataSource">DataSet</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ReganM1]	12/26/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    'Public Shared Sub BindReport(ByRef CRViewer As CrystalReportViewer, ByRef Report As ReportDocument, ByRef DataSource As DataSet)
    '    Report.SetDataSource(DataSource.Tables(0))
    '    CRViewer.ReportSource = Report
    '    CRViewer.DataBind()
    '    CRViewer.DisplayToolbar = False
    'End Sub
End Class