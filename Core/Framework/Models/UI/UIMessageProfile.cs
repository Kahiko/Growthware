using System.Globalization;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models.UI;

public class UIMessageProfile
{
    public UIMessageProfile(){}

    public UIMessageProfile(MMessage message)
    {
        this.Body = message.Body;
        this.Description = message.Description;
        this.FormatAsHtml = message.FormatAsHtml;
        this.Id = message.Id;
        this.Name = message.Name;
        this.Title = message.Title;
    }

    public string AvalibleTags {get; set;}
    public string Body {get; set;}
    public string Description {get; set;}
    public bool FormatAsHtml {get; set;}
    public int Id {get; set;}
    public string Name {get; set;}
    public string Title {get; set;}
}