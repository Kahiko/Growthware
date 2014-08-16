using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GrowthWare.WebSupport.Context
{
    /// <summary>
    /// MFunctionProfile contains properties from the ZF_FUNCTIONS table.
    /// </summary>
    /// <remarks></remarks>
    [Serializable(), CLSCompliant(true)]
    public class MFunctionProfile : MBaseSecurity, IMProfile
    {
        private string m_Action = string.Empty;
        private string m_Description = string.Empty;
        private bool m_Enable_View_State = false;
        private bool m_Enable_Notifications = false;
        private bool m_Is_Nav = false;
        private bool m_No_UI = false;
        private string m_Function_Name = string.Empty;
        private int m_Nav_Type_Seq_ID = 2;
        private string m_Notes = string.Empty;
        private int m_Function_Type_Seq_ID = -1;
        private int m_ParentmFunction_Seq_ID = 1;
        private string m_Source = string.Empty;
        private string m_Transformations = string.Empty;
        private bool m_Redirect_On_Timeout = true;
        private string m_MetaKeyWords;

        /// <summary>
        /// Will return a Function profile with the default vaules
        /// </summary>
        /// <remarks></remarks>

        public MFunctionProfile()
        {
        }

        /// <summary>
        /// Will return a fully populated Function profile.
        /// </summary>
        /// <param name="drowProfile">A data row containing the Function information</param>
        /// <param name="drowSecurity">A data row containing all of the roles</param>
        /// <remarks></remarks>
        public MFunctionProfile(DataRow drowProfile, DataRow[] drowSecurity)
        {
            Init(drowProfile, drowSecurity);
        }

        private void Init(DataRow drowMain, DataRow[] drowSecurity)
        {
            base.NameColumnName = "ACTION";
            base.IdColumnName = "FUNCTION_SEQ_ID";
            base.Init(drowMain, drowSecurity);
            base.Id = base.GetInt(drowMain, "FUNCTION_SEQ_ID");
            m_Function_Type_Seq_ID = base.GetInt(drowMain, "FUNCTION_TYPE_SEQ_ID");
            m_Function_Name = base.GetString(drowMain, "NAME");
            m_Description = base.GetString(drowMain, "DESCRIPTION");
            m_Notes = base.GetString(drowMain, "NOTES");
            m_Source = base.GetString(drowMain, "SOURCE");
            m_Enable_View_State = base.GetBool(drowMain, "ENABLE_VIEW_STATE");
            m_Enable_Notifications = base.GetBool(drowMain, "ENABLE_NOTIFICATIONS");
            m_Redirect_On_Timeout = base.GetBool(drowMain, "REDIRECT_ON_TIMEOUT");
            m_Is_Nav = base.GetBool(drowMain, "IS_NAV");
            m_No_UI = base.GetBool(drowMain, "No_UI");
            m_Nav_Type_Seq_ID = base.GetInt(drowMain, "NAVIGATION_NVP_SEQ_DET_ID");
            m_ParentmFunction_Seq_ID = base.GetInt(drowMain, "PARENT_Function_Seq_ID");
            m_Action = base.GetString(drowMain, "ACTION");
            // need to set the the base class name with the action.
            // the names can repeate but the action is unique and lower case.
            base.Name = m_Action.ToString();
            m_MetaKeyWords = base.GetString(drowMain, "META_KEY_WORDS");
        }

        public string Action
        {
            get { return m_Action; }
            set
            {
                m_Action = value.Trim();
                base.Name = m_Action.ToString();
            }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value.Trim(); }
        }

        public bool EnableViewState
        {
            get { return m_Enable_View_State; }
            set { m_Enable_View_State = value; }
        }

        public bool EnableNotifications
        {
            get { return m_Enable_Notifications; }
            set { m_Enable_Notifications = value; }
        }

        public bool IS_NAV
        {
            get { return m_Is_Nav; }
            set { m_Is_Nav = value; }
        }

        public int Function_Type_Seq_ID
        {
            get { return m_Function_Type_Seq_ID; }
            set { m_Function_Type_Seq_ID = value; }
        }

        public string MetaKeyWords
        {
            get { return m_MetaKeyWords; }
            set { m_MetaKeyWords = value.Trim(); }
        }

        public new string Name
        {
            get { return m_Function_Name; }
            set { m_Function_Name = value.Trim(); }
        }

        public int NAV_TYPE_SEQ_ID
        {
            get { return m_Nav_Type_Seq_ID; }
            set { m_Nav_Type_Seq_ID = value; }
        }

        public string Notes
        {
            get { return m_Notes; }
            set { m_Notes = value.Trim(); }
        }

        public bool No_UI
        {
            get { return m_No_UI; }
            set { m_No_UI = value; }
        }

        public int ParentID
        {
            get { return m_ParentmFunction_Seq_ID; }
            set { m_ParentmFunction_Seq_ID = value; }
        }

        public bool RedirectOnTimeout
        {
            get { return m_Redirect_On_Timeout; }
            set { m_Redirect_On_Timeout = value; }
        }

        public string Source
        {
            get { return m_Source; }
            set { m_Source = value.Trim(); }
        }
    }
}
