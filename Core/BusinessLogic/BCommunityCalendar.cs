using System;
using System.Data;
using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.BusinessLogic;

/// <summary>
/// Process business logic for community calendar
/// </summary>
/// <remarks>
/// <![CDATA[
/// MSecurityEntity can be found in the GrowthWare.Framework.Model.Profiles namespace.  
/// 
/// The following properties are necessary for correct business logic operation.
/// .ConnectionString
/// .DALName
/// .DALNameSpace
/// ]]>
/// </remarks>
/// <example> This sample shows how to create an instance of the class.
/// <code language="VB.NET">
/// <![CDATA[
/// Dim myBll as new BCommunityCalendar(mySecurityEntityProfile)
/// ]]>
/// </code>
/// </example>
public class BCommunityCalendar : AbstractBusinessLogic
{
    private ICommunityCalendar m_DCommunityCalendar;

    /// <summary>
    /// Private BCommunityCalendar() to ensure only new instances with passed parameters is used.
    /// </summary>
    /// <remarks></remarks>
    private BCommunityCalendar()
    {
    }

    /// <summary>
    /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
    /// </summary>
    /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
    /// <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
    /// <remarks></remarks>
    /// <example> This sample shows how to create an instance of the class.
    /// <code language="VB.NET">
    /// <![CDATA[
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BCommunityCalendar mBAccount = BCommunityCalendar = New BCommunityCalendar(MSecurityEntity, ConfigSettings.CentralManagement);
    /// ]]>
    /// </code>
    /// <code language="C#">
    /// <![CDATA[
    /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
    /// MSecurityEntity.DAL = ConfigSettings.DAL
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
    /// 
    /// Dim mBAccount As BCommunityCalendar = New BCommunityCalendar(MSecurityEntity, ConfigSettings.CentralManagement)
    /// ]]>
    /// </code>
    /// </example>
    public BCommunityCalendar(MSecurityEntity securityEntityProfile, bool centralManagement)
    {
            if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
            if (!centralManagement)
            {
                if (m_DCommunityCalendar == null)
                {
                    m_DCommunityCalendar = (ICommunityCalendar)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DCommunityCalendar");
                }
            }
            else
            {
                m_DCommunityCalendar = (ICommunityCalendar)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DCommunityCalendar");
            }

            m_DCommunityCalendar.ConnectionString = securityEntityProfile.ConnectionString;
            m_DCommunityCalendar.SecurityEntitySeqId = securityEntityProfile.Id;
    }

    public bool GetCalendarData(ref DataSet calendarDataSet, String calendarName, DateTime startDate, DateTime endDate)
    {
        m_DCommunityCalendar.CalendarName = calendarName;
        try
        {
            m_DCommunityCalendar.GetCalendarData(ref calendarDataSet, startDate, endDate);
            return true;
        }
        catch (System.Exception ex)
        {
            throw new BusinessLogicLayerException("Could not retrieve the calendar data", ex);
        }
    }

    public bool SaveCalendarData(String calendarName, String comment, DateTime entryDate, int accountSeqId)
    {
        m_DCommunityCalendar.CalendarName = calendarName;
        throw new NotImplementedException();
    }

    public bool DeleteCalendarData(String calendarName, String comment, DateTime entryDate, int accountSeqId)
    {
        m_DCommunityCalendar.CalendarName = calendarName;
        throw new NotImplementedException();
    }
}