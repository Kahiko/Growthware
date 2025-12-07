namespace GrowthWare.Framework.Enumerations;

public enum AccountTables
{
    // The first table contains the account details and has 1 row
    AccountDetails = 0,
    // The second table contains the refresh tokens for the account
    RefreshTokens = 1,
    // The third table contains directly associated roles with the account
    AssignedRoles = 2,
    // The forth table contains directly associated groups with the account
    AssignedGroups = 3,
    // The fifth table contains roles associated with the account via both the directly assocated roles and roles associated with any groups
    DerivedRoles = 4,
}