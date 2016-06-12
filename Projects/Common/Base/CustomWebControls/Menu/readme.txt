skmMenu has been ported to VB

*******************
skmMenu Version 2.2 (Bug-Fixes and Changes)

Fixed:  Changed JavaScript to work with IE 5.0 (removed references to .splice() and .push())

Changed:	Added <backcolor>, <bordercolor>, and <borderwidth> elements to <menuItem> XML element
			Added <menuSpacer> element.  For details see:
				http://skmmenu.com/menu/Download/XMLStructure.html

Added:		Role-based permissions to MenuItems and Menu classes.
				More information at: http://skmmenu.com/menu/Download/documentation/skmMenu.Menu.UserRoles.html
				Live demo at: http://skmmenu.com/menu/Examples/Roles.aspx

Known Issues/Bugs:
		skmMenu does not display correctly across frames in a page.  That is, if skmMenu is in a top frame, the
		drop-down submenus are "hidden" by the lower frame.  For more info see:
		http://www.gotdotnet.com/Community/MessageBoard/Thread.aspx?id=190596


*******************
skmMenu Version 2.1 (Bug-Fixes and Changes)

Fixed:  Fixed SSL "issues" by adding IFrameSrc property to Menu class.  See:
		http://www.gotdotnet.com/Community/MessageBoard/Thread.aspx?id=188042
		
		Fixed client-side error problems due to not first checking that objects have been initialized.  See:
		http://www.gotdotnet.com/Community/MessageBoard/Thread.aspx?id=188870
		
		Fixed menuItems loosing WebControl-derived properties on postback when programmatically creating menu structure.
		See: http://www.gotdotnet.com/Community/MessageBoard/Thread.aspx?id=187557
		
		Fixed Bug 000010, see: http://www.gotdotnet.com/Community/Workspaces/BugDetails.aspx?bugid=8721
		

Changed:  Removed AccessKey property from MenuItem class.
		  Removed internal SetDirty() method (was not being called from any class in project)
		  Added top-level menu's client ID to the client-side subMenuIDs array

Known Issues/Bugs:
		skmMenu does not display correctly across frames in a page.  That is, if skmMenu is in a top frame, the
		drop-down submenus are "hidden" by the lower frame.  For more info see:
		http://www.gotdotnet.com/Community/MessageBoard/Thread.aspx?id=190596


*******************
skmMenu Version 2.0 (Bug-Fix Beta) Changes

Added:  Implemented an improved client-side skm_closeSubMenus() function.  (Thanks to Alan Downie.)
Fixed:  Fixed script rendering order problem first identified by Steve (spextreme).  For more info, see this post:
		http://www.gotdotnet.com/Community/MessageBoard/Thread.aspx?id=180999
Changed:  Further documentation added.

Known Issues/Bugs:

In IE, the iframe used to hide windowed controls interfers with the Opacity setting.

*******************
skmMenu Version 1.9c (Bug-Fix Beta) Changes

Added:  Additional AddSpacer call which accepts text to display in spacer.  This can make it more like a header.
Fixed:  Submenus shown in wrong place when menu is absolutely positioned or if menu is in a positioned element.  Fix is courtesy of Scott who reworked the positioning in the javascript file.
Changed:  Documentation added (chm file) courtesy of Scott.

Known Issues/Bugs:

In IE, the iframe used to hide windowed controls interfers with the Opacity setting.

*******************
skmMenu Version 1.9b (Bug-Fix Beta) Changes

Fixed:  Issue where submenu would show under (instead of on top) when adjusted to fit on screen.  This was caused by error in js code.
Added:  Spacers....Use SubItems.AddSpacer to add a spacer to a menu.  Parameters include Height and (optionally) CssClass for spacer.  Internally, spacers are simply menuitems with no text or image and (presumably) fixed height.  You can use the CssClass parameter to control what the spacer looks like.  After adding a spacer, you could further customize it if you wanted.  Example#1 now has a spacer in it.
Added:  Headers....Use SubItems.AddHeader to add a header to a menu.  Only parameter is text to display.  Implemented internally also as a menuitem.
Fixed:  ClickToOpen now works correctly...fix compliments of Didier (didier@ii5.net).  Thanks for your help in pinpointing the problem!!!
Added:  On MenuItems - Enabled and Visible properties now work as expected for individual menuitems.
Added:  AccessKey property is now output if set.
Fixed:  Found and corrected cause of the following issue: Setting CssClass of SelectedMenuItemStyle has no effect, I have tested it with both IE 6.0 and 5.0, neither works. 
Added:  To xml file support:  Accesskey, HorizontalAlign, VerticalAlign, Enabled, Visible, Width and Height.
Added:  LeftImage, LeftImageRightPadding, LeftImageAlign, RightImage, RightImageLeftPadding and RightImageAlign.  These allow you to place an image to the left or right of any text on the menu.  The implementation of this at current works great for images on the left...for images on the right it might be able to be improved in the future a bit.
Added:  ImageAltText - allows you to set the alternate text for the image for a menuitem.
Fixed:  Error in nav.xml file used for one of the examples.

