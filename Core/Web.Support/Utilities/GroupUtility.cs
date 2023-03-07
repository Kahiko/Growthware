using System.Collections;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;

namespace GrowthWare.Web.Support.Utilities;
public static class GroupUtility
{
    public static ArrayList GetGroupsArrayListBySecurityEntity(int securityEntityId)
    {
        DataTable mGroupsTable = GetAllGroupsBySecurityEntity(securityEntityId);
        ArrayList mRetVal = new ArrayList();
        foreach (DataRow item in mGroupsTable.Rows)
        {
            mRetVal.Add((string)item["NAME"]);
        }
        return mRetVal;
    }

    static DataTable GetAllGroupsBySecurityEntity(int securityEntityId)
    {
        // TODO: Add cache
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        DataTable mGroups = mBGroups.GetGroupsBySecurityEntity(securityEntityId);
        return mGroups;
    }
}