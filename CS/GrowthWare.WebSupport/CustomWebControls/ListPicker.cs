using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GrowthWare.WebSupport.CustomWebContswitchList
rols.Designers;

namespace GrowthWare.WebSupport.CustomWebControls
{
    /// <summary>
    /// Class ListPicker
    /// </summary>
    [DefaultProperty("Text"), Designer(typeof(CustomDesigner)), ToolboxData("<{0}:ListPicker runat=server></{0}:ListPicker>")]
    public class ListPicker : WebControl, IPostBackDataHandler, INamingContainer
    {

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [Bindable(true), Category("Appearance"), DefaultValue(""), Localizable(true)]
        public string Text
        {
            get
            {
                string s = (string)ViewState["Text"];
                if (s == null)
                {
                    return string.Empty;
                }
                else
                {
                    return s;
                }
            }

            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// Occurs when [list changed].
        /// </summary>
        public event EventHandler ListChanged;

        /// <summary>
        /// 
        /// </summary>
        private ArrayList m_AllItems = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        private ArrayList m_SelectedItems = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        private IEnumerable m_DataSource = null;
        /// <summary>
        /// 
        /// </summary>
        private string m_DataField = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private string m_SelectButtonText = " -> ";
        /// <summary>
        /// 
        /// </summary>
        private string m_SelectAllButtonText = " ->> ";
        /// <summary>
        /// 
        /// </summary>
        private string m_DeSelectButtonText = " <- ";
        /// <summary>
        /// 
        /// </summary>
        private string m_DeSelectAllButtonText = " <<- ";
        /// <summary>
        /// 
        /// </summary>
        private string m_ButtonWidth = "50px";

        // The following controls are used by downlevel browsers 
        /// <summary>
        /// 
        /// </summary>
        private ListBox lstAllItems = new ListBox();
        /// <summary>
        /// 
        /// </summary>
        private ListBox lstSelectedItems = new ListBox();
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ListPicker" /> is changed.
        /// </summary>
        /// <value><c>true</c> if changed; otherwise, <c>false</c>.</value>
        public bool Changed
        {
            get
            {
                if (ViewState["Changed"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["Changed"];
                }
            }
            set { ViewState["Changed"] = value; }
        }

        /// <summary>
        /// Gets or sets the selected items text.
        /// </summary>
        /// <value>The selected items text.</value>
        public string SelectedItemsText
        {
            get
            {
                if (ViewState["SelectedItemsText"] == null)
                {
                    return "Selected Items";
                }
                else
                {
                    return (string)ViewState["SelectedItemsText"];
                }
            }
            set { ViewState["SelectedItemsText"] = value; }
        }

        /// <summary>
        /// Gets or sets all items text.
        /// </summary>
        /// <value>All items text.</value>
        public string AllItemsText
        {
            get
            {
                if (ViewState["AllItemsText"] == null)
                {
                    return "All Items";
                }
                else
                {
                    return (string)ViewState["AllItemsText"];
                }
            }
            set { ViewState["AllItemsText"] = value.Trim(); }
        }

        /// <summary>
        /// Used to determine if the destination will be sorted
        /// when a selection is made.
        /// </summary>
        public bool SortOnChange
        {
            get
            {
                if (ViewState["SortOnChange"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)ViewState["SortOnChange"];
                }
            }
            set { ViewState["SortOnChange"] = value; }
        }

        /// <summary>
        /// Used to determine if the sort up and down
        /// buttons will appear for the destination
        /// </summary>
        public bool DestinationSortable
        {
            get
            {
                if (ViewState["DestinationSortable"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["DestinationSortable"];
                }
            }
            set { ViewState["DestinationSortable"] = value; }
        }

        /// <summary>
        /// Gets or sets the select button text.
        /// </summary>
        /// <value>The select button text.</value>
        public string SelectButtonText
        {
            get { return m_SelectButtonText; }

            set { m_SelectButtonText = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the select all button text.
        /// </summary>
        /// <value>The select all button text.</value>
        public string SelectAllButtonText
        {
            get { return m_SelectAllButtonText; }

            set { m_SelectAllButtonText = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets the de select button text.
        /// </summary>
        /// <value>The de select button text.</value>
        public string DeSelectButtonText
        {
            get { return m_DeSelectButtonText; }

            set { m_DeSelectButtonText = value; }
        }

        /// <summary>
        /// Gets or sets the de select all button text.
        /// </summary>
        /// <value>The de select all button text.</value>
        public string DeSelectAllButtonText
        {
            get { return m_DeSelectAllButtonText; }

            set { m_DeSelectAllButtonText = value; }
        }

        /// <summary>
        /// Gets or sets the width of the button.
        /// </summary>
        /// <value>The width of the button.</value>
        public string ButtonWidth
        {
            get { return m_ButtonWidth; }
            set
            {
                if (value.Trim().EndsWith("px", StringComparison.OrdinalIgnoreCase))
                {
                    m_ButtonWidth = value.Trim();
                }
                else
                {
                    m_ButtonWidth = value.Trim() + "px";
                }
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size
        {
            get
            {
                if (ViewState["Size"] == null)
                {
                    return 200;
                }
                else
                {
                    return int.Parse(ViewState["Size"].ToString());
                }
            }
            set { ViewState["Size"] = value; }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows
        {
            get
            {
                if (ViewState["Rows"] == null)
                {
                    return 6;
                }
                else
                {
                    return int.Parse(ViewState["Rows"].ToString());
                }
            }
            set { ViewState["Rows"] = value; }
        }

        /// <summary>
        /// Gets or sets the state of the selected.
        /// </summary>
        /// <value>The state of the selected.</value>
        public string SelectedState
        {
            get
            {
                if (ViewState["selectedState"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ViewState["selectedState"];
                }
            }
            set { ViewState["selectedState"] = value; }
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public IEnumerable DataSource
        {
            get { return m_DataSource; }
            set { m_DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the data field.
        /// </summary>
        /// <value>The data field.</value>
        public string DataField
        {
            get { return m_DataField; }
            set { m_DataField = value; }
        }

        /// <summary>
        /// Gets the selected helper ID.
        /// </summary>
        /// <value>The selected helper ID.</value>
        protected string SelectedHelperId
        {
            get { return ClientID + "_SelectedState"; }
        }

        /// <summary>
        /// Gets all helper ID.
        /// </summary>
        /// <value>All helper ID.</value>
        protected string AllHelperId
        {
            get { return ClientID + "_AllState"; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if ((Page != null))
            {
                base.OnInit(e);
                Page.RegisterRequiresPostBack(this);
                string scriptUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "GrowthWare.CustomWebControls.JS.ListPicker.js");
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "GrowthWareListPicker", scriptUrl);
            }
        }

        /// <summary>
        /// Loads the post data.
        /// </summary>
        /// <param name="postDataKey">The post data key.</param>
        /// <param name="values">The values.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection values)
        {
            string _allState = null;
            string _selectedState = null;

            // return if null 
            if (values[AllHelperId] == null)
            {
                return false;
            }
            _allState = values[AllHelperId].Trim();
            _selectedState = values[SelectedHelperId].Trim();
            if (string.IsNullOrEmpty(_allState))
            {
                m_AllItems.Clear();
            }
            else
            {
                m_AllItems = new ArrayList(_allState.Split(','));
            }
            if (string.IsNullOrEmpty(_selectedState))
            {
                m_SelectedItems.Clear();
            }
            else
            {
                m_SelectedItems = new ArrayList(_selectedState.Split(','));
            }
            // No change, return false 
            if (SelectedState == _selectedState.Trim())
            {
                return false;
            }
            // Change, return true and update state 
            Changed = true;
            SelectedState = _selectedState;
            return true;
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            OnListChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="E:ListChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnListChanged(EventArgs e)
        {
            if (ListChanged != null)
            {
                ListChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.DataBinding" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            IEnumerator objDataEnum = default(IEnumerator);

            // bind all items 
            if ((m_DataSource != null))
            {

                // Populate all items 
                objDataEnum = m_DataSource.GetEnumerator();
                while (objDataEnum.MoveNext())
                {
                    if (m_DataField == string.Empty)
                    {
                        m_AllItems.Add((string)objDataEnum.Current.ToString());
                    }
                    else
                    {
                        m_AllItems.Add((string)DataBinder.Eval(objDataEnum.Current, m_DataField));
                    }
                }
            }

            // Remove selected items from all items 
            foreach (var _item in m_SelectedItems)
            {
                m_AllItems.Remove(_item);
            }
        }

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
        public string[] SelectedItems
        {
            get { return (string[])m_SelectedItems.ToArray(typeof(string)); }
            set
            {
                m_SelectedItems = new ArrayList(value);
                SelectedState = string.Join(",", value);
            }
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <value>All items.</value>
        public string[] AllItems
        {
            get { return (string[])m_AllItems.ToArray(typeof(string)); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPicker" /> class.
        /// </summary>
        public ListPicker()
            : base(HtmlTextWriterTag.Table)
        {
        }

        /// <summary>
        /// BTNs the add click.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void btnAddClick(Object s, EventArgs e)
        {
            if (lstAllItems.SelectedIndex != -1)
            {
                // Move the item 
                int x = 0;
                for (x = lstAllItems.Items.Count - 1; x >= 0; x += -1)
                {
                    if (lstAllItems.Items[x].Selected)
                    {
                        lstSelectedItems.Items.Add(lstAllItems.Items[x]);
                        lstAllItems.Items.Remove(lstAllItems.Items[x]);
                    }
                }
                lstSelectedItems.SelectedIndex = -1;

                // update changed status items 
                Changed = true;
            }
        }

        /// <summary>
        /// BTNs the add all click.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void btnAddAllClick(Object s, EventArgs e)
        {
            // Move the item 
            int x = 0;
            for (x = lstAllItems.Items.Count - 1; x >= 0; x += -1)
            {
                lstSelectedItems.Items.Add(lstAllItems.Items[x]);
                lstAllItems.Items.Remove(lstAllItems.Items[x]);
            }
            lstSelectedItems.SelectedIndex = -1;

            // update changed status items 
            Changed = true;
        }

        /// <summary>
        /// BTNs the remove click.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void btnRemoveClick(Object s, EventArgs e)
        {
            if (lstSelectedItems.SelectedIndex != -1)
            {
                int x = 0;
                for (x = lstSelectedItems.Items.Count - 1; x >= 0; x += -1)
                {
                    if (lstSelectedItems.Items[x].Selected)
                    {
                        lstAllItems.Items.Add(lstSelectedItems.Items[x]);
                        lstSelectedItems.Items.Remove(lstSelectedItems.Items[x]);
                    }
                }

                // update changed status items 
                Changed = true;
            }
        }

        /// <summary>
        /// BTNs the remove all click.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void btnRemoveAllClick(Object s, EventArgs e)
        {
            int x = 0;
            for (x = lstSelectedItems.Items.Count - 1; x >= 0; x += -1)
            {
                lstAllItems.Items.Add(lstSelectedItems.Items[x]);
                lstSelectedItems.Items.Remove(lstSelectedItems.Items[x]);
            }

            // update changed status items 
            Changed = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // next two lines for conversion to framework 2.0 
            Page.ClientScript.RegisterHiddenField(SelectedHelperId, string.Join(",", SelectedItems));
            Page.ClientScript.RegisterHiddenField(AllHelperId, string.Join(",", AllItems));
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // start the row 
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            // Add Labels 
            writer.RenderBeginTag(HtmlTextWriterTag.Td);// First TD
            writer.Write(AllItemsText);
            writer.RenderEndTag(); // end the td 

            writer.RenderBeginTag(HtmlTextWriterTag.Td);// second TD
            writer.Write("&nbsp;");
            writer.RenderEndTag();// end the td 

            writer.RenderBeginTag(HtmlTextWriterTag.Td);// third TD
            writer.Write(SelectedItemsText);
            writer.RenderEndTag();// end the td 

            if (DestinationSortable && !SortOnChange)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);// 4th TD to contain the move up/down when changing order
                //writer.Write(SelectedItemsText);
                writer.RenderEndTag();// end the td 
            }
            writer.RenderEndTag();// end the tr 




            // start the next row 
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            // begin the first cell 
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            // set the alignment to top 
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            // All list box 
            HtmlSelect mySelect = new HtmlSelect();
            mySelect.Multiple = true;
            mySelect.Attributes.Add("Style", "width: " + Size + "px");
            mySelect.Size = int.Parse(Rows.ToString());
            mySelect.ID = ClientID + "_SrcList";
            foreach (var _item in m_AllItems)
            {
                ListItem myItem = new ListItem(_item.ToString(), _item.ToString());
                myItem.Attributes.Add("title", _item.ToString());
                mySelect.Items.Add(myItem);
            }
            //writer.Write(String.Format("<option value=""{0}"">{0}</option>", item)) 
            mySelect.RenderControl(writer);
            // end the first cell 
            writer.RenderEndTag();
            // begin the second cell 
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            // begin the second cell 
            // Add Button 
            HtmlInputButton myButton = new HtmlInputButton();
            myButton.Value = m_SelectButtonText;
            myButton.Attributes.Add("onclick", string.Format("GW.ListPicker.switchList(this.form.{0}_SrcList, this.form.{0}_DstList,'{1}')", ClientID, SortOnChange));
            myButton.Attributes.Add("class", "listPickerArrow");
            myButton.Attributes.Add("style", "width: " + m_ButtonWidth);
            myButton.RenderControl(writer);
            writer.WriteBreak();

            myButton = new HtmlInputButton();
            myButton.Value = m_SelectAllButtonText;
            myButton.Attributes.Add("onclick", string.Format("GW.ListPicker.switchAll(this.form.{0}_SrcList, this.form.{0}_DstList,'{1}')", ClientID, SortOnChange));
            myButton.Attributes.Add("class", "listPickerArrow");
            myButton.Attributes.Add("style", "width: " + m_ButtonWidth);
            myButton.RenderControl(writer);
            writer.WriteBreak();

            myButton = new HtmlInputButton();
            myButton.Value = m_DeSelectButtonText;
            myButton.Attributes.Add("onclick", string.Format("GW.ListPicker.switchList(this.form.{0}_DstList, this.form.{0}_SrcList,'true')", ClientID));
            myButton.Attributes.Add("class", "listPickerArrow");
            myButton.Attributes.Add("style", "width: " + m_ButtonWidth);
            myButton.RenderControl(writer);
            writer.WriteBreak();

            myButton = new HtmlInputButton();
            myButton.Value = m_DeSelectAllButtonText;
            myButton.Attributes.Add("onclick", string.Format("GW.ListPicker.switchAll(this.form.{0}_DstList, this.form.{0}_SrcList,'true')", ClientID));
            myButton.Attributes.Add("class", "listPickerArrow");
            myButton.Attributes.Add("style", "width: " + ButtonWidth);
            myButton.RenderControl(writer);

            writer.RenderEndTag();
            // end the second cell 
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //begin third cell 
            mySelect = new HtmlSelect();
            mySelect.Multiple = true;
            mySelect.Attributes.Add("Style", "width: " + Size + "px");
            mySelect.Size = int.Parse(Rows.ToString());
            mySelect.ID = ClientID + "_DstList";
            foreach (var _item in m_SelectedItems)
            {
                ListItem myItem = new ListItem(_item.ToString(), _item.ToString());
                mySelect.Items.Add(myItem);
            }
            //writer.Write(String.Format("<option value=""{0}"">{0}</option>", item)) 
            mySelect.RenderControl(writer);
            writer.RenderEndTag();// end third cell 

            if (DestinationSortable && !SortOnChange)
            {

                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);// add the 4th td

                myButton = new HtmlInputButton();
                myButton.Value = "▲";
                myButton.Attributes.Add("onclick", string.Format("GW.ListPicker.moveUp(this.form.{0}_DstList)", ClientID));
                myButton.Attributes.Add("class", "listPickerArrow");
                myButton.Attributes.Add("style", "width: " + m_ButtonWidth);
                myButton.RenderControl(writer);
                writer.WriteBreak();

                myButton = new HtmlInputButton();
                myButton.Value = "▼";
                myButton.Attributes.Add("onclick", string.Format("GW.ListPicker.moveDown(this.form.{0}_DstList)", ClientID));
                myButton.Attributes.Add("class", "listPickerArrow");
                myButton.Attributes.Add("style", "width: " + m_ButtonWidth);
                myButton.RenderControl(writer);
                writer.WriteBreak();
                writer.RenderEndTag();
            }



            writer.RenderEndTag();// end the tr 
        }
    }
}
