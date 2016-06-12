Imports ApplicationBase.Model
Imports System.Configuration
Imports System.Web
Imports System.IO
Imports System.Text.RegularExpressions

Namespace Globals
    Public Class BaseSettings
        Private Shared _Always_Show_LeftNav As String = String.Empty
        Private Shared _App_Displayed_Name As String = String.Empty
        Private Shared _App_Name As String = String.Empty
        Private Shared _Append_To_File As String = String.Empty
        Private Shared _Auth_Type As String = String.Empty
        Private Shared _Base_Page As String = String.Empty
        Private Shared _Business_Unit_Translation As String = String.Empty
        Private Shared _Cache_Path As String = String.Empty
        Private Shared _Common_Script_Path As String = String.Empty
        Private Shared _Common_Style_Path As String = String.Empty
        Private Shared _Connection_String As String = String.Empty
        Private Shared _Conversion_Pattern As String = String.Empty
        Private Shared _DAL As String = String.Empty
        Private Shared _Default_Action As String = String.Empty
        Private Shared _Default_Business_Unit_ID As String = String.Empty
        Private Shared _Expected_Up_By As String = String.Empty
        Private Shared _EnableCache As String = String.Empty
        Private Shared _EnableEncryption As String = String.Empty
        Private Shared _Environment As String = String.Empty
        Private Shared _Force_HTTPS As String = String.Empty
        Private Shared _FQDNPage As String = String.Empty
        Private Shared _HTTP_Schema As String = String.Empty
        Private Shared _Image_Path As String = String.Empty
        Private Shared _LDAP_Domain As String = String.Empty
        Private Shared _LDAP_Server As String = String.Empty
        Private Shared _Log_Path As String = String.Empty
        Private Shared _Log_Priority As String = String.Empty
        Private Shared _Root_Site As String = String.Empty
        Private Shared _Server_Side_View_State As String = String.Empty
        Private Shared _SMTP_From As String = String.Empty
        Private Shared _SMTP_Server As String = String.Empty
        Private Shared _SMTP_Domain As String = String.Empty
        Private Shared _Styles_Path As String = String.Empty
        Private Shared _Skin_Path As String = String.Empty
        Private Shared _SyncPassword As String = String.Empty
        Private Shared _UISkinPath As String = String.Empty
        Private Shared _Under_Construction As String = String.Empty
        Private Shared _Version As String = String.Empty

        '*********************************************************************
        '
        ' AlwaysLeftNav Property
        '
        ' Represents AlwaysLeftNav setting in the web.config file.
        '
        '*********************************************************************
        Public Shared ReadOnly Property alwaysLeftNav() As Boolean
            Get
                If _Always_Show_LeftNav = String.Empty Then
                    _Always_Show_LeftNav = ConfigurationManager.AppSettings("Always_Left_Nav")
                End If
                Return _Always_Show_LeftNav
            End Get
        End Property

        Public Shared ReadOnly Property appDisplayedName() As String
            Get
                If _App_Displayed_Name = String.Empty Then
                    Dim titleAttr As System.Reflection.AssemblyTitleAttribute
                    Dim asm As System.Reflection.Assembly
                    asm = System.Reflection.Assembly.GetExecutingAssembly
                    titleAttr = asm.GetCustomAttributes(GetType(System.Reflection.AssemblyTitleAttribute), False)(0)
                    _App_Displayed_Name = titleAttr.Title
                End If
                Return _App_Displayed_Name
            End Get
        End Property

        Public Shared ReadOnly Property appendToFile() As String
            Get
                If _Append_To_File = String.Empty Then _Append_To_File = ConfigurationManager.AppSettings("Append_To_File")
                Return _Append_To_File
            End Get
        End Property

        Public Shared ReadOnly Property appName() As String
            Get
                If _App_Name = String.Empty Then
                    _App_Name = ConfigurationManager.AppSettings("App_Name")
                End If
                Return _App_Name
            End Get
        End Property

        Public Shared ReadOnly Property appPath() As String
            Get
                If HttpContext.Current.Request.ApplicationPath = "/" Then
                    Return String.Empty
                End If
                Return HttpContext.Current.Request.ApplicationPath
            End Get
        End Property

        Public Shared ReadOnly Property authenticationType() As String
            Get
                If _Auth_Type = String.Empty Then
                    _Auth_Type = ConfigurationManager.AppSettings(environment & "Authentication_Type")
                End If
                Return _Auth_Type
            End Get
        End Property
        Public Shared ReadOnly Property basePage() As String
            Get
                If _Base_Page = String.Empty Then
                    _Base_Page = ConfigurationManager.AppSettings("Base_Page")
                End If
                Return _Base_Page
            End Get
        End Property
        Public Shared ReadOnly Property businessUnitTranslation() As String
            Get
                If _Business_Unit_Translation = String.Empty Then
                    _Business_Unit_Translation = ConfigurationManager.AppSettings("Business_Unit_Translation")
                End If
                Return _Business_Unit_Translation
            End Get
        End Property

        Public Shared ReadOnly Property cachePath() As String
            Get
                If _Cache_Path = String.Empty Then
                    _Cache_Path = HttpContext.Current.Server.MapPath("~\") & "CacheDependency\"
                End If
                Return _Cache_Path
            End Get
        End Property
        Public Shared ReadOnly Property commonScriptPath() As String
            Get
                If _Common_Script_Path = String.Empty Then
                    _Common_Script_Path = ConfigurationManager.AppSettings("Common_Script_Path")
                End If
                Return _Common_Script_Path
            End Get
        End Property

        Public Shared ReadOnly Property commonStylePath() As String
            Get
                If _Common_Style_Path = String.Empty Then
                    _Common_Style_Path = ConfigurationManager.AppSettings("Common_Style_Path")
                End If
                Return _Common_Style_Path
            End Get
        End Property

        Public Shared ReadOnly Property connectionString() As String
            Get
                If _Connection_String = String.Empty Then
                    _Connection_String = ConfigurationManager.AppSettings("Common_Style_Path")
                End If
                Return _Connection_String
            End Get
        End Property
        Public Shared ReadOnly Property conversionPattern() As String
            Get
                If _Conversion_Pattern = String.Empty Then _Conversion_Pattern = ConfigurationManager.AppSettings("Conversion_Pattern")
                Return _Conversion_Pattern
            End Get
        End Property

        Public Shared ReadOnly Property applicationBaseDAL() As String
            Get
                If _DAL = String.Empty Then
                    _DAL = ConfigurationManager.AppSettings(environment & "DAL")
                End If
                Return _DAL
            End Get
        End Property

        Public Shared ReadOnly Property defaultAction() As String
            Get
                If _Default_Action = String.Empty Then
                    _Default_Action = ConfigurationManager.AppSettings("Default_Action")
                End If
                Return _Default_Action
            End Get
        End Property
        Public Shared ReadOnly Property defaultBusinessUnitID() As String
            Get
                If _Default_Business_Unit_ID = String.Empty Then
                    _Default_Business_Unit_ID = ConfigurationManager.AppSettings(environment & "Default_Business_Unit_ID")
                End If
                Return _Default_Business_Unit_ID
            End Get
        End Property

        Public Shared ReadOnly Property expectedUpBy() As String
            Get
                If _Expected_Up_By = String.Empty Then
                    _Expected_Up_By = ConfigurationManager.AppSettings("Expected_Up_By")
                End If
                Return _Expected_Up_By
            End Get
        End Property

        Public Shared ReadOnly Property enableCache() As Boolean
            Get
                If _EnableCache = String.Empty Then
                    _EnableCache = ConfigurationManager.AppSettings(environment & "Enable_Cache")
                End If
                Return _EnableCache
            End Get
        End Property

        Public Shared ReadOnly Property enableEncryption() As Boolean
            Get
                If _EnableEncryption = String.Empty Then
                    _EnableEncryption = ConfigurationManager.AppSettings(environment & "Enable_Encryption")
                End If
                Return CBool(_EnableEncryption)
            End Get
        End Property
        Public Shared ReadOnly Property environment() As String
            Get
                If _Environment = String.Empty Then
                    _Environment = ConfigurationManager.AppSettings("Environment")
                End If
                Return _Environment
            End Get
        End Property

        Public Shared Property forceHTTPS() As Boolean
            Get
                If _Force_HTTPS = String.Empty Then
                    _Force_HTTPS = ConfigurationManager.AppSettings(environment & "Force_HTTPS")
                End If
                Return CBool(_Force_HTTPS)
            End Get
            Set(ByVal Value As Boolean)
                If Value Then
                    _HTTP_Schema = "HTTPS"
                Else
                    _HTTP_Schema = "HTTP"
                End If
                _Force_HTTPS = Value
            End Set
        End Property

        Public Shared ReadOnly Property FQDNPage() As String
            Get
                If _FQDNPage = String.Empty Then
                    _FQDNPage = rootSite & ConfigurationManager.AppSettings("Base_Page")
                Else
                    If Not (_FQDNPage = rootSite & ConfigurationManager.AppSettings("Base_Page")) Then
                        _FQDNPage = rootSite & ConfigurationManager.AppSettings("Base_Page")
                    End If
                End If
                Return _FQDNPage
            End Get
        End Property ' FQDNBasePage

        Public Shared Function getURL() As String
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

        Public Shared ReadOnly Property hTTPSchema() As String
            Get
                If _HTTP_Schema = String.Empty Then
                    _HTTP_Schema = ConfigurationManager.AppSettings(environment & "HTTP_Schema")
                End If
                Return _HTTP_Schema
            End Get
        End Property

        Public Shared ReadOnly Property imagePath() As String
            Get
                If _Image_Path = String.Empty Then
                    _Image_Path = RootSite & "Images/"
                End If
                Return _Image_Path
            End Get
        End Property

        Public Shared ReadOnly Property LDAPDomain() As String
            Get
                If _LDAP_Domain = String.Empty Then
                    _LDAP_Domain = ConfigurationManager.AppSettings(environment & "LDAP_Domain")
                End If
                Return _LDAP_Domain
            End Get
        End Property

        Public Shared ReadOnly Property LDAPServer() As String
            Get
                If _LDAP_Server = String.Empty Then
                    _LDAP_Server = ConfigurationManager.AppSettings(environment & "LDAP_Server")
                End If
                Return _LDAP_Server
            End Get
        End Property

        Public Shared ReadOnly Property logPath() As String
            Get
                If _Log_Path = String.Empty Then _Log_Path = ConfigurationManager.AppSettings(environment & "Log_Path")
                Return _Log_Path
            End Get
        End Property

        Public Shared ReadOnly Property logPriority() As String
            Get
                If _Log_Priority = String.Empty Then _Log_Priority = ConfigurationManager.AppSettings(environment & "Log_Priority")
                Return _Log_Priority
            End Get
        End Property
        Public Shared ReadOnly Property serverSideViewState() As Boolean
            Get
                If _Server_Side_View_State = String.Empty Then
                    _Server_Side_View_State = ConfigurationManager.AppSettings("Server_Side_View_State")
                End If
                Return _Server_Side_View_State
            End Get
        End Property

        Public Shared ReadOnly Property SMTPFrom() As String
            Get
                If _SMTP_From = String.Empty Then
                    _SMTP_From = ConfigurationManager.AppSettings(environment & "SMTP_From")
                End If
                Return _SMTP_From
            End Get
        End Property

        Public Shared ReadOnly Property SMTPServer() As String
            Get
                If _SMTP_Server = String.Empty Then
                    _SMTP_Server = ConfigurationManager.AppSettings(environment & "SMTP_Server")
                End If
                Return _SMTP_Server
            End Get
        End Property

        Public Shared ReadOnly Property stylesPath() As String
            Get
                If _Styles_Path = String.Empty Then
                    _Styles_Path = rootSite & "Images/"
                End If
                Return _Styles_Path
            End Get
        End Property
        Public Shared ReadOnly Property SMTPDomain() As String
            Get
                If _SMTP_Domain = String.Empty Then
                    _SMTP_Domain = ConfigurationManager.AppSettings(environment & "SMTP_Domain")
                End If
                Return _SMTP_Domain
            End Get
        End Property

        Public Shared ReadOnly Property syncPassword() As String
            Get
                If _SyncPassword = String.Empty Then
                    If authenticationType.Trim.ToLower <> "internal" Then
                        _SyncPassword = True
                    Else
                        _SyncPassword = False
                    End If
                End If
                Return _SyncPassword
            End Get
        End Property    ' SyncPassword

        Public Shared ReadOnly Property skin(ByVal Business_Unit_Seq_ID As Integer) As String
            Get
                Dim retVal As String = "Default"
                'Dim businessUnitProfileInfo As New MBusinessUnitProfileInfo
                'Dim businessUnitProfileInfoCollection As New MBusinessUnitProfileInfoCollection
                'BusinessUnitUtility.GetBusinessProfileCollection(businessUnitProfileInfoCollection)
                'businessUnitProfileInfo = businessUnitProfileInfoCollection.GetBusinessUnitByID(BUSINESS_UNIT_SEQ_ID)
                'retVal = businessUnitProfileInfo.Skin
                Return retVal
            End Get
        End Property

        Public Shared ReadOnly Property skinPath() As String
            Get
                If _Skin_Path = String.Empty Then
                    _Skin_Path = ConfigurationManager.AppSettings(environment & "Skin_Path")
                End If
                Return _Skin_Path
            End Get
        End Property

        Public Shared ReadOnly Property rootSite() As String
            Get
                If _HTTP_Schema = String.Empty Then
                    If forceHTTPS Then
                        _HTTP_Schema = "HTTPS"
                    Else
                        _HTTP_Schema = HttpContext.Current.Request.Url.Scheme
                    End If
                End If

                If HttpContext.Current.Request.ApplicationPath = "/" Then
                    If _Root_Site = String.Empty Then
                        _Root_Site = _HTTP_Schema & "://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/"
                    End If
                Else
                    If _Root_Site = String.Empty Then
                        _Root_Site = _HTTP_Schema & "://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/" & appName & "/"
                    End If
                End If
                Return _Root_Site
            End Get
        End Property

        Public Shared ReadOnly Property uIPath() As String
            Get
                If _UISkinPath = String.Empty Then
                    _UISkinPath = ConfigurationManager.AppSettings(environment & "UI_Skin_Path")
                End If
                Return _UISkinPath
            End Get
        End Property

        Public Shared ReadOnly Property underConstruction() As Boolean
            Get
                If _Under_Construction = String.Empty Then
                    _Under_Construction = ConfigurationManager.AppSettings("Under_Construction")
                End If
                Return CBool(_Under_Construction)
            End Get
        End Property

        Public Shared ReadOnly Property verison() As String
            Get
                If _Version = String.Empty Then
                    '_Version = System.Reflection.Assembly.GetCallingAssembly.GetName.Version.ToString
                    Dim myAssembly As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly
                    If Not myAssembly Is Nothing Then
                        _Version = myAssembly.GetName.Version.ToString
                    End If
                    'Dim myAssembly2 As Reflection.Assembly = Reflection.Assembly.GetEntryAssembly
                    'Dim myAssembly3 As Reflection.Assembly = Reflection.Assembly.GetCallingAssembly
                End If
                Return _Version
            End Get
        End Property
    End Class

    Public Class BaseHelpers
        ' *********************************************************************
        '  Text Formatting Methods
        ' *********************************************************************
        Public Shared Function FormatText(ByVal useHtml As AllowHtml, ByVal sectionID As Integer, ByVal [text] As String) As String
            ' Return when null
            If [text] Is Nothing Then
                Return String.Empty
            End If

            ' if no HTML allowed, HTML Encode 
            If useHtml = AllowHtml.None Then
                [text] = FormatPlainText([text])
            Else
                [text] = EnsureSafeAnchors([text])
            End If
            ' Apply transformations
            '[text] = ApplyTransformations(sectionID, [text])

            Return [text]
        End Function 'FormatText

        '*********************************************************************
        '
        ' FormatPlainText Method
        '
        ' This method is used for formatting all text that SHOULD NOT
        ' contain HTML. This method 
        ' HtmlEncodes the text and adds line breaks. 
        '
        '*********************************************************************
        Public Shared Function FormatPlainText(ByVal [text] As String) As String
            ' HTML encode the text
            [text] = HttpUtility.HtmlEncode([text])

            ' Add line breaks 
            [text] = Regex.Replace([text], vbLf + vbLf, "<p>")
            [text] = Regex.Replace([text], vbLf, "<br>")

            Return [text]
        End Function 'FormatPlainText

        Private Shared Function EnsureSafeAnchors(ByVal stringToTransform As String) As String
            Dim matchs As MatchCollection

            ' Ensure we have safe anchors
            matchs = Regex.Matches(stringToTransform, "&lt;a.href=(&quot;)?(?<url>http://((.|\n)*?))(&quot;)?&gt;(?<target>((.|\n)*?))&lt;/a&gt;", RegexOptions.IgnoreCase Or RegexOptions.Compiled)

            Dim m As Match
            For Each m In matchs
                stringToTransform = stringToTransform.Replace(m.ToString(), "<a target=""_new"" href=""" + m.Groups("url").ToString() + """>" + m.Groups("target").ToString() + "</a>")
            Next m

            Return stringToTransform
        End Function 'EnsureSafeAnchors
    End Class
End Namespace