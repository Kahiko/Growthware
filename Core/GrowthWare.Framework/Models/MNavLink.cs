using System;
using System.Collections.Generic;

/// Note this matches typescript object 
/// @Growthware/Lib/src/lib/features/navigation/nav-link.model.ts
/// any changes needed here will also need to be made there

namespace GrowthWare.Framework.Models;

[Serializable(), CLSCompliant(true)]
public class MNavLink
{

    public MNavLink()
    {
        this.Children = new List<MNavLink>();
    }

    public MNavLink(
        string icon, 
        string link, 
        string linkText,
        bool isRouterLink = true,
        string styleClass = "",
        string routerLinkActive = ""
    ){
        this.Children = new List<MNavLink>();
        this.Icon = icon;
        this.IsRouterLink = isRouterLink;
        this.Link = link;
        this.LinkText = linkText;
        this.RouterLinkActive = routerLinkActive;
    }

    public bool Disabled { get; set; }
    public string Icon { get; set; }
    public string Link { get; set; }
    public string LinkText { get; set; }
    public bool IsRouterLink { get; set; }
    public string StyleClass { get; set; }
    public string RouterLinkActive { get; set; }
    public List<MNavLink> Children { get; set; }
}