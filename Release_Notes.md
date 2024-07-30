# Release Notes

## Net_Core Version 4.1.0.x (2024-xx-xx)

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
