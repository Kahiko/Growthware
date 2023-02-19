using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Jwt;

namespace GrowthWare.WebSupport.BaseControllers;

public class DirectoryInfoLight
{
    public string FullName { get; set; }
    public bool HasChildren { get; set; }
    public string Name { get; set; }
    public string Parent { get; set; }
}
public class FileTreeManager
{
    private DirectoryInfo mDirectoryInfo;

    public FileTreeManager(string aEntryPoint)
    {
        SetEntryPoint(aEntryPoint);
    }

    public void SetEntryPoint(string aEntryPoint)
    {
        this.mDirectoryInfo = new DirectoryInfo(aEntryPoint);
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        foreach (DirectoryInfo dir in this.mDirectoryInfo.EnumerateDirectories())
        {
            result.Append("|-- " + dir.Name + Environment.NewLine);
        }
        return result.ToString();
    }
}

[CLSCompliant(false)]
public abstract class AbstractFileController : ControllerBase
{
    [Authorize("GetFiles")]
    [HttpGet("GetFiles")]
    public ActionResult<DirectoryInfoLight[]> GetFiles()
    {
        return Ok();
    }

    [HttpGet("GetDirectories")]
    public ActionResult<MDirectoryTree> GetDirectories(int functionSeqId)
    {
        string mCurrentDirectory = Directory.GetCurrentDirectory();
        mCurrentDirectory = "D:\\Development\\Growthware\\Core\\GrowthWare.Web.Angular\\Angular\\projects\\gw-lib\\src\\lib\\features";
        // https://stackoverflow.com/questions/24725775/converting-a-directory-structure-and-parsing-to-json-format-in-c-sharp
        MDirectoryTree mDirTree = new MDirectoryTree(new DirectoryInfo(mCurrentDirectory));
        string result =  mDirTree.ToJson();
        return Ok(mDirTree);
    }
}
