Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities

Public Class UpdateAnonymousCache
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CacheController.RemoveFromCache(AccountUtility.AnonymousAccountProfile)
        CacheController.RemoveFromCache("AnonymousClientChoicesState")
        Dim enumValues As Array = System.[Enum].GetValues(GetType(MenuType))
        For Each resource As MenuType In enumValues
            CacheController.RemoveFromCache(resource.ToString() + "Anonymous")
        Next
    End Sub

End Class