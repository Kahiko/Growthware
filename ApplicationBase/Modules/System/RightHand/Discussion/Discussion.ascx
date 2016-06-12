<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.Discussion" Codebehind="Discussion.ascx.vb" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<div align="right">
<CustomWebControls:Notify Text="Notify me of new comments" runat="Server" />
</div>
<p>
<CustomWebControls:AddContent Text="New Thread" ImageUrl="~/Images/Discuss/NewThread.gif" NavigateUrl="Discuss_AddPost.aspx" Runat="Server"/>
</p>

<CustomWebControls:ContentList id="ContentList" width="100%" runat="Server">
    <HeaderTemplate>
        <table width="100%" class="discussTable" cellspacing="1">
            <tr>
                <th colspan="2" nowrap="nowrap">Thread</th>    
                <th nowrap="nowrap">Started By</th>
                <th nowrap="nowrap">Replies</th>
                <th nowrap="nowrap">Views</th>
                <th nowrap="nowrap">Last Post</th>
            </tr>
    </HeaderTemplate>
    
    <ItemTemplate>
    <tr>
        <td class="discussCell"><CustomWebControls:ItemDiscussIcon PopularCount="40" runat="Server"/></td>
        <td class="discussCell"><CustomWebControls:ItemTitleLink runat="Server"/></td>
        <td class="discussCellHiLite"><CustomWebControls:ItemAuthor runat="Server"/></td>
        <td class="discussCellHiLite"><CustomWebControls:ItemCommentCount runat="Server"/></td>
        <td class="discussCellHiLite"><CustomWebControls:ItemViewCount runat="Server"/></td>
        <td class="discussCellHiLite" align="center">
            <CustomWebControls:ItemDiscussPinnedPost text="Pinned Post" runat="Server" />
            <CustomWebControls:ItemDiscussDateCommented dateformatstring="{0:g}" DisplayTimeZone="true" runat="Server"/>
            <br />by <CustomWebControls:ItemDiscussLastCommentUsername runat="Server" />
        </td>
    </tr>
    </ItemTemplate>
    
    <FooterTemplate>
    </table>
    </FooterTemplate>
    
    <NoContentTemplate>
        There are currently no posts.
    </NoContentTemplate>
</CustomWebControls:ContentList>
