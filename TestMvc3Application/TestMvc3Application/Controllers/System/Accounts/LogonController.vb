Namespace TestMvc3Application
    Public Class LogonController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Logon

        Function Index() As ActionResult
            Return View()
        End Function

        '
        ' GET: /Logon/Details/5

        Function Details(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        '
        ' GET: /Logon/Create

        Function Create() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Logon/Create

        <HttpPost> _
        Function Create(ByVal collection As FormCollection) As ActionResult
            Try
                ' TODO: Add insert logic here
                Return RedirectToAction("Index")
            Catch
                Return View()
            End Try
        End Function
        
        '
        ' GET: /Logon/Edit/5

        Function Edit(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        '
        ' POST: /Logon/Edit/5

        <HttpPost> _
        Function Edit(ByVal id As Integer, ByVal collection As FormCollection) As ActionResult
            Try
                ' TODO: Add update logic here

                Return RedirectToAction("Index")
            Catch
                Return View()
            End Try
        End Function

        '
        ' GET: /Logon/Delete/5

        Function Delete(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        '
        ' POST: /Logon/Delete/5

        <HttpPost> _
        Function Delete(ByVal id As Integer, ByVal collection As FormCollection) As ActionResult
            Try
                ' TODO: Add delete logic here

                Return RedirectToAction("Index")
            Catch
                Return View()
            End Try
        End Function     
    End Class
End Namespace
