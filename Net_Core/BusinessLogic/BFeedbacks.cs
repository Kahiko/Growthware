using System;
using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.BusinessLogic;

public class BFeedbacks : AbstractBusinessLogic
{
    private IFeedbacks m_DataAccess;

        /// <summary>
        /// Private BFeedbacks() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BFeedbacks()
        {
        }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
        /// </summary>
        /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        /// <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
        /// <remarks></remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="C#">
        /// <![CDATA[
        /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
        /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
        /// MSecurityEntity.DAL = ConfigSettings.DAL;
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BFeedbacks mBusinessLogic = BFeedbacks = New BFeedbacks(MSecurityEntity);
        /// ]]>
        /// </code>
        /// <code language="VB.NET">
        /// <![CDATA[
        /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
        /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
        /// MSecurityEntity.DAL = ConfigSettings.DAL
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// Dim mBusinessLogic As BFeedbacks = New BFeedbacks(MSecurityEntity)
        /// ]]>
        /// </code>
        /// </example>
        public BFeedbacks(MSecurityEntity securityEntityProfile)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile can not be null!");
            }
            m_DataAccess = (IFeedbacks)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DFeedbacks");
            m_DataAccess.ConnectionString = securityEntityProfile.ConnectionString;
            m_DataAccess.SecurityEntitySeqId = securityEntityProfile.Id;
        }

        public UIFeedback GetFeedback(int feedbackId)
        {
            UIFeedback mRetVal = null;
            if (DatabaseIsOnline()) 
            {
                m_DataAccess.Profile = new MFeedback();
                m_DataAccess.Profile.FeedbackId = feedbackId;
                mRetVal = new UIFeedback(m_DataAccess.GetFeedback);
            }
            return mRetVal;
        }

        public UIFeedback SaveFeedback(MFeedback feedback)
        {
            UIFeedback mRetVal = null;
            if (DatabaseIsOnline()) 
            {
                m_DataAccess.Profile = feedback;
                mRetVal = new UIFeedback(m_DataAccess.SaveFeedback);
            }
            return mRetVal;
        }
}