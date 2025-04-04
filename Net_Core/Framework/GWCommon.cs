
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GrowthWare.Framework;

/// <summary>
/// The GWCommon class is just a collection of static methods where the methods just don't fit anywhere else.
/// </summary>
public static class GWCommon
{
    /// <summary>
    /// Sorts the methods by name
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>A MethodInfo[] Sorted by method names</returns>
    public static MethodInfo[] GetMethods(object obj)
    {
        // This was intended to help ensure that the public and or static methods are covered in NUnit tests
        MethodInfo[] mInfos;
        if (obj is Type type)
        {
            mInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
        }
        else
        {
            mInfos = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static);
        }
        Array.Sort(mInfos, delegate (MethodInfo methodInfo1, MethodInfo methodInfo2)
        {
            return methodInfo1.Name.CompareTo(methodInfo2.Name);
        });
        return mInfos;
    }
    
}