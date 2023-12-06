namespace GrowthWare.Framework.Models.UI;

public class UIGroupProfile
{
    public UIGroupProfile()
    {
        RolesInGroup = new string[]{};
        RolesNotInGroup = new string[]{};
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string[] RolesInGroup { get; set; }

    public string[] RolesNotInGroup { get; set; }
}