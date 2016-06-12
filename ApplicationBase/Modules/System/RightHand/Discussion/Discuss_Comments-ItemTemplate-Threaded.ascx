<%@ Control %>
<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<li><CustomWebControls:ItemTitleLink id="lnkTitle" CssClass="Comments_ListTitleLink" Runat="Server" /> 
    by <CustomWebControls:ItemAuthor id="lnkAuthor" CssClass="Comments_ListAuthorLink" Runat="Server"/>
    on <CustomWebControls:ItemDateCreated DisplayTimeZone="true" CssClass="Comments_ListDateCreated" Runat="Server" />
<ul>
<CustomWebControls:ContentList RepeatLayout="Flow" id="ThreadedComments" Width="100%" Runat="Server"/>
</ul>
