using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Configuration
{
    public partial class AddEditWebConfig : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            populatePage();
        }

        private void populatePage()
        {
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            string[] currentWebEnvironments = ConfigSettings.Environments.Split(',');
            string environment = string.Empty;
            dropEnvironments.Items.Clear();
            dropEnvironments.Items.Add(new ListItem("New", "New"));
            foreach (string environment_loopVariable in currentWebEnvironments)
            {
                environment = environment_loopVariable;
                ListItem myListItem = new ListItem(environment, environment);
                dropEnvironments.Items.Add(myListItem);
            }
            txtEnvironments.Text = ConfigSettings.Environments;
        }
    }
}