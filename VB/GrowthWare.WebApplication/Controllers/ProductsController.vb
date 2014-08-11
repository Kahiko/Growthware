Imports System.Net
Imports System.Web.Http

Namespace Controllers
    Public Class ProductsController
        Inherits ApiController

        Private products As Product() = New Product() _
        {
            New Product() With {.Id = 1, .Name = "Tomato Soup", .Category = "Groceries", .Price = 1},
            New Product() With {.Id = 2, .Name = "Yo-yo", .Category = "Toys", .Price = 3.75D},
            New Product() With {.Id = 3, .Name = "Hammer", .Category = "Hardware", .Price = 16.99D}
        }

        Public Function GetAllProducts() As IEnumerable(Of Product)
            Return products
        End Function

        Public Function GetProduct(id As Integer) As IHttpActionResult
            Dim product = products.FirstOrDefault(Function(p) p.Id = id)
            If product Is Nothing Then Return NotFound()
            Return Ok(product)
        End Function
    End Class
End Namespace