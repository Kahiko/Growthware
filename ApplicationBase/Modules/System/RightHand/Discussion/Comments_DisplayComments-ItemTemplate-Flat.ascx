<%@ Control %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<table width="100%" cellspacing=0 cellpadding=5 class="Comments_ListTable">
<tr>
    <td class="Comments_ListItem">
        <CustomWebControls:ItemTitle id="lblTitle" CssClass="Comments_ListTitle" Runat="Server" />
        <br>By <CustomWebControls:ItemAuthor id="lnkAuthor" CssClass="Comments_ListAuthorLink" Runat="Server"/>
        on 
        <CustomWebControls:ItemDateCreated id="lblDateCreated" CssClass="Comments_ListDateCreated" Runat="Server"/>
            
    </td>
</tr>
<tr>
    <td class="Comments_ListItem2">
        <CustomWebControls:ItemCommentText id="lblCommentText" CssClass="Comments_ListBriefDescription" Runat="Server" />            
        <p>
        <CustomWebControls:ItemCommentReply id="lnkCommentReply" CssClass="Comments_ListReplyLink" Text="Reply to this Comment" FormatText="({0})" Runat="Server"/>
        <CustomWebControls:ItemCommentRating id="ctlRateAction" SubmitText="Rate this Comment" Runat="Server" />
    </td>
</tr>
</table>
