<%@ Control %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<table width="100%" cellspacing=0 cellpadding=5 class="Comments_ListTable">
<tr>
    <td class="Comments_ListItem">
        <CustomWebControls:ItemTitle id="lblTitle" CssClass="Comments_ListTitle" Runat="Server" />
        <br>By
        <CustomWebControls:ItemAuthor id="lnkAuthor" CssClass="Comments_ListAuthorLink" Runat="Server"/>
        on
        <CustomWebControls:ItemDateCreated CssClass="Comments_ListDateCreated" Runat="Server"/>
   </td>
</tr>
<tr>
    <td class="Comments_ListItem2">
        <CustomWebControls:ItemCommentText id="lblCommentText" CssClass="Comments_ListBriefDescription" Runat="Server" />            
        <p>
        <CustomWebControls:ItemCommentReply id="lnkCommentReply" Text="Reply to this Comment" FormatText="({0})" CssClass="Comments_ListLinkReply" Runat="Server"/>
        
        <CustomWebControls:ItemCommentRating id="ctlRateAction" SubmitText="Rate this Comment" Runat="Server" />
        
        <table cellpadding="5" width="100%">
        <tr>
            <td width="20"></td>
            <td>
            <CustomWebControls:ContentList id="NestedComments" Runat="Server"/>
            </td>
        </tr>
        </table>
    </td>
</tr>
</table>
