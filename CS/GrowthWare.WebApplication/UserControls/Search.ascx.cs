using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.UserControls
{
    public partial class Search : System.Web.UI.UserControl
    {
        /// <summary>
        /// Returns the ClientChoices state from context.
        /// </summary>
        public MClientChoicesState ClientChoicesState
        {
            get { return (MClientChoicesState)Context.Items[MClientChoices.SessionName]; }
        }

        private bool m_ShowAddLink = true;
        private bool m_ShowSelect = false;
        private bool m_ShowDeleteAll = false;
        private bool m_ShowRefresh = false;

        public string SearchURL { get; set; }

        public bool ShowAddLink
        {
            get { return m_ShowAddLink; }
            set { m_ShowAddLink = value; }
        }

        public bool ShowSelect
        {
            get { return m_ShowSelect; }
            set { m_ShowSelect = value; }
        }

        public bool ShowDeleteAll
        {
            get { return m_ShowDeleteAll; }
            set { m_ShowDeleteAll = value; }
        }

        public bool ShowRefresh
        {
            get { return m_ShowRefresh; }
            set { m_ShowRefresh = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!m_ShowDeleteAll) imgDeleteAll.Style.Add("display", "none");
            if (!m_ShowSelect) cmdSelect.Style.Add("display", "none");
            addNew.Visible = m_ShowAddLink;
            btnRefesh.Visible = m_ShowRefresh;
            txtRecordsPerPage.Value = ClientChoicesState[MClientChoices.RecordsPerPage];
            imgDeleteAll.Src = GWWebHelper.RootSite + ConfigSettings.AppName + "Public/GrowthWare/Images/delete_red.png";
        }
    }
}