using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace GrowthWare.WebSupport.CustomWebControls.Designers
{
    /// <summary>
    /// Class CustomDesigner
    /// </summary>
    public class CustomDesigner : ControlDesigner
    {

        /// <summary>
        /// Gets a value indicating whether the control can be resized in the design-time environment.
        /// </summary>
        /// <value><c>true</c> if [allow resize]; otherwise, <c>false</c>.</value>
        /// <returns>true, if the control can be resized; otherwise, false.</returns>
        public override bool AllowResize
        {
            get { return true; }
        }

        /// <summary>
        /// Retrieves the HTML markup that is used to represent the control at design time.
        /// </summary>
        /// <returns>The HTML markup used to represent the control at design time.</returns>
        public override string GetDesignTimeHtml()
        {
            WebControl mControl = (WebControl)Component;

            StringWriter mStringWriter = null;
            using (mStringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                HtmlTextWriter mHtmlWriter = new HtmlTextWriter(mStringWriter);
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black");
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "GainsBoro");
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.Width, mControl.Width.ToString());
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.Height, mControl.Height.ToString());
                mHtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);
                mHtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                mHtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                mHtmlWriter.Write(mControl.GetType().Name);
                mHtmlWriter.RenderEndTag();
                mHtmlWriter.RenderEndTag();
                mHtmlWriter.RenderEndTag();
            }
            if (mControl != null) mControl.Dispose();
            return mStringWriter.ToString();
        }

        /// <summary>
        /// Retrieves the HTML markup that provides information about the specified exception.
        /// </summary>
        /// <param name="e">The exception that occurred.</param>
        /// <returns>The design-time HTML markup for the specified exception.</returns>
        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            if (e != null) 
            {
                return CreatePlaceHolderDesignTimeHtml("error:" + e.Message + e.StackTrace);
            }
            else 
            {
                return CreatePlaceHolderDesignTimeHtml("Error getting the design time HTML.");
            }
        }
    }
}
