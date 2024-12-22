using System;
using System.Data;
using System.Text.RegularExpressions;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Web.Support.Utilities;

public static class FeedbackUtility
{
    private static BFeedbacks m_BusinessLogic = null;

    private static BFeedbacks getBusinessLogic
    {
        get 
        {
            if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile);
            }
            return m_BusinessLogic;
        }
    }

    public static UIFeedback GetFeedback(int feedbackId)
    {
        testProfile();
        UIFeedback mRetVal = getBusinessLogic.GetFeedback(feedbackId);
        return mRetVal;
    }

    public static UIFeedback SaveFeedback(MFeedback feedback)
    {
        UIFeedback mRetVal = null;
        mRetVal = getBusinessLogic.SaveFeedback(feedback);
        return mRetVal;
    }

    private static void testProfile()
    {
        bool mUseBrackets = true;
        string mPrimaryKeyName = "GroupSeqId";
        MGroupProfile mProfile = GroupUtility.GetGroupProfile(1);

        string mTableName = mProfile.TableName;
        
        string mDeleteWithParameters = MGroupProfile.GenerateDeleteWithParameters(mPrimaryKeyName, mUseBrackets);
        string mDeleteWithValues = mProfile.GenerateDeleteWithValues<MGroupProfile>(mPrimaryKeyName, mUseBrackets);
        string mDeleteWithValuesSpecifyKeyValue = MGroupProfile.GenerateDeleteWithValues(mPrimaryKeyName, "'yaba'", mUseBrackets);
        string mInsertWithParameters = MGroupProfile.GenerateInsertWithParameters<MGroupProfile>(mUseBrackets);
        string mInsertWithValues = mProfile.GenerateInsertWithValues<MGroupProfile>(mUseBrackets);
        string mUpdateWithParameters = MGroupProfile.GenerateUpdateWithParameters<MGroupProfile>(mPrimaryKeyName, mUseBrackets);
        string mUpdateWithValues = mProfile.GenerateUpdateWithValues<MGroupProfile>(mPrimaryKeyName, mUseBrackets);
        string mUpdateWithValuesSpecifyKeyValue = mProfile.GenerateUpdateWithValues<MGroupProfile>(mPrimaryKeyName, "'yaba'", mUseBrackets);

        DataTable mDataTable = MGroupProfile.GenerateEmptyTable<MGroupProfile>("Accounts", false);
        mDataTable = null;
        mDataTable = MGroupProfile.GenerateEmptyTable<MGroupProfile>("Accounts", true);
        string mStop = string.Empty;
    }
}
