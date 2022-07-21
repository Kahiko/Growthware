
namespace GrowthWare.Framework.Models.UI;
public class UISearchCriteria
{
    public string[] searchColumns{ get; set; }
    public string[] sortColumnInfo { get; set; }
    public int pageSize { get; set; }
    public string searchText { get; set; }
    public int selectedPage { get; set; } 
}
