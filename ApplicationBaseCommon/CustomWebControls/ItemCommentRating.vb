Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemCommentRating Class
    '
    ' WebControl that enables users to rate comments. 
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class ItemCommentRating
        Inherits WebControl
        Implements INamingContainer 'ToDo: Add Implements Clauses for implementation methods of these interface(s)

        Private DefaultRatingImagePath As String = BaseSettings.imagePath & "Ratings/"

        Private _ratingImagePath As String = String.Empty


        Private rdlRating As RadioButtonList
        Private btnSubmit As LinkButton

        'Private objUserInfo As UserInfo
        'Private objSectionInfo As SectionInfo

        Private objModuleProfileInfo As MModuleProfileInfo
        Private objSecurityProfile As MAccountSecurityInfo

        Private _displayOnly As Boolean = False
        Private _averageRatingText As String = "Average Rating: "
        Private _yourRatingText As String = "Your Rating: "

        Private _goodText As String = "Good"
        Private _badText As String = "Bad"
        Private _submitText As String = "Submit Rating"





        '*********************************************************************
        '
        ' ItemCommentRating Constructor
        '
        ' Retrieves user info object from context.
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.Table)

            If Not (Context Is Nothing) Then
                ' Get UserInfo object
                'objUserInfo = CType(Context.Items("UserInfo"), UserInfo)
                ' Get SectionInfo object
                'objSectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)
                objModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
                objSecurityProfile = New MAccountSecurityInfo(objModuleProfileInfo)
            End If

            ' Set default class
            CssClass = "itemRating"
            EnableViewState = False
        End Sub 'New



        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim item As ContentItem

            If TypeOf NamingContainer Is ContentItem Then
                item = CType(NamingContainer, ContentItem)
            Else
                item = CType(NamingContainer.NamingContainer, ContentItem)
            End If
            Dim objContentInfo As ContentInfo = CType(item.DataItem, ContentInfo)
            AverageRating = objContentInfo.AverageRating
            YourRating = objContentInfo.YourRating
            ContentPageID = objContentInfo.ContentPageID
        End Sub 'OnDataBinding




        '*********************************************************************
        '
        ' Rating_Changed Event
        '
        ' Event that is raised when a user picks a new rating.
        '
        '*********************************************************************
        Public Event Rating_Changed As EventHandler




        '*********************************************************************
        '
        ' DisplayOnly Property
        '
        ' When true, only displays ratings and does not enable user
        ' to update ratings. Useful for smaller display footprint.
        '
        '*********************************************************************

        Public Property DisplayOnly() As Boolean
            Get
                Return _displayOnly
            End Get
            Set(ByVal value As Boolean)
                _displayOnly = value
            End Set
        End Property

        '*********************************************************************
        '
        ' AverageRatingText Property
        '
        ' The text that is displayed for Average Rating.
        '
        '*********************************************************************

        Public Property AverageRatingText() As String
            Get
                If ViewState("AverageRatingText") Is Nothing Then
                    Return "Average Rating: "
                Else
                    Return CStr(ViewState("AverageRatingText"))
                End If
            End Get
            Set(ByVal value As String)
                ViewState("AverageRatingText") = value
            End Set
        End Property

        '*********************************************************************
        '
        ' YourRatingText Property
        '
        ' The text that is displayed for Your Rating.
        '
        '*********************************************************************

        Public Property YourRatingText() As String
            Get
                Return _yourRatingText
            End Get
            Set(ByVal value As String)
                _yourRatingText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' RatingImagePath Property
        '
        ' The path to the directory that contains rating images
        '
        '*********************************************************************

        Public Property RatingImagePath() As String
            Get
                Return _ratingImagePath
            End Get
            Set(ByVal value As String)
                _ratingImagePath = value
            End Set
        End Property


        '*********************************************************************
        '
        ' ContentPageID Property
        '
        ' The Content ID for this item.
        '
        '*********************************************************************

        Public Property ContentPageID() As Integer
            Get
                If ViewState("ContentPageID") Is Nothing Then
                    Return -1
                Else
                    Return Fix(ViewState("ContentPageID"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("ContentPageID") = value
            End Set
        End Property


        '*********************************************************************
        '
        ' AverageRating Property
        '
        ' The Average Rating for this item.
        '
        '*********************************************************************

        Public Property AverageRating() As Integer
            Get
                If ViewState("AverageRating") Is Nothing Then
                    Return -1
                Else
                    Return Fix(ViewState("AverageRating"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("AverageRating") = value
            End Set
        End Property

        '*********************************************************************
        '
        ' YourRating Property
        '
        ' The current user's rating.
        '
        '*********************************************************************

        Public Property YourRating() As Integer
            Get
                If ViewState("YourRating") Is Nothing Then
                    Return -1
                Else
                    Return Fix(ViewState("YourRating"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("YourRating") = value
            End Set
        End Property

        '*********************************************************************
        '
        ' GoodText Property
        '
        ' The text that is displayed for highly rated items.
        '
        '*********************************************************************

        Public Property GoodText() As String
            Get
                Return _goodText
            End Get
            Set(ByVal value As String)
                _goodText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' BadText Property
        '
        ' The text that is displayed for lowly rated items.
        '
        '*********************************************************************

        Public Property BadText() As String
            Get
                Return _badText
            End Get
            Set(ByVal value As String)
                _badText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' SubmitText Property
        '
        ' The text that is displayed for submitting a rating.
        '
        '*********************************************************************

        Public Property SubmitText() As String
            Get
                Return _submitText
            End Get
            Set(ByVal value As String)
                _submitText = value
            End Set
        End Property




        '*********************************************************************
        '
        ' btnSubmit_Click Method
        '
        ' This method is executed when a user submits a new rating.
        '
        '*********************************************************************
        Private Sub btnSubmit_Click(ByVal s As [Object], ByVal e As EventArgs)
            Dim _rating As Integer
            If rdlRating.SelectedIndex <> -1 Then
                _rating = Int32.Parse(rdlRating.SelectedItem.Value)
                'AverageRating = RatingUtility.AddRating(objUserInfo.Username, ContentPageID, _rating)
                AverageRating = 1
                YourRating = _rating
            End If
        End Sub 'btnSubmit_Click



        '*********************************************************************
        '
        ' Rating_Click Method
        '
        ' The method raises the Rating_Changed event.
        '
        '*********************************************************************
        Private Sub Rating_Clicked(ByVal sender As [Object], ByVal e As EventArgs)
            RaiseEvent Rating_Changed(Me, EventArgs.Empty)
        End Sub 'Rating_Clicked




        '*********************************************************************
        '
        ' CreateChildControls Method
        '
        ' If the user can rate content, add the submit button.
        '
        '*********************************************************************
        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            If objSecurityProfile.MayEdit Then
                ' Create RadioButton List
                rdlRating = New RadioButtonList()
                rdlRating.RepeatDirection = RepeatDirection.Horizontal

                Dim i As Integer
                For i = 1 To 5
                    rdlRating.Items.Add(i.ToString())
                Next i
                Controls.Add(rdlRating)

                ' Create Submit Button
                btnSubmit = New LinkButton()
                btnSubmit.ID = "btnSubmit"
                btnSubmit.Text = SubmitText
                AddHandler btnSubmit.Click, AddressOf btnSubmit_Click
                Controls.Add(btnSubmit)
            End If
        End Sub 'CreateChildControls 


        '*********************************************************************
        '
        ' Controls Property
        '
        ' Make sure that when you access a child control, the
        ' CreateChildControls method has been called.
        '
        '*********************************************************************

        Public Overrides ReadOnly Property Controls() As ControlCollection
            Get
                EnsureChildControls()
                Return MyBase.Controls
            End Get
        End Property





        '*********************************************************************
        '
        ' Render Method
        '
        ' Don't render if ratings are not enabled.
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal tw As HtmlTextWriter)
            ' Check if ratings are enabled
            'If objSectionInfo.EnableCommentRatings Then
            '    MyBase.Render(tw)
            'End If
            MyBase.Render(tw)
        End Sub 'Render



        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Render the Rating control UI to the browser.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal tw As HtmlTextWriter)


            ' Display Checkbox List
            If objSecurityProfile.MayEdit AndAlso YourRating = -1 AndAlso Not DisplayOnly Then
                tw.RenderBeginTag(HtmlTextWriterTag.Tr)

                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                tw.Write(_badText)
                tw.RenderEndTag()

                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                rdlRating.RenderControl(tw)
                tw.RenderEndTag()

                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                tw.Write(_goodText)
                tw.RenderEndTag()

                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                tw.Write("|")
                tw.RenderEndTag()

                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                btnSubmit.RenderControl(tw)
                tw.RenderEndTag()

                tw.RenderEndTag() ' close tr

                ' Display Current Rating
                If AverageRating <> -1 Then
                    tw.RenderBeginTag(HtmlTextWriterTag.Tr)
                    tw.AddAttribute(HtmlTextWriterAttribute.Colspan, "4")
                    tw.RenderBeginTag(HtmlTextWriterTag.Td)
                    tw.Write(_averageRatingText)
                    tw.Write(GetRatingImage(AverageRating))
                    tw.RenderEndTag()
                    tw.RenderEndTag() ' close tr
                End If
            Else
                tw.RenderBeginTag(HtmlTextWriterTag.Tr)

                ' Display Your Rating
                If YourRating <> -1 Then
                    tw.RenderBeginTag(HtmlTextWriterTag.Td)
                    tw.Write(_yourRatingText)
                    tw.Write(GetRatingImage(YourRating))
                    tw.RenderEndTag()
                End If

                If DisplayOnly Then
                    tw.RenderEndTag() ' close tr
                    tw.RenderBeginTag(HtmlTextWriterTag.Tr)
                End If

                ' Display Average Rating
                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                If AverageRating <> -1 Then
                    tw.Write(_averageRatingText)
                    tw.Write(GetRatingImage(AverageRating))
                Else
                    tw.Write("&nbsp;")
                End If
                tw.RenderEndTag()
                tw.RenderEndTag() ' close tr
            End If
        End Sub 'RenderContents





        '*********************************************************************
        '
        ' GetRatingImage Method
        '
        ' Returns the appropriate image for a particular rating.
        '
        '*********************************************************************
        Private Function GetRatingImage(ByVal ratingNumber As Integer) As String
            If _ratingImagePath <> String.Empty Then
                Return String.Format("<img src=""{0}Rating{1}.gif"">", Page.ResolveUrl(_ratingImagePath), ratingNumber)
            Else
                Return String.Format("<img src=""{0}Rating{1}.gif"">", Page.ResolveUrl(DefaultRatingImagePath), ratingNumber)
            End If
        End Function 'GetRatingImage 
    End Class 'ItemCommentRating 
End Namespace
