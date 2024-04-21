using System;
using System.Collections.Generic;
using GrowthWare.Framework.Enumerations;

/// Note this matches typescript object 
/// @Growthware/features/navigation/nav-link.model.ts
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
        LinkBehaviors LinkBehavior,
        string linkText,
        bool isRouterLink = true,
        string styleClass = "",
        string routerLinkActive = ""
    ){
        this.Children = new List<MNavLink>();
        this.Icon = icon;
        this.IsRouterLink = isRouterLink;
        this.Link = link;
        this.LinkBehavior = (int)LinkBehavior;
        this.LinkText = linkText;
        this.RouterLinkActive = routerLinkActive;
    }

    public bool Disabled { get; set; }
    public string Icon { get; set; }
    public string Link { get; set; }
    public int LinkBehavior { get; set; }
    public string LinkText { get; set; }
    public bool IsRouterLink { get; set; }
    public string StyleClass { get; set; }
    public string RouterLinkActive { get; set; }
    public List<MNavLink> Children { get; set; }
}