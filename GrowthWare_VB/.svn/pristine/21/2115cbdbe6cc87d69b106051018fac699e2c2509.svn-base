@ModelType IEnumerable(Of GrowthWare.Framework.Model.Profiles.MFunctionProfile)

@Code
    ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table>
    <tr>
        <th></th>
        <th>
            Action
        </th>
        <th>
            Description
        </th>
        <th>
            EnableViewState
        </th>
        <th>
            EnableNotifications
        </th>
        <th>
            IsNav
        </th>
        <th>
            LinkBehavior
        </th>
        <th>
            FunctionTypeSeqID
        </th>
        <th>
            MetaKeyWords
        </th>
        <th>
            Name
        </th>
        <th>
            NavigationTypeSeqId
        </th>
        <th>
            Notes
        </th>
        <th>
            No_UI
        </th>
        <th>
            ParentID
        </th>
        <th>
            RedirectOnTimeout
        </th>
        <th>
            Source
        </th>
        <th>
            IdColumnName
        </th>
        <th>
            PermissionColumn
        </th>
        <th>
            RoleColumn
        </th>
        <th>
            AddedBy
        </th>
        <th>
            AddedDate
        </th>
        <th>
            UpdatedBy
        </th>
        <th>
            UpdatedDate
        </th>
    </tr>

@For Each item In Model
    Dim currentItem = item
    @<tr>
        <td nowrap="nowrap">
            @Html.ActionLink("Edit", "Edit", New With {.id = currentItem.Id}) |
            @Html.ActionLink("Details", "Details", New With {.id = currentItem.Id}) |
            @Html.ActionLink("Delete", "Delete", New With {.id = currentItem.Id})
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Action)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Description)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.EnableViewState)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.EnableNotifications)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.IsNav)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.LinkBehavior)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.FunctionTypeSeqID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.MetaKeyWords)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Name)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.NavigationTypeSeqId)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Notes)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.No_UI)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.ParentID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.RedirectOnTimeout)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Source)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.IdColumnName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.PermissionColumn)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.RoleColumn)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.AddedBy)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.AddedDate)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.UpdatedBy)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.UpdatedDate)
        </td>
    </tr>
Next

</table>
