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
    public string RelitivePath { get; set; }

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
        foreach (DirectoryInfo mDirectoryInfo in directoryInfo.GetDirectories())
        {
            Children.Add(new MDirectoryTree(mDirectoryInfo, mRootPath));
        }
    }

    public string ToJson()
    {
        return "[" + JsonSerializer.Serialize(this) + "]";
    }
}
