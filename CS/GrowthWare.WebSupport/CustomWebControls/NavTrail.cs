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

namespace GrowthWare.WebSupport.CustomWebControls
{

	/// <summary>
	/// Class NavTrail
	/// </summary>
	[ControlBuilderAttribute(typeof(NavTrailControlBuilder)), ParseChildrenAttribute(false), Designer(typeof(CustomDesigner)), Bindable(true), Localizable(true), Category("Data"), DefaultValue(""), Description("Used to display a line type menu or a trail for navigation.")]
	public class NavTrail : WebControl, INamingContainer
	{

		private IEnumerable mDataSource;

		private Orientation m_Orentation = Orientation.Horizontal;

		private ArrayList mNavTrailTab = new ArrayList();
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
		/// Initializes a new instance of the <see cref="NavTrail" /> class.
		/// </summary>
		public NavTrail()
		{
		}

		/// <summary>
		/// Gets the data source
		/// </summary>
		/// <returns>IEnumerable</returns>
		/// <remarks></remarks>
		protected virtual IEnumerable GetDataSource()
		{
			if (mDataSource == null) {
				return null;
			}
			IEnumerable resolvedDataSource = null;
			resolvedDataSource = mDataSource;
			return resolvedDataSource;
		}

		/// <summary>
		/// Creates a navtrailtab for each item in a dataview
		/// </summary>
		/// <param name="useViewState"></param>
		/// <remarks>
		/// Please note that there must at least 4 data items and 
		/// data item 1 and 3 are used for text and URL respectively
		/// </remarks>
		protected virtual void CreateMyControlHeirarchy(bool useViewState)
		{
			IEnumerable resolvedDataSource = null;
			if (useViewState) {
				if ((this.ViewState["RowCount"] != null))
				{
					resolvedDataSource = new object[int.Parse(this.ViewState["RowCount"].ToString()) + 1];
				}
				else {
                    throw new CustomWebControlException("Unable to retrieve expected data from ViewState");
				}
			}
			else {
				resolvedDataSource = GetDataSource();
			}

			if ((resolvedDataSource != null)) {
				DataRowView dataItem = null;
				TableRow row = new TableRow();
				foreach (object dataItem_loopVariable in resolvedDataSource) {
					dataItem = (DataRowView)dataItem_loopVariable;
					NavTrailTab myNavTrailTab = new NavTrailTab();
					myNavTrailTab.Text = dataItem[1].ToString();
					myNavTrailTab.Action = dataItem[3].ToString();
					mNavTrailTab.Add(myNavTrailTab);
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
				CreateMyControlHeirarchy(useViewState);
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
			CreateMyControlHeirarchy(useViewState);
			ChildControlsCreated = true;
		}

		/// <summary>
		/// Only add NavTrailTab to the mNavTrailTab collection.
		/// </summary>
		/// <param name="obj"></param>
		/// <remarks></remarks>
		protected override void AddParsedSubObject(Object obj)
		{
			if (obj is NavTrailTab) {
				mNavTrailTab.Add(obj);
			}
		}
		
		/// <summary>
		/// Display Navigation Trail.
		/// </summary>
		/// <param name="writer"></param>
		/// <remarks></remarks>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			// Display the tabs
			int i = 0;
			for (i = 0; i <= mNavTrailTab.Count - 1; i++) {
				NavTrailTab objTab = (NavTrailTab)mNavTrailTab[i];
				HyperLink hyperLink = new HyperLink();
				hyperLink.RenderBeginTag(writer);
				//hyperLink.CssClass = "NavTrail";
				hyperLink.Text = objTab.Text;
                hyperLink.NavigateUrl = objTab.Action;
				hyperLink.ToolTip = objTab.ToolTip;
				hyperLink.RenderEndTag(writer);
				hyperLink.RenderControl(writer);

				if(m_Orentation == Orientation.Horizontal)
				{
					if(i < mNavTrailTab.Count - 1)
					{
						writer.Write("&nbsp;|&nbsp;");
					}
				}else
				{
					if(i < mNavTrailTab.Count - 1)
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
	/// Class NavTrailControlBuilder
	/// </summary>
	[Designer(typeof(CustomDesigner))]
	public class NavTrailControlBuilder : ControlBuilder
	{


		/// <summary>
		/// Gets the type of the child control.
		/// </summary>
		/// <param name="tagName">Name of the tag.</param>
		/// <param name="attributes">The attributes.</param>
		/// <returns>Type.</returns>
		public override Type GetChildControlType(string tagName, IDictionary attributes)
		{
			if (string.Compare(tagName, "NavTrailTab", true) == 0) {
				return typeof(NavTrailTab);
			}

			return null;
		}
	}
}
namespace GrowthWare.WebSupport.CustomWebControls
{

	/// <summary>
	/// Represents individual links in the NavTrail.
	/// </summary>
	[Designer(typeof(CustomDesigner))]
	public class NavTrailTab : Control
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

