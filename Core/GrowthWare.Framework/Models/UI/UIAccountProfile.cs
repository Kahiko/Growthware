namespace GrowthWare.Framework.Models.UI;
public class UIAccountProfile
{
    public string Account;
    public UIAccountGroups AccountGroups;
    public UIAccountRoles AccountRoles;
    public bool CanSaveRoles;
    public bool CanSaveGroups;
    public int Id;
    public bool EnableNotifications;
    public string EMail;
    public int Status;
    public string FirstName;
    public string MiddleName;
    public string LastName;
    public string PreferredName;
    public bool IsSystemAdmin;
    public int TimeZone;
    public string Location;
}