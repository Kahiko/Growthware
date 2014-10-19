using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// Class MenuUtility
    /// </summary>
    public static class MenuUtility
    {
        /// <summary>
        /// Generates and order list form hierarchical data.
        /// </summary>
        /// <param name="menuData">The menu data.</param>
        /// <param name="value">The string builder.</param>
        /// <returns>String.</returns>
        public static String GenerateUnorderedList(DataTable menuData, StringBuilder value)
        {
            if (value == null) throw new ArgumentNullException("value", "value cannot be a null reference (Nothing in Visual Basic)!");
            if (menuData == null) throw new ArgumentNullException("menuData", "menuData cannot be a null reference (Nothing in Visual Basic)!");
            value.AppendLine("<ul>");
            DataView datView = null;
            try
            {
                datView = new DataView(menuData);
                datView.RowFilter = "ParentID = 1";
                //Populate menu with top menu items;
                foreach (DataRowView rowVeiw in datView)
                {
                    //Define new menu item
                    if (int.Parse(rowVeiw["FUNCTION_TYPE_SEQ_ID"].ToString(), CultureInfo.InvariantCulture) == 3)
                    {
                        value.AppendLine(createLIItem(rowVeiw["Title"].ToString(), rowVeiw["URL"].ToString(), rowVeiw["Description"].ToString(), true));
                    }
                    else
                    {
                        value.AppendLine(createLIItem(rowVeiw["Title"].ToString(), rowVeiw["URL"].ToString(), rowVeiw["Description"].ToString(), false));
                    }
                    //Populate child items of this parent
                    addChildItems(menuData, int.Parse(rowVeiw["MenuID"].ToString(), CultureInfo.InvariantCulture), ref value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (datView != null) datView.Dispose();
            }

            value.AppendLine("<ul>");
            return value.ToString();
        }

        /// <summary>
        /// Add the child items to the stringBuilder
        /// </summary>
        /// <param name="menuData">DataTable</param>
        /// <param name="parentID">Integer</param>
        /// <param name="stringBuilder">stringBuilder</param>
        /// <remarks></remarks>
        private static void addChildItems(DataTable menuData, int parentID, ref StringBuilder stringBuilder)
        {
            //Populate DataView
            DataView datView = null;
            try
            {
                datView = new DataView(menuData);
                //Filter child menu items
                datView.RowFilter = "parentid = " + parentID;
                //Populate parent menu item with child menu items
                stringBuilder.AppendLine("<ul>");
                foreach (DataRowView datRow in datView)
                {
                    //Define new menu item
                    if (int.Parse(datRow["FUNCTION_TYPE_SEQ_ID"].ToString(), CultureInfo.InvariantCulture) == 3)
                    {
                        stringBuilder.AppendLine(createLIItem(datRow["Title"].ToString(), datRow["URL"].ToString(), datRow["Description"].ToString(), true));
                    }
                    else
                    {
                        stringBuilder.AppendLine(createLIItem(datRow["Title"].ToString(), datRow["URL"].ToString(), datRow["Description"].ToString(), false));
                    }
                    //stringBuilder.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), mHasChildren))
                    //Populate child items of this parent
                    addChildItems(menuData, int.Parse(datRow["MenuID"].ToString(), CultureInfo.InvariantCulture), ref stringBuilder);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (datView != null) datView.Dispose();
            }
            stringBuilder.AppendLine("</ul>");
        }

        /// <summary>
        /// Add the li with either the class has-sub or without
        /// </summary>
        /// <param name="hrefText">Link Text</param>
        /// <param name="action">Used to build the javascript</param>
        /// <param name="hrefToolTip">Used for the tooltip property</param>
        /// <param name="hasChildren">Determines the class='has-sub' for the li tag</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static String createLIItem(String hrefText, String action, String hrefToolTip, Boolean hasChildren)
        {
            String retVal = String.Empty;
            if (!hasChildren)
            {
                retVal = "<li><a href=\"" + action + "\" title=\"" + hrefToolTip + "\"><span>" + hrefText + "</span></a>";
            }
            else
            {
                retVal = "<li class='has-sub'><a href='#' title=\"" + hrefToolTip + "\"><span>" + hrefText + "</span></a>";
            }
            return retVal;
        }
    }
}
