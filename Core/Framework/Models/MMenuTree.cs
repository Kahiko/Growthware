using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework.Models;

public class MMenuTree : ITreeNode<MMenuTree>
{
    public MMenuTree()
    {

    }

    public static List<MMenuTree> FillRecursive(IList<MMenuTree> flatObjects, int parentId)
    {
        List<MMenuTree> recursiveObjects = new List<MMenuTree>();
        var mResults = flatObjects.Where(x => x.ParentId == parentId).ToList();
        foreach (MMenuTree item in mResults)
        {
            recursiveObjects.Add(new MMenuTree
            {
                Action = item.Action,
                Description = item.Description,
                Id = item.Id,
                Label = item.Label,
                ParentId = item.ParentId,
                Children = FillRecursive(flatObjects, item.Id)
            });
        }
        return recursiveObjects;
    }

    public static List<MMenuTree> GetFlatList(DataTable dataTable)
    {
        List<MMenuTree> mRetVal = new List<MMenuTree>(); 
        foreach (DataRow mDataRow in dataTable.Rows) {
            MMenuTree mMenuTree = new MMenuTree(mDataRow);
            int mParentId = (int)mDataRow["ParentID"];
            if (mParentId != 1)
            {
                mMenuTree.Parent = new MMenuTree();
                mMenuTree.Parent.Id = mParentId;
                mMenuTree.ParentId = mParentId;
            }
            mRetVal.Add(mMenuTree);
        }
        return mRetVal;
    }

    public MMenuTree(DataRow dataRow)
    {
        Action = dataRow["URL"].ToString();
        Description = dataRow["Description"].ToString();
        Id = int.Parse(dataRow["MenuID"].ToString());
        Label = dataRow["Title"].ToString();
    }
#region Properties
    public string Action {get; set;}
    public IList<MMenuTree> Children {get; set;}
    public string Description {get; set;}
    public int Id {get; set;}
    public string Label {get; set;}
    public MMenuTree Parent {get; set;}
    public int ParentId { get; set; }
#endregion

}