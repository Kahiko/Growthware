<%@ Control %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<tr>
    <td valign="top" class="discussCell">
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
        </td>
    </tr>
    </table>
    </td>
</tr>
