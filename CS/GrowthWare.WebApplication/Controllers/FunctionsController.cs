using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GrowthWare.WebApplication.Controllers
{
    public class FunctionsController : ApiController
    {
        [HttpGet()]
        public Collection<FunctionInformation> GetFunctionData()
        {
            FunctionInformation mFunctionInformation = null;
            Collection<FunctionInformation> mRetVal = new Collection<FunctionInformation>();
            Collection<MFunctionProfile> mFunctions = FunctionUtility.Functions();
            string mAppName = ConfigSettings.AppName;
            if (mAppName.Length != 0)
            {
                mAppName = "/" + mAppName + "/";
            }
            foreach (MFunctionProfile mProfile in mFunctions)
            {
                if (mProfile.FunctionTypeSeqId != 2)
                {
                    mFunctionInformation = new FunctionInformation();
                    mFunctionInformation.Action = mProfile.Action;
                    mFunctionInformation.Location = mAppName + mProfile.Source;
                    mFunctionInformation.Description = mProfile.Description;
                    mFunctionInformation.LinkBehavior = mProfile.LinkBehavior;
                    mRetVal.Add(mFunctionInformation);
                }
            }

            return mRetVal;
        }

        [HttpGet()]
        public List<UIFunctionMenuOrder> GetFunctionOrder([FromUri()] int functionSeqId) 
        {
            List<UIFunctionMenuOrder> mRetVal = new List<UIFunctionMenuOrder>();
            MFunctionProfile profile = FunctionUtility.GetProfile(functionSeqId);
            DataView myDataView = FunctionUtility.GetFunctionMenuOrder(profile).DefaultView;
            foreach (DataRowView mRow in myDataView)
            {
                UIFunctionMenuOrder mItem = new UIFunctionMenuOrder();
                mItem.Function_Seq_Id = mRow["Function_Seq_Id"].ToString();
                mItem.Name = mRow["Name"].ToString();
                mItem.Action = mRow["Action"].ToString();
                mRetVal.Add(mItem);
            }
            return mRetVal;
        }

        [HttpPost()]
        public bool MoveMenu(int functionSeqId, string direction) 
        {
            bool mRetVal = false;
            MFunctionProfile mProfile = FunctionUtility.GetProfile(functionSeqId);
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
            DirectionType mDirection;
            Enum.TryParse(direction, out mDirection);
            if (direction == "up")
            {
                FunctionUtility.Move(mProfile, DirectionType.Up, mAccountProfile.Id, DateTime.Now);
            }
            else
            {
                FunctionUtility.Move(mProfile, DirectionType.Down, mAccountProfile.Id, DateTime.Now);
            }
            return mRetVal;
        }

        [HttpPost()]
        public string SetSelectedSecurityEntity(int selectedSecurityEntityId)
        {
            MSecurityEntityProfile targetSEProfile = SecurityEntityUtility.GetProfile(selectedSecurityEntityId);
            MSecurityEntityProfile currentSEProfile = SecurityEntityUtility.CurrentProfile();
            MClientChoicesState mClientChoicesState = (MClientChoicesState)HttpContext.Current.Cache[MClientChoices.SessionName];
            MMessageProfile mMessageProfile = null;
            try
            {
                if (!ConfigSettings.CentralManagement)
                {
                    //SecurityEntityUtility.SetSessionSecurityEntity(targetSEProfile)
                    mClientChoicesState[MClientChoices.SecurityEntityId] = targetSEProfile.Id.ToString(CultureInfo.InvariantCulture);
                    mClientChoicesState[MClientChoices.SecurityEntityName] = targetSEProfile.Name;
                }
                else
                {
                    if (currentSEProfile.ConnectionString == targetSEProfile.ConnectionString)
                    {
                        mClientChoicesState[MClientChoices.SecurityEntityId] = targetSEProfile.Id.ToString(CultureInfo.InvariantCulture);
                        mClientChoicesState[MClientChoices.SecurityEntityName] = targetSEProfile.Name;
                    }
                    else
                    {
                        mClientChoicesState[MClientChoices.SecurityEntityId] = ConfigSettings.DefaultSecurityEntityId.ToString(CultureInfo.InvariantCulture);
                        mClientChoicesState[MClientChoices.SecurityEntityName] = "System";
                    }
                }
                ClientChoicesUtility.Save(mClientChoicesState);
                AccountUtility.RemoveInMemoryInformation(true);
                mMessageProfile = MessageUtility.GetProfile("ChangedSelectedSecurityEntity");
            }
            catch (Exception ex)
            {
                MMessageProfile myMessageProfile = new MMessageProfile();
                Logger mLog = Logger.Instance();
                mMessageProfile = MessageUtility.GetProfile("NoDataFound");
                Exception myEx = new Exception("SelectSecurityEntity:: reported an error.", ex);
                mLog.Error(myEx);
            }
            // update all of your in memory information
            return mMessageProfile.Body;
        }

        [HttpPost()]
        public IHttpActionResult Save(UIFunctionProfile uiProfile) 
        {
            if (uiProfile == null) throw new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            string mRetVal = "false";
            Logger mLog = Logger.Instance();
            if (HttpContext.Current.Items["EditId"] != null)
            {
                int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
                if (mEditId == uiProfile.Id) 
                {
                    MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
                    MFunctionProfile profile = new MFunctionProfile();
                    MDirectoryProfile directoryProfile = new MDirectoryProfile();
                    if (uiProfile.Id != -1)
                    {
                        profile = FunctionUtility.GetProfile(uiProfile.Id);
                        profile.UpdatedBy = mAccountProfile.Id;
                        profile.UpdatedDate = DateTime.Now;
                    }
                    else 
                    {
                        profile.AddedBy = mAccountProfile.Id;
                        profile.AddedDate = DateTime.Now;                    
                    }
                    string viewCommaRoles = String.Join(",", uiProfile.RolesAndGroups.ViewRoles);
                    string addCommaRoles = String.Join(",", uiProfile.RolesAndGroups.AddRoles);
                    string editCommaRoles = String.Join(",", uiProfile.RolesAndGroups.EditRoles);
                    string deleteCommaRoles = String.Join(",", uiProfile.RolesAndGroups.DeleteRoles);

                    string viewCommaGroups = String.Join(",", uiProfile.RolesAndGroups.ViewGroups);
                    string addCommaGroups = String.Join(",", uiProfile.RolesAndGroups.AddGroups);
                    string editCommaGroups = String.Join(",", uiProfile.RolesAndGroups.EditGroups);
                    string deleteCommaGroups = String.Join(",", uiProfile.RolesAndGroups.DeleteGroups);

                    bool saveGroups = false;
                    bool saveRoles = false;

                    if (profile.GetCommaSeparatedAssignedRoles(PermissionType.View) != viewCommaRoles)
                    {
                        profile.SetAssignedRoles(viewCommaRoles, PermissionType.View);
                        saveRoles = true;
                    }

                    if (profile.GetCommaSeparatedAssignedRoles(PermissionType.Add) != addCommaRoles)
                    {
                        profile.SetAssignedRoles(addCommaRoles, PermissionType.Add);
                        saveRoles = true;
                    }

                    if (profile.GetCommaSeparatedAssignedRoles(PermissionType.Edit) != editCommaRoles)
                    {
                        profile.SetAssignedRoles(editCommaRoles, PermissionType.Edit);
                        saveRoles = true;
                    }

                    if (profile.GetCommaSeparatedAssignedRoles(PermissionType.Delete) != deleteCommaRoles)
                    {
                        profile.SetAssignedRoles(deleteCommaRoles, PermissionType.Delete);
                        saveRoles = true;
                    }

                    if (profile.GetCommaSeparatedGroups(PermissionType.View) != viewCommaGroups)
                    {
                        profile.SetGroups(viewCommaGroups, PermissionType.View);
                        saveGroups = true;
                    }
                    if (profile.GetCommaSeparatedGroups(PermissionType.Add) != addCommaGroups)
                    {
                        profile.SetGroups(addCommaGroups, PermissionType.Add);
                        saveGroups = true;
                    }
                    if (profile.GetCommaSeparatedGroups(PermissionType.Edit) != editCommaGroups)
                    {
                        profile.SetGroups(editCommaGroups, PermissionType.Edit);
                        saveGroups = true;
                    }
                    if (profile.GetCommaSeparatedGroups(PermissionType.Delete) != deleteCommaGroups)
                    {
                        profile.SetGroups(deleteCommaGroups, PermissionType.Delete);
                        saveGroups = true;
                    }
                    profile.Action = uiProfile.Action;
                    profile.EnableNotifications = uiProfile.EnableNotifications;
                    profile.EnableViewState = uiProfile.EnableViewState;
                    profile.FunctionTypeSeqId = uiProfile.FunctionTypeSeqID;
                    profile.Id = uiProfile.Id;
                    profile.IsNavigable = uiProfile.IsNav;
                    profile.LinkBehavior = uiProfile.LinkBehavior;
                    profile.MetaKeywords = uiProfile.MetaKeyWords;
                    profile.Name = uiProfile.Name;
                    profile.NavigationTypeSeqId = uiProfile.NavigationTypeSeqId;
                    profile.Notes = uiProfile.Notes;
                    profile.NoUI = uiProfile.NoUI;
                    profile.ParentId = uiProfile.ParentID;
                    profile.Source = uiProfile.Source;
                    profile.Description = uiProfile.Description;
                    profile.RedirectOnTimeout = uiProfile.RedirectOnTimeout;
                    FunctionUtility.Save(profile, saveGroups, saveRoles);
                    profile = FunctionUtility.GetProfile(uiProfile.Action);
                    if (!String.IsNullOrEmpty(uiProfile.DirectoryData.Directory))
                    {
                        if(directoryProfile == null) directoryProfile = new MDirectoryProfile();
                        directoryProfile.FunctionSeqId = profile.Id;
                        directoryProfile.Directory = uiProfile.DirectoryData.Directory;
                        directoryProfile.Impersonate = uiProfile.DirectoryData.Impersonate;
                        directoryProfile.ImpersonateAccount = uiProfile.DirectoryData.ImpersonateAccount;
                        directoryProfile.ImpersonatePassword = uiProfile.DirectoryData.ImpersonatePassword;
                        directoryProfile.Name = uiProfile.DirectoryData.Directory;
                        directoryProfile.UpdatedBy = mAccountProfile.Id;
                        DirectoryUtility.Save(directoryProfile);
                    }
                    else 
                    {
                        if (directoryProfile != null) 
                        { 
                            directoryProfile.Directory = "";
                            directoryProfile.Name = "";
                            DirectoryUtility.Save(directoryProfile);              
                        }
                    }
                } else 
                {
                    Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);                   
                }
            }
            else 
            { 
                Exception mError = new Exception("Identifier could not be determined, nothing has been saved!!!!");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
            return Ok(mRetVal);
        }

    }


    public class UIFunctionProfile
    {
        public string Action;
        public string Description;
        public UIDirectoryProfile DirectoryData;
        public bool EnableNotifications;
        public bool EnableViewState;
        public int FunctionTypeSeqID;
        public int Id;
        public bool IsNav;
        public int LinkBehavior;
        public string MetaKeyWords;
        public string Name;
        public int NavigationTypeSeqId;
        public bool NoUI;
        public string Notes;
        public int ParentID;
        public bool RedirectOnTimeout;
        public string Source;
        public UIFunctionRolesGroups RolesAndGroups;

    }


    public class UIDirectoryProfile 
    {
        public string Directory;
        public bool Impersonate;
        public string ImpersonateAccount;
        public string ImpersonatePassword;
    }

    public class UIFunctionRolesGroups
    {
        public string[] ViewRoles;
        public string[] AddRoles;
        public string[] EditRoles;
        public string[] DeleteRoles;
        public string[] ViewGroups;
        public string[] AddGroups;
        public string[] EditGroups;
        public string[] DeleteGroups;
    }

    public class FunctionInformation
    {
        public string Action { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int LinkBehavior { get; set; }
    }

    public class UIFunctionMenuOrder
    {
        public string Function_Seq_Id;
        public String Name;
        public String Action;
    }
}