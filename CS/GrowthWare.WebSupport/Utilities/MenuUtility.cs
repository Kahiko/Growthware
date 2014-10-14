using System;
using System.Collections.Generic;
using System.Data;
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
        /// <param name="stringBuilder">The string builder.</param>
        /// <returns>String.</returns>
        public static String GenerateULLI(DataTable menuData, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("<ul>");
            DataView datView = new DataView(menuData);
            datView.RowFilter = "ParentID = 1";
            //Populate menu with top menu items;
            foreach (DataRowView rowVeiw in datView)
            {
                //Define new menu item
                if (int.Parse(rowVeiw["FUNCTION_TYPE_SEQ_ID"].ToString()) == 3)
                {
                    stringBuilder.AppendLine(createLIItem(rowVeiw["Title"].ToString(), rowVeiw["URL"].ToString(), rowVeiw["Description"].ToString(), true));
                }
                else
                {
                    stringBuilder.AppendLine(createLIItem(rowVeiw["Title"].ToString(), rowVeiw["URL"].ToString(), rowVeiw["Description"].ToString(), false));
                }
                //Populate child items of this parent
                addChildItems(menuData, int.Parse(rowVeiw["MenuID"].ToString()), ref stringBuilder);
            }
            stringBuilder.AppendLine("<ul>");
            return stringBuilder.ToString();
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
            DataView datView = new DataView(menuData);
            //Filter child menu items
            datView.RowFilter = "parentid = " + parentID;
            //Populate parent menu item with child menu items
            stringBuilder.AppendLine("<ul>");
            foreach (DataRowView datRow in datView)
            {
                //Define new menu item
                if (int.Parse(datRow["FUNCTION_TYPE_SEQ_ID"].ToString()) == 3)
                {
                    stringBuilder.AppendLine(createLIItem(datRow["Title"].ToString(), datRow["URL"].ToString(), datRow["Description"].ToString(), true));
                }
                else
                {
                    stringBuilder.AppendLine(createLIItem(datRow["Title"].ToString(), datRow["URL"].ToString(), datRow["Description"].ToString(), false));
                }
                //stringBuilder.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), mHasChildren))
                //Populate child items of this parent
                addChildItems(menuData, int.Parse(datRow["MenuID"].ToString()), ref stringBuilder);
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
