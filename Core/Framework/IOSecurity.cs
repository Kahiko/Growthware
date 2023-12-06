using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace GrowthWare.Framework;
// https://stackoverflow.com/questions/57458934/checking-if-a-directory-has-read-and-write-permissions-using-net-core
public class IOSecurity
{
    // /**
    // * This is a windows only solution...
    // * Checked this in but may not want to tie Growthware to Windows
    // * I'll decide later.
    // */
    // WindowsIdentity m_CurrentUser;
    // WindowsPrincipal m_CurrentPrincipal;

    // public IOSecurity()
    // {
    //     m_CurrentUser = WindowsIdentity.GetCurrent();
    //     m_CurrentPrincipal = new WindowsPrincipal(m_CurrentUser);
    // }

    // public bool HasAccess(DirectoryInfo directory, FileSystemRights right)
    // {
    //     // Get the collection of authorization rules that apply to the directory.
    //     AuthorizationRuleCollection acl = directory.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
    //     return HasFileOrDirectoryAccess(right, acl);
    // }

    // public bool HasAccess(FileInfo file, FileSystemRights right)
    // {
    //     // Get the collection of authorization rules that apply to the file.
    //     AuthorizationRuleCollection acl = file.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
    //     return HasFileOrDirectoryAccess(right, acl);
    // }

    // private bool HasFileOrDirectoryAccess(FileSystemRights right, AuthorizationRuleCollection acl)
    // {
    //     bool allow = false;
    //     bool inheritedAllow = false;
    //     bool inheritedDeny = false;

    //     for (int i = 0; i < acl.Count; i++) {
    //         var currentRule = (FileSystemAccessRule)acl[i];
    //         // If the current rule applies to the current user.
    //         if (m_CurrentUser.User.Equals(currentRule.IdentityReference) ||
    //             m_CurrentPrincipal.IsInRole(
    //                             (SecurityIdentifier)currentRule.IdentityReference)) {

    //             if (currentRule.AccessControlType.Equals(AccessControlType.Deny)) {
    //                 if ((currentRule.FileSystemRights & right) == right) {
    //                     if (currentRule.IsInherited) {
    //                         inheritedDeny = true;
    //                     } else { // Non inherited "deny" takes overall precedence.
    //                         return false;
    //                     }
    //                 }
    //             } else if (currentRule.AccessControlType
    //                                               .Equals(AccessControlType.Allow)) {
    //                 if ((currentRule.FileSystemRights & right) == right) {
    //                     if (currentRule.IsInherited) {
    //                         inheritedAllow = true;
    //                     } else {
    //                         allow = true;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     if (allow) { // Non inherited "allow" takes precedence over inherited rules.
    //         return true;
    //     }
    //     return inheritedAllow && !inheritedDeny;
    // }
}