Known Issues/Bugs:

No known issues at present.

*******************
skmMenu Version 1.8 (Bug-Fix Beta) Changes

Fixed:  MenuItems were not persisted in viewstate correctly.  On Postback, this resulted in either no/blank menu or duplicate menus if you rebound/recreated menu on postback.  Problem was that viewstate items were not set to 'dirty' when changed from default.  Added calls to SetDirty and now all menuitem properties are persisted correctly.
Changed:  All examples to no rebind/recreate menu on postback.  Also added a submit button on first two examples (for testing purposes mainly).

Known Issues/Bugs:

ClickToOpen works incorrectly.  Submenus disappear immediately as you move mouse over them.  Appears to be some type of javascript issue but I can't narrow it down and need someone's help to pinpoint.
Setting CssClass of SelectedMenuItemStyle has no effect, I have tested it with both IE 6.0 and 5.0, neither works. 

*******************
skmMenu Version 1.7 (Beta) Changes

Changed: AbsoluteURL renamed to DefaultResolveURL and now uses control.ResolveURL method.
Added:  ResolveURL property to menuitem class.  Whether the URL is resolved is determined by whether DefaultResolveURL (menu class) or ResolveURL (individual menuitem) is true.
Added:  HorizontalAlign and VerticalAlign to menuitem class.  Allows setting of alignment in menu item table cells.
Added:  Example #7 - Example showing how to make one menuitem different from others when using XML databinding.  (Same as #6 except XML)
Added:  SubMenuCssClass - Currently CssClass was used as style for table tags for both main menu and submenus.  Changed so CssClass is used for main menu and SubMenuCssClass is used for submenu tables.  If SubMenuCssClass is blank and CssClass is not, CssClass is used for submenu tables instead.

Known Issues/Bugs:

ClickToOpen works incorrectly.  Submenus disappear immediately as you move mouse over them.  Appears to be some type of javascript issue but I can't narrow it down and need someone's help to pinpoint.

*******************
skmMenu Version 1.6 (Beta) Changes

Changed: convert.todouble to use US Culture to prevent issues when number is formatted with commas instead of periods.
Added:  Positioning, Left and Top properties.  Used to position top menu.
Added:  Additional javascript to detect when submenu would open off screen to right and adjust it to left instead.  Tested in IE5+ and Mozilla.
Added:  AbsoluteURL property.  If true, all url's are resolved to full url before being output (i.e. http://blah.com/somewhere.html).
Added:  Optional name parameter to menuitems.  You may use it to access a menuitem after adding it...i.e. menu.Items["blah"] instead of menu.items[0].
Added:  6th Example showing how to make a menuitem different from others.


Known Issues/Bugs:

ClickToOpen works incorrectly.  Submenus disappear immediately as you move mouse over them.  Appears to be some type of javascript issue but I can't narrow it down and need someone's help to pinpoint.

*******************
skmMenu Version 1.5 (Beta) Changes

Changed: zindex default changed to 1000.
Added: target property to menuitems.  When target is specified, onclick handler will use window.open to open the link in targetted window/frame.
Added: DefaultTarget property to menuclass.  You can use this to avoid having to specify target for each menuitem.
Fixed: Issue "Setting CssClass of SelectedMenuItemStyle has no effect, I have tested it with both IE 6.0 and 5.0, neither works." is corrected although I would like to ask someone to validate the fix.
Fixed: Issue with viewstate.  Viewstate is now saved/restored properly.  (thanks Scott)
Fixed: Issue with swedish local where browser version will have comma instead of period to indicate decimal place.  Fixed by simply doing  replace(",",".").
Documentation Added:  Added XML comments in code.  Use NDoc to generate help file.

Known Issues/Bugs:

ClickToOpen works incorrectly.  Submenus disappear immediately as you move mouse over them.  Appears to be some type of javascript issue but I can't narrow it down and need someone's help to pinpoint.
Mozilla 1.4 - (maybe other versions?) - submenu does not appears in correct location - it is too high up.

*******************
skmMenu Version 1.4 (Beta) Changes

Images are now all preloaded.
MenuStyle has been removed.  Replaced by inherited webcontrol properties for Menu class.
Added ClickToOpen property.  When true, click menu to open submenu.  Submenus don't open at mouseover.
Added Cursor property.  Controls mouse cursor displayed when hovering over a menu item with a Command or URL associated with it.  Default or Hand are permitted.
Added MouseDownCssClass, MouseUpCssClass, MouseDownImage, MouseUpImage properties to menuitems.
Added DefaultMouseDownCssClass, DefaultMouseUpCssClass, DefaultMouseDownImage, DefaultMouseUpImage properties to menu.

Renamed - SelectedCssClass and DefaultSelectedCssClass have been renamed as MouseOverCssClass and DefaultMouseOverCssClass.
Renamed - SelectedImage has been renamed MouseOverImage.
Renamed - CellSpacing has been renamed ItemSpacing (to avoid reference to fact table is used internally)
Renamed - CellPadding has been renamed ItemPadding (to avoid reference to fact table is used internally)

Added - 2 more examples (#4 & #5)
Changed - First 3 examples to match format with what Scott had placed online.

Changed zIndex property defaults to 100 instead of non-specified.

Fixed Issue in IE 5.0 where iframe was in front of submenus.  In IE 5.0 you apparently can not z-index anything in front of an IFRAME.  This was fixed in IE 5.5.  Changed menu.cs to not output iframe code for IE 5.0.


Known Issues/Bugs:

ClickToOpen works incorrectly.  Submenus disappear immediately as you move mouse over them.  Appears to be some type of javascript issue but I can't narrow it down and need someone's help to pinpoint.

*******************
skmMenu Version 1.3 (Beta version) - Released 12/6/2003

Changes:

Both Menu and MenuItem inherit from WebControl

This adds the following properties to both:

Width
height
CssClass 
ToolTip
BorderWidth
BorderColor
BorderStyle
ForeColor
BackColor
Font


Menu Class Changes:

Additional Properties Added:

HighlightTopmenu - true/false boolean - true to leave top menu highlightede when submenu is displayed.  (javascript contribution by 'kcsgotdotnet').
ScriptPath - optional complete external script path and filename for javascript file.  If this path is specified in the web.config, it takes precedence over any specified using this property.  Otherwise this property may be used also.
CellSpacing (default of 0) - spacing for menu items (top level and submenus)
CellPadding (default of 2) - spacing for menu items (top level and submenus)
Gridlines (default of none)
DefaultItemCssClass - default value for menuitems if the menuitem doesn't specify a CssClass value.
DefaultItemSelectedCssClass - default value for menuitems if the menuitem doesn't specify a SelectedCssClass value.

Change in behavior of MenuFadeDelay - to allow more control, now indicates half seconds (instead of seconds).  Default is 4 (2 seconds - same as before).  This allows for greater control over the delay.


MenuItems Class Changes:

Additional Property Added:

SelectedCssClass - css to apply when item is selected.
Image - image to show instead of text
SelectedImage - image to show when menu is selected.  I.E. rollover image.  (These two enhancments come from Steven Perry.)

MenuStyle, SelectedMenuStyle and UnselectedMenuStyle are still supported.  Support for CssClass attribute has been added.  Note:  properties set individually on Menu or MenuItem will override those set by these styles.


Dynamically Built Menu Change:

ItemID is no longer passed when creating menu items.  Instead, the itemids (renamed as MenuID) are determined just before rendering in the CreateChildControls function.  This relieves the user of having to supply these for dynamic menus when they can instead be determined by walking the menuitem hierarchy.
Added several new versions of the add method to support new parameters.
Added validation to ensure that either text or image is passed when adding menu items.


XML File Support During Databind:

Added:

tooltip
cssclass
selectedcssclass


Javascript file changes:

Support for CSSClass in the styleinfo structure.
Removed code that changes mouse pointer.  This is better controlled by CSS (granted my opinion only)
In skm_mousedovermenu and skm_mousedoutmenu - added class parameter.  This is the class (selected or unselected) from the menuitem class to be applied (if any).  Overrides the selected and unselected menu styles if specified.
onclick for menuitems now includes call first to skm_closeSubMenus.  This closes the menus immediately when the click occurs.  Previously menu might remain displayed for several seconds if the page being move to is slow to come up.
URL supports custom javascript.  Prefix with "javascript:" and it will be output directly to onclick handler.
Changed line if(skm_subMenuIDs != undefined) to if(skm_subMenuIDs != "undefined") (added double quotes - suggestion by tonywang)
Added support in skm_mousedovermenu and skm_mousedoutmenu for image and rollover image.


Other Changes/Notes:

Built basic set of 3 examples (one contributed) and included test project in distribution.
Changed Render method to suppress the enclosing span tag that inheriting from webcontrol adds.
Fix for the issue where the menu appears behind select form elements.  For IE 5.0+, iframes are output in the html and then are used when the (sub)menus are shown and hidden.  The iframe floats above select elements but under the menu item thereby hiding the select elements.  Note in Mozilla 1.4 or better, this issue doesn't occur at all.  


