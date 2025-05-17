# Release Notes
## Net_Core Version x.x.x.x (yyyy-mm-dd)

### New Features
- Added Oracle support to the Database Manager

### Improvements
- Added removeProperty to the Oracle Database Manager to remove a property
## Net_Core Version 6.0.0.0 (2025-05-16)
### Deprecated Classes
- AbstractDatabaseFunctions
- AbstractBaseModel

### New Features
- Added Search DB Logs
    - Can download all of the systems logs including the datbase table
    - Can view the details of a given DB log entry

### Improvements
- Converted database calls to async/await

### Known Bugs
- There is an issue where the refresh token is being revoked when it shouldn't be, causing a logout to occure
- The Dynamic table causes the error - NG0956: The configured tracking expression (track by identity) caused re-creation of the entire collection of size 10.  The cause should be around line 121 where the tracy by is the entire row "track row;".  At this point I don't have a solution due to the nature of the dynamic table and the fact the names of the columns being returned are not consistent.  This should not present it self as a problem to the client because of the amount of data being returned, but I don't want to loose track of the issue.
- Manage Name/Value Pairs is not updating the search results after saving
- Need to have a behavior message on the "Edit Role" page indicating the effect of the "System ONLY" property
- Recieving error message when PWA Chrome starts: Could not read source map for chrome-error://chromewebdata/: Unexpected 503 response from chrome-error://chromewebdata/neterror.rollup.js.map: Unsupported protocol "chrome-error:"
- BAccounts is being created in two utilities (JwtUtility and AccountUtility) and should only be created in AccountUtility (AccountUtility is referenced in JwtUtility)
- GroupUtility is translating the return data and by design this is supposed to be done in the business logic layer
- When editting a role the members are not being always being populated
- Caching is not implemented correctly in the Message Utility

### Bug Fixes
- Fixed warning "Cannot convert null literal to non-nullable reference type." in DAccounts.cs
- feature(AccountUtilityTests) fixed the warning "Cannot convert null literal to non-nullable reference type." in AccountUtilityTests.cs by commenting out unused filed m_Origin.
- Fixed "When opening a modal for the second time a dropdown box will open under the .modal-background selector (z-index: 1000)"
- Fixed Edit DB Information not notifiing the user that the DB information was updated
- Fixed bug when editing a message for the first time where the security entity was not 1 the edit screen would be blank

## Net_Core Version 5.2.0.0 (2025-02-10)
- Upgraded to .Net Core 9.0
### Deprecated Classes
- AbstractDatabaseFunctions
- AbstractBaseModel

### New Features
- Added Feedback support
- Converted the Arc "skin" to standalone
- Converted the Blue Arrow "skin" to standalone
- Converted the Dashboard "skin" to standalone
- Lazy loading has been implemented properly and moved back to the gw-front-end project
    Added a readme.md to the gw-front-end/src/app/routes folder
- Forgot password's cancel now closes the modal and opens a new one for login
- Moved version number to GrowthWare.Framework.csproj
    - Update both launch.json and GrowthWare.Framework.csproj because launch.json still works for the DatabaseManager.
- Moved Table/MGroupRolePermissionSecurity.cs to Base/AGroupRolePermissionSecurity.cs
- Renamed UploadResponse.cs to DTO_UploadResponse.cs
- Moved MLogging.cs to Table/MLogging.cs
- Added the GWCommon class to the Growthware.Framework.csproj to help with common functions
- Moved the "Chunk Size" from the typescript to the configuration file and it is now retrieved from the API
- Updated the file manager service:
    - Now uses the chunk size from the configuration file
    - Now retries on unexpected errors for large files
    - Now notifies if coded data is incorrect to help with debugging and development
    - Optimized mergeFiles in the API
- Added Select/Unselect All, Delete Selected, Sorting and Filtering to the file-list.component and the table-file-list.component
- Added the following improvements over AbstractDatabaseFunctions:
	DataRowHelper to hold the Get functions that were originally in the AbstractDatabaseFunctions class
	IDatabaseTable and IAddedUpdated interfaces as a replacement for IDatabaseFunctions and IBaseModel respectfully
	ADatabaseTable as a replacement for AbstractDatabaseFunctions
	AAddedUpdated as a replacement for AbstractBaseModel
	MTestDatabaseTable to aid in testing ADatabaseTable and AAddedUpdated abstract classes
- Added ADatabaseTable and IDatabaseTable as a replacement for AbstractDatabaseFunctions and IDatabaseFunctions and adding the following changes:
    - Added the following methods:
        public static string GenerateDeleteUsingParameter
        public string GenerateDeleteUsingValues
        public string GenerateDeleteUsingValues

        public static string GenerateInsertUsingParameters
        public string GenerateInsertUsingValues

        public static string GenerateUpdateUsingParameters
        public string GenerateUpdateUsingValues
    - Added addtributes to the properties giving clairty to them
    - Converted:
        - MSecurityEntity                       to AAddedUpdated
        - MRegistrationInformation              to AAddedUpdated
        - MAccountProfile                       to AAddedUpdated
        - MRefreshToken                         to ADatabaseTable
        - MCalendarEvent                        to AAddedUpdated
        - MCalendar                             to AAddedUpdated
        - MDBInformation                        to AAddedUpdated
        - MDirectoryProfile                     to AAddedUpdated
        - MFeedback                             to ADatabaseTable (The Feedback is an "in-line" history table, Stored Procedures ONLY)
        - AbstractGroupRolePermissionSecurity   to AAddedUpdated
        - MFunctionTypeProfile                  to AAddedUpdated
        - MGroupProfile                         to AAddedUpdated
        - MGroupRoles                           to AAddedUpdated (Inserts fail no primary key defined)
        - MLoggingProfile                       to ADatabaseTable
        - MMessage                              to AAddedUpdated
        - MNameValuePair                        to AAddedUpdated
        - MRole                                 to AAddedUpdated
        - MState                                to AAddedUpdated
        - MNameValuePairDetails                 to AAddedUpdated
            - MLinkBehaviors    - Renamed SetupClass to setDefaults
            - MNavigationType   - Renamed SetupClass to setDefaults
            - MPermissions      - Renamed SetupClass to setDefaults
            - MWorkFlows        - Renamed SetupClass to setDefaults
- Added component for testing the Modal feature
- Added the Benchmark project and benchmark tests for in DAccounts for GetProfile and GetProfileAsync

### Improvements
- Optimized the CacheHelper it now use a per-file change token (isolated per cache entry)
- Moved files from assets folder to the public folder to better conform to Augular 18
- Enhanced logout functionality in LogoutComponent
    - Added LogoutComponent to handle user logout.
    - Integrated AccountService to call the logout method on initialization.
    - Updated the component's structure with necessary imports and lifecycle hooks
- Picklist: Converted from tables to div cleaned up stylesheet
- List: Converted from tables to div cleaned up stylesheet
- #### Modal:
    - The modal window is now centered vertically
    - Improved event listener management and resize functionality in the modal window
    - Added resizing to the modal window for the bottom side, right side, and the bottom-right corner
    - Added support for initial data for template modals
    - Enhance JSDoc documentation
- Rebuilt Web.Angular project from scratch using Angular 18.2.0
    - Fixes serveral hidden errors
    - Ensures that the project is compatible with Angular 18.2.0
    - Prepares the project for upgrade to Angular 19.x
- Replaced the Swagger UI logo
- Moved the swagger-ui folder from the assets folder to the public folder
- Addressed all ESLint issues
- Replaced logConsole method with LoggingService.console in the modal.service
- ObjectFactor.Create can now create an object using a constructor with or with out parameters.
- DAccounts Added a constructor that accepts connectionString and securityEntitySeqID so we don't have to set the properties in BAccounts.
    - BAccounts changed the constructor to pass the connectionString and securityEntitySeqID to the DAccounts constructor
    - BAccounts now accounts for CentralManagement
- AbstractDBInteraction Added Async methods to the base class
- DClientChoices Added a constructor that accepts connectionString so we don't have to set the properties in BClientChoices.
    - BClientChoices changed the constructor to pass the connectionString to the DClientChoices constructor
    - BClientChoices now accounts for CentralManagement
- DCommunityCalendar Added a constructor that accepts connectionString so we don't have to set the properties in BCommunityCalendar.
    - BCommunityCalendar changed the constructor to pass the connectionString and the securityEntitySeqID to the DCommunityCalendar constructor
    - BCommunityCalendar now accounts for CentralManagement
- DDBInformation Added a constructor that accepts connectionString so we don't have to set the properties in BDBInformation.
    - BDBInformation changed the constructor to pass the connectionString to the DDBInformation constructor
    - BDBInformation now accounts for CentralManagement
- DDirectories Added a constructor that accepts connectionString so we don't have to set the properties in BDirectories.
    - BDirectories changed the constructor to pass the connectionString and the securityEntitySeqID to the DDirectories constructor
    - BDirectories now accounts for CentralManagement
- DFeedbacks Added a constructor that accepts connectionString so we don't have to set the properties in BFeedbacks.
    - BFeedbacks changed the constructor to pass the connectionString to the DFeedbacks constructor
    - BFeedbacks now accounts for CentralManagement
- DFunctions Added a constructor that accepts connectionString so we don't have to set the properties in BFunctions.
    - BFunctions changed the constructor to pass the connectionString and the securityEntitySeqID to the DFunctions constructor
    - BFunctions now accounts for CentralManagement
- DGroups Added a constructor that accepts connectionString and the securityEntitySeqID so we don't have to set the properties in DGroups.
    - DGroups changed the constructor to pass the connectionString and the securityEntitySeqID to the DGroups constructor
    - DGroups now accounts for CentralManagement
- DLogging Added a constructor that accepts connectionString so we don't have to set the properties in DLogging.
    - BLogger changed the constructor to pass the connectionString to the DLogging constructor
    - BLogger now accounts for CentralManagement
- DMessages Added a constructor that accepts connectionString so we don't have to set the properties in DMessages.
    - BMessages changed the constructor to pass the connectionString to the DMessages constructor
    - BMessages now accounts for CentralManagement
- DNameValuePairs Added a constructor that accepts connectionString and the securityEntitySeqID so we don't have to set the properties in BNameValuePairs.
    - BNameValuePairs changed the constructor to pass the connectionString and the securityEntitySeqID to the DNameValuePairs constructor
    - BNameValuePairs now accounts for CentralManagement
- DRoles Added a constructor that accepts connectionString and the securityEntitySeqID so we don't have to set the properties in BRoles.
    - BRoles changed the constructor to pass the connectionString and the securityEntitySeqID to the DRoles constructor
    - BRoles now accounts for CentralManagement
- DSearch Added a constructor that accepts connectionString and the securityEntitySeqID so we don't have to set the properties in BSearch.
    - BSearch changed the constructor to pass the connectionString and the securityEntitySeqID to the DSearch constructor
    - BSearch now accounts for CentralManagement
- DSecurityEntities Added a constructor that accepts connectionString so we don't have to set the properties in BSecurityEntities.
    - BSecurityEntities changed the constructor to pass the connectionString to the DSecurityEntities constructor
    - BSecurityEntities now accounts for CentralManagement
- DState Added a constructor that accepts connectionString and the securityEntitySeqID so we don't have to set the properties in BStates.
    - BStates changed the constructor to pass the connectionString and the securityEntitySeqID to the DState constructor
    - BStates now accounts for CentralManagement

### Known Bugs
- There is an issue where the refresh token is being revoked when it shouldn't be causing a logout to occure
- The Dynamic table causes the error - NG0956: The configured tracking expression (track by identity) caused re-creation of the entire collection of size 10.  The cause should be around line 121 where the tracy by is the entire row "track row;".  At this point I don't have a solution due to the nature of the dynamic table and the fact the names of the columns being returned are not consistent.  This should not present it self as a problem to the client because of the amount of data being returned, but I don't want to loose track of the issue.
- Manage Name/Value Pairs is not updating the search results after saving
- Need to have a behavior message on the "Edit Role" page indicating the effect of the "System ONLY" property
- Recieving error message when PWA Chrome starts: Could not read source map for chrome-error://chromewebdata/: Unexpected 503 response from chrome-error://chromewebdata/neterror.rollup.js.map: Unsupported protocol "chrome-error:"
- BAccounts is being created in two utilities (JwtUtility and AccountUtility) and should only be created in AccountUtility (AccountUtility is referenced in JwtUtility)

### Bug Fixes
- Fixed NG0955 error in horizontal.component.html track by was by "action" truns out there can be a duplicate in the collection use case is where the "Favoriate Link" and the "Feedback Link" are the same.  I added the id to INavLink interface and now use that in the track by.
- Removed the ngModule from the gw-frontend/features/home
- Updated the /* Form */ section was missing formSectionContents for the Professional "Skin"
- Fixed not being able save the "IsSysAdmin" property in AccountDetailsComponent
- Fixed DAccounts not saving ResetTokenExpires and MiddleName correctly
- Fixed saving the directory profile where the ImpersonatePassword was not being handeled correctly.  If "Impersonate" is false then both the Password and ImpersonatePassword be saved as string.empty.  The password is no longer displayed in the UI as was desinged (leaving it blank with keep the same password)
- Fixed bug where you couldn't save a group without roles
- Cleaned up AbstractGroupController.SaveGroup(UIGroupProfile groupProfile)
- Fixed bug where cache was not being updated after saving a Security Entity
- Fixed MSecurityInfo it never checked the groups only the dervied roles
- Fixed error in block size when decrypting using both Des and TrippleDes
- Fixed Avoid inexact read with 'System.IO.FileStream.Read(byte[], int, int)'
- Fixed Upload was not setting startingByte and endingByte correctly in FileManagerService.uploadFile for the first call to multiPartFileUpload
- Fixed CacheHelper where the item was not being removed from the MemoryCache collection in the changeCallback method
- Fixed sorting in the TableFileListComponent, it was not sorting the size correctly
- Fixed multiple modal windows closing when using the ESC key
- Fixed Docker by hosting the both the Angular Material Icons and Fonts
- Fixed error when renaming a directory then clicking on subdirectories

## Net_Core Version 5.1.1.x (YYYY-MM-DD)

### New Features

### Known Bugs
- There is an issue where the refresh token is being revoked when it shouldn't be causing a logout to occure
- The Dynamic table causes the error - NG0956: The configured tracking expression (track by identity) caused re-creation of the entire collection of size 10.  The cause should be around line 121 where the tracy by is the entire row "track row;".  At this point I don't have a solution due to the nature of the dynamic table and the fact the names of the columns being returned are not consistent.  This should not present it self as a problem to the client because of the amount of data being returned, but I don't want to loose track of the issue.
- When adding a Name/Value Pair the search results are not being updated

### Bug Fixes
- Fixed lable type-o in the Event Details Component from "Angular forms" to "Event Date"
- Fixed Editing your account information in the Account Details Component
- Fixed Saving an account
    UI elements will now displaying correctly in the Account Details Component
    When the "action" is "accounts/edit-my-account"
        The "Status" field and "System Administrator" row is hidden
    The "System Administrator" and "Failed Logon Attempts" row will not display if the roles are not displayed
    "System Administrator" field is disabled if the client is not a system administrator
    - In the API call (SaveAccount)
        Will now save if the requesting account is the same as the account being saved
        FailedAttempts and Status can not be changed by the "same" account
- Fixed app.component.spec.ts to work correctly with signals
- Fixed ERROR: 'ERROR', TypeError: this._SecurityEntitySvc.getSecurityEntity(...).then is not a function in app.component.spec.ts
- Fixed File manager does not populate the file list upon initial load

## Net_Core Version 5.1.0.x (2024-10-24)

### New Features
- Added a "Page" for testing logging

### Improvements
- Updated the color scheme information in the ClientChoices to be more consistent and useful
- Added the ClientChoices feature in order to access the ClientChoices object without causing a circular dependency in the GroupDetailsComponent and RoleDetailsComponent.
- Can now change the event date in the Event Details Component
- Standarized form layout (height/width)

### Known Bugs
- There is an issue where the refresh token is being revoked when it shouldn't be causing a logout to occure
- The Dynamic table causes the error - NG0956: The configured tracking expression (track by identity) caused re-creation of the entire collection of size 10

### Bug Fixes
- Fixed where "Delete roles" pick list allItemsText was blank
- Set the color properties for the PickListComponent in AccountDetailsComponent, GroupDetailsComponent and RoleDetailsComponent.
- Fixed "API" being displayed in the "Favorite Link" drop down of Select Preferences.
- Fixed NG8109: securityEntityTranslation is a function in SecurityEntityDetailsComponent
- Fixed NavigationComponentBase was doubling the flyout navigation
- Fixed Setting @V_MessageSeqId in upgrade/downgrade Version_4_0_1_0.sql can return more than 1 entry in the sub query
- Fixed Log API call not getting the destination properties (was missing), the API now logs to both the database and the file.
- Fixed when the database has just been created and an account first logs on the authentication doesn't seem to get updated and you can not navigate to other links until you log out and back in
- Fixed after DB has been created and an account logs on the app, clicking on a guarded link will result in being logged out
- Added sort order to the Name Value Pair Details Component
-- Fixed NG0956: The configured tracking expression in navigation

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
