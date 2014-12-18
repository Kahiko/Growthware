using GrowthWare.WebSupport.BasePages;
using GrowthWare.WebSupport.Utilities;
using System;

namespace GrowthWare.WebApplication.Functions.System.Accounts
{
    public partial class UpdateSession : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountUtility.RemoveInMemoryInformation(true);
        }
    }
}