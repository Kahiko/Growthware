# Release Notes
## Net_Core Version 5.1.0.x (yyyy-mm-dd)

### New Features
- Added a "Page" for testing logging

### Improvements
- Updated the color scheme information in the ClientChoices to be more consistent and useful
- Added the ClientChoices feature in order to access the ClientChoices object without causing a circular dependency in the GroupDetailsComponent and RoleDetailsComponent.
- Can now change the event date in the Event Details Component

### Known Bugs
- There is an issue where the refresh token is being revoked when it shouldn't be causing a logout to occure

### Bug Fixes
- Fixed where "Delete roles" pick list allItemsText was blank
- Set the color properties for the PickListComponent in AccountDetailsComponent, GroupDetailsComponent and RoleDetailsComponent.
- Fixed "API" being displayed in the "Favorite Link" drop down of Select Preferences.
- Fixed NG8109: securityEntityTranslation is a function in SecurityEntityDetailsComponent
- Fixed NavigationComponentBase was doubling the flyout navigation
- Fixed Setting @V_MessageSeqId in upgrade/downgrade Version_4_0_1_0.sql can return more than 1 entry in the sub query

## Net_Core Version 5.0.1.x (2024-09-26)

### New Features
- No new features added

### Improvements
- Changed SQL Server database manager to remove the database property from the connection in a more robust way
- Updated Angular to 18.2
- Updated the Docker files to use Angular 18.2
- appInitializer now navigates to the clientChoices.action
- Added iconName property to SnakeListComponent
- Re-worked AccountService so that afterAuthentication is now called after authenticate, changePassword, logout, refreshToken, resetPassword, saveClientChoices and verifyAccount
- Changed all gw- tages to self closing tags
- Updated all @Input/@Output types to input/output in prep for being able to drop Zone.js
- Re-Worked the navagation feature to reduce the number of components needed
- The Toast feature now works on signals only
- The Dynamic Table now uses ClientChoices.subHeadColor to set the header font color
- Added color properties to ListComponent
- Added color properties to PickListComponent

### Bug Fixes
- Fixed NG0956: The configured tracking expression (track by identity) in file-list.component.html
- Fixed Pager not displaying the correct number of pages
- Fixed the "derived" roles not being displayed/populated
- Fixed the "Blue Arrow" skin not displaying Envirnment correctly
- Fixed Logoff so that it wouldn't through an error and sends back the Anonumous Profile

- Fixed roles and groups not being saved in AccountDetailsComponent
- Fixed roles and groups not being saved in FunctionDetailsComponent
- Fixed roles not being saved in GroupDetailsComponent
- Fixed Members not being saved in RoleDetailsComponent

## Net_Core Version 5.0.0.x (2024-08-31)

### New Features
- No new features added

### Improvements
- Connection strings are now handled in DDatabaseManager
- AbstractDBInteraction.BulkInsert now uses a DTO for the parameter making it generic
- Vertical menu items now hightlight when clicked on
- Select Preferences Component html builds from typescript data
- Dynamic Table now uses ClientChoices colors
- Added dividers to the Horizontal Component
- Added fontColor to the VerticalComponent
- Added anchor styles to dev-ops-layout.component.scss

### Bug Fixes
- Fixed type-o in Change Password
- Fixed "Old Password" not showing in Change Password when it should
- Prevent "update" link from being displayed, not needed
- Fixed when using mat-drawer to using mat-sidenav, though functionally the same the two could change in the future and Angular recommends to use mat-sidenav instead
- Fixed being able to select multiple radio buttons in Select Preferences Component
- Fixed Account Details Component showing the "System Administrator" and "Failed Logon Attempts" when it should not be displayed
- Fixed Hierarchical Vertical Flyout not working
- Fixed fore color not being set in the HorizontalComponent
- Fixed GrowthWare.DatabaseManager - Changed if conditions to hand the most common condition first

### Notes:
- Removed any code pertaining to Oracle in the Database Manager, this is being moved to it's own development branch due to it's size and complexity

## Net_Core Version 4.1.0.x (2024-08-09)

### Bug Fixes
- Fixed Copy Function always showing "source can not be equal to the target"

## Net_Core Version 4.1.0.x (2024-08-09)

### New Features
- Added Register
    - Registration information is now stored in the database for each security entity
- Added Verify Email

