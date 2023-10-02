using System;
using System.Data;

namespace GrowthWare.Framework.Models.UI;

public class UIValidSecurityEntity
{
    public UIValidSecurityEntity() { }

    public UIValidSecurityEntity(DataRow dataRowView)
    {
        if(dataRowView["SecurityEntityID"] != DBNull.Value)
        {
            this.Id = int.Parse(dataRowView["SecurityEntityID"].ToString());
        }
        if(dataRowView["Name"] != DBNull.Value)
        {
            this.Name = dataRowView["Name"].ToString();
        }
        if(dataRowView["Description"] != DBNull.Value) 
        {
            this.Description = dataRowView["Description"].ToString();
        }
        
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}