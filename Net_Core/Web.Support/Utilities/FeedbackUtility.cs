using System;
using System.Data;
using System.Linq;
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
        string mPrimaryKeyName = "NVP_DetailSeqId";
        string mDataTableName = "Temp_Table";
        MNameValuePair mNameValuePair = NameValuePairUtility.GetNameValuePairs().FirstOrDefault();
        MNameValuePairDetail mProfile = NameValuePairUtility.GetNameValuePairDetail(mNameValuePair.Id, 1);
        mProfile.SetTableName(mNameValuePair.SchemaName, mNameValuePair.StaticName, true);

        string mTableName = mProfile.TableName;
        
        string mDeleteWithParameters = MNameValuePairDetail.GenerateDeleteWithParameters(mPrimaryKeyName, mUseBrackets);
        string mDeleteWithValues = mProfile.GenerateDeleteWithValues<MNameValuePairDetail>(mPrimaryKeyName, mUseBrackets);
        string mDeleteWithValuesSpecifyKeyValue = MNameValuePairDetail.GenerateDeleteWithValues(mPrimaryKeyName, "'yaba'", mUseBrackets);
        string mInsertWithParameters = MNameValuePairDetail.GenerateInsertWithParameters<MNameValuePairDetail>(mUseBrackets);
        string mInsertWithValues = mProfile.GenerateInsertWithValues<MNameValuePairDetail>(mUseBrackets);
        string mUpdateWithParameters = MNameValuePairDetail.GenerateUpdateWithParameters<MNameValuePairDetail>(mPrimaryKeyName, mUseBrackets);
        string mUpdateWithValues = mProfile.GenerateUpdateWithValues<MNameValuePairDetail>(mPrimaryKeyName, mUseBrackets);
        string mUpdateWithValuesSpecifyKeyValue = mProfile.GenerateUpdateWithValues<MNameValuePairDetail>(mPrimaryKeyName, "'yaba'", mUseBrackets);

        DataTable mDataTable = MNameValuePairDetail.GenerateEmptyTable<MNameValuePairDetail>(mDataTableName, false);
        mDataTable = null;
        mDataTable = MNameValuePairDetail.GenerateEmptyTable<MNameValuePairDetail>(mDataTableName, true);
        string mStop = string.Empty;
    }
}
