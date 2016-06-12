<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DiscussionDetails.ascx.vb" Inherits="ApplicationBase.DiscussionDetails" %>
<%@ Register Src="Comments_DisplayComments.ascx" TagName="Comments_DisplayComments" TagPrefix="Comments_DisplayComments" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>

<div align="right">
    <CustomWebControls:Notify Text="Notify me of new comments" runat="Server" />
</div>
<br />
<table width="100%" class="discussTable" cellpadding="0" cellspacing="1">
    <tr>
        <th width="100" align="left">Author</th>
        <th align="left">Thread:<CustomWebControls:Title runat="Server" /></th>
    </tr>
    <tr>
        <td valign="top" class="discussCell">
            <CustomWebControls:Author runat="Server" />
        </td>
        <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="discussCellHilite">
                        <b><CustomWebControls:Title runat="Server" /></b>
                        <br />
                        Posted:<CustomWebControls:DateCreated DateFormatString="{0:f}" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td class="discussCell">
                        <div align="right">
                            <CustomWebControls:DisplayTopic id="topicPreview" runat="Server"/>
                        </div>
                        <CustomWebControls:PostBodyText runat="Server" />
                        <p>
                            <CustomWebControls:DiscussCommentReply ReplyUrl="Discuss_ReplyPost.aspx?id={0}" ImageUrl="~/Images/Discuss/Reply.gif" runat="Server" />
                            <br />
                            <CustomWebControls:Rating runat="Server" />
                        </p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<Comments_DisplayComments:Comments_DisplayComments ID="Comments_DisplayComments1" runat="server" />

<p>
<CustomWebControls:DiscussEditContent EditText="Edit this Post" DeleteText="Delete this Post" Runat="Server" />
</p>

