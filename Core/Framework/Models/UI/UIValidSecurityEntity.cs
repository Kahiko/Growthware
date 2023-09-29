using System.Data;

namespace GrowthWare.Framework.Models.UI;

public class UIValidSecurityEntity
{
    public UIValidSecurityEntity() { }

    public UIValidSecurityEntity(DataRowView dataRowView)
    {
        if(dataRowView["SecurityEntitySeqId"] != null)
        {
            this.Id = int.Parse(dataRowView["SecurityEntitySeqId"].ToString());
        }
        if(dataRowView["Name"] != null)
        {
            this.Name = dataRowView["Name"].ToString();
        }
        if(dataRowView["Description"] != null) 
        {
            this.Description = dataRowView["Description"].ToString();
        }
        
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}