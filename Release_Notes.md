# Release Notes

## Net_Core Version 4.0.1 (2024-XX-XX)

### New Features
- Added Table View in File Manager
- Fully implemented "Forgot Password"

### Improvements
- Added Docker containers via .devcontainer directory with Dockerfiles' and a docker-compose.yml:
    From "Net_Core" directory execute "run docker-compose up --build -d" to
    build and start the containers.  The containers currectly do the following:
        1.) Build
        2.) Run
        3.) Can be "Reopened in container"
        4.) Breakpoints work
- Changed DDatabaseManager to update the Security Entities connection string for each row
- Changed DDatabaseManager to update the Acounts password string for each row
- Added connection string examples to sql-server\Dockerfile

### Bug Fixes
- Fixed Version_0.0.0.0.sql causing error when creating the DB on a non windows machine
- Fixed Status not updating when you are "forced" to change your password
- Fixed not navigating to the "favoriate" action after logon
- Fixed being logged out when saving a function
- Fixed [ZGWSecurity].[Get_Function_Sort] where nothing is returned due to the ParentSeqId <> 1 predicate, this should have been ParentSeqId = @V_Parent_ID
- Fixed [ZGWSecurity].[Accounts].[ResetToken] Size (no need for MAX now set to 256)
- Fixed Change password not setting focus correctly
- Fixed Change password styling where error message was not displayed correctly

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
