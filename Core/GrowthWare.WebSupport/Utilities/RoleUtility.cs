using System.Collections;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;

namespace GrowthWare.WebSupport.Utilities;

public static class RoleUtility
{

    public static ArrayList GetRolesArrayListBySecurityEntity(int securityEntityId)
    {
        DataTable mGroupsTable = GetAllRolesBySecurityEntity(securityEntityId);
        ArrayList mRetVal = new ArrayList();
        foreach (DataRow item in mGroupsTable.Rows)
        {
            mRetVal.Add((string)item["NAME"]);
        }
        return mRetVal;        
    }

    static DataTable GetAllRolesBySecurityEntity(int securityEntityId)
    {
        // TODO: Add cache
        BRoles mBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        DataTable mRoles = mBRoles.GetRolesBySecurityEntity(securityEntityId);
        return mRoles;
    }
}