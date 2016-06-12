<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Discussion.ascx.vb" Inherits="BaseApplication.Discussion" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
        <td class="discussCell">
			&nbsp;
        </td>
        <td class="discussCell">
			&nbsp;
		</td>
        <td class="discussCellHiLite">&nbsp;</td>
        <td class="discussCellHiLite">&nbsp;</td>
        <td class="discussCellHiLite">&nbsp;</td>
        <td class="discussCellHiLite" align="center">
            <br />by 
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