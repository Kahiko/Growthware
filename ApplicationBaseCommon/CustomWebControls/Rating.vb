Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' Rating Class
    '
    ' WebControl that enables users to rate community content. 
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class Rating
        Inherits WebControl
        Implements INamingContainer 'ToDo: Add Implements Clauses for implementation methods of these interface(s)

        Private DefaultRatingImagePath As String = BaseSettings.imagePath & "Ratings/"

        Private _ratingImagePath As String = String.Empty
        Private rdlRating As RadioButtonList
        Private btnSubmit As LinkButton
        Private objAccountProfileInfo As MAccountProfileInfo
        Private objModuleProfileInfo As MModuleProfileInfo
        Private objContentInfo As ContentInfo

        Private _contentPageID As Integer
        Private _averageRating As Integer = -1
        Private _yourRating As Integer = -1

        Private _displayOnly As Boolean = False
        Private _averageRatingText As String = "Average Rating: "
        Private _yourRatingText As String = "Your Rating: "

        Private _goodText As String = "Good"
        Private _badText As String = "Bad"
        Private _submitText As String = "Submit Rating"

        '*********************************************************************
        '
        ' RateAction Constructor
        '
        ' Retrieves user info object from context.
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.Table)
            ' Set default CSS class
            CssClass = "rating"

            If Not (Context Is Nothing) Then
                ' Get UserInfo object
                objAccountProfileInfo = CType(Context.Session("AccountProfileInfo"), MAccountProfileInfo)

                ' Get SectionInfo object
                objModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)

                ' Get ContentInfo
                objContentInfo = CType(Context.Items("ContentInfo"), ContentInfo)

                Try
                    _averageRating = objContentInfo.AverageRating
                    _yourRating = objContentInfo.YourRating
                    _contentPageID = objContentInfo.ContentPageID
                Catch ex As Exception

                End Try
            End If
        End Sub 'New

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
        ' AverageRating Property
        '
        ' The Average Rating for this item.
        '
        '*********************************************************************
        Public Property AverageRating() As Integer
            Get
                Return _averageRating
            End Get
            Set(ByVal value As Integer)
                _averageRating = value
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
                Return _yourRating
            End Get
            Set(ByVal value As Integer)
                _yourRating = value
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
            Dim rating As Integer
            If rdlRating.SelectedIndex <> -1 Then
                rating = Int32.Parse(rdlRating.SelectedItem.Value)
                'needs help
                '_averageRating = RatingUtility.AddRating(objAccountProfileInfo.ACCOUNT, _contentPageID, rating)
                _yourRating = rating
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
            ' needs help
            If objAccountProfileInfo.IsAuthenticated Then
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
            'needs help
            'If ModuleProfileInfo.EnableRatings Then
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
            ' needs help
            If objAccountProfileInfo.IsAuthenticated AndAlso YourRating = -1 AndAlso Not DisplayOnly Then
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
                ' Display Averate Rating
                ' Only when not -1
                If _averageRating <> -1 Then
                    tw.RenderBeginTag(HtmlTextWriterTag.Tr)

                    tw.AddAttribute(HtmlTextWriterAttribute.Colspan, "4")
                    tw.RenderBeginTag(HtmlTextWriterTag.Td)
                    tw.Write(_averageRatingText)
                    tw.Write(GetRatingImage(_averageRating))
                    tw.RenderEndTag()

                    tw.RenderEndTag() ' close tr
                End If
            Else
                tw.RenderBeginTag(HtmlTextWriterTag.Tr)

                ' Display Your Rating
                If YourRating <> -1 Then
                    tw.RenderBeginTag(HtmlTextWriterTag.Td)
                    tw.Write(_yourRatingText)
                    tw.Write(GetRatingImage(_yourRating))
                    tw.RenderEndTag()
                End If

                If DisplayOnly Then
                    tw.RenderEndTag() ' close tr
                    tw.RenderBeginTag(HtmlTextWriterTag.Tr)
                End If

                tw.RenderBeginTag(HtmlTextWriterTag.Td)
                ' Display Average Rating
                If _averageRating <> -1 Then
                    tw.Write(_averageRatingText)
                    tw.Write(GetRatingImage(_averageRating))
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
        Private Function GetRatingImage(ByVal rating As Integer) As String

            If _ratingImagePath <> String.Empty Then
                Return String.Format("<img src=""{0}Rating{1}.gif"">", Page.ResolveUrl(_ratingImagePath), rating)
            Else
                Return String.Format("<img src=""{0}Rating{1}.gif"">", Page.ResolveUrl(DefaultRatingImagePath), rating)
            End If
        End Function 'GetRatingImage 
    End Class 'Rating 
End Namespace