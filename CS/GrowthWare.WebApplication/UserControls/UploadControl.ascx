<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadControl.ascx.cs" Inherits="GrowthWare.WebApplication.UserControls.UploadControl" %>
<script type="text/javascript" language="javascript">
	function initUpload() {
		GW.Upload.init();	
	}
</script>
<div id="divFrame" style="width: 286px; height: 32px;"></div>
<div id="divUploadMessage" style="padding-top: 4px; display: none;"></div>
<div id="divUploadProgress" style="padding-top: 4px; height: 20px; display: none">
	<span style="font-size: smaller">Uploading...</span>
	<div>
		<table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
			<tbody>
				<tr>
					<td id="tdProgress1">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress2">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress3">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress4">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress5">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress6">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress7">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress8">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress9">
						&nbsp; &nbsp;
					</td>
					<td id="tdProgress10">
						&nbsp; &nbsp;
					</td>
				</tr>
			</tbody>
		</table>
	</div>
</div>