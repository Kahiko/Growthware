<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Comments_DisplayComments.ascx.vb" Inherits="ApplicationBase.Comments_DisplayComments" %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<asp:Panel id="pnlComments" Runat="Server">
  <table width="100%" cellspacing="0" cellpadding="0">
      <tr>
        <td><span class="Content_SubTitleRow">Comments:</span></td>
        <td align="right">
        <asp:DropDownList id="CommentView" AutoPostBack="true" Runat="Server">
            <asp:ListItem Text="Flat" value="0"/>
            <asp:ListItem Text="Nested" value="1"/>
            <asp:ListItem Text="Threaded" value="2"/>
            <asp:ListItem Text="Embedded" value="3"/>
        </asp:DropDownList>
        </td>
        <td align="right">
        <asp:DropDownlist id="OrderBy" Runat="Server">
            <asp:ListItem Text="Oldest First" value="0"/>
            <asp:ListItem Text="Newest First" value="1"/>
        </asp:DropDownlist>
        </td>
      </tr>
  </table>
  <br />
  <CustomWebControls:ContentList id="CommentList" Runat="Server"/>
</asp:Panel>