namespace GrowthWare.Framework.Models.UI;

public class UIGroupProfile
{
    public UIGroupProfile()
    {
        Id = -1;
        RolesInGroup = new string[]{};
        RolesNotInGroup = new string[]{};
    }

    public UIGroupProfile(MGroupProfile groupProfile)
    {
        Description = groupProfile.Description;
        Id = groupProfile.Id;
        Name = groupProfile.Name;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string[] RolesInGroup { get; set; }

    public string[] RolesNotInGroup { get; set; }
}