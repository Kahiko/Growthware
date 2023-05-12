using System.Collections.Generic;
using System.Data;

namespace GrowthWare.Framework.Interfaces;

/// <summary>
/// An object which implements this interface is considered a node in a tree.
/// </summary>
public interface ITreeNode<T> where T : class
{
    // https://www.codeproject.com/articles/23949/building-trees-from-lists-in-net

    /// <summary>
    /// Returns a Hierarchical tree given a list of flat objects that has a parrent id
    /// </summary>
    /// <param name="flatObjects">A flat list of this type of objects</param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public List<T> FillRecursive(IList<T> flatObjects, int parentId);

    /// <summary>
    /// Returns flat list of this type of object
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public List<T> GetFlatList(DataTable dataTable);

    /// <summary>
    /// A unique identifier for the node.
    /// </summary>
    int Id
    {
        get;
    }

    /// <summary>
    /// The parent of this node, or null if it is the root of the tree.
    /// </summary>
    int ParentId
    {
        get;
        set;
    }

    /// <summary>
    /// The children of this node, or an empty list if this is a leaf.
    /// </summary>
    IList<T> Children
    {
        get;
        set;
    }

}