namespace GrowthWare.Framework.Models.UI;

public class UIRole
{
    public UIRole(MRole roleProfile)
    {
        this.Description = roleProfile.Description;
        this.Id = roleProfile.Id;
        this.IsSystem = roleProfile.IsSystem;
        this.IsSystemOnly = roleProfile.IsSystemOnly;
        this.Name = roleProfile.Name;
        AccountsInRole = new string[]{};
        AccountsNotInRole = new string[]{};
    }

    public UIRole()
    {
        AccountsInRole = new string[]{};
        AccountsNotInRole = new string[]{};
    }

    public string[] AccountsInRole { get; set; }

    public string[] AccountsNotInRole { get; set; }

    public int Id { get; set; }

    public bool IsSystem { get; set; }

    public bool IsSystemOnly { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}