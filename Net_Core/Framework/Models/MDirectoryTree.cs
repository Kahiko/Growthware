using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GrowthWare.Framework.Models;

public class MDirectoryTree
{
    public List<MDirectoryTree> Children { get; set; }
    public int DirectoryCount { get; set; }
    public int FileCount { get; set; }
    public bool IsFolder { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string ParentRelitivePath { get; set; }
    public string RelitivePath { get; set; }
    public string Size { get; set; }
    public string SizeWithChildren { get; set; }
    public long SizeInBytes { get; set; }
    public long SizeInBytesWithChildren { get; set; }

    public MDirectoryTree(DirectoryInfo directoryInfo, string rootPath)
    {
        // Set the properties
        string mRootPath = rootPath; // it is good practace not to change the paramater
        mRootPath = mRootPath.Replace(@"\", @"/");
        mRootPath = mRootPath.Replace(@"/", Path.DirectorySeparatorChar.ToString());        
        mRootPath = mRootPath.TrimEnd(Path.DirectorySeparatorChar);
        Children = new List<MDirectoryTree>();
        DirectoryCount = directoryInfo.GetDirectories().Length;
        FileCount = directoryInfo.GetFiles().Length;
        IsFolder = true;
        Key = directoryInfo.Name.Replace(" ", "").ToLower();
        Name = directoryInfo.Name;
        RelitivePath = directoryInfo.FullName.Replace(mRootPath, "");
        ParentRelitivePath = this.RelitivePath.Replace(directoryInfo.Name, "");
        ParentRelitivePath = ParentRelitivePath.Replace(@"\", @"/");
        ParentRelitivePath = ParentRelitivePath.Replace(@"/", Path.DirectorySeparatorChar.ToString());
        int mLastInstance = ParentRelitivePath.LastIndexOf(Path.DirectorySeparatorChar);
        if (mLastInstance > 0) 
        {
            ParentRelitivePath = ParentRelitivePath.Substring(0, mLastInstance);        
        }
        if (ParentRelitivePath.Length < 1) 
        { 
            ParentRelitivePath = Path.DirectorySeparatorChar.ToString(); 
        }
        // Add file sizes.
        FileInfo[] mFiles = directoryInfo.GetFiles();        
        long mSize = 0;
        foreach (FileInfo mFileInfo in mFiles)
        {
            mSize += mFileInfo.Length;
        }
        Size = FileUtility.ToFileSize(mSize);
        SizeInBytes = mSize;
        foreach (DirectoryInfo mDirectoryInfo in directoryInfo.GetDirectories())
        {
            Children.Add(new MDirectoryTree(mDirectoryInfo, mRootPath));
        }
        SizeInBytesWithChildren += SizeInBytes;
        foreach (MDirectoryTree mDirectoryTree in Children)
        {
            SizeInBytesWithChildren += mDirectoryTree.SizeInBytes;
        }
        SizeWithChildren = FileUtility.ToFileSize(SizeInBytesWithChildren);
    }
    public string ToJson()
    {
        return "[" + JsonSerializer.Serialize(this) + "]";
    }
}
