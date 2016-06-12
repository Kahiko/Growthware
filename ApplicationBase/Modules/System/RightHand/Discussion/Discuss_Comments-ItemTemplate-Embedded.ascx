<%@ Control %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<p>
<table width="100%" class="discussTable" cellpadding="0" cellspacing="1">
<tr>
    <td valign="top" width="100" class="discussCell">
    <CustomWebControls:ItemAuthor runat="Server" />
    </td>
    <td>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="discussCellHilite">
        <b><CustomWebControls:ItemTitle runat="Server"/></b>
        <br>
        Posted: <CustomWebControls:ItemDateCreated DateFormatString="{0:f}" DisplayTimeZone="true" runat="Server"/>
        </td>
    </tr>
    <tr>
        <td class="discussCell">
        <CustomWebControls:ItemCommentText runat="Server" />
        <p>
        <CustomWebControls:ItemCommentReply ReplyUrl="Discuss_ReplyComment.aspx?id={0}" ImageUrl="~/Images/Discuss/Reply.gif" runat="Server" />
        </p>
        <CustomWebControls:ItemCommentRating id="ctlRateAction" SubmitText="Rate this Comment" Runat="Server" />

            <table cellpadding="0" width="100%" border="0" cellspacing="0">
            <tr>
                <td width="20"></td>
                <td>
                <CustomWebControls:ContentList repeatLayout="flow" id="NestedComments" Runat="Server"/>
                </td>
            </tr>
            </table>        
        </td>
    </tr>
    </table>
    </td>
</tr>
</table>
</p>
