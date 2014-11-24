using System;
using System.Collections;
using System.Web.UI;

namespace GrowthWare.WebApplication.UserControls
{
    public partial class GroupControl : UserControl
    {
        #region Pulbic Properties
        public ArrayList AllGroups;
        public Array SelectedViewGroups;
        public Array SelectedAddGroups;
        public Array SelectedEditGroups;
        public Array SelectedDeleteGroups;
        public string ViewGroups
        {
            get { return ctlViewGroups.SelectedState; }
        }
        public bool ViewGroupsChanged
        {
            get { return ctlViewGroups.Changed; }
        }
        public string AddGroups
        {
            get { return ctlAddGroups.SelectedState; }
        }
        public bool AddGroupsChanged
        {
            get { return ctlAddGroups.Changed; }
        }
        public string EditGroups
        {
            get { return ctlEditGroups.SelectedState; }
        }
        public bool EditGroupsChanged
        {
            get { return ctlEditGroups.Changed; }
        }
        public string DeleteGroups
        {
            get { return ctlDeleteGroups.SelectedState; }
        }
        public bool DeleteGroupsChanged
        {
            get { return ctlDeleteGroups.Changed; }
        }
        #endregion

        #region Page Event Handlers
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ctlViewGroups.DataSource = AllGroups;
            ctlAddGroups.DataSource = AllGroups;
            ctlEditGroups.DataSource = AllGroups;
            ctlDeleteGroups.DataSource = AllGroups;
            if ((SelectedViewGroups != null)) ctlViewGroups.SelectedItems = (string[])SelectedViewGroups;
            if ((SelectedAddGroups != null)) ctlAddGroups.SelectedItems = (string[])SelectedAddGroups;
            if ((SelectedEditGroups != null)) ctlEditGroups.SelectedItems = (string[])SelectedEditGroups;
            if ((SelectedDeleteGroups != null)) ctlDeleteGroups.SelectedItems = (string[])SelectedDeleteGroups;
            DataBind();
        }
        #endregion
    }
}