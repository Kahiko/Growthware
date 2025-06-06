using System;
using System.Data;
using System.Threading.Tasks;
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

    #region Member Fields
    private ICommunityCalendar m_DCommunityCalendar;
    #endregion

    #region Constructors
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
    /// <code language="C#">
    /// <![CDATA[
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BCommunityCalendar mBAccount = BCommunityCalendar = New BCommunityCalendar(MSecurityEntity);
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
    /// Dim mBAccount As BCommunityCalendar = New BCommunityCalendar(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BCommunityCalendar(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        // m_DCommunityCalendar = (ICommunityCalendar)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DCommunityCalendar");
        // m_DCommunityCalendar.ConnectionString = securityEntityProfile.ConnectionString;
        // m_DCommunityCalendar.SecurityEntitySeqId = securityEntityProfile.Id;
        if (m_DCommunityCalendar == null)
        {
            this.m_DCommunityCalendar = (ICommunityCalendar)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DCommunityCalendar", securityEntityProfile.ConnectionString, securityEntityProfile.Id);
            if (this.m_DCommunityCalendar == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DCommunityCalendar.");
            }
        }
    }
    #endregion

    public async Task<MCalendarEvent> GetEvent(int calendarEventSeqId)
    {
        try
        {
            m_DCommunityCalendar.CalendarSeqId = 1;
            return new(await m_DCommunityCalendar.GetEvent(calendarEventSeqId));
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<DataTable> GetEvents(int functionSeqId, DateTime startDate, DateTime endDate)
    {
        try
        {
            m_DCommunityCalendar.CalendarSeqId = 0; // not used in this method
            return await m_DCommunityCalendar.GetEvents(functionSeqId, startDate, endDate);
        }
        catch (System.Exception ex)
        {
            throw new BusinessLogicLayerException("Could not retrieve the calendar data", ex);
        }
    }

    public async Task<MCalendarEvent> SaveCalendarEvent(int functionSeqId, MCalendarEvent calendarEvent)
    {
        m_DCommunityCalendar.CalendarSeqId = 1; // not used in this method
        MCalendarEvent mRetVal = new(await m_DCommunityCalendar.SaveCalendarEvent(functionSeqId, calendarEvent));
        return mRetVal;
    }

    public async Task<bool> DeleteEvent(int calendarEventSeqId)
    {
        m_DCommunityCalendar.CalendarSeqId = 0; // not used in this method
        try
        {
            return await m_DCommunityCalendar.DeleteEvent(calendarEventSeqId);
        }
        catch (System.Exception ex)
        {
            throw new BusinessLogicLayerException("Could not delete the calendar event", ex);
        }
    }
}