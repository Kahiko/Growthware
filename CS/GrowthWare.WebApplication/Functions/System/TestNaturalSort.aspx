<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestNaturalSort.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.TestNaturalSort" %>
<script type="text/javascript" language="javascript">
    var sortDirection = $('#dropSortDirection').val();
    var $mSortContent = $('#SortContent');
    var mRetHTML = "";

    function changeSort() {
        if (sortDirection == "ASC") {
            sortDirection = "DESC";
        } else {
            sortDirection = "ASC";
        }
        try {
            var options = GW.Model.DefaultWebMethodOptions();
            options.url = GW.Common.getBaseURL() + "/Functions/System/TestNaturalSort.aspx?SortDirection=" + sortDirection;
            options.dataType = 'html';
            GW.Common.JQueryHelper.callWeb(options, onSuccess, onError);
        } catch (e) {
           	mRetHTML = 'Error attempting to call logon\n' + e.message;
            $mSortContent.css({ display: 'none' });
            $mSortContent.html(mRetHTML.toString()).fadeIn(3000);
        }
        return true;
    }

    function onSuccess(htmlResponse) {
        $mSortContent.html(htmlResponse.toString());
    }

    function onError(xhr, status, error) {
        mRetHTML = 'Error getting content\n' + xhr.responseText;
        $mSortContent.css({ display: 'none' });
        $mSortContent.html(mRetHTML.toString()).fadeIn(3000);
    }        
</script>
<form id="TestNaturalSort" runat="server">
	<div id="SortContent" runat="server">
		Direction: 
		<select id="dropSortDirection" onchange="javascript:changeSort();" Class="rounded" runat="server">
			<option value="ASC">Ascending</option>
			<option value="DESC">Decending</option>
		</select>
		&nbsp;&nbsp;&nbsp;
		Start: <asp:Label ID="StartTime" runat="server"></asp:Label>
		Stop:  <asp:Label ID="StopTime" runat="server"></asp:Label>
		Total Time in Milliseconds:  <asp:Label ID="lblTotalTime" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
		<table border="0" cellpadding="0" cellspacing="3">
			<tr>
				<td>
					Natural Sort:<br />
					<asp:GridView ID="GridView2" runat="server" CellPadding="3" GridLines="Vertical" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">
						<FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
						<RowStyle BackColor="White" ForeColor="Black" Wrap="false" />
						<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
						<SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
						<HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
						<AlternatingRowStyle BackColor="#DCDCDC" Wrap="false" />
					</asp:GridView>
				</td>
				<td>
					DataView using .Sort = "COL1 ASC/DESC":<br />
					<asp:GridView ID="GridView3" runat="server" CellPadding="3" GridLines="Vertical" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">
						<FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
						<RowStyle BackColor="White" ForeColor="Black" />
						<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
						<SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
						<HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
						<AlternatingRowStyle BackColor="#DCDCDC" />
					</asp:GridView>
				</td>
			</tr>
			<tr>
				<td>
					Natural Sort:<br />
					<asp:DropDownList ID="DropDownList2" CssClass="rounded" runat="server"></asp:DropDownList>
				</td>
				<td>
					DataView using .Sort = "COL1 ASC":<br />
					<asp:DropDownList ID="DropDownList3" CssClass="rounded" runat="server"></asp:DropDownList>
				</td>
			</tr>
		</table>
	</div>
</form>
