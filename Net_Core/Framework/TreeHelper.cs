using System;
using System.Collections.Generic;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework;

/// <summary>
/// A helper class for objects which implement <see cref="ITreeNode{T}"/>, providing
/// methods to convert flat lists to and from hierarchical trees, iterators, and
/// other utility methods.
/// </summary>
public static class TreeHelper
{
// https://www.codeproject.com/articles/23949/building-trees-from-lists-in-net

#region Private methods

    [System.Diagnostics.Conditional("DEBUG")]
    private static void EnsureTreePopulated<T>(T node, string parameterName)
        where T : class, ITreeNode<T>
    {
        if (node == null)
        {
            throw new ArgumentNullException(parameterName, "The given node cannot be null.");
        }
        if (node.Children == null)
        {
            throw new ArgumentException("The children of " + parameterName + " is null. Have you populated the tree fully by calling TreeHelper<T>.ConvertToForest(IEnumerable<T> flatNodeList)?", parameterName);
        }
    }

#endregion

#region Tree structure methods

    /// <summary>
    /// Converts a heirachacle Array of Tree Nodes into a flat array of nodes. The order
    /// of the returned nodes is the same as a depth-first traversal of each tree.
    /// </summary>
    /// <remarks>The relationships between Parent/Children are retained.</remarks>
    public static List<T> ConvertToFlatArray<T>(this IEnumerable<T> trees)
        where T : class, ITreeNode<T>
    {
        List<T> treeNodeList = new List<T>();
        foreach (T rootNode in trees)
        {
            foreach (T node in DepthFirstTraversal(rootNode))
            {
                treeNodeList.Add(node);
            }
        }
        return treeNodeList;
    }

#endregion

#region Search methods

    /// <summary>Finds the TreeNode with the given Id in the given tree by searching the descendents.
    /// Returns null if the node cannot be found.</summary>
    public static T FindDescendant<T>(this T searchRoot, int id)
        where T : class, ITreeNode<T>
    {
        EnsureTreePopulated(searchRoot, "searchRoot");

        foreach (T child in DepthFirstTraversal(searchRoot))
        {
            if (child.Id == id)
            {
                return child;
            }
        }
        return null;
    }

    /// <summary>Finds the TreeNode with the given id from the given forest of trees.
    /// Returns null if the node cannot be found.</summary>
    public static T FindTreeNode<T>(this IEnumerable<T> trees, int id)
        where T : class, ITreeNode<T>
    {

        foreach (T rootNode in trees)
        {
            if (rootNode.Id == id)
            {
                return rootNode;
            }
            T descendant = FindDescendant(rootNode, id);
            if (descendant != null)
            {
                return descendant;
            }
        }

        return null;
    }

#endregion

#region Iterators

    /// <summary>
    /// Returns an Iterator which starts at the given node, and traverses the tree in
    /// a depth-first search manner.
    /// </summary>
    /// <param name="startNode">The node to start iterating from.  This will be the first node returned by the iterator.</param>
    public static IEnumerable<T> DepthFirstTraversal<T>(this T startNode)
        where T : class, ITreeNode<T>
    {
        EnsureTreePopulated(startNode, "node");

        yield return startNode;
        foreach (T child in startNode.Children)
        {
            foreach (T grandChild in DepthFirstTraversal(child))
            {
                yield return grandChild;
            }
        }
    }

    /// <summary>
    /// Returns an Iterator which traverses a forest of trees in a depth-first manner.
    /// </summary>
    /// <param name="trees">The forest of trees to traverse.</param>
    public static IEnumerable<T> DepthFirstTraversalOfList<T>(this IEnumerable<T> trees)
        where T : class, ITreeNode<T>
    {
        foreach (T rootNode in trees)
        {
            foreach (T node in DepthFirstTraversal(rootNode))
            {
                yield return node;
            }
        }
    }

    /// <summary>
    /// Traverses the tree in a breadth-first fashion.
    /// </summary>
    /// <param name="node">The node to start at.</param>
    /// <param name="returnRootNode">If true, the given node will be returned; if false, traversal starts at the node's children.</param>
    public static IEnumerable<T> BreadthFirstTraversal<T>(this T node, bool returnRootNode)
        where T : class, ITreeNode<T>
    {
        EnsureTreePopulated(node, "node");

        if (returnRootNode)
        {
            yield return node;
        }

        foreach (T child in node.Children)
        {
            yield return child;
        }


        foreach (T child in node.Children)
        {
            foreach (T grandChild in BreadthFirstTraversal(child, false))
            {
                yield return grandChild;
            }
        }

    }

#endregion

}