### Improvements
- Popup modals are now draggable
- Normalize some of the CSS
- Added baseURLWithoutPort to GWCommon.ts
- Added GetProfileByURL to SecurityEntityUtility and AbstractSecurityEntityController
- Updated ClientChoicesUtility.CurrentState and ClientChoicesUtility.getClientChoicesState to account for ConfigSettings.SecurityEntityFromUrl
- Updated SecurityEntityUtility.CurrentProfile to account for ConfigSettings.SecurityEntityFromUrl
- Renamed CacheController.cs to CachenHelper.cs and moved to \Helpers
- Renamed SessionController.cs to SessionHelper.cs and moved to \Helpers
- MMessage now uses ExcludedTags so as not to show some properties returned in method GetTags
- AbstractDBInteraction now logs exceptions to the log file
- Moved check for ConfigSettings.CentralManagement from the BusinessLogic project to the Web.Support project, this reduces the number of times the business logic class is created resulting in better performance
- Implemented Edit DB Information (Useful for the purpose of testing)
- Added copy text to clipboard in GUID Helper
- Added Icon to file list to help with a visual separation of the items

### Bug Fixes
- Fixed the namespace in the WebApi project controllers from GrowthWare.Web.Angular.Controllers to GrowthWare.Web.Api.Controllers
- Fixed Roles and Groups being returned from search by adding to the were clause to account for the selected Security Entity
- Fixed deleteOldLogs causing exception file not found on "Linux"
- Fixed Dockerfile not being able to run
- Modal drag and drop now works only with the header
- Fixed non "isAdmin" accounts could not log in, MSecurityInfo was not setting properties correctly
- Fixed Error when searching for groups or roles (incorrect column name)
- Fixed AbstractAccountController.EditAccount incorrectly throwing a 401 error
- Fixed PagerComponent not displaying the correct number of pages
- Fixed Where DAccount did not account for Middle_Name or Preferred_Name were throwing an error for not passing the associated SQL parameters
- Added missing fields Parent, Skin, Encryption Type and Status to the UI
- Fixed Fixed Height/Width of the modal for ManageNameValuePairsComponent
- Fixed Set Log Level UI
- Fixed Random Numbers UI
- Fixed Line Count UI
- Fixed GUID Helper UI
- Fixed Encrypt/Decrypt Component UI
- Fixed Change Password UI
- Fixed Select Preferences UI
- Fixed Copy Function Security UI
- Removed update-anonymous-profile from the DB and accounts-routing.module.ts
- Fixed when the refresh token is not found in the DB and the UI looks like an account is logged in

## Net_Core Version 4.0.1 (2024-06-19)

### New Features
- Added Table View in File Manager
- Fully implemented "Forgot Password"

### Improvements
- Added Docker directory with all of the necessary files to create a Development environment for Growthware in a Docker container
- Changed DDatabaseManager to update the Security Entities connection string for each row
- Changed DDatabaseManager to update the Acounts password string for each row
- Added connection string examples to sql-server\Dockerfile
- Added logging logic to AbstractDBInteraction.GetDataSet

### Bug Fixes
- Fixed Version_0.0.0.0.sql causing error when creating the DB on a non windows machine
- Fixed Status not updating when you are "forced" to change your password
- Fixed not navigating to the "favoriate" action after logon
- Fixed being logged out when saving a function
- Fixed [ZGWSecurity].[Get_Function_Sort] where nothing is returned due to the ParentSeqId <> 1 predicate, this should have been ParentSeqId = @V_Parent_ID
- Fixed [ZGWSecurity].[Accounts].[ResetToken] Size (no need for MAX now set to 256)
- Fixed Change password not setting focus correctly
- Fixed Change password styling where error message was not displayed correctly
- Fixed Cannot match any routes. URL Segment: 'favorite' during logon
- Fixed DatabaseManager where you could not specifity the USE [YourDatabaseName]; in the upgrade/downgrade scripts
- Fixed old password not being hidden in ChangePasswordComponent

## Net_Core Version 4.0.0 (2024-04-30)

### New Features
- None

### Improvements
- Split Web.Angular into Web.Api and Web.Angular.  Moving all .Net code/items out of Web.Angular and into Web.Api
- Authenticate/Logoff now returns both the authentication response and the client choices for improved performance.
- Improved AbstractAccountController.ipAddress will use headers if the IP Address is not using the first method

### Bug Fixes
- Fixed AuthorizeAttribute.OnAuthorization from getting a null result when getting MAccountProfile
- Fixed the default-header.component displaying the Authenticated drop down when it shouldn't
- Fixed explicit-any in default-header.component.ts
- Fixed "Favorite" menu item not updating when selecting a new "Favorite Link:" in "Select Preferences"
- Fixed Line Count updated the directory to D:/Development/Growthware/Net_Core
