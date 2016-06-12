@Section SPAViews
    @Html.Partial("_Home")
End Section
@Section Scripts
    @Scripts.Render("~/bundles/knockout")
    @Scripts.Render("~/bundles/app")
End Section