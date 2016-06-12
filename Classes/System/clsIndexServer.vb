Imports System.Data.OleDb
Imports System.Security.Principal
Imports System.Text ' for stringbuilder
'Imports Cisso
#Region " Notes "
' The clsIndexServer is not currently used with in the application.
' The class has been included for furture useage for the filemanagercontrol.ascx
' It is more that possible for a requirement to come down the line
' allowing for searching the file structure and should that
' happen this will come in handy to at least start
#End Region
Public Class clsIndexServer
	Public Function GetIndexServerTableData(ByRef yourDataGrid As DataGrid, ByVal SearchString As String, Optional ByVal pSOrderBy As String = "rank[a]") As DataGrid
		Dim DA As New OleDbDataAdapter
		Dim DS As New DataSet("IndexServerResults")		  ' give the dataset a name
		Dim Q As Object = CreateObject("ixsso.Query")
		'Dim Q As New CissoQuery
		Dim Util As Object = CreateObject("ixsso.Util")
		'Dim Util As New CissoUtil
		Dim returnColumns As String
		Dim strbldSearch As New StringBuilder(SearchString)

		' EXLUDE FOLDERS AND FILES FROM SEARCH
		' Anything that you don't want people to find in their
		' search results should go here.
		strbldSearch.Append(" and not #vpath = *\_vti_*")
		strbldSearch.Append(" and not #vpath = *\images*")
		strbldSearch.Append(" and not #filename *.config")

		returnColumns = "filename, path, vpath, size, write, " _
		 & "characterization, DocTitle, DocAuthor, " _
		 & "DocKeywords, rank, hitcount"
		With Q
			.Query = strbldSearch.ToString
			.Catalog = ConfigurationSettings.AppSettings("ISCatalog").ToString()			 ' name of your IndexServer Catalog
			.SortBy = pSOrderBy			 ' a-ascending, d-descending
			.Columns = returnColumns			 ' The columns you would like
			.MaxRecords = 250			  ' for best performance keep below 500
		End With

		' INCLUDE FOLDERS IN SEARCH
		' (1) Always include root directory, but not sub folders
		Util.AddScopeToQuery(Q, "\", "deep")
		' (2) Include folders selected in check box list AND their
		'     their sub folders. See sub Page_Load check list array
		' this should be the full path or vpath ie full path "c:\mycatalog\somefoler"

		'Dim itmFolder As ListItem
		'For Each itmFolder In chkListFolders.Items
		'    If itmFolder.Selected Then
		'        Util.AddScopeToQuery(Q, itmFolder.Value, "deep")
		'    End If
		'Next

		Try
			Dim impContext As WindowsImpersonationContext = impersonateAnonymous()
			DA.Fill(DS, Q.CreateRecordset("nonsequential"), "IndexServerResults")
			Dim intRowCount As Integer
			intRowCount = DS.Tables("IndexServerResults").Rows.Count
			yourDataGrid.DataSource = DS
			yourDataGrid.DataBind()
			'lblResults.Text = "There are " & intRowCount & " records."

			Q = Nothing
			Util = Nothing

		Catch ex As Exception
			'lblResults.Text = "There were no search results.  Please rephrase your query"
			'lblResults.Text = exc.Message
		End Try
	End Function
	Private Shared Function impersonateAnonymous() As WindowsImpersonationContext
		'Grab the current Http context 
		Dim context As HttpContext = HttpContext.Current

		'Set up a Service Provider based on this context
		Dim iServiceProvider As IServiceProvider = CType(context, IServiceProvider)

		'Create a type which represents an HTTPContext
		Dim httpWorkerRequestType As Type = GetType(HttpWorkerRequest)

		'Get the HttpWorkerRequest service from the service provider
		Dim workerRequest As HttpWorkerRequest = _
		  CType(iServiceProvider.GetService(httpWorkerRequestType), HttpWorkerRequest)

		'Get the token passed by IIS from the workerRequest service
		Dim ptrUserToken As IntPtr = workerRequest.GetUserToken()

		'Create a Windows Identity from the token
		Dim winIdentity As New WindowsIdentity(ptrUserToken)

		'Send back the IIS identity
		Return winIdentity.Impersonate
	End Function
End Class