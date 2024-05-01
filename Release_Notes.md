# Release Notes

## Version 4.0.1 (2024-XX-XX)

### New Features
- Implemented Detail view in File Manager

### Improvements
- Added .devcontainer and docker-compose.yml the containers:
    From "Net_Core" run docker-compose up --build -d
    1.) Build
    2.) Run
    3.) Can be "Reopened in container"
    TODO: Need to get this working together

### Bug Fixes
- Fixed Version_0.0.0.0.sql causing error when creating the DB on a non windows machine

## Version 4.0.0 (2024-04-30)

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
