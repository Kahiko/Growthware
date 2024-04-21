using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.AnonymousAccount
{
    public partial class UpdateAnonymousCache : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CacheController.RemoveFromCache(AccountUtility.AnonymousAccountProfile);
            CacheController.RemoveFromCache("AnonymousClientChoicesState");

            Array enumValues = Enum.GetValues(typeof(MenuType));

            foreach (MenuType resource in enumValues)
            {
                CacheController.RemoveFromCache(resource.ToString() + "Anonymous");
            }
        }
    }
}