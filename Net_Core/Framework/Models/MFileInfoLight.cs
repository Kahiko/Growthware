using System;
using System.IO;

namespace GrowthWare.Framework.Models;

public class FileInfoLight
{
    public DateTime Created {get; set;}
    public string CreatedShort {get; set;}
    public string Extension {get; set;}
    public string FullName {get; set;}
    public DateTime Modified {get; set;}
    public string ModifiedShort {get; set;}
    public string Name {get; set;}
    public bool Selected {get; set;}
    public string ShortFileName{get; set;}
    public string Size {get; set;}
    public bool Visible {get; set;}

    public FileInfoLight(FileInfo fileInfo)
    {
        string mFilename = fileInfo.Name;
        const int mByteConversion = 1024;
        double mBytes = Convert.ToDouble(fileInfo.Length);

        // this.CreationTime = fileInfo.CreationTime;
        this.Created = File.GetCreationTime(fileInfo.Directory.FullName + Path.DirectorySeparatorChar.ToString() + mFilename);
        this.CreatedShort = this.Created.ToString();
        this.Extension = fileInfo.Extension;
        this.FullName = Path.GetFileName(fileInfo.DirectoryName + Path.DirectorySeparatorChar.ToString() + mFilename);
        this.Modified = File.GetLastWriteTime(fileInfo.Directory.FullName + Path.DirectorySeparatorChar.ToString() + fileInfo.Name);
        this.ModifiedShort = this.Modified.ToString();
        this.Name = fileInfo.Name;
        this.Selected = false;
        this.ShortFileName = mFilename.Remove(mFilename.Length - fileInfo.Extension.Length, fileInfo.Extension.Length);
        if (mBytes >= Math.Pow(mByteConversion, 3)) //GB Range
        {
            this.Size = string.Concat(Math.Round(mBytes / Math.Pow(mByteConversion, 3), 2), " GB");
        }
        else if (mBytes >= Math.Pow(mByteConversion, 2)) //MB Range
        {
            this.Size = string.Concat(Math.Round(mBytes / Math.Pow(mByteConversion, 2), 2), " MB");
        }
        else if (mBytes >= mByteConversion) //KB Range
        {
            this.Size = string.Concat(Math.Round(mBytes / mByteConversion, 2), " KB");
        }
        else //Bytes
        {
            this.Size = string.Concat(mBytes, " Bytes");
        }
        this.Visible = true;
    }
}
