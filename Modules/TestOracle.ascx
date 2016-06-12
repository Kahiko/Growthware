<%@ Control Language="vb" AutoEventWireup="false" Codebehind="TestOracle.ascx.vb" Inherits="BaseApplication.TestOracle" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>


<asp:DataGrid id="DataGrid1" runat="server" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical" AllowPaging="True">
<FooterStyle ForeColor="Black" BackColor="#CCCCCC">
</FooterStyle>

<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C">
</SelectedItemStyle>

<AlternatingItemStyle BackColor="#DCDCDC">
</AlternatingItemStyle>

<ItemStyle ForeColor="Black" BackColor="#EEEEEE">
</ItemStyle>

<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084">
</HeaderStyle>

<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages">
</PagerStyle></asp:DataGrid>
