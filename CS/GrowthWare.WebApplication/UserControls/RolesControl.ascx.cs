using System;
using System.Collections;
using System.Web.UI;

namespace GrowthWare.WebApplication.UserControls
{
    public partial class RolesControl : UserControl
    {
        #region Public Properties
        public ArrayList AllRoles;
        public Array SelectedViewRoles;
        public Array SelectedAddRoles;
        public Array SelectedEditRoles;
        public Array SelectedDeleteRoles;
        public bool ViewRolesChanged
        {
            get { return ctlViewRoles.Changed; }
        }
        public string ViewRoles
        {
            get { return ctlViewRoles.SelectedState; }
        }
        public string AddRoles
        {
            get { return ctlAddRoles.SelectedState; }
        }
        public bool AddRolesChanged
        {
            get { return ctlAddRoles.Changed; }
        }
        public string EditRoles
        {
            get { return ctlEditRoles.SelectedState; }
        }
        public bool EditRolesChanged
        {
            get { return ctlEditRoles.Changed; }
        }
        public string DeleteRoles
        {
            get { return ctlDeleteRoles.SelectedState; }
        }
        public bool DeleteRolesChanged
        {
            get { return ctlDeleteRoles.Changed; }
        }
        #endregion

        #region Page Event Handlers
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ctlViewRoles.DataSource = AllRoles;
            ctlAddRoles.DataSource = AllRoles;
            ctlEditRoles.DataSource = AllRoles;
            ctlDeleteRoles.DataSource = AllRoles;
            if ((SelectedViewRoles != null)) ctlViewRoles.SelectedItems = (string[])SelectedViewRoles;
            if ((SelectedAddRoles != null)) ctlAddRoles.SelectedItems = (string[])SelectedAddRoles;
            if ((SelectedEditRoles != null)) ctlEditRoles.SelectedItems = (string[])SelectedEditRoles;
            if ((SelectedDeleteRoles != null)) ctlDeleteRoles.SelectedItems = (string[])SelectedDeleteRoles;
            DataBind();
        }
        #endregion
    }
}