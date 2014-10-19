using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GrowthWare.WebSupport.CustomWebControls.Designers;
using System.Data;
using System.Globalization;

namespace GrowthWare.WebSupport.CustomWebControls
{

	/// <summary>
	/// Class NavigationTrail
	/// </summary>
	[ControlBuilderAttribute(typeof(NavigationTrailControlBuilder)), ParseChildrenAttribute(false), Designer(typeof(CustomDesigner)), Bindable(true), Localizable(true), Category("Data"), DefaultValue(""), Description("Used to display a line type menu or a trail for navigation.")]
	public class NavigationTrail : WebControl, INamingContainer
	{

		private IEnumerable mDataSource;

		private Orientation m_Orentation = Orientation.Horizontal;

		private ArrayList mNavigationTrailTab = new ArrayList();
		/// <summary>
		/// DataSource is the source of data used with the bind
		/// </summary>
		/// <value></value>
		/// <returns>IEnumerable</returns>
		/// <remarks></remarks>
		public virtual IEnumerable DataSource {
			get { return mDataSource; }
			set {
				if (value is IEnumerable || value == null) {
					mDataSource = value;
				}
				else {
                    throw new ArgumentNullException("value", "value can not be null (Nothing in VB)!");
				}
			}
		}

		/// <summary>
		/// Valid settings are Horizontal or Vertical
		/// </summary>
		/// <value>Horizontal or Vertical</value>
		/// <returns>String</returns>
		public virtual Orientation Orientation
		{
			get { return m_Orentation; }

			set { m_Orentation = value; }
		}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get {
				string s = (string)this.ViewState["Text"];
				if (s == null) {
					return string.Empty;
				}
				else {
					return s;
				}
			}

			set { this.ViewState["Text"] = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationTrail" /> class.
		/// </summary>
		public NavigationTrail()
		{
		}

		/// <summary>
		/// Creates a NavigationTrailtab for each item in a dataview
		/// </summary>
		/// <param name="useViewState"></param>
		/// <remarks>
		/// Please note that there must at least 4 data items and 
		/// data item 1 and 3 are used for text and URL respectively
		/// </remarks>
        protected virtual void CreateMyControlHierarchy(bool useViewState)
		{
			IEnumerable resolvedDataSource = null;
			if (useViewState) {
				if ((this.ViewState["RowCount"] != null))
				{
                    resolvedDataSource = new object[int.Parse(this.ViewState["RowCount"].ToString(), CultureInfo.InvariantCulture) + 1];
				}
				else {
                    throw new CustomWebControlException("Unable to retrieve expected data from View State");
				}
			}
			else {
				resolvedDataSource = DataSource;
			}

			if ((resolvedDataSource != null)) {
                foreach (DataRowView dataItem in resolvedDataSource)
                {
                    using (NavigationTrailTab myNavigationTrailTab = new NavigationTrailTab()) 
                    { 
					    myNavigationTrailTab.Text = dataItem[1].ToString();
					    myNavigationTrailTab.Action = dataItem[3].ToString();
					    mNavigationTrailTab.Add(myNavigationTrailTab);
                    }
				}
			}
		}

		/// <summary>
		/// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			Controls.Clear();

			if ((this.ViewState["RowCount"] != null))
			{
				bool useViewState = true;
                CreateMyControlHierarchy(useViewState);
			}

		}

		/// <summary>
		/// Binds a data source to the invoked server control and all its child controls.
		/// </summary>
		public override void DataBind()
		{
			base.OnDataBinding(EventArgs.Empty);
			Controls.Clear();
			ClearChildViewState();
			TrackViewState();
			bool useViewState = false;
            CreateMyControlHierarchy(useViewState);
			ChildControlsCreated = true;
		}

		/// <summary>
		/// Only add NavigationTrailTab to the mNavigationTrailTab collection.
		/// </summary>
		/// <param name="obj"></param>
		/// <remarks></remarks>
		protected override void AddParsedSubObject(Object obj)
		{
			if (obj is NavigationTrailTab) {
				mNavigationTrailTab.Add(obj);
			}
		}
		
		/// <summary>
		/// Display Navigation Trail.
		/// </summary>
		/// <param name="writer"></param>
		/// <remarks></remarks>
		protected override void RenderContents(HtmlTextWriter writer)
		{
            if (writer == null) throw new ArgumentNullException("writer", "writer cannot be a null reference (Nothing in Visual Basic)");
			// Display the tabs
			int i = 0;
			for (i = 0; i <= mNavigationTrailTab.Count - 1; i++) {
				NavigationTrailTab objTab = (NavigationTrailTab)mNavigationTrailTab[i];
                using (HyperLink hyperLink = new HyperLink()) 
                {
                    hyperLink.RenderBeginTag(writer);
                    //hyperLink.CssClass = "NavigationTrail";
                    hyperLink.Text = objTab.Text;
                    hyperLink.NavigateUrl = objTab.Action;
                    hyperLink.ToolTip = objTab.ToolTip;
                    hyperLink.RenderEndTag(writer);
                    hyperLink.RenderControl(writer);
                }

				if(m_Orentation == Orientation.Horizontal)
				{
					if(i < mNavigationTrailTab.Count - 1)
					{
						writer.Write("&nbsp;|&nbsp;");
					}
				}else
				{
					if(i < mNavigationTrailTab.Count - 1)
					{
						writer.Write("<br/>");
					}
				}
			}
		}
	}
}
namespace GrowthWare.WebSupport.CustomWebControls
{

	/// <summary>
	/// Class NavigationTrailControlBuilder
	/// </summary>
	[Designer(typeof(CustomDesigner))]
	public class NavigationTrailControlBuilder : ControlBuilder
	{


		/// <summary>
		/// Gets the type of the child control.
		/// </summary>
		/// <param name="tagName">Name of the tag.</param>
        /// <param name="attribs">The attributes.</param>
		/// <returns>Type.</returns>
        public override Type GetChildControlType(string tagName, IDictionary attribs)
		{
            if (attribs == null) throw new ArgumentNullException("attribs", "attribs cannot be a null reference (Nothing in Visual Basic)");
            if (string.Compare(tagName, "NavigationTrailTab", StringComparison.OrdinalIgnoreCase) == 0)
            {
				return typeof(NavigationTrailTab);
			}

			return null;
		}
	}
}
namespace GrowthWare.WebSupport.CustomWebControls
{

	/// <summary>
	/// Represents individual links in the NavigationTrail.
	/// </summary>
	[Designer(typeof(CustomDesigner))]
	public class NavigationTrailTab : Control
	{

		private string _text;

		private string _action;

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get { return _text; }
			set { _text = value; }
		}

		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		/// <value>The action.</value>
		public string Action {
			get { return _action; }
			set { _action = value; }
		}

		/// <summary>
		/// Gets or sets the tool tip.
		/// </summary>
		/// <value>The tool tip.</value>
		public string ToolTip{set; get;}
	}
    /// <summary>
    /// Enum orientation
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Represents the horizontal orientation
        /// </summary>
        Horizontal = 0,

        /// <summary>
        /// Represents the vertical orientation
        /// </summary>
        Vertical = 1,
    }
}



