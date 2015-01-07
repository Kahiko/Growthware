using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;

namespace GrowthWare.WebApplication.Functions.System.ExternalAuth
{
    public partial class UpdateSession : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountUtility.RemoveInMemoryInformation(true);
        }
    }
}