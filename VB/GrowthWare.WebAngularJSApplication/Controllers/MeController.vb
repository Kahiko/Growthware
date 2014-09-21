Imports System.Security.Claims
Imports System.Threading.Tasks
Imports System.Web
Imports System.Web.Http
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

<Authorize>
Public Class MeController
    Inherits ApiController

    Private _userManager As ApplicationUserManager

    Public Sub New()
    End Sub

    Public Sub New(userManager As ApplicationUserManager)
        Me.UserManager = userManager
    End Sub
    
    Public Property UserManager() As ApplicationUserManager
      Get
          Return If(_userManager, HttpContext.Current.GetOwinContext().GetUserManager(Of ApplicationUserManager)())
      End Get
      Private Set(value As ApplicationUserManager)
          _userManager = value
      End Set
    End Property

    ' GET api/Me
    Public Function [Get]() As GetViewModel
        Dim userInfo As ApplicationUser = UserManager.FindById(User.Identity.GetUserId()) 
        Return New GetViewModel() With { .Hometown = userInfo.Hometown }
    End Function
End Class
