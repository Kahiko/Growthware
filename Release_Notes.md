# Release Notes

## Version 4.0.0 (2024-04-21)

### New Features
- ???

### Improvements
- Split Web.Angular into Web.Api and Web.Angular, removed all .Net code/items
- Authenticate/Logoff now returns both the authentication response and the client choices for improved performance.

### Bug Fixes
- Fixed AuthorizeAttribute.OnAuthorization from getting a null result when getting MAccountProfile

