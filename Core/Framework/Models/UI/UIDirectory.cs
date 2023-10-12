using GrowthWare.Framework.Models;

public class UIDirectory
{

    public UIDirectory(MDirectoryProfile directoryProfile)
    {
        if(directoryProfile != null) 
        {
            this.Directory = directoryProfile.Directory;
            this.Id = directoryProfile.Id;
            this.Impersonate = directoryProfile.Impersonate;
            this.ImpersonateAccount = directoryProfile.ImpersonateAccount;
            this.ImpersonatePassword = directoryProfile.ImpersonatePassword;
        } else 
        {
            this.Directory = string.Empty;
            this.Id = -1;
            this.Impersonate = false;
            this.ImpersonateAccount = string.Empty;
            this.ImpersonatePassword = string.Empty;
        }
    }

    public string Directory{get; set;}
    public int Id{get; set;}
    public bool Impersonate{get; set;}
    public string ImpersonateAccount{get; set;}
    public string ImpersonatePassword{get; set;}

}