using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport.BaseControllers;

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
    // [Authorize("GetFiles")]
    // [HttpGet("GetFiles")]
    // public ActionResult<FileInfo[]> GetFiles()
    // {
    //     return Ok();
    // }

    [HttpGet("GetDirectories")]
    public ActionResult<MDirectoryTree> GetDirectories(string action)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayView)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            // https://stackoverflow.com/questions/24725775/converting-a-directory-structure-and-parsing-to-json-format-in-c-sharp
            MDirectoryTree mDirTree = new MDirectoryTree(new DirectoryInfo(mDirectoryProfile.Directory));
            string result =  mDirTree.ToJson();
            return Ok(mDirTree);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpGet("GetFiles")]
    public ActionResult<List<FileInfoLight>> GetFiles(string action, string path)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayView)
        {
            MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            DirectoryInfo mDirectoryInto = new DirectoryInfo(mDirectoryProfile.Directory);
            FileInfo mm = new FileInfo("maba");
            List<FileInfoLight> mRetVal = new List<FileInfoLight>();
            foreach (FileInfo item in mDirectoryInto.GetFiles())
            {
                mRetVal.Add(new FileInfoLight(item));
            }
            // Console.WriteLine(mRetVal[0]);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }
}
