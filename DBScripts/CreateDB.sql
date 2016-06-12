if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_ACCOUNT_CHOICES_ZB_ACCOUNTS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_ACCOUNT_CHOICES] DROP CONSTRAINT FK_ZB_ACCOUNT_CHOICES_ZB_ACCOUNTS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_ACCTS_SECURITY_ZB_ACCOUNTS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_ACCTS_SECURITY] DROP CONSTRAINT FK_ZB_ACCTS_SECURITY_ZB_ACCOUNTS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_BU_SECURITY_ZB_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_BU_SECURITY] DROP CONSTRAINT FK_ZB_BU_SECURITY_ZB_BUSINESS_UNITS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_BUSINESS_UNITS_ZB_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_BUSINESS_UNITS] DROP CONSTRAINT FK_ZB_BUSINESS_UNITS_ZB_BUSINESS_UNITS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_CALENDAR_ZB_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_CALENDAR] DROP CONSTRAINT FK_ZB_CALENDAR_ZB_BUSINESS_UNITS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_DIRECTORIES_ZB_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_DIRECTORIES] DROP CONSTRAINT FK_ZB_DIRECTORIES_ZB_BUSINESS_UNITS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_MESSAGES_ZB_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_MESSAGES] DROP CONSTRAINT FK_ZB_MESSAGES_ZB_BUSINESS_UNITS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_ACCTS_SECURITY_ZB_BU_SECURITY]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_ACCTS_SECURITY] DROP CONSTRAINT FK_ZB_ACCTS_SECURITY_ZB_BU_SECURITY
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_DROP_BOX_SECURITY_ZB_BU_SECURITY]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_DROP_BOX_SECURITY] DROP CONSTRAINT FK_ZB_DROP_BOX_SECURITY_ZB_BU_SECURITY
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_MODULES_SECURITY_ZB_BU_SECURITY]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_MODULES_SECURITY] DROP CONSTRAINT FK_ZB_MODULES_SECURITY_ZB_BU_SECURITY
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_DROP_BOX_DETAIL_ZB_DROP_BOXES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_DROP_BOX_DETAIL] DROP CONSTRAINT FK_ZB_DROP_BOX_DETAIL_ZB_DROP_BOXES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_DROP_BOX_SECURITY_ZB_DROP_BOXES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_DROP_BOX_SECURITY] DROP CONSTRAINT FK_ZB_DROP_BOX_SECURITY_ZB_DROP_BOXES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_BU_SECURITY_ZB_GRPS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_BU_SECURITY] DROP CONSTRAINT FK_ZB_BU_SECURITY_ZB_GRPS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_MODULES_ZB_MODULES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_MODULES] DROP CONSTRAINT FK_ZB_MODULES_ZB_MODULES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_MODULES_SECURITY_ZB_MODULES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_MODULES_SECURITY] DROP CONSTRAINT FK_ZB_MODULES_SECURITY_ZB_MODULES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_WORK_FLOWS_ZB_MODULES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_WORK_FLOWS] DROP CONSTRAINT FK_ZB_WORK_FLOWS_ZB_MODULES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_MODULES_ZB_NAVIGATION_TYPE]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_MODULES] DROP CONSTRAINT FK_ZB_MODULES_ZB_NAVIGATION_TYPE
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_MODULES_SECURITY_ZB_PERMISSIONS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_MODULES_SECURITY] DROP CONSTRAINT FK_ZB_MODULES_SECURITY_ZB_PERMISSIONS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_BU_SECURITY_ZB_RLS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_BU_SECURITY] DROP CONSTRAINT FK_ZB_BU_SECURITY_ZB_RLS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_NOTIFICATIONS_ZB_STATES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_NOTIFICATIONS] DROP CONSTRAINT FK_ZB_NOTIFICATIONS_ZB_STATES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_ZIPCODES_ZB_STATES]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_ZIPCODES] DROP CONSTRAINT FK_ZB_ZIPCODES_ZB_STATES
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_ACCOUNTS_ZB_SYSTEM_STATUS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_ACCOUNTS] DROP CONSTRAINT FK_ZB_ACCOUNTS_ZB_SYSTEM_STATUS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_BUSINESS_UNITS_ZB_SYSTEM_STATUS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_BUSINESS_UNITS] DROP CONSTRAINT FK_ZB_BUSINESS_UNITS_ZB_SYSTEM_STATUS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_DROP_BOX_DETAIL_ZB_SYSTEM_STATUS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_DROP_BOX_DETAIL] DROP CONSTRAINT FK_ZB_DROP_BOX_DETAIL_ZB_SYSTEM_STATUS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ZB_STATES_ZB_SYSTEM_STATUS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ZB_STATES] DROP CONSTRAINT FK_ZB_STATES_ZB_SYSTEM_STATUS
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBF_CHOOSE_BUSINESS_UNIT_ID]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[ZBF_CHOOSE_BUSINESS_UNIT_ID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBF_GET_DEFAULT_BUSINESS_UNIT_ID]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[ZBF_GET_DEFAULT_BUSINESS_UNIT_ID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_ACCOUNT_CHOICES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_ACCOUNT_CHOICES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_ACCOUNT_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_ACCOUNT_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_BUSINESS_UNIT_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_BUSINESS_UNIT_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_CALENDAR_DATA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_CALENDAR_DATA]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_DROP_BOX]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_DROP_BOX]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_DROP_BOX_DETAILS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_DROP_BOX_DETAILS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_DROP_BOX_ROLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_DROP_BOX_ROLE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_GROUP_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_GROUP_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_MODULE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_MODULE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_ROLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_ROLE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_UPDATE_DIRECTORY_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_UPDATE_DIRECTORY_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_ADD_WORK_FLOW_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_ADD_WORK_FLOW_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_ACCOUNT_GRPS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_ACCOUNT_GRPS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_ACCOUNT_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_ACCOUNT_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_ALL_ACCTS_ROLE_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_ALL_ACCTS_ROLE_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_CALENDAR_DATA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_CALENDAR_DATA]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_DROBOX_SECURITY_BY_ROLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_DROBOX_SECURITY_BY_ROLE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_DROP_BOX]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_DROP_BOX]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_DROP_BOX_SECURITY]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_DROP_BOX_SECURITY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_GROUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_GROUP]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_GROUP_ROLES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_GROUP_ROLES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_MODULE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_MODULE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_MODULE_SECTY_BY_ROLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_MODULE_SECTY_BY_ROLE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_MODULE_SECURITY]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_MODULE_SECURITY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_MODULE_SECURITY_BY_GROUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_MODULE_SECURITY_BY_GROUP]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_ROLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_ROLE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_DEL_WORK_FLOW_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_DEL_WORK_FLOW_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ACCOUNTS_BY_LETTER]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ACCOUNTS_BY_LETTER]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ACCOUNT_CHOICESINFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ACCOUNT_CHOICESINFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ACCOUNT_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ACCOUNT_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ACCTS_BY_LETTER]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ACCTS_BY_LETTER]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_ACCTS_N_ROLE_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_ACCTS_N_ROLE_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_ACCTS_ROLE_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_ACCTS_ROLE_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_ACTIVE_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_ACTIVE_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_ACTIVE_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_ACTIVE_BUSINESS_UNITS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_BUSINESSUNITS_ALL_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_BUSINESSUNITS_ALL_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_BU_ALL_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_BU_ALL_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_DIRECTORYINFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_DIRECTORYINFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_DROP_BOXES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_DROP_BOXES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_ENABLED_MODULES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_ENABLED_MODULES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_GRPS_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_GRPS_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_GRPS_FOR_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_GRPS_FOR_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_MESSAGES_FOR_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_MESSAGES_FOR_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_MODULES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_MODULES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_RLS_FOR_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_RLS_FOR_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ALL_STATES_ALL_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ALL_STATES_ALL_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_CALENDAR_DATA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_CALENDAR_DATA]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_DROP_BOXES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_DROP_BOXES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_DROP_BOX_BU_SELTD_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_DROP_BOX_BU_SELTD_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_DROP_BOX_DETAILS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_DROP_BOX_DETAILS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_GROUP_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_GROUP_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_GRPS_FOR_ACCOUNT]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_GRPS_FOR_ACCOUNT]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_GRPS_FOR_ACCOUNT_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_GRPS_FOR_ACCOUNT_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_HIERARCHICAL_MENU_DATA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_HIERARCHICAL_MENU_DATA]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_LEFT_NAVLINKS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_LEFT_NAVLINKS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_LINEMENU_NAVLINKS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_LINEMENU_NAVLINKS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_MESSAGE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_MESSAGE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_MESSAGE_BY_ID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_MESSAGE_BY_ID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_MODULE_BU_SELECTED_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_MODULE_BU_SELECTED_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_MODULE_BU_SELTD_GRPS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_MODULE_BU_SELTD_GRPS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_MODULE_GRPS_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_MODULE_GRPS_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_MODULE_RLS_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_MODULE_RLS_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_NAV_TYPES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_NAV_TYPES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_RLS_FOR_ACCOUNT]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_RLS_FOR_ACCOUNT]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_RLS_FOR_ACCOUNT_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_RLS_FOR_ACCOUNT_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_RLS_FOR_GROUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_RLS_FOR_GROUP]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_RLS_FOR_GROUP_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_RLS_FOR_GROUP_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ROLE_NAME_BY_ID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ROLE_NAME_BY_ID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_ROOT_LINKS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_ROOT_LINKS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_VAID_BU_FOR_ACCOUNT]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_VAID_BU_FOR_ACCOUNT]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_GET_WORK_FLOWS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_GET_WORK_FLOWS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_ACCOUNT_CHOICES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_ACCOUNT_CHOICES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_ACCOUNT_GRPS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_ACCOUNT_GRPS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_ACCOUNT_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_ACCOUNT_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_ACCOUNT_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_ACCOUNT_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_BUSINESS_UNIT_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_BUSINESS_UNIT_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_BU_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_BU_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_DROP_BOX]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_DROP_BOX]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_DROP_BOX_DETAILS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_DROP_BOX_DETAILS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_GROUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_GROUP]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_GROUP_INFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_GROUP_INFO]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_GROUP_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_GROUP_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_MESSAGE_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_MESSAGE_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_MODULE_GRPS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_MODULE_GRPS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_MODULE_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_MODULE_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_MODULE_RLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_MODULE_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_ROLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_ROLE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_STATE_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_STATE_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPDATE_WORK_FLOW_PROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPDATE_WORK_FLOW_PROFILE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZBP_UPD_ALL_ACCTS_ROLE_BY_BU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZBP_UPD_ALL_ACCTS_ROLE_BY_BU]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_ACCOUNTS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_ACCOUNTS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_ACCOUNT_CHOICES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_ACCOUNT_CHOICES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_ACCTS_SECURITY]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_ACCTS_SECURITY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_BUSINESS_UNITS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_BUSINESS_UNITS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_BU_SECURITY]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_BU_SECURITY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_CALENDAR]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_CALENDAR]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_DIRECTORIES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_DIRECTORIES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_DROP_BOXES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_DROP_BOXES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_DROP_BOX_DETAIL]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_DROP_BOX_DETAIL]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_DROP_BOX_SECURITY]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_DROP_BOX_SECURITY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_GRPS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_GRPS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_MESSAGES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_MESSAGES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_MODULES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_MODULES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_MODULES_SECURITY]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_MODULES_SECURITY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_NAVIGATION_TYPE]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_NAVIGATION_TYPE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_NOTIFICATIONS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_NOTIFICATIONS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_PERMISSIONS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_PERMISSIONS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_RLS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_RLS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_STATES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_STATES]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_SYSTEM_STATUS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_SYSTEM_STATUS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_WORK_FLOWS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_WORK_FLOWS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZB_ZIPCODES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ZB_ZIPCODES]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE   FUNCTION ZBF_CHOOSE_BUSINESS_UNIT_ID(
@IS_SYSTEM INT,
@BUSINESS_UNIT_ID INT
) 
RETURNS int AS 
BEGIN
	DECLARE @THE_BUSINESS_UNIT_ID INT
	
	IF(@IS_SYSTEM = 1)
		SET @THE_BUSINESS_UNIT_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()
	ELSE
		SET @THE_BUSINESS_UNIT_ID = @BUSINESS_UNIT_ID
RETURN @THE_BUSINESS_UNIT_ID
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  FUNCTION ZBF_GET_DEFAULT_BUSINESS_UNIT_ID () 
RETURNS int AS 
BEGIN
 
 RETURN 1
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE TABLE [dbo].[ZB_ACCOUNTS] (
	[ACCOUNT_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[SYSTEM_STATUS_ID] [int] NOT NULL ,
	[ACCOUNT] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FIRST_NAME] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LAST_NAME] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[MIDDLE_NAME] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PREFERED_NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EMAIL] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PWD] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FAILED_ATTEMPTS] [int] NOT NULL ,
	[ADDED_BY] [int] NOT NULL ,
	[ADDED_DATE] [datetime] NULL ,
	[LAST_LOGIN] [datetime] NULL ,
	[TIME_ZONE] [int] NULL ,
	[LOCATION] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ENABLE_NOTIFICATIONS] [bit] NULL ,
	[UPDATED_BY] [int] NOT NULL ,
	[UPDATED_DATE] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_ACCOUNT_CHOICES] (
	[ACCOUNT] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[BUSINESS_UNIT_SEQ_ID] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[BUSINESS_UNIT_NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[BACK_COLOR] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LEFT_COLOR] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[HEAD_COLOR] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[SUB_HEAD_COLOR] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[COLOR_SCHEME] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[MODULE_ACTION] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[RECORDS_PER_PAGE] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_ACCTS_SECURITY] (
	[ACCOUNT_SEQ_ID] [int] NOT NULL ,
	[BU_SECURITY_SEQ_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_BUSINESS_UNITS] (
	[BUSINESS_UNIT_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DESCRIPTION] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[SKIN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PARENT_BUSINESS_UNIT_SEQ_ID] [int] NOT NULL ,
	[CONNECTION_STRING] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[STATUS_SEQ_ID] [int] NOT NULL ,
	[DAL] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_BU_SECURITY] (
	[BU_SECURITY_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[BUSINESS_UNIT_SEQ_ID] [int] NOT NULL ,
	[GROUP_SEQ_ID] [int] NULL ,
	[ROLE_SEQ_ID] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_CALENDAR] (
	[BUSINESS_UNIT_SEQ_ID] [int] NOT NULL ,
	[CALENDAR_NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ENTRY_DATE] [smalldatetime] NOT NULL ,
	[COMMENT] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ACTIVE] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_DIRECTORIES] (
	[DIRECTORY_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[BUSINESS_UNIT_SEQ_ID] [int] NOT NULL ,
	[DIRECTORY] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[IMPERSONATE] [bit] NOT NULL ,
	[IMPERSONATE_ACCOUNT] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[IMPERSONATE_PWD] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_DROP_BOXES] (
	[DROP_BOX_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[DESCRIPTION] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ADDED_BY] [int] NOT NULL ,
	[ADDED_DATE] [datetime] NOT NULL ,
	[UPDATED_BY] [int] NULL ,
	[UPDATED_DATE] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_DROP_BOX_DETAIL] (
	[DROP_BOX_DET_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[DROP_BOX_SEQ_ID] [int] NOT NULL ,
	[DROP_BOX_DET_CODE] [int] NOT NULL ,
	[DROP_BOX_DET_VALUE] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DROP_BOX_DET_STATUS] [int] NOT NULL ,
	[ADDED_BY] [int] NOT NULL ,
	[ADDED_DATE] [datetime] NOT NULL ,
	[UPDATED_BY] [int] NULL ,
	[UPDATED_DATE] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_DROP_BOX_SECURITY] (
	[DROP_BOX_SEQ_ID] [int] NOT NULL ,
	[BU_SECURITY_SEQ_ID] [int] NOT NULL ,
	[PERMISSIONS_SEQ_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_GRPS] (
	[GROUP_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[GROUP_NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DESCRIPTION] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_MESSAGES] (
	[MESSAGE_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[BUSINESS_UNIT_SEQ_ID] [int] NOT NULL ,
	[NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[TITLE] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DESCRIPTION] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[BODY] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_MODULES] (
	[MODULE_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[NAME] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DESCRIPTION] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[SOURCE] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ENABLE_VIEW_STATE] [bit] NOT NULL ,
	[PARENT_MODULE_SEQ_ID] [int] NOT NULL ,
	[IS_NAV] [bit] NOT NULL ,
	[NAV_TYPE_SEQ_ID] [int] NOT NULL ,
	[MODULE_ACTION] [nchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_MODULES_SECURITY] (
	[MODULE_SEQ_ID] [int] NOT NULL ,
	[BU_SECURITY_SEQ_ID] [int] NOT NULL ,
	[PERMISSIONS_SEQ_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_NAVIGATION_TYPE] (
	[NAV_TYPE_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[DESCRIPTION] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_NOTIFICATIONS] (
	[NOTIFICATION_SEQ_ID] [int] NOT NULL ,
	[STATE] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ORIGINAL_MESSAGE_ID] [int] NOT NULL ,
	[ACCOUNT_SEQ_ID] [int] NOT NULL ,
	[SEND_MESSAGE_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_PERMISSIONS] (
	[PERMISSIONS_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[DESCRIPTION] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_RLS] (
	[ROLE_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[ROLE_NAME] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DESCRIPTION] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[IS_SYSTEM] [bit] NOT NULL ,
	[IS_SYSTEM_ONLY] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_STATES] (
	[STATE] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DESCRIPTION] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[STATUS_SEQ_ID] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_SYSTEM_STATUS] (
	[STATUS_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[DESCRIPTION] [char] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_WORK_FLOWS] (
	[WORK_FLOW_SEQ_ID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[ORDER_ID] [int] NOT NULL ,
	[WORK_FLOW_NAME] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[MODULE_SEQ_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ZB_ZIPCODES] (
	[STATE] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ZIP_CODE] [int] NOT NULL ,
	[AREA_CODE] [int] NOT NULL ,
	[CITY] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[TIME_ZONE] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ZB_ACCOUNTS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_ACCOUNTS] PRIMARY KEY  CLUSTERED 
	(
		[ACCOUNT_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_BUSINESS_UNITS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_BUSINESS_UNITS] PRIMARY KEY  CLUSTERED 
	(
		[BUSINESS_UNIT_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_BU_SECURITY] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_BU_SECURITY] PRIMARY KEY  CLUSTERED 
	(
		[BU_SECURITY_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_DIRECTORIES] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_DIRECTORIES] PRIMARY KEY  CLUSTERED 
	(
		[DIRECTORY_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_DROP_BOXES] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_DROP_BOXES] PRIMARY KEY  CLUSTERED 
	(
		[DROP_BOX_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_GRPS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_GRPS] PRIMARY KEY  CLUSTERED 
	(
		[GROUP_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_MESSAGES] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_MESSAGES] PRIMARY KEY  CLUSTERED 
	(
		[MESSAGE_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_MODULES] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_MODULES] PRIMARY KEY  CLUSTERED 
	(
		[MODULE_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_NAVIGATION_TYPE] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_NAVIGATION_TYPE] PRIMARY KEY  CLUSTERED 
	(
		[NAV_TYPE_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_NOTIFICATIONS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_NOTIFICATIONS] PRIMARY KEY  CLUSTERED 
	(
		[NOTIFICATION_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_PERMISSIONS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_PERMISSIONS] PRIMARY KEY  CLUSTERED 
	(
		[PERMISSIONS_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_RLS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_RLS] PRIMARY KEY  CLUSTERED 
	(
		[ROLE_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_STATES] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_STATES] PRIMARY KEY  CLUSTERED 
	(
		[STATE]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_SYSTEM_STATUS] WITH NOCHECK ADD 
	CONSTRAINT [PK_ZB_SYSTEM_STATUS] PRIMARY KEY  CLUSTERED 
	(
		[STATUS_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_ACCOUNTS] ADD 
	CONSTRAINT [UK_ZB_ACCOUNTS] UNIQUE  NONCLUSTERED 
	(
		[ACCOUNT]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_ACCOUNT_CHOICES] ADD 
	CONSTRAINT [UK_ZB_ACCOUNT_CHOICES] UNIQUE  NONCLUSTERED 
	(
		[ACCOUNT]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_BU_SECURITY] ADD 
	CONSTRAINT [UK_ZB_BU_SECURITY] UNIQUE  NONCLUSTERED 
	(
		[BUSINESS_UNIT_SEQ_ID],
		[GROUP_SEQ_ID],
		[ROLE_SEQ_ID]
	)  ON [PRIMARY] 
GO

 CREATE  INDEX [UK_ZB_DROP_BOX_SECURITY] ON [dbo].[ZB_DROP_BOX_SECURITY]([DROP_BOX_SEQ_ID], [BU_SECURITY_SEQ_ID], [PERMISSIONS_SEQ_ID]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ZB_MODULES_SECURITY] ADD 
	CONSTRAINT [UK_ZB_MODULES_SECURITY] UNIQUE  NONCLUSTERED 
	(
		[MODULE_SEQ_ID],
		[BU_SECURITY_SEQ_ID],
		[PERMISSIONS_SEQ_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ZB_ACCOUNTS] ADD 
	CONSTRAINT [FK_ZB_ACCOUNTS_ZB_SYSTEM_STATUS] FOREIGN KEY 
	(
		[SYSTEM_STATUS_ID]
	) REFERENCES [dbo].[ZB_SYSTEM_STATUS] (
		[STATUS_SEQ_ID]
	)
GO

ALTER TABLE [dbo].[ZB_ACCOUNT_CHOICES] ADD 
	CONSTRAINT [FK_ZB_ACCOUNT_CHOICES_ZB_ACCOUNTS] FOREIGN KEY 
	(
		[ACCOUNT]
	) REFERENCES [dbo].[ZB_ACCOUNTS] (
		[ACCOUNT]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_ACCTS_SECURITY] ADD 
	CONSTRAINT [FK_ZB_ACCTS_SECURITY_ZB_ACCOUNTS] FOREIGN KEY 
	(
		[ACCOUNT_SEQ_ID]
	) REFERENCES [dbo].[ZB_ACCOUNTS] (
		[ACCOUNT_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_ACCTS_SECURITY_ZB_BU_SECURITY] FOREIGN KEY 
	(
		[BU_SECURITY_SEQ_ID]
	) REFERENCES [dbo].[ZB_BU_SECURITY] (
		[BU_SECURITY_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_BUSINESS_UNITS] ADD 
	CONSTRAINT [FK_ZB_BUSINESS_UNITS_ZB_BUSINESS_UNITS] FOREIGN KEY 
	(
		[PARENT_BUSINESS_UNIT_SEQ_ID]
	) REFERENCES [dbo].[ZB_BUSINESS_UNITS] (
		[BUSINESS_UNIT_SEQ_ID]
	),
	CONSTRAINT [FK_ZB_BUSINESS_UNITS_ZB_SYSTEM_STATUS] FOREIGN KEY 
	(
		[STATUS_SEQ_ID]
	) REFERENCES [dbo].[ZB_SYSTEM_STATUS] (
		[STATUS_SEQ_ID]
	)
GO

ALTER TABLE [dbo].[ZB_BU_SECURITY] ADD 
	CONSTRAINT [FK_ZB_BU_SECURITY_ZB_BUSINESS_UNITS] FOREIGN KEY 
	(
		[BUSINESS_UNIT_SEQ_ID]
	) REFERENCES [dbo].[ZB_BUSINESS_UNITS] (
		[BUSINESS_UNIT_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_BU_SECURITY_ZB_GRPS] FOREIGN KEY 
	(
		[GROUP_SEQ_ID]
	) REFERENCES [dbo].[ZB_GRPS] (
		[GROUP_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_BU_SECURITY_ZB_RLS] FOREIGN KEY 
	(
		[ROLE_SEQ_ID]
	) REFERENCES [dbo].[ZB_RLS] (
		[ROLE_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_CALENDAR] ADD 
	CONSTRAINT [FK_ZB_CALENDAR_ZB_BUSINESS_UNITS] FOREIGN KEY 
	(
		[BUSINESS_UNIT_SEQ_ID]
	) REFERENCES [dbo].[ZB_BUSINESS_UNITS] (
		[BUSINESS_UNIT_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_DIRECTORIES] ADD 
	CONSTRAINT [FK_ZB_DIRECTORIES_ZB_BUSINESS_UNITS] FOREIGN KEY 
	(
		[BUSINESS_UNIT_SEQ_ID]
	) REFERENCES [dbo].[ZB_BUSINESS_UNITS] (
		[BUSINESS_UNIT_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_DROP_BOX_DETAIL] ADD 
	CONSTRAINT [FK_ZB_DROP_BOX_DETAIL_ZB_DROP_BOXES] FOREIGN KEY 
	(
		[DROP_BOX_SEQ_ID]
	) REFERENCES [dbo].[ZB_DROP_BOXES] (
		[DROP_BOX_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_DROP_BOX_DETAIL_ZB_SYSTEM_STATUS] FOREIGN KEY 
	(
		[DROP_BOX_DET_STATUS]
	) REFERENCES [dbo].[ZB_SYSTEM_STATUS] (
		[STATUS_SEQ_ID]
	)
GO

ALTER TABLE [dbo].[ZB_DROP_BOX_SECURITY] ADD 
	CONSTRAINT [FK_ZB_DROP_BOX_SECURITY_ZB_BU_SECURITY] FOREIGN KEY 
	(
		[BU_SECURITY_SEQ_ID]
	) REFERENCES [dbo].[ZB_BU_SECURITY] (
		[BU_SECURITY_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_DROP_BOX_SECURITY_ZB_DROP_BOXES] FOREIGN KEY 
	(
		[DROP_BOX_SEQ_ID]
	) REFERENCES [dbo].[ZB_DROP_BOXES] (
		[DROP_BOX_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_MESSAGES] ADD 
	CONSTRAINT [FK_ZB_MESSAGES_ZB_BUSINESS_UNITS] FOREIGN KEY 
	(
		[BUSINESS_UNIT_SEQ_ID]
	) REFERENCES [dbo].[ZB_BUSINESS_UNITS] (
		[BUSINESS_UNIT_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_MODULES] ADD 
	CONSTRAINT [FK_ZB_MODULES_ZB_MODULES] FOREIGN KEY 
	(
		[PARENT_MODULE_SEQ_ID]
	) REFERENCES [dbo].[ZB_MODULES] (
		[MODULE_SEQ_ID]
	),
	CONSTRAINT [FK_ZB_MODULES_ZB_NAVIGATION_TYPE] FOREIGN KEY 
	(
		[NAV_TYPE_SEQ_ID]
	) REFERENCES [dbo].[ZB_NAVIGATION_TYPE] (
		[NAV_TYPE_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_MODULES_SECURITY] ADD 
	CONSTRAINT [FK_ZB_MODULES_SECURITY_ZB_BU_SECURITY] FOREIGN KEY 
	(
		[BU_SECURITY_SEQ_ID]
	) REFERENCES [dbo].[ZB_BU_SECURITY] (
		[BU_SECURITY_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_MODULES_SECURITY_ZB_MODULES] FOREIGN KEY 
	(
		[MODULE_SEQ_ID]
	) REFERENCES [dbo].[ZB_MODULES] (
		[MODULE_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_ZB_MODULES_SECURITY_ZB_PERMISSIONS] FOREIGN KEY 
	(
		[PERMISSIONS_SEQ_ID]
	) REFERENCES [dbo].[ZB_PERMISSIONS] (
		[PERMISSIONS_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_NOTIFICATIONS] ADD 
	CONSTRAINT [FK_ZB_NOTIFICATIONS_ZB_STATES] FOREIGN KEY 
	(
		[STATE]
	) REFERENCES [dbo].[ZB_STATES] (
		[STATE]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_STATES] ADD 
	CONSTRAINT [FK_ZB_STATES_ZB_SYSTEM_STATUS] FOREIGN KEY 
	(
		[STATUS_SEQ_ID]
	) REFERENCES [dbo].[ZB_SYSTEM_STATUS] (
		[STATUS_SEQ_ID]
	)
GO

ALTER TABLE [dbo].[ZB_WORK_FLOWS] ADD 
	CONSTRAINT [FK_ZB_WORK_FLOWS_ZB_MODULES] FOREIGN KEY 
	(
		[MODULE_SEQ_ID]
	) REFERENCES [dbo].[ZB_MODULES] (
		[MODULE_SEQ_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[ZB_ZIPCODES] ADD 
	CONSTRAINT [FK_ZB_ZIPCODES_ZB_STATES] FOREIGN KEY 
	(
		[STATE]
	) REFERENCES [dbo].[ZB_STATES] (
		[STATE]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_ACCOUNT_CHOICES (
	@P_ACCOUNT			NVARCHAR(50),
	@P_DEFAULT_ACCOUNT	NVARCHAR(50) = NULL,
	@P_ADDUPD_BY		INT
)
AS
	BEGIN
		DECLARE @BUSINESS_UNIT_SEQ_ID NVARCHAR(10)
		DECLARE @BUSINESS_UNIT_NAME NVARCHAR(50)
		DECLARE @BACK_COLOR NVARCHAR(15)
		DECLARE @LEFT_COLOR NVARCHAR(15)
		DECLARE @HEAD_COLOR NVARCHAR(15)
		DECLARE @SUB_HEAD_COLOR NVARCHAR(15)
		DECLARE @COLOR_SCHEME NVARCHAR(15)
		DECLARE @MODULE_ACTION NVARCHAR(25)
		DECLARE @RECORDS_PER_PAGE NVARCHAR(1000)
		IF @P_DEFAULT_ACCOUNT= NULL SET @P_DEFAULT_ACCOUNT = 'DEFAULT'
		SELECT -- FILL THE DEFAULT VALUES
			@BUSINESS_UNIT_SEQ_ID = BUSINESS_UNIT_SEQ_ID,
			@BUSINESS_UNIT_NAME = BUSINESS_UNIT_NAME,
			@BACK_COLOR = BACK_COLOR,
			@LEFT_COLOR = LEFT_COLOR,
			@HEAD_COLOR = HEAD_COLOR,
			@SUB_HEAD_COLOR = SUB_HEAD_COLOR,
			@COLOR_SCHEME = COLOR_SCHEME,
			@MODULE_ACTION = MODULE_ACTION,
			@RECORDS_PER_PAGE = RECORDS_PER_PAGE
		FROM
			ZB_ACCOUNT_CHOICES
		WHERE 
			ACCOUNT = @P_DEFAULT_ACCOUNT
			-- INSERT PROFILE
			INSERT INTO ZB_ACCOUNT_CHOICES VALUES(
				@P_ACCOUNT,
				@BUSINESS_UNIT_SEQ_ID,
				@BUSINESS_UNIT_NAME,
				@BACK_COLOR,
				@LEFT_COLOR,
				@HEAD_COLOR,
				@SUB_HEAD_COLOR,
				@COLOR_SCHEME,
				@MODULE_ACTION,
				@RECORDS_PER_PAGE
				)
	END
	RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_ACCOUNT_PROFILE (
	@P_SYSTEM_STATUS_ID	INT = 1, -- SYSTEM STATUS ID TAKEN FROM ZB_SYSTEM_STATUS .. 1 = CHANGE PASSWORD
	@P_ACCOUNT			NVARCHAR(50),
	@P_FIRST_NAME			NVARCHAR(30),
	@P_LAST_NAME			NVARCHAR(30),
	@P_MIDDLE_NAME		NVARCHAR(30),
	@P_PREFERED_NAME		NCHAR(90),
	@P_EMAIL			NVARCHAR(50),
	@P_PWD				NVARCHAR(256),
	@P_FAILED_ATTEMPTS		INT,
	@P_CREATED_BY		INT,
	@P_DATE_CREATED		DATETIME,
	@P_LAST_LOGIN			DATETIME,
	@P_TIME_ZONE			INT,
	@P_LOCATION			NVARCHAR(100),
	@P_ENABLENOTIFICATIONS 		BIT,
	@P_UPDATED_BY		INT,
	@P_UPDATED_DATE		DATETIME,
	@P_DEFAULT_CLIENTCHOICES_ACCOUNT NVARCHAR(50),
	@P_ADDUPD_BY		INT
)
AS
BEGIN
	DECLARE @ROLE_ID_AUTHENTICATED INT
	SET @ROLE_ID_AUTHENTICATED = 3
	IF (SELECT COUNT(*) FROM ZB_ACCOUNTS WHERE ACCOUNT=@P_ACCOUNT) = 0
		BEGIN
			INSERT INTO ZB_ACCOUNTS VALUES (
					@P_SYSTEM_STATUS_ID,
					@P_ACCOUNT,
					@P_FIRST_NAME,
					@P_LAST_NAME,
					@P_MIDDLE_NAME,
					@P_PREFERED_NAME,
					@P_EMAIL,
					@P_PWD,
					@P_FAILED_ATTEMPTS,
					@P_CREATED_BY,
					@P_DATE_CREATED,
					@P_LAST_LOGIN,
					@P_TIME_ZONE,
					@P_LOCATION,
					@P_ENABLENOTIFICATIONS,
					@P_UPDATED_BY,
					@P_UPDATED_DATE
			)
			DECLARE @THENEWID AS INT
			SET @THENEWID = (SELECT ACCOUNT_SEQ_ID FROM ZB_ACCOUNTS WHERE ACCOUNT=@P_ACCOUNT)
			DECLARE @BU_SECURITY_SEQ_ID AS INT
			SET @BU_SECURITY_SEQ_ID = (
						SELECT
							BU_SECURITY_SEQ_ID
						FROM
							ZB_BU_SECURITY
						WHERE
							ROLE_SEQ_ID = @ROLE_ID_AUTHENTICATED AND
							BUSINESS_UNIT_SEQ_ID = (SELECT DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
							AND GROUP_SEQ_ID IS NULL
					)
			
			INSERT INTO ZB_ACCTS_SECURITY(BU_SECURITY_SEQ_ID,ACCOUNT_SEQ_ID) VALUES (@BU_SECURITY_SEQ_ID,@THENEWID)
			EXEC ZBP_ADD_ACCOUNT_CHOICES @P_ACCOUNT,@P_DEFAULT_CLIENTCHOICES_ACCOUNT,@P_ADDUPD_BY
		END
	ELSE
	BEGIN
		RAISERROR ('THE ACCOUNT YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
		RETURN	
	END
END
IF @@ERROR = 0 RETURN 1
ELSE
RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_BUSINESS_UNIT_PROFILE (
	@P_NAME				NVARCHAR(50),
	@P_DESCRIPTION			NVARCHAR(50),
	@P_SKIN					NVARCHAR(50),
	@P_PARENT_BUSINESS_UNIT_SEQ_ID 	INT,
	@P_STATUS_SEQ_ID 			INT,
	@P_CONNECTION_STRING		NVARCHAR(50),
	@P_DAL				NVARCHAR(50),
	@P_ADDUPD_BY	INT
)
 AS
	-- CHECK FOR DUPLICATE NAME
	IF EXISTS( 
		SELECT [NAME] 
		FROM ZB_BUSINESS_UNITS
		WHERE [NAME] = @P_NAME
	)BEGIN
		RAISERROR ('THE ENTRY ALREADY EXISTS IN THE DATABASE.',16,1)
		RETURN
	END
	INSERT ZB_BUSINESS_UNITS(
		[NAME],
		[DESCRIPTION],
		SKIN,
		PARENT_BUSINESS_UNIT_SEQ_ID,
		STATUS_SEQ_ID,
		CONNECTION_STRING,
		DAL	
	)
	VALUES
	(
		@P_NAME,
		@P_DESCRIPTION,
		@P_SKIN,
		@P_PARENT_BUSINESS_UNIT_SEQ_ID,
		@P_STATUS_SEQ_ID,
		@P_CONNECTION_STRING,
		@P_DAL
	)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_CALENDAR_DATA(
	@P_BUSINESS_UNIT_SEQ_ID		INT,
	@P_CALENDAR_NAME		NVARCHAR(50),
	@P_ENTRY			NVARCHAR(100),
	@P_ENTRYDATE 			SMALLDATETIME,
	@P_ADDUPD_BY	INT
)
AS
	DECLARE @ACTIVE AS BIT
	SELECT @ACTIVE = 1
	IF (
		SELECT
			COUNT(*) 
		FROM ZB_CALENDAR
		WHERE 
			BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
			CALENDAR_NAME = @P_CALENDAR_NAME AND
			ENTRY_DATE = @P_ENTRYDATE AND
			COMMENT = @P_ENTRY
	) > 0
		BEGIN -- DO AN UPDATE
			
			UPDATE
				ZB_CALENDAR
			SET
				ACTIVE = @ACTIVE
			WHERE
				BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
				CALENDAR_NAME = @P_CALENDAR_NAME AND
				ENTRY_DATE = @P_ENTRYDATE AND
				COMMENT = @P_ENTRY
		END
	ELSE
		BEGIN -- TRY TO INSERT
			INSERT INTO ZB_CALENDAR(
				BUSINESS_UNIT_SEQ_ID,
				CALENDAR_NAME,
				ENTRY_DATE,
				COMMENT,
				ACTIVE
			)
			VALUES(
				@P_BUSINESS_UNIT_SEQ_ID,
				@P_CALENDAR_NAME,
				@P_ENTRYDATE,
				@P_ENTRY,
				@ACTIVE
			)
		END
IF @@ERROR = 0 RETURN 1 ELSE RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_DROP_BOX(
	@P_ACCOUNT_SEQ_ID	INT,
	@P_DESCRIPTION	NVARCHAR(255),
	@P_ADDUPD_BY	INT
)
AS
	DECLARE @RETURN_VALUE AS INT
-- CHECK FOR DUPLICATE NAME
	IF EXISTS( 
		SELECT [DESCRIPTION]
		FROM ZB_DROP_BOXES
		WHERE [DESCRIPTION] = @P_DESCRIPTION
	)
	BEGIN
		RAISERROR ('THE DROP DOWN BOX YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
		RETURN
	END
	INSERT ZB_DROP_BOXES(
		[DESCRIPTION],
		ADDED_BY,
		ADDED_DATE
	)
	VALUES
	(
		@P_DESCRIPTION,
		@P_ACCOUNT_SEQ_ID,
		GETDATE()
	)
	SET @RETURN_VALUE = (SELECT DROP_BOX_SEQ_ID FROM ZB_DROP_BOXES WHERE [DESCRIPTION] = @P_DESCRIPTION)
	RETURN @RETURN_VALUE

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_DROP_BOX_DETAILS
@P_CODE INT,
@P_VALUE VARCHAR(300),
@P_STATUS INT,
@P_DROP_BOX_SEQ_ID INT,
@P_ACCOUNT_SEQ_ID INT,
@P_ADDUPD_BY INT
AS
BEGIN
IF NOT EXISTS(SELECT * FROM ZB_DROP_BOX_DETAIL
		WHERE ZB_DROP_BOX_DETAIL.DROP_BOX_DET_CODE = @P_CODE AND 
			  ZB_DROP_BOX_DETAIL.DROP_BOX_DET_VALUE = @P_VALUE AND 
			  ZB_DROP_BOX_DETAIL.DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID	) 
	BEGIN
		INSERT INTO ZB_DROP_BOX_DETAIL
			(DROP_BOX_DET_CODE,
			DROP_BOX_DET_VALUE,
			DROP_BOX_SEQ_ID,
		    DROP_BOX_DET_STATUS,
		    ADDED_BY,	
			ADDED_DATE)
		VALUES (@P_CODE,
			    @P_VALUE,
				@P_DROP_BOX_SEQ_ID,
				@P_STATUS,
				@P_ACCOUNT_SEQ_ID,
				GETDATE())
		RETURN @@ERROR
	END
ELSE
    BEGIN
	RETURN -1
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_DROP_BOX_ROLE (
	@P_ROLE NVARCHAR(100),
	@P_DROP_BOX_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_PERMISSIONS_SEQ_ID INT,
	@P_ADDUPD_BY	INT
) 
AS
	DECLARE @ROLE_SEQ_ID AS INT
	DECLARE @BU_SECURITY_SEQ_ID AS INT
-- GET THE ROLE_SEQ_ID
	SET @ROLE_SEQ_ID = (SELECT ZB_RLS.ROLE_SEQ_ID FROM ZB_RLS WHERE ROLE_NAME=@P_ROLE)
	SET @BU_SECURITY_SEQ_ID = (
				SELECT
					BU_SECURITY_SEQ_ID
				FROM
					ZB_BU_SECURITY
				WHERE
					ROLE_SEQ_ID = @ROLE_SEQ_ID AND
					BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID)
	IF NOT EXISTS(
		SELECT 
			BU_SECURITY_SEQ_ID 
		FROM 
			ZB_DROP_BOX_SECURITY 
		WHERE 
				DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID AND
				BU_SECURITY_SEQ_ID = @BU_SECURITY_SEQ_ID AND
				PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID
		)
		BEGIN
			INSERT ZB_DROP_BOX_SECURITY (
				DROP_BOX_SEQ_ID,
				BU_SECURITY_SEQ_ID,
				PERMISSIONS_SEQ_ID
			)
			VALUES (
				@P_DROP_BOX_SEQ_ID,
				@BU_SECURITY_SEQ_ID,
				@P_PERMISSIONS_SEQ_ID
			)
		END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE     PROCEDURE ZBP_ADD_GROUP_INFO (
		@P_BUSINESS_UNIT_SEQ_ID		INT,
		@P_GROUP_NAME			NVARCHAR(50),
		@P_GROUP_DESCRIPTION		NVARCHAR(50),
		@P_ADDUPD_BY			INT
)
AS
BEGIN
	DECLARE 	@GROUP_SEQ_ID INT
	SELECT  @GROUP_SEQ_ID = GROUP_SEQ_ID FROM ZB_GRPS WHERE GROUP_NAME = @P_GROUP_NAME
	IF @GROUP_SEQ_ID IS NULL
	BEGIN
		INSERT INTO ZB_GRPS(
				GROUP_NAME,
				[DESCRIPTION]) 
			VALUES (
				@P_GROUP_NAME,
				@P_GROUP_DESCRIPTION
				)
		SET @GROUP_SEQ_ID = SCOPE_IDENTITY()
		INSERT INTO ZB_BU_SECURITY(
			BUSINESS_UNIT_SEQ_ID,GROUP_SEQ_ID) 
			VALUES 
			(@P_BUSINESS_UNIT_SEQ_ID,@GROUP_SEQ_ID)
	END
	ELSE
	BEGIN
		--IF THE GROUP NAME ALREADY EXISTS IN THE ROUP TABE AND IF 
		--IT IS NOT AVAILABLE IN THE BUSINESS SECURITY TABLE FOR THE 
		--BUSINESS UNIT THEN CREATE AN ENTRY IN THERE :-)
		IF(SELECT COUNT(*) FROM ZB_BU_SECURITY WHERE BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND GROUP_SEQ_ID = @GROUP_SEQ_ID AND ROLE_SEQ_ID IS NULL) = 0 
		BEGIN
			INSERT INTO ZB_BU_SECURITY(
			BUSINESS_UNIT_SEQ_ID,GROUP_SEQ_ID) 
			VALUES 
			(@P_BUSINESS_UNIT_SEQ_ID,@GROUP_SEQ_ID)	
		END
		ELSE
		BEGIN
			RAISERROR ('THE GROUP YOU ENTERED ALREADY EXISTS IN THE DATABASE FOR THE SELECTED BUSINESS UNIT.',16,1)
			RETURN
		END
	END
	IF @@ERROR <> 0			
	BEGIN
		RAISERROR ('ERROR CREATING A NEW GROUP!!',16,1) 	
		RETURN 0		
	END
	ELSE
		RETURN @GROUP_SEQ_ID	
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_MODULE (
	@P_NAME			NVARCHAR(255),
	@P_DESCRIPTION		NVARCHAR(255),
	@P_SOURCE		NVARCHAR(511),
	@P_ENABLE_VIEW_STATE	BIT,
	@P_IS_NAV		BIT,
	@P_NAV_TYPE_SEQ_ID	INT,
	@P_PARENTID		INT,
	@P_ACTION		NVARCHAR(255),
	@P_ADDUPD_BY		INT
)
AS
-- CHECK FOR DUPLICATE NAME
	IF EXISTS( 
		SELECT MODULE_ACTION 
		FROM ZB_MODULES
		WHERE MODULE_ACTION = @P_ACTION
	)BEGIN
	RAISERROR ('THE MODULE YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
	RETURN
END
INSERT ZB_MODULES(
	NAME,
	DESCRIPTION,
	SOURCE,
	ENABLE_VIEW_STATE,
	PARENT_MODULE_SEQ_ID,
	IS_NAV,
	NAV_TYPE_SEQ_ID,
	MODULE_ACTION
)
VALUES
(
	@P_NAME,
	@P_DESCRIPTION,
	@P_SOURCE,
	@P_ENABLE_VIEW_STATE,
	@P_PARENTID,
	@P_IS_NAV,
	@P_NAV_TYPE_SEQ_ID,
	@P_ACTION
)
RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_ROLE (
	@P_ROLE_NAME NVARCHAR(25),
	@P_DESCRIPTION NVARCHAR(25),
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY	INT
) 
AS
	DECLARE @RLS_SEQ_ID INT
	IF (SELECT COUNT(*) FROM ZB_RLS WHERE IS_SYSTEM_ONLY = 1 AND ROLE_NAME = @P_ROLE_NAME) > 0
	BEGIN
		DECLARE @MYMSG AS NVARCHAR(128)
		SET @MYMSG = 'THE ROLE YOU ENTERED ' + @P_ROLE_NAME + ' IS FOR SYSTEM USE ONLY.'
		RAISERROR (@MYMSG,16,1)
		RETURN
	END
		IF (SELECT COUNT(*) FROM ZB_RLS WHERE ROLE_NAME=@P_ROLE_NAME) = 0
		BEGIN -- ADD ROLE TO RLS TABLE
			INSERT ZB_RLS (
				ROLE_NAME,
				DESCRIPTION,
				IS_SYSTEM,
				IS_SYSTEM_ONLY
			)
			VALUES (
				@P_ROLE_NAME,
				@P_DESCRIPTION,
				0,
				0
			)
		END -- ADD ROLE TO RLS TABLE
	SET @RLS_SEQ_ID = (SELECT ZB_RLS.ROLE_SEQ_ID FROM ZB_RLS WHERE ROLE_NAME=@P_ROLE_NAME)
	IF(SELECT COUNT(*) FROM ZB_BU_SECURITY WHERE BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND ROLE_SEQ_ID = @RLS_SEQ_ID AND GROUP_SEQ_ID IS NULL) = 0 
	BEGIN  -- ADD ROLE REFERENCE TO BU_SECURITY
		
			INSERT ZB_BU_SECURITY (
				ROLE_SEQ_ID,
				BUSINESS_UNIT_SEQ_ID
			)
			VALUES (
				@RLS_SEQ_ID,
				@P_BUSINESS_UNIT_SEQ_ID
			)
	END
	RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_UPDATE_DIRECTORY_INFO(
	@P_BUSINESS_UNIT_SEQ_ID	INT,
	@P_DIRECTORY			NVARCHAR(255),
	@P_IMPERSONATE			BIT,
	@P_IMPERSONATE_ACCOUNT		NVARCHAR(50),
	@P_IMPERSONATE_PWD		NCHAR(50),
	@P_ADDUPD_BY			INT
)
AS
BEGIN
	IF (SELECT COUNT(*) FROM ZB_DIRECTORIES WHERE BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID) = 1
	-- UPDATE PROFILE
		UPDATE ZB_DIRECTORIES SET
			DIRECTORY = @P_DIRECTORY,
			IMPERSONATE = @P_IMPERSONATE,
			IMPERSONATE_ACCOUNT = @P_IMPERSONATE_ACCOUNT,
			IMPERSONATE_PWD = @P_IMPERSONATE_PWD
		WHERE
			BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
	ELSE
	-- INSERT
		INSERT INTO ZB_DIRECTORIES VALUES (
			@P_BUSINESS_UNIT_SEQ_ID,
			@P_DIRECTORY,
			@P_IMPERSONATE,
			@P_IMPERSONATE_ACCOUNT,
			@P_IMPERSONATE_PWD
			)
END
IF @@ERROR = 0 
	RETURN 1
ELSE
	RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_ADD_WORK_FLOW_PROFILE (
	@P_ORDER_ID AS INT,
	@P_WORK_FLOW_NAME AS NVARCHAR(50),
	@P_ACTION AS NVARCHAR(255),
	@P_ADDUPD_BY		INT
)
 AS
BEGIN
-- ADD PROFILE
	IF (SELECT COUNT(*) FROM ZB_WORK_FLOWS WHERE WORK_FLOW_NAME=@P_WORK_FLOW_NAME AND ORDER_ID = @P_ORDER_ID) = 0
		BEGIN
			INSERT INTO ZB_WORK_FLOWS VALUES (
					@P_ORDER_ID,
					@P_WORK_FLOW_NAME,
					@P_ACTION
			)
		END
	ELSE
	     RAISERROR ('THE ORDER ALREADY EXIST FOR WORK FLOW.',16,1)
END
IF @@ERROR = 0 RETURN 1
ELSE
RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_DEL_ACCOUNT_GRPS (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY INT
)
AS
	DELETE
		ZB_ACCTS_SECURITY
	WHERE
		ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
		ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID IN (
				SELECT BU_SECURITY_SEQ_ID 
					FROM ZB_BU_SECURITY 
				WHERE BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID 
					AND GROUP_SEQ_ID IS NOT NULL AND ROLE_SEQ_ID IS NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_DEL_ACCOUNT_RLS (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY INT
)
AS
	DELETE
		ZB_ACCTS_SECURITY
	WHERE
		ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
		ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID IN (
				SELECT BU_SECURITY_SEQ_ID 
					FROM ZB_BU_SECURITY 
				WHERE BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID 
					AND GROUP_SEQ_ID IS NULL AND ROLE_SEQ_ID IS NOT NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_DEL_ALL_ACCTS_ROLE_BY_BU(
	@P_ROLE_SEQ_ID AS INT,
	@P_BUSINESS_UNIT_SEQ_ID AS INT,
	@P_ADDUPD_BY	INT
)
AS
	DELETE
		ZB_ACCTS_SECURITY
	WHERE
		BU_SECURITY_SEQ_ID IN (
		SELECT BU_SECURITY_SEQ_ID 
		FROM ZB_BU_SECURITY 
			WHERE ROLE_SEQ_ID = @P_ROLE_SEQ_ID AND GROUP_SEQ_ID IS NULL
				AND BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_DEL_CALENDAR_DATA(
	@P_BUSINESS_UNIT_SEQ_ID	INT,
	@P_CALENDAR_NAME		NVARCHAR(50),
	@P_ENTRY				NVARCHAR(100),
	@P_ENTRYDATE 			SMALLDATETIME,
	@P_ADDUPD_BY			INT
)
AS
	UPDATE
		ZB_CALENDAR
	SET
		ACTIVE = 0
	WHERE
		BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
		CALENDAR_NAME = @P_CALENDAR_NAME AND
		COMMENT = @P_ENTRY AND
		ENTRY_DATE = @P_ENTRYDATE

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_DEL_DROBOX_SECURITY_BY_ROLE(
	@P_DROP_BOX_SEQ_ID INT,
	@P_PERMISSIONS_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID AS INT,
  @P_ADDUPD_BY			INT
) 
AS
	DELETE
		ZB_DROP_BOX_SECURITY
	WHERE
		DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID AND
		PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID AND
		BU_SECURITY_SEQ_ID IN (SELECT BU_SECURITY_SEQ_ID FROM ZB_BU_SECURITY WHERE BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_DEL_DROP_BOX
(
  @P_DROP_BOX_SEQ_ID INT,
  @P_ADDUPD_BY			INT
) 
AS
	DELETE ZB_DROP_BOX
	WHERE DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID
	EXEC ZBP_DEL_DROP_BOX_SECURITY @P_DROP_BOX_SEQ_ID,@P_ADDUPD_BY

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_DEL_DROP_BOX_SECURITY
(
  @P_DROP_BOX_SEQ_ID INT,
  @P_ADDUPD_BY			INT
) 
AS
-- USED BY STORE PROCEEDURE "DELETE_MODULE"
	DELETE ZB_DROP_BOX_SECURITY
	WHERE DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_DEL_GROUP (
	@P_GROUP_SEQ_ID NVARCHAR(25),
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY		INT
) 
AS
	DECLARE @BU_SECURITY_ID INT
	--GET THE BU_SECURITY_SEQ_ID FROM ZB_BU_SECURITY
	SELECT @BU_SECURITY_ID = BU_SECURITY_SEQ_ID 
		FROM  ZB_BU_SECURITY 
		WHERE 
		GROUP_SEQ_ID = @P_GROUP_SEQ_ID AND
			BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
			AND ROLE_SEQ_ID IS NULL 
		
	IF NOT @BU_SECURITY_ID IS NULL
	BEGIN
		--DELETE FROM ALL REFERENCE TABLES 
		/*
		 1. DELETE ENTRY FROM THE ZB_GRPS_RLS
		 2. DELETE ENTRY FROM ZB_ACCTS_GRPS_BU
		 3. DELETE ENTRY FROM ZB_BU_SECURITY
		 4. THEN FINALLY DELETE ENTRY FROM ZB_GRPS
		*/
/*
			DELETE ZB_GRPS_RLS
			WHERE BU_SECURITY_SEQ_ID = @BU_SECURITY_ID
			PRINT 'DELETED FROM ZB_GRPS_RLS'
*/
			--THIS MAY NOT BE REQUIRED IF ZB_BU_SECURITY HAS A CASCADE DELETE ON
			DELETE ZB_ACCTS_SECURITY
			WHERE BU_SECURITY_SEQ_ID = @BU_SECURITY_ID	
			PRINT 'DELETED FROM ZB_ACCTS_SECURITY'
			
			--*******NOTE: THS HAS TO BE A CASCADE DELETE *************
			DELETE ZB_BU_SECURITY
			WHERE GROUP_SEQ_ID = @P_GROUP_SEQ_ID
			PRINT 'DELETED FROM ZB_BU_SECURITY'
			DELETE ZB_GRPS
			WHERE GROUP_SEQ_ID = @P_GROUP_SEQ_ID
			PRINT 'FINALLY DELETED FROM ZB_GRPS'
	END	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_DEL_GROUP_ROLES (
	@P_GROUP_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY	INT
)
AS
	DELETE
		ZB_BU_SECURITY
	WHERE
		BU_SECURITY_SEQ_ID IN (SELECT BU_SECURITY_SEQ_ID 
					FROM ZB_BU_SECURITY 
					WHERE BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID
					AND GROUP_SEQ_ID = @P_GROUP_SEQ_ID AND ROLE_SEQ_ID IS NOT NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_DEL_MODULE	
(
  @P_MODULE_SEQ_ID INT,
	@P_ADDUPD_BY	INT
) 
AS
	EXEC ZBP_DEL_MODULE_SECURITY @P_MODULE_SEQ_ID,@P_ADDUPD_BY
	DELETE ZB_MODULES
	WHERE MODULE_SEQ_ID = @P_MODULE_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_DEL_MODULE_SECTY_BY_ROLE(
	@P_MODULE_SEQ_ID	INT,
	@P_PERMISSIONS_SEQ_ID 	INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY	INT
) 
AS
	DELETE
		ZB_MODULES_SECURITY
	WHERE
		MODULE_SEQ_ID = @P_MODULE_SEQ_ID AND
		PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID AND
		BU_SECURITY_SEQ_ID IN (SELECT BU_SECURITY_SEQ_ID 
				FROM ZB_BU_SECURITY 
				WHERE BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID
				AND ROLE_SEQ_ID IS NOT NULL AND GROUP_SEQ_ID IS NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_DEL_MODULE_SECURITY
(
  @P_MODULE_SEQ_ID INT,
  @P_ADDUPD_BY 	INT
) 
AS
-- USED BY STORE PROCEEDURE "DELETE_MODULE"
	DELETE ZB_MODULES_SECURITY
	WHERE MODULE_SEQ_ID = @P_MODULE_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_DEL_MODULE_SECURITY_BY_GROUP(
	@P_MODULE_SEQ_ID	INT,
	@P_PERMISSIONS_SEQ_ID 	INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY	INT
) 
AS
	DELETE
		ZB_MODULES_SECURITY
	WHERE
		MODULE_SEQ_ID = @P_MODULE_SEQ_ID AND
		PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID AND
		BU_SECURITY_SEQ_ID IN (SELECT BU_SECURITY_SEQ_ID 
				FROM ZB_BU_SECURITY 
				WHERE BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID
				AND GROUP_SEQ_ID IS NOT NULL AND ROLE_SEQ_ID IS NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_DEL_ROLE (
	@P_ROLE_NAME NVARCHAR(25),
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ADDUPD_BY		INT,
	@P_ISVALID NVARCHAR(50)  OUTPUT
) 
AS
	/*
	NOTE : ** CASCADE DELETE SHOULD BE TURNED ON IN
		ZB_BU_SECURITY FOR THIS TO WORK ELSE
		THIS MIGH THROW AN ERROR
		**** 
	*/
	DECLARE @ROLE_SEQ_ID INT
	SET @ROLE_SEQ_ID = (SELECT ROLE_SEQ_ID FROM ZB_RLS WHERE ROLE_NAME = @P_ROLE_NAME)
	BEGIN -- DELETE ROLE FROM ZB_BU_SECURITY
		DELETE ZB_BU_SECURITY
		WHERE (
			ROLE_SEQ_ID = @ROLE_SEQ_ID AND
			BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
		       )
	END 
	BEGIN -- DELETE ROLE FROM ZB_RLS
		IF (SELECT COUNT(*) FROM
			ZB_RLS INNER JOIN
		        ZB_BU_SECURITY ON ZB_RLS.ROLE_SEQ_ID = ZB_BU_SECURITY.ROLE_SEQ_ID
			WHERE	ZB_RLS.ROLE_SEQ_ID = @ROLE_SEQ_ID) = 0
		BEGIN
			DELETE ZB_RLS
			WHERE (ROLE_SEQ_ID = @ROLE_SEQ_ID)
		END
	END --  DELETE ROLE FROM ZB_RLS
	IF @@ERROR = 0 
		SET @P_ISVALID = 1
	ELSE
		SET @P_ISVALID = 0
 RETURN @P_ISVALID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_DEL_WORK_FLOW_PROFILE (
	@P_ORDER_ID AS INT,
	@P_WORK_FLOW_NAME AS NVARCHAR(50),
	@P_ADDUPD_BY			INT
) AS
	DELETE FROM ZB_WORK_FLOWS WHERE ORDER_ID = @P_ORDER_ID AND WORK_FLOW_NAME = @P_WORK_FLOW_NAME
	IF @@ERROR = 0 RETURN 1
	ELSE
	RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ACCOUNTS_BY_LETTER (
	@P_TYPE INT, -- WHAT TYPE OF SELECT 1 SYS ADMIN 2 STATE ADMIN
	@P_BUSINESS_UNIT_SEQ_ID INT
) 
AS
-- CREATE A TEMP TABLE TO STORE THE SELECT RESULTS
CREATE TABLE #PAGEINDEX (
	INDEXID INT IDENTITY (1, 1) NOT NULL,
	ACCOUNT_SEQ_ID INT
)
BEGIN -- 1
	BEGIN -- POPULATE THE TEMP TABLE
		INSERT INTO #PAGEINDEX (ACCOUNT_SEQ_ID)
		SELECT 
			ACCOUNT_SEQ_ID
		FROM 
			ZB_ACCOUNTS
	END
-- GET THE USERS
	IF @P_TYPE = 1 
		SELECT DISTINCT 
			ZB_ACCOUNTS.ACCOUNT_SEQ_ID, 
			ZB_ACCOUNTS.ACCOUNT, 
			ZB_ACCOUNTS.LAST_NAME + ', ' + ZB_ACCOUNTS.FIRST_NAME AS FULLNAME,
			ZB_ACCOUNTS.EMAIL, 
			ZB_ACCOUNTS.ADDED_DATE, 
			ZB_ACCOUNTS.LAST_LOGIN
		FROM
			ZB_ACCOUNTS
	ELSE
		SELECT DISTINCT TOP 100 PERCENT 
			ZB_ACCOUNTS.ACCOUNT_SEQ_ID, 
			ZB_ACCOUNTS.ACCOUNT, 
			ZB_ACCOUNTS.LAST_NAME + ', ' + ZB_ACCOUNTS.FIRST_NAME AS FULLNAME,
			ZB_ACCOUNTS.EMAIL, 
			ZB_ACCOUNTS.ADDED_DATE, 
			ZB_ACCOUNTS.LAST_LOGIN
		FROM
			ZB_ACCOUNTS INNER JOIN ZB_ACCTS_SECURITY ON 
				ZB_ACCOUNTS.ACCOUNT_SEQ_ID = ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID 
				   INNER JOIN ZB_BU_SECURITY ON 
					ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		WHERE
			(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
			AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL)
END -- 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ACCOUNT_CHOICESINFO
(
  @P_ACCOUNT NVARCHAR(25)
)
AS
SELECT * 
FROM 
	ZB_ACCOUNT_CHOICES
WHERE 
	ACCOUNT = @P_ACCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ACCOUNT_PROFILE 
(
  @P_ACCOUNT NVARCHAR(25)
)
AS
SELECT * 
FROM 
	ZB_ACCOUNTS
WHERE 
	ACCOUNT = @P_ACCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ACCTS_BY_LETTER (
	@P_TYPE INT, -- WHAT TYPE OF SELECT 1 SYS ADMIN 2 STATE ADMIN
	@P_BUSINESS_UNIT_SEQ_ID INT
) 
AS
-- CREATE A TEMP TABLE TO STORE THE SELECT RESULTS
CREATE TABLE #PAGEINDEX (
	INDEXID INT IDENTITY (1, 1) NOT NULL,
	ACCOUNT_SEQ_ID INT
)
BEGIN -- 1
	BEGIN -- POPULATE THE TEMP TABLE
		INSERT INTO #PAGEINDEX (ACCOUNT_SEQ_ID)
		SELECT 
			ACCOUNT_SEQ_ID
		FROM 
			ZB_ACCOUNTS
	END
-- GET THE USERS
	IF @P_TYPE = 1 -- GET ALL ACCOUNTS FROM ALL STATES
		SELECT DISTINCT 
			ZB_ACCOUNTS.ACCOUNT_SEQ_ID, 
			ZB_ACCOUNTS.ACCOUNT, 
			ZB_ACCOUNTS.LAST_NAME + ', ' + ZB_ACCOUNTS.FIRST_NAME AS FULLNAME,
			ZB_ACCOUNTS.EMAIL, 
			ZB_ACCOUNTS.ADDED_DATE, 
			ZB_ACCOUNTS.LAST_LOGIN
		FROM
			ZB_ACCOUNTS
	ELSE
		SELECT DISTINCT TOP 100 PERCENT 
			ZB_ACCOUNTS.ACCOUNT_SEQ_ID, 
			ZB_ACCOUNTS.ACCOUNT, 
			ZB_ACCOUNTS.LAST_NAME + ', ' + ZB_ACCOUNTS.FIRST_NAME AS FULLNAME,
			ZB_ACCOUNTS.EMAIL, 
			ZB_ACCOUNTS.ADDED_DATE, 
			ZB_ACCOUNTS.LAST_LOGIN
		FROM
			ZB_ACCOUNTS INNER JOIN ZB_ACCTS_SECURITY ON 
				ZB_ACCOUNTS.ACCOUNT_SEQ_ID = ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID 
				   INNER JOIN ZB_BU_SECURITY ON 
					ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		WHERE
			(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
			AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL)
END -- 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_ACCTS_N_ROLE_BY_BU(
	@P_ROLE_SEQ_ID AS INT,
	@P_BUSINESS_UNIT_SEQ_ID AS INT
)
AS
SELECT DISTINCT TOP 100 PERCENT 
	ZB_ACCOUNTS.ACCOUNT
FROM
	ZB_ACCOUNTS INNER JOIN ZB_ACCTS_SECURITY ON 
		ZB_ACCOUNTS.ACCOUNT_SEQ_ID = ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID INNER JOIN ZB_BU_SECURITY ON 
			ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
WHERE
	ZB_BU_SECURITY.ROLE_SEQ_ID <> @P_ROLE_SEQ_ID AND 
	ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL AND
	(ZB_ACCOUNTS.SYSTEM_STATUS_ID <> 2) AND
	(NOT (ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID IN (@P_BUSINESS_UNIT_SEQ_ID))) AND 
	(NOT 
		(ZB_ACCOUNTS.ACCOUNT IN 
			(
				SELECT DISTINCT TOP 100 PERCENT 
					ZB_ACCOUNTS.ACCOUNT
				FROM
					ZB_ACCOUNTS INNER JOIN  ZB_ACCTS_SECURITY ON 
						ZB_ACCOUNTS.ACCOUNT_SEQ_ID = ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID 
							INNER JOIN ZB_BU_SECURITY ON 
							ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
				 WHERE
					(ZB_BU_SECURITY.ROLE_SEQ_ID = @P_ROLE_SEQ_ID) AND 
					(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID)
					AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL	
				ORDER BY ZB_ACCOUNTS.ACCOUNT
			)
		)
	)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_ACCTS_ROLE_BY_BU(
	@P_ROLE_SEQ_ID AS INT,
	@P_BUSINESS_UNIT_SEQ_ID AS INT
)
AS
	SELECT DISTINCT TOP 100 PERCENT 
		ZB_ACCOUNTS.ACCOUNT
	FROM
		ZB_ACCOUNTS INNER JOIN ZB_ACCTS_SECURITY ON
			ZB_ACCOUNTS.ACCOUNT_SEQ_ID = ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID 
				INNER JOIN ZB_BU_SECURITY ON 
				ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
	WHERE
		(ZB_BU_SECURITY.ROLE_SEQ_ID = @P_ROLE_SEQ_ID AND GROUP_SEQ_ID IS NULL ) AND 
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID)
	ORDER BY 
		ZB_ACCOUNTS.ACCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_ACTIVE_BU
AS
	SELECT DISTINCT
		ZB_BUSINESS_UNITS.BUSINESS_UNIT_SEQ_ID
	FROM
		ZB_BUSINESS_UNITS
	WHERE
		ZB_BUSINESS_UNITS.STATUS_SEQ_ID = 0
RETURN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_ACTIVE_BUSINESS_UNITS
AS
	SELECT DISTINCT
		ZB_BUSINESS_UNITS.BUSINESS_UNIT_SEQ_ID
	FROM
		ZB_BUSINESS_UNITS
	WHERE
		ZB_BUSINESS_UNITS.STATUS_SEQ_ID = 0
RETURN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_BUSINESSUNITS_ALL_INFO
AS
	SELECT 
		*
	FROM
		ZB_BUSINESS_UNITS
	ORDER BY [NAME]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_BU_ALL_INFO
AS
	SELECT 
		*
	FROM
		ZB_BUSINESS_UNITS
	ORDER BY [NAME]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_DIRECTORYINFO
AS
	SELECT * 
	FROM 
		ZB_DIRECTORIES

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_DROP_BOXES
AS
	SELECT
		*
	FROM
		ZB_DROP_BOXS

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_ENABLED_MODULES
AS
	SELECT
		MODULE_SEQ_ID, 
		NAME, DESCRIPTION, 
		SOURCE, 
		ENABLE_VIEW_STATE,
		IS_NAV, NAV_TYPE_SEQ_ID, 
		PARENT_MODULE_SEQ_ID,
		MODULE_ACTION
	FROM
		ZB_MODULES

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_GET_ALL_GRPS_BY_BU (
	@P_GROUP_NAME NVARCHAR(50),
	@P_BUSINESS_UNIT_SEQ_ID INT
) 
AS
	SELECT DISTINCT
			ZB_GRPS.GROUP_SEQ_ID, 
			ZB_GRPS.GROUP_NAME as Name,
			ZB_GRPS.DESCRIPTION as Description
	FROM
			ZB_GRPS INNER JOIN ZB_BU_SECURITY ON 
				ZB_GRPS.GROUP_SEQ_ID = ZB_BU_SECURITY.GROUP_SEQ_ID
				AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
	WHERE		
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_GRPS_FOR_BU (
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	DECLARE @SELECTCLAUSE VARCHAR(1000)
	DECLARE @FROMCLAUSE VARCHAR(1000)
	DECLARE @WHERECLAUSE VARCHAR(1000)
	DECLARE @ORDERCLAUSE VARCHAR(1000)
	SET @SELECTCLAUSE = 'SELECT DISTINCT
			ZB_GRPS.GROUP_SEQ_ID,
			ZB_GRPS.GROUP_NAME, 
			ZB_GRPS.DESCRIPTION'
	SET @FROMCLAUSE = ' FROM
			ZB_GRPS INNER JOIN ZB_BU_SECURITY ON 
				ZB_GRPS.GROUP_SEQ_ID = ZB_BU_SECURITY.GROUP_SEQ_ID'
	SET @WHERECLAUSE = ' WHERE ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL AND
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = ' + CONVERT(VARCHAR,@P_BUSINESS_UNIT_SEQ_ID) + ')'
	SET @ORDERCLAUSE = ' ORDER BY ZB_GRPS.GROUP_NAME'
	EXEC (@SELECTCLAUSE + @FROMCLAUSE + @WHERECLAUSE + @ORDERCLAUSE)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_MESSAGES_FOR_BU(
	@P_BUSINESS_UNIT_SEQ_ID AS INT
)
 AS
	SELECT * FROM ZB_MESSAGES WHERE BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_MODULES
AS
	SELECT
		*
	FROM
		ZB_MODULES

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_RLS 
AS
	SELECT 
		*
	FROM 
		ZB_RLS
	ORDER BY DESCRIPTION ASC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_RLS_FOR_BU (
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	DECLARE @SELECTCLAUSE VARCHAR(1000)
	DECLARE @FROMCLAUSE VARCHAR(1000)
	DECLARE @WHERECLAUSE VARCHAR(1000)
	DECLARE @ORDERCLAUSE VARCHAR(1000)
	SET @SELECTCLAUSE = 'SELECT DISTINCT
			ZB_RLS.ROLE_SEQ_ID,
			ZB_RLS.ROLE_NAME, 
			ZB_RLS.DESCRIPTION, 
			ZB_RLS.IS_SYSTEM'
	SET @FROMCLAUSE = ' FROM
			ZB_RLS INNER JOIN ZB_BU_SECURITY ON 
				ZB_RLS.ROLE_SEQ_ID = ZB_BU_SECURITY.ROLE_SEQ_ID'
	SET @WHERECLAUSE = ' WHERE ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL AND
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = ' + CONVERT(VARCHAR,@P_BUSINESS_UNIT_SEQ_ID) + ')'
	SET @ORDERCLAUSE = ' ORDER BY ZB_RLS.ROLE_NAME'
	EXEC (@SELECTCLAUSE + @FROMCLAUSE + @WHERECLAUSE + @ORDERCLAUSE)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ALL_STATES_ALL_INFO
AS
	SELECT 
		STATE,
		DESCRIPTION AS LONGNAME,
		STATUS_SEQ_ID
	FROM
		ZB_STATES

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_CALENDAR_DATA(
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_CALENDAR_NAME NVARCHAR(50)
)
AS
	SELECT
		*
	FROM
		ZB_CALENDAR
	WHERE
		[ACTIVE] = 1 AND
		BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
		CALENDAR_NAME = @P_CALENDAR_NAME

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_DROP_BOXES (
	@P_ACCOUNT_SEQ_ID AS INT
)
AS
	SELECT
		ZB_DROP_BOXES.DROP_BOX_SEQ_ID, 
		ZB_DROP_BOXES.DESCRIPTION
	FROM
		ZB_DROP_BOX_SECURITY INNER JOIN ZB_DROP_BOXES ON 
			ZB_DROP_BOX_SECURITY.DROP_BOX_SEQ_ID = ZB_DROP_BOXES.DROP_BOX_SEQ_ID
				INNER JOIN ZB_BU_SECURITY ON 
					ZB_DROP_BOX_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
						INNER JOIN ZB_ACCTS_SECURITY ON 
							ZB_BU_SECURITY.BU_SECURITY_SEQ_ID = ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID
	WHERE
		(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_DROP_BOX_BU_SELTD_RLS (
	@P_PERMISSIONS_SEQ_ID INT,
	@P_DROP_BOX_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	/* 
		CURRENTLY THERE IS NO NEED TO SEPORATE THIS BY THE BUSINESS UNIT SO ALL WILL
		THE DEFAULT BUSINESS UNIT ID ... THE APPLICATION CODE WILL PASS IN THE CORRECT
		BUSINESS UNIT ID... IF THIS NEEDS TO BE DONE IN THE FUTURE THIS WILL BE THE ONLY CHANGE NECESSARY
	*/
	SET @P_BUSINESS_UNIT_SEQ_ID = (SELECT DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
	SELECT DISTINCT TOP 100 PERCENT 
		ZB_RLS.ROLE_NAME, 
		ZB_DROP_BOX_SECURITY.PERMISSIONS_SEQ_ID, 
		ZB_DROP_BOX_SECURITY.DROP_BOX_SEQ_ID, 
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID
	FROM
		ZB_DROP_BOX_SECURITY INNER JOIN ZB_BU_SECURITY ON 
			ZB_DROP_BOX_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID INNER JOIN ZB_RLS ON 
				ZB_BU_SECURITY.ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID
	WHERE
		(ZB_DROP_BOX_SECURITY.PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID) AND 
		(ZB_DROP_BOX_SECURITY.DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID) AND 
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID)
	ORDER BY
		ZB_DROP_BOX_SECURITY.DROP_BOX_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_DROP_BOX_DETAILS
AS
	SELECT
		*
	FROM
		ZB_DROP_BOX_DETAIL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_GET_GROUP_INFO 
(
  @P_GROUP_ID INT
)
AS
SELECT * 
FROM 
	ZB_GRPS
WHERE 
	GROUP_SEQ_ID = @P_GROUP_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_GET_GRPS_FOR_ACCOUNT (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	SELECT
		ZB_GRPS.GROUP_NAME AS GRPS
	FROM
		ZB_GRPS 
		INNER JOIN ZB_BU_SECURITY ON 
		ZB_GRPS.GROUP_SEQ_ID = ZB_BU_SECURITY.GROUP_SEQ_ID 
		INNER JOIN ZB_ACCTS_SECURITY ON 
		ZB_BU_SECURITY.BU_SECURITY_SEQ_ID = ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID
		INNER JOIN ZB_ACCOUNTS ON
		ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = ZB_ACCOUNTS.ACCOUNT_SEQ_ID
						
	WHERE
		ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
		AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_GET_GRPS_FOR_ACCOUNT_BY_BU (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
SELECT
		ZB_GRPS.GROUP_NAME AS GRPS
	FROM
		ZB_GRPS 
		INNER JOIN ZB_BU_SECURITY ON 
		ZB_GRPS.GROUP_SEQ_ID = ZB_BU_SECURITY.GROUP_SEQ_ID 
		INNER JOIN ZB_ACCTS_SECURITY ON 
		ZB_BU_SECURITY.BU_SECURITY_SEQ_ID = ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID
		INNER JOIN ZB_ACCOUNTS ON
		ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = ZB_ACCOUNTS.ACCOUNT_SEQ_ID	
				
	WHERE
		ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID 
		AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE       PROCEDURE ZBP_GET_HIERARCHICAL_MENU_DATA (
	@P_BUSINESS_UNIT_SEQ_ID AS INT,
	@P_ACCOUNT_SEQ_ID AS INT
)
AS
DECLARE
	@DEFAULT_BUSINESS_SEQ_ID 	INT,
	@DEFAULT_PERMISSION_SEQ_ID	INT,
	@DEFAULT_NAV_TYPE_SEQ_ID 	INT,
	@DEFAULT_IS_NAV			INT
       	SET 	@DEFAULT_BUSINESS_SEQ_ID = dbo.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()
	SET	@DEFAULT_PERMISSION_SEQ_ID = 1
	SET	@DEFAULT_NAV_TYPE_SEQ_ID = 4
	SET	@DEFAULT_IS_NAV	= 1
	
	SELECT DISTINCT
			ZB_MODULES.MODULE_SEQ_ID AS ID, 
			ZB_MODULES.NAME AS TITLE, 
			ZB_MODULES.DESCRIPTION, 
			ZB_MODULES.MODULE_ACTION AS URL, 
			ZB_MODULES.PARENT_MODULE_SEQ_ID AS PARENT
	FROM
		ZB_MODULES
	WHERE
		MODULE_SEQ_ID IN(
			SELECT
				P.MODULE_SEQ_ID
			FROM
				ZB_RLS,
				(SELECT DISTINCT
					X.MODULE_SEQ_ID,
					X.PERMISSIONS_SEQ_ID,
					Y.ROLE_SEQ_ID 
				FROM
					ZB_MODULES_SECURITY X,
					(SELECT DISTINCT 
						A.MODULE_SEQ_ID,
						C.BU AS ROLE_SEQ_ID
					FROM
						ZB_MODULES_SECURITY A , 
						(SELECT DISTINCT 
							B.ROLE_SEQ_ID AS BU,
							A.BU_SECURITY_SEQ_ID AS BU_1
						FROM
							ZB_BU_SECURITY B,
							ZB_BU_SECURITY A
						WHERE
							B.BUSINESS_UNIT_SEQ_ID=A.BUSINESS_UNIT_SEQ_ID AND
							(A.GROUP_SEQ_ID=B.GROUP_SEQ_ID OR A.BU_SECURITY_SEQ_ID=B.BU_SECURITY_SEQ_ID) AND
							(B.BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID OR B.BUSINESS_UNIT_SEQ_ID=@DEFAULT_BUSINESS_SEQ_ID)
						) AS C
					WHERE
						C.BU_1=A.BU_SECURITY_SEQ_ID) AS Y
				WHERE
					X.MODULE_SEQ_ID=Y.MODULE_SEQ_ID) AS P
			WHERE
				P.ROLE_SEQ_ID=ZB_RLS.ROLE_SEQ_ID AND
				P.PERMISSIONS_SEQ_ID = @DEFAULT_PERMISSION_SEQ_ID AND 
				P.ROLE_SEQ_ID IN(
						SELECT
							ZB_BU_SECURITY.ROLE_SEQ_ID
						FROM
							ZB_ACCTS_SECURITY ,  
							ZB_BU_SECURITY  
						WHERE
							ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
							(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
							(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND
							ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
						UNION
				
						SELECT
							ROLE_SEQ_ID
						FROM
							ZB_BU_SECURITY
						WHERE
							ROLE_SEQ_ID IN(
								SELECT
									ROLE_SEQ_ID
								FROM
									ZB_BU_SECURITY
								WHERE
									ZB_BU_SECURITY.GROUP_SEQ_ID IN(
											SELECT
												ZB_BU_SECURITY.GROUP_SEQ_ID
											FROM
												ZB_ACCTS_SECURITY ,  
												ZB_BU_SECURITY  
											WHERE
												ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
												(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
												(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND
												ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
									) AND
									ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
							) AND
							GROUP_SEQ_ID IS NULL AND
							(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID)
						)
	) AND
	(ZB_MODULES.IS_NAV = @DEFAULT_IS_NAV) AND 
	(ZB_MODULES.NAV_TYPE_SEQ_ID = @DEFAULT_NAV_TYPE_SEQ_ID)
	ORDER BY
		ZB_MODULES.NAME, 
		ZB_MODULES.MODULE_SEQ_ID,
		ZB_MODULES.PARENT_MODULE_SEQ_ID
/* ------------------------ old way ------------------------------
		SELECT DISTINCT
				ZB_MODULES.MODULE_SEQ_ID AS ID, 
				ZB_MODULES.NAME AS TITLE, 
				ZB_MODULES.DESCRIPTION, 
				 ZB_MODULES.MODULE_ACTION AS URL, 
				ZB_MODULES.PARENT_MODULE_SEQ_ID AS PARENT
		FROM
			ZB_MODULES_SECURITY 
			INNER JOIN ZB_MODULES ON 
				ZB_MODULES_SECURITY.MODULE_SEQ_ID = ZB_MODULES.MODULE_SEQ_ID
		WHERE 
			(ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID = @DEFAULT_PERMISSION_SEQ_ID) AND 
			(ZB_MODULES.IS_NAV = @DEFAULT_IS_NAV) AND 
			(ZB_MODULES.NAV_TYPE_SEQ_ID = @DEFAULT_NAV_TYPE_SEQ_ID) AND
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID IN(
		SELECT
			ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM
			ZB_ACCTS_SECURITY ,  
			ZB_BU_SECURITY  
		WHERE
			ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
			(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
			(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID)
		UNION
		
		SELECT
			ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM
			ZB_BU_SECURITY,
			(SELECT
				ZB_BU_SECURITY.ROLE_SEQ_ID,
				ZB_BU_SECURITY.GROUP_SEQ_ID
			FROM
				ZB_BU_SECURITY 
			WHERE
				ZB_BU_SECURITY.GROUP_SEQ_ID IN(
					SELECT
						ZB_BU_SECURITY.GROUP_SEQ_ID
					FROM
						ZB_BU_SECURITY 
					WHERE
						ZB_BU_SECURITY.ROLE_SEQ_ID IN(
							SELECT
								ZB_BU_SECURITY.ROLE_SEQ_ID
							FROM 
								ZB_BU_SECURITY 
							WHERE
								ZB_BU_SECURITY.GROUP_SEQ_ID IN(
									SELECT
										ZB_BU_SECURITY.GROUP_SEQ_ID
									FROM
										ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
									WHERE
										ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
									      	ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
										(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND 
										ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL AND
										ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
				    					) OR
										 ZB_BU_SECURITY.BU_SECURITY_SEQ_ID IN(
													SELECT
														ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
													FROM
														ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
													WHERE
														ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
														ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND
														(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) 
											                                    )  AND 
								ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
			   				)
					)
 			) AS TEMP1
		WHERE
			TEMP1.GROUP_SEQ_ID=ZB_BU_SECURITY.GROUP_SEQ_ID OR 
			TEMP1.ROLE_SEQ_ID=ZB_BU_SECURITY.ROLE_SEQ_ID
	)
ORDER BY
	ZB_MODULES.NAME, 
	ZB_MODULES.PARENT_MODULE_SEQ_ID, 
	ZB_MODULES.MODULE_SEQ_ID
*/

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE     PROCEDURE ZBP_GET_LEFT_NAVLINKS
(
  @P_ACCOUNT_SEQ_ID INT,
  @P_BUSINESS_UNIT_SEQ_ID INT
) 
AS
	DECLARE
		@DEFAULT_BUSINESS_SEQ_ID 	INT,
		@DEFAULT_PERMISSION_SEQ_ID	INT,
		@DEFAULT_NAV_TYPE_SEQ_ID 	INT,
		@DEFAULT_IS_NAV			INT
	SET 	@DEFAULT_BUSINESS_SEQ_ID = dbo.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()
	SET	@DEFAULT_PERMISSION_SEQ_ID = 1
	SET	@DEFAULT_NAV_TYPE_SEQ_ID = 2
	SET	@DEFAULT_IS_NAV	= 1
		SELECT DISTINCT
			ZB_MODULES.NAME,
			ZB_MODULES.MODULE_ACTION
		FROM
			ZB_MODULES_SECURITY 
			INNER JOIN ZB_MODULES ON 
				ZB_MODULES_SECURITY.MODULE_SEQ_ID = ZB_MODULES.MODULE_SEQ_ID
		WHERE 
			(ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID = @DEFAULT_PERMISSION_SEQ_ID) AND 
			(ZB_MODULES.IS_NAV = @DEFAULT_IS_NAV) AND 
			(ZB_MODULES.NAV_TYPE_SEQ_ID = @DEFAULT_NAV_TYPE_SEQ_ID) AND
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID IN(
		SELECT
			ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM
			ZB_ACCTS_SECURITY ,  
			ZB_BU_SECURITY  
		WHERE
			ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
			(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
			(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID)
		UNION
		
		SELECT
			ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM
			ZB_BU_SECURITY,
			(SELECT
				ZB_BU_SECURITY.ROLE_SEQ_ID,
				ZB_BU_SECURITY.GROUP_SEQ_ID
			FROM
				ZB_BU_SECURITY 
			WHERE
				ZB_BU_SECURITY.GROUP_SEQ_ID IN(
					SELECT
						ZB_BU_SECURITY.GROUP_SEQ_ID
					FROM
						ZB_BU_SECURITY 
					WHERE
						ZB_BU_SECURITY.ROLE_SEQ_ID IN(
							SELECT
								ZB_BU_SECURITY.ROLE_SEQ_ID
							FROM 
								ZB_BU_SECURITY 
							WHERE
								ZB_BU_SECURITY.GROUP_SEQ_ID IN(
									SELECT
										ZB_BU_SECURITY.GROUP_SEQ_ID
									FROM
										ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
									WHERE
										ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
									      	ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
										(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND 
										ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL AND
										ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
				    					) OR
										 ZB_BU_SECURITY.BU_SECURITY_SEQ_ID IN(
													SELECT
														ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
													FROM
														ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
													WHERE
														ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
														ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND
														(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) 
											                                    )  AND 
								ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
			   				)
					)
 			) AS TEMP1
		WHERE
			TEMP1.GROUP_SEQ_ID=ZB_BU_SECURITY.GROUP_SEQ_ID OR 
			TEMP1.ROLE_SEQ_ID=ZB_BU_SECURITY.ROLE_SEQ_ID
	)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE      PROCEDURE ZBP_GET_LINEMENU_NAVLINKS
(
  @P_ACCOUNT_SEQ_ID INT,
  @P_BUSINESS_UNIT_SEQ_ID INT
) 
AS
DECLARE @DEFAULT_BUSINESS_SEQ_ID 	INT,
	@DEFAULT_PERMISSION_SEQ_ID	INT,
	@DEFAULT_NAV_TYPE_SEQ_ID 	INT,
	@DEFAULT_IS_NAV			INT
	SET 	@DEFAULT_BUSINESS_SEQ_ID = dbo.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()
	SET	@DEFAULT_PERMISSION_SEQ_ID = 1
	SET	@DEFAULT_NAV_TYPE_SEQ_ID = 1
	SET	@DEFAULT_IS_NAV	= 1
	SELECT DISTINCT
			ZB_MODULES.NAME, 
			ZB_MODULES.MODULE_ACTION AS [Action]
	FROM
		ZB_MODULES
	WHERE
		MODULE_SEQ_ID IN(
			SELECT
				P.MODULE_SEQ_ID
			FROM
				ZB_RLS,
				(SELECT DISTINCT
					X.MODULE_SEQ_ID,
					X.PERMISSIONS_SEQ_ID,
					Y.ROLE_SEQ_ID 
				FROM
					ZB_MODULES_SECURITY X,
					(SELECT DISTINCT 
						A.MODULE_SEQ_ID,
						C.BU AS ROLE_SEQ_ID
					FROM
						ZB_MODULES_SECURITY A , 
						(SELECT DISTINCT 
							B.ROLE_SEQ_ID AS BU,
							A.BU_SECURITY_SEQ_ID AS BU_1
						FROM
							ZB_BU_SECURITY B,
							ZB_BU_SECURITY A
						WHERE
							B.BUSINESS_UNIT_SEQ_ID=A.BUSINESS_UNIT_SEQ_ID AND
							(A.GROUP_SEQ_ID=B.GROUP_SEQ_ID OR A.BU_SECURITY_SEQ_ID=B.BU_SECURITY_SEQ_ID) AND
							(B.BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID OR B.BUSINESS_UNIT_SEQ_ID=@DEFAULT_BUSINESS_SEQ_ID)
						) AS C
					WHERE
						C.BU_1=A.BU_SECURITY_SEQ_ID) AS Y
				WHERE
					X.MODULE_SEQ_ID=Y.MODULE_SEQ_ID) AS P
			WHERE
				P.ROLE_SEQ_ID=ZB_RLS.ROLE_SEQ_ID AND
				P.PERMISSIONS_SEQ_ID = @DEFAULT_PERMISSION_SEQ_ID AND 
				P.ROLE_SEQ_ID IN(
						SELECT
							ZB_BU_SECURITY.ROLE_SEQ_ID
						FROM
							ZB_ACCTS_SECURITY ,  
							ZB_BU_SECURITY  
						WHERE
							ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
							(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
							(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND
							ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
						UNION
				
						SELECT
							ROLE_SEQ_ID
						FROM
							ZB_BU_SECURITY
						WHERE
							ROLE_SEQ_ID IN(
								SELECT
									ROLE_SEQ_ID
								FROM
									ZB_BU_SECURITY
								WHERE
									ZB_BU_SECURITY.GROUP_SEQ_ID IN(
											SELECT
												ZB_BU_SECURITY.GROUP_SEQ_ID
											FROM
												ZB_ACCTS_SECURITY ,  
												ZB_BU_SECURITY  
											WHERE
												ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
												(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
												(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND
												ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
									) AND
									ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
							) AND
							GROUP_SEQ_ID IS NULL AND
							(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID)
						)
		) AND
		(ZB_MODULES.IS_NAV = @DEFAULT_IS_NAV) AND 
		(ZB_MODULES.NAV_TYPE_SEQ_ID = @DEFAULT_NAV_TYPE_SEQ_ID)
	ORDER BY
		ZB_MODULES.NAME
/*
SELECT
	ZB_MODULES.NAME, 
        	ZB_MODULES.MODULE_ACTION AS [Action]
FROM
	ZB_MODULES_SECURITY 
	INNER JOIN ZB_MODULES ON 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID = ZB_MODULES.MODULE_SEQ_ID
WHERE 
	(ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID = @DEFAULT_PERMISSION_SEQ_ID) AND 
	(ZB_MODULES.IS_NAV = @DEFAULT_IS_NAV) AND 
	(ZB_MODULES.NAV_TYPE_SEQ_ID = @DEFAULT_NAV_TYPE_SEQ_ID) AND
	ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID IN(
		SELECT
			ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM
			ZB_ACCTS_SECURITY ,  
			ZB_BU_SECURITY  
		WHERE
			ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
			(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
			(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID)
		UNION
		
		SELECT
			ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM
			ZB_BU_SECURITY,
			(SELECT
				ZB_BU_SECURITY.ROLE_SEQ_ID,
				ZB_BU_SECURITY.GROUP_SEQ_ID
			FROM
				ZB_BU_SECURITY 
			WHERE
				ZB_BU_SECURITY.GROUP_SEQ_ID IN(
					SELECT
						ZB_BU_SECURITY.GROUP_SEQ_ID
					FROM
						ZB_BU_SECURITY 
					WHERE
						ZB_BU_SECURITY.ROLE_SEQ_ID IN(
							SELECT
								ZB_BU_SECURITY.ROLE_SEQ_ID
							FROM 
								ZB_BU_SECURITY 
							WHERE
								ZB_BU_SECURITY.GROUP_SEQ_ID IN(
									SELECT
										ZB_BU_SECURITY.GROUP_SEQ_ID
									FROM
										ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
									WHERE
										ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
									      	ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
										(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) AND 
										ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL AND
										ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
                                                                          or ZB_BU_SECURITY.BU_SECURITY_SEQ_ID IN 
				                                    (
				                                     SELECT  ZB_BU_SECURITY.bu_security_seq_id
									FROM 	ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
									WHERE 	ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
				      					ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID
				      					AND (ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
									OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID)
                                                                      )
				    					) AND 
								ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
			   				)
					)
 			) AS TEMP1
		WHERE
			TEMP1.GROUP_SEQ_ID=ZB_BU_SECURITY.GROUP_SEQ_ID OR 
			TEMP1.ROLE_SEQ_ID=ZB_BU_SECURITY.ROLE_SEQ_ID
	)
*/

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_MESSAGE (
  @P_BUSINESS_UNIT_SEQ_ID INT,
  @P_MESSAGENAME NVARCHAR(50)
)
AS
	SELECT
		*
	FROM
		ZB_MESSAGES
	WHERE
		BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
		AND [NAME] = @P_MESSAGENAME

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_MESSAGE_BY_ID (
  @P_MESSAGE_ID INT
)
AS
	SELECT
		*
	FROM
		ZB_MESSAGES
	WHERE
		MESSAGE_SEQ_ID = @P_MESSAGE_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_GET_MODULE_BU_SELECTED_RLS (
	@P_PERMISSIONS_SEQ_ID INT,
	@P_MODULE_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	SELECT DISTINCT TOP 100 PERCENT 
		ZB_RLS.ROLE_NAME, 
		ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID, 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID, 
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID
	FROM
		ZB_MODULES_SECURITY INNER JOIN ZB_BU_SECURITY ON 
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID 
			INNER JOIN ZB_RLS ON 
				ZB_BU_SECURITY.ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID
	WHERE
		(ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID) AND 
		(ZB_MODULES_SECURITY.MODULE_SEQ_ID = @P_MODULE_SEQ_ID) AND 
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID)
		AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL
	ORDER BY
		ZB_MODULES_SECURITY.MODULE_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_GET_MODULE_BU_SELTD_GRPS (
	@P_PERMISSIONS_SEQ_ID INT,
	@P_MODULE_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	SELECT DISTINCT TOP 100 PERCENT 
		ZB_GRPS.GROUP_NAME, 
		ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID, 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID, 
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID
	FROM
		ZB_MODULES_SECURITY 
		INNER JOIN ZB_BU_SECURITY ON 
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID 
		INNER JOIN ZB_GRPS ON 
			ZB_BU_SECURITY.GROUP_SEQ_ID = ZB_GRPS.GROUP_SEQ_ID
	WHERE
		ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID AND 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID = @P_MODULE_SEQ_ID AND 
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
		AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL 
		AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL
	ORDER BY
		ZB_MODULES_SECURITY.MODULE_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_GET_MODULE_GRPS_BY_BU (
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
--	SELECT TOP 100 PERCENT
	SELECT	ZB_GRPS.GROUP_NAME AS [GROUP], 
		ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID, 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID
	FROM
		ZB_MODULES_SECURITY 
		INNER JOIN ZB_BU_SECURITY ON 
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID 
		INNER JOIN ZB_GRPS ON 
			ZB_BU_SECURITY.GROUP_SEQ_ID = ZB_GRPS.GROUP_SEQ_ID
	
	WHERE
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID 
	OR 	ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
		AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL 
		AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL
	ORDER BY
		ZB_MODULES_SECURITY.MODULE_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE       PROCEDURE ZBP_GET_MODULE_RLS_BY_BU (
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
SELECT DISTINCT
	ZB_RLS.ROLE_NAME AS ROLE,
	XY.PERMISSIONS_SEQ_ID,
	XY.MODULE_SEQ_ID
FROM ZB_RLS,
	(	
	SELECT DISTINCT
		P.MODULE_SEQ_ID,
		Q.PERMISSIONS_SEQ_ID,
		Q.ROLE_SEQ_ID
	FROM 
		(SELECT
			MS.MODULE_SEQ_ID
		FROM
			ZB_MODULES_SECURITY MS ,
			ZB_BU_SECURITY BS
		WHERE
			MS.BU_SECURITY_SEQ_ID=BS.BU_SECURITY_SEQ_ID AND
			(BS.BUSINESS_UNIT_SEQ_ID=@P_BUSINESS_UNIT_SEQ_ID OR BS.BUSINESS_UNIT_SEQ_ID=DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
		) P,
		(SELECT
			MS.MODULE_SEQ_ID,
			MS.PERMISSIONS_SEQ_ID,
			BS.ROLE_SEQ_ID
		FROM
			ZB_MODULES_SECURITY MS ,
			ZB_BU_SECURITY BS
		WHERE
			MS.BU_SECURITY_SEQ_ID=BS.BU_SECURITY_SEQ_ID AND
			ROLE_SEQ_ID IS NOT NULL
	UNION
	SELECT
		X.MODULE_SEQ_ID,
		X.PERMISSIONS_SEQ_ID,
		Y.ROLE_SEQ_ID
	FROM 
		(SELECT * 
		FROM
			ZB_MODULES_SECURITY AS M
		WHERE
			M.BU_SECURITY_SEQ_ID IN(SELECT
							B.BU_SECURITY_SEQ_ID
						FROM
							ZB_BU_SECURITY B
						WHERE
							B.BU_SECURITY_SEQ_ID=M.BU_SECURITY_SEQ_ID
						)
		) X,
		(SELECT
			A.BU_SECURITY_SEQ_ID,
			B.ROLE_SEQ_ID
		FROM 
			(SELECT
				BU_SECURITY_SEQ_ID,
				GROUP_SEQ_ID
			FROM ZB_BU_SECURITY 
			) A,
			(SELECT
				ROLE_SEQ_ID ,
				GROUP_SEQ_ID
			FROM
				ZB_BU_SECURITY
			)B
		WHERE
			A.GROUP_SEQ_ID=B.GROUP_SEQ_ID AND 
			ROLE_SEQ_ID IS NOT NULL
		) Y
	WHERE
		X.BU_SECURITY_SEQ_ID=Y.BU_SECURITY_SEQ_ID
	) Q
	WHERE P.MODULE_SEQ_ID=Q.MODULE_SEQ_ID) XY
WHERE XY.ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID
/*
	SELECT  DISTINCT
		ZB_RLS.ROLE_NAME AS ROLE, 
		ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID, 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID
	FROM
		ZB_MODULES_SECURITY INNER JOIN ZB_BU_SECURITY ON 
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID 
				INNER JOIN ZB_RLS ON 
					ZB_BU_SECURITY.ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID
	
	WHERE
		(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID 
	OR 	ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
	AND 	ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL
UNION
	SELECT DISTINCT
		ZB_RLS.ROLE_NAME AS ROLE, 
		ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID, 
		ZB_MODULES_SECURITY.MODULE_SEQ_ID
	FROM
		ZB_MODULES_SECURITY INNER JOIN ZB_BU_SECURITY ON 
			ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID 
		INNER JOIN ZB_BU_SECURITY BU1 ON
			ZB_BU_SECURITY.GROUP_SEQ_ID = BU1.GROUP_SEQ_ID
		INNER JOIN ZB_RLS ON
			BU1.ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID
	WHERE
		ZB_BU_SECURITY.group_seq_id is not null and ZB_BU_SECURITY.role_seq_id is null 
	and	BU1.GROUP_SEQ_ID IS NOT NULL AND BU1.ROLE_SEQ_ID IS NOT NULL
	AND	(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID 
	OR 	ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID())
	AND     (BU1.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID 
	OR 	BU1.BUSINESS_UNIT_SEQ_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()) 
	ORDER BY ZB_MODULES_SECURITY.MODULE_SEQ_ID
*/
/* ALSO BROKEN
SELECT
	ZB_RLS.ROLE_NAME AS ROLE,
	P.PERMISSIONS_SEQ_ID,
	P.MODULE_SEQ_ID
FROM
	ZB_RLS,
	(SELECT DISTINCT 
		X.MODULE_SEQ_ID,
		X.PERMISSIONS_SEQ_ID,
		Y.ROLE_SEQ_ID 
	FROM
		ZB_MODULES_SECURITY X,
		(SELECT DISTINCT 
			A.MODULE_SEQ_ID,
			C.BU AS ROLE_SEQ_ID
		FROM 
			ZB_MODULES_SECURITY A , 
			(SELECT DISTINCT B.ROLE_SEQ_ID AS 
				BU,A.BU_SECURITY_SEQ_ID AS BU_1
			FROM
				ZB_BU_SECURITY B,
				ZB_BU_SECURITY A
			WHERE
				B.BUSINESS_UNIT_SEQ_ID=A.BUSINESS_UNIT_SEQ_ID AND 
				(A.GROUP_SEQ_ID=B.GROUP_SEQ_ID OR A.BU_SECURITY_SEQ_ID=B.BU_SECURITY_SEQ_ID) AND 
				B.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID or B.BUSINESS_UNIT_SEQ_ID = DBO.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()) AS C
		WHERE  
			C.BU_1=A.BU_SECURITY_SEQ_ID) AS Y
	WHERE 
		X.MODULE_SEQ_ID=Y.MODULE_SEQ_ID) AS P
WHERE
	P.ROLE_SEQ_ID=ZB_RLS.ROLE_SEQ_ID
ORDER BY 3
*/

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_NAV_TYPES AS
SELECT
	NAV_TYPE_SEQ_ID,
	[DESCRIPTION]
FROM
	ZB_NAVIGATION_TYPE

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE   PROCEDURE ZBP_GET_RLS_FOR_ACCOUNT (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
DECLARE 	@DEFAULT_BUSINESS_SEQ_ID INT
        SET 	@DEFAULT_BUSINESS_SEQ_ID = dbo.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()
	
SELECT DISTINCT
	ZB_RLS.ROLE_NAME AS RLS
FROM ZB_RLS INNER JOIN ZB_BU_SECURITY
	ON ZB_RLS.ROLE_SEQ_ID = ZB_BU_SECURITY.ROLE_SEQ_ID
WHERE ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
IN(
		SELECT      ZB_BU_SECURITY.BU_SECURITY_SEQ_ID
		FROM         ZB_ACCTS_SECURITY ,  ZB_BU_SECURITY  
		WHERE ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
		     (ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND (ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID or ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID )
		UNION
		
                SELECT ZB_BU_SECURITY.BU_SECURITY_SEQ_ID FROM ZB_BU_SECURITY,
                 (
	              SELECT  ZB_BU_SECURITY.ROLE_SEQ_ID,ZB_BU_SECURITY.GROUP_SEQ_ID
			FROM    ZB_BU_SECURITY 
			WHERE 	ZB_BU_SECURITY.GROUP_SEQ_ID 
			IN (
				SELECT ZB_BU_SECURITY.GROUP_SEQ_ID
				FROM         ZB_BU_SECURITY 
				WHERE  ZB_BU_SECURITY.ROLE_SEQ_ID 
				IN (
					SELECT  ZB_BU_SECURITY.ROLE_SEQ_ID
					FROM    ZB_BU_SECURITY 
					WHERE 	ZB_BU_SECURITY.GROUP_SEQ_ID 
					IN (
						SELECT  ZB_BU_SECURITY.GROUP_SEQ_ID
						FROM 	ZB_ACCTS_SECURITY ,ZB_BU_SECURITY  
						WHERE 	ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID AND 
						      	ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID 
						      	AND (ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID 
								OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID) 
						        AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NOT NULL
							                      AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NULL
					    ) AND ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL
				   )
			)
	 	) AS TEMP1
	WHERE TEMP1.GROUP_SEQ_ID=ZB_BU_SECURITY.GROUP_SEQ_ID OR TEMP1.ROLE_SEQ_ID=ZB_BU_SECURITY.ROLE_SEQ_ID
	
)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_GET_RLS_FOR_ACCOUNT_BY_BU (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	SELECT
		ZB_RLS.ROLE_NAME AS ROLE_NAME
	FROM
		ZB_RLS 
		INNER JOIN ZB_BU_SECURITY ON 
		ZB_RLS.ROLE_SEQ_ID = ZB_BU_SECURITY.ROLE_SEQ_ID 
		INNER JOIN ZB_ACCTS_SECURITY ON 
		ZB_BU_SECURITY.BU_SECURITY_SEQ_ID = ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID
		INNER JOIN ZB_ACCOUNTS ON
		ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = ZB_ACCOUNTS.ACCOUNT_SEQ_ID
				
	WHERE
		ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID AND 
		ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
		ZB_BU_SECURITY.ROLE_SEQ_ID IS NOT NULL AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_GET_RLS_FOR_GROUP (
	@P_GROUP_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	SELECT DISTINCT TOP 100 PERCENT 
		ZB_RLS.ROLE_NAME AS RLS
	FROM
		ZB_RLS   
	WHERE
		ZB_RLS.ROLE_SEQ_ID 
		IN(SELECT ROLE_SEQ_ID FROM ZB_BU_SECURITY
			WHERE ZB_BU_SECURITY.GROUP_SEQ_ID = @P_GROUP_SEQ_ID AND 
			ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
			ROLE_SEQ_ID IS NOT NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_GET_RLS_FOR_GROUP_BY_BU (
	@P_GROUP_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT
)
AS
	SELECT DISTINCT TOP 100 PERCENT 
		ZB_RLS.ROLE_NAME AS ROLE_NAME
	FROM
		ZB_RLS   
	WHERE
		ZB_RLS.ROLE_SEQ_ID 
		IN(SELECT ROLE_SEQ_ID FROM ZB_BU_SECURITY
			WHERE ZB_BU_SECURITY.GROUP_SEQ_ID = @P_GROUP_SEQ_ID AND 
			ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
			ROLE_SEQ_ID IS NOT NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ROLE_NAME_BY_ID(
	@P_ROLE_SEQ_ID AS INT
)
AS
	SELECT
		ROLE_NAME
	FROM
		ZB_RLS
	WHERE ROLE_SEQ_ID = @P_ROLE_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_ROOT_LINKS(
	@P_ACCOUNT_SEQ_ID AS INT,
	@P_BUSINESS_UNIT_SEQ_ID AS INT
) 
AS
DECLARE @DEFAULT_BUSINESS_SEQ_ID 	INT,
	@DEFAULT_PERMISSION_SEQ_ID	INT,
	@DEFAULT_NAV_TYPE_SEQ_ID 	INT,
	@ANOTHER_NAV_TYPE_SEQ_ID 	INT,
	@DEFAULT_IS_NAV			INT
        SET 	@DEFAULT_BUSINESS_SEQ_ID = dbo.ZBF_GET_DEFAULT_BUSINESS_UNIT_ID()
	SET	@DEFAULT_PERMISSION_SEQ_ID = 1
	SET	@DEFAULT_NAV_TYPE_SEQ_ID = 2
	SET	@ANOTHER_NAV_TYPE_SEQ_ID = 4
	SET	@DEFAULT_IS_NAV	= 1
SELECT DISTINCT 
	ZB_MODULES.NAME, 
	ZB_MODULES.MODULE_ACTION
FROM
	ZB_MODULES_SECURITY INNER JOIN ZB_BU_SECURITY ON
		ZB_MODULES_SECURITY.BU_SECURITY_SEQ_ID = ZB_BU_SECURITY.BU_SECURITY_SEQ_ID 
			INNER JOIN ZB_ACCTS_SECURITY ON 
				ZB_BU_SECURITY.BU_SECURITY_SEQ_ID = ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID
					INNER JOIN  ZB_MODULES ON 
						ZB_MODULES_SECURITY.MODULE_SEQ_ID = ZB_MODULES.MODULE_SEQ_ID
WHERE
	(ZB_MODULES_SECURITY.PERMISSIONS_SEQ_ID = @DEFAULT_PERMISSION_SEQ_ID ) AND 
	(ZB_MODULES.IS_NAV = @DEFAULT_IS_NAV) AND 
	(ZB_MODULES.NAV_TYPE_SEQ_ID = @DEFAULT_NAV_TYPE_SEQ_ID OR ZB_MODULES.NAV_TYPE_SEQ_ID = @ANOTHER_NAV_TYPE_SEQ_ID ) AND 
	(ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID) AND 
	(ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID = @DEFAULT_BUSINESS_SEQ_ID  OR ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID =@P_BUSINESS_UNIT_SEQ_ID) AND 
	(ZB_MODULES.PARENT_MODULE_SEQ_ID <> 0) AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_VAID_BU_FOR_ACCOUNT (
	@P_ACCOUNT_SEQ_ID INT,
	@P_ISSYSADMIN BIT
 )
AS
	DECLARE @SELECTCLAUSE VARCHAR(1000)
	DECLARE @FROMCLAUSE VARCHAR(1000)
	DECLARE @WHERECLAUSE VARCHAR(1000)
	DECLARE @ORDERCLAUSE VARCHAR(1000)
	SET @SELECTCLAUSE = 'SELECT DISTINCT TOP 100 PERCENT 
		ZB_BUSINESS_UNITS.BUSINESS_UNIT_SEQ_ID, ZB_BUSINESS_UNITS.NAME'
	IF (@P_ISSYSADMIN = 1)
		BEGIN 
			SET @FROMCLAUSE = '	FROM ZB_BUSINESS_UNITS'
		END
	ELSE
		BEGIN
			SET @FROMCLAUSE = '	FROM
				ZB_BUSINESS_UNITS INNER JOIN ZB_BU_SECURITY ON 
					ZB_BUSINESS_UNITS.BUSINESS_UNIT_SEQ_ID = ZB_BU_SECURITY.BUSINESS_UNIT_SEQ_ID 
						INNER JOIN ZB_ACCTS_SECURITY ON 
							ZB_BU_SECURITY.BU_SECURITY_SEQ_ID = ZB_ACCTS_SECURITY.BU_SECURITY_SEQ_ID
						                       INNER JOIN ZB_RLS ON 
								ZB_BU_SECURITY.ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID'
			SET @WHERECLAUSE = ' WHERE (ZB_ACCTS_SECURITY.ACCOUNT_SEQ_ID = ' + CONVERT(VARCHAR,@P_ACCOUNT_SEQ_ID) + ') AND (ZB_RLS.IS_SYSTEM_ONLY = 0) AND ZB_BU_SECURITY.GROUP_SEQ_ID IS NULL '
		END
	SET @ORDERCLAUSE = ' ORDER BY ZB_BUSINESS_UNITS.NAME'
	EXEC (@SELECTCLAUSE + @FROMCLAUSE + @WHERECLAUSE + @ORDERCLAUSE) 
RETURN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_GET_WORK_FLOWS (
	@P_WORK_FLOW_NAME AS NVARCHAR(50) = NULL
)
AS
	IF @P_WORK_FLOW_NAME = NULL
		BEGIN
			SELECT DISTINCT 
				WORK_FLOW_NAME 
			FROM 
				ZB_WORK_FLOWS
			ORDER BY
				WORK_FLOW_NAME
		END
	ELSE
		BEGIN
			SELECT
				WORK_FLOW_SEQ_ID,
				WORK_FLOW_NAME,
				ORDER_ID,
				MODULE_SEQ_ID
			 FROM
				ZB_WORK_FLOWS
			WHERE
				WORK_FLOW_NAME = @P_WORK_FLOW_NAME
			ORDER BY
				ORDER_ID
		END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_ACCOUNT_CHOICES (
	@P_ACCOUNT				NVARCHAR(25),
	@P_BUSINESS_UNIT_SEQ_ID		INT,
	@P_BUSINESS_UNIT_NAME		NVARCHAR(50),
	@P_BACK_COLOR			NVARCHAR(15),
	@P_LEFT_COLOR 			NVARCHAR(15),
	@P_HEAD_COLOR 			NVARCHAR(15),
	@P_SUB_HEAD_COLOR		NVARCHAR(15),
	@P_COLOR_SCHEME 			NVARCHAR(15),
	@P_MODULE_ACTION 			NVARCHAR(50),
	@P_RECORDS_PER_PAGE		NVARCHAR(10)
--,	@P_ADDUPD_BY			INT
)
AS
	BEGIN
		-- UPDATE PROFILE
		UPDATE ZB_ACCOUNT_CHOICES SET
			ACCOUNT = @P_ACCOUNT,
			BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID,
			BUSINESS_UNIT_NAME = @P_BUSINESS_UNIT_NAME,
			BACK_COLOR = @P_BACK_COLOR,
			LEFT_COLOR = @P_LEFT_COLOR,
			HEAD_COLOR = @P_HEAD_COLOR,
			SUB_HEAD_COLOR = @P_SUB_HEAD_COLOR,
			COLOR_SCHEME = @P_COLOR_SCHEME,
			MODULE_ACTION = @P_MODULE_ACTION,
			RECORDS_PER_PAGE = @P_RECORDS_PER_PAGE
		WHERE ACCOUNT= @P_ACCOUNT
	END
	RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_UPDATE_ACCOUNT_GRPS (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_GRPS NVARCHAR(1000),
	@P_ADDUPD_BY AS INT
)
AS
	DECLARE @GROUP_SEQ_ID 		INT,
		@BU_SECURITY_SEQ_ID 	INT,
		@GROUP_NAME		VARCHAR(50),
		@IS_SYSTEM 		INT,
		@Pos 			INT
		SET @P_GRPS = LTRIM(RTRIM(@P_GRPS))+ ','
		SET @Pos = CHARINDEX(',', @P_GRPS, 1)
		--NEED TO DELETE EXISTING ROLES ASSOCITAED BEFORE 
		-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
		-- IS MOVED FROM CODE			
		EXEC ZBP_DEL_ACCOUNT_GRPS @P_ACCOUNT_SEQ_ID,@P_BUSINESS_UNIT_SEQ_ID,@P_ADDUPD_BY	
		
		PRINT '********DELETED'
	
		IF REPLACE(@P_GRPS, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @GROUP_NAME = LTRIM(RTRIM(LEFT(@P_GRPS, @Pos - 1)))
				IF @GROUP_NAME <> ''
				BEGIN
					
					SELECT  @GROUP_SEQ_ID = GROUP_SEQ_ID
					FROM 	ZB_GRPS
					WHERE GROUP_NAME = @GROUP_NAME	
				
					SELECT
						@BU_SECURITY_SEQ_ID = BU_SECURITY_SEQ_ID
					FROM
						ZB_BU_SECURITY
					WHERE
						GROUP_SEQ_ID = @GROUP_SEQ_ID AND
						BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND 
						ROLE_SEQ_ID IS NULL
				
				
					IF @BU_SECURITY_SEQ_ID IS NOT NULL
					BEGIN
						
						INSERT ZB_ACCTS_SECURITY (
							BU_SECURITY_SEQ_ID,
							ACCOUNT_SEQ_ID
						)VALUES(
							@BU_SECURITY_SEQ_ID,
							@P_ACCOUNT_SEQ_ID
						)
				
						PRINT '*****************INSERTED INTO ZB_ACCTS_SECURITY'
					END
	
				END
				SET @P_GRPS = RIGHT(@P_GRPS, LEN(@P_GRPS) - @Pos)
				SET @Pos = CHARINDEX(',', @P_GRPS, 1)
	
			END
		END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_ACCOUNT_PROFILE
(
	@P_ACCOUNT_SEQ_ID	INT,
	@P_ACCOUNT			NVARCHAR(25),
	@P_UPDATED_BY		INT,
	@P_CREATED_BY		INT,
	@P_SYSTEM_STATUS_ID	INT,
	@P_FIRST_NAME		NVARCHAR(30),
	@P_LAST_NAME		NVARCHAR(30),
	@P_MIDDLE_NAME		NVARCHAR(30),
	@P_PREFERED_NAME		NCHAR(90),
	@P_EMAIL			NVARCHAR(50),
	@P_PWD			NVARCHAR(256),
	@P_FAILED_ATTEMPTS	INT,
	@P_DATE_CREATED		DATETIME,
	@P_UPDATED_DATE		DATETIME,
	@P_LAST_LOGIN		DATETIME,
	@P_TIME_ZONE		INT,
	@P_LOCATION			NVARCHAR(100),
	@P_ENABLENOTIFICATIONS 	BIT,
	@P_ADDUPD_BY		INT
)
AS
BEGIN
-- UPDATE PROFILE
UPDATE ZB_ACCOUNTS SET
	SYSTEM_STATUS_ID = @P_SYSTEM_STATUS_ID,
	ACCOUNT = @P_ACCOUNT,
	ADDED_BY = @P_CREATED_BY,
	UPDATED_BY = @P_UPDATED_BY,
	FIRST_NAME = @P_FIRST_NAME,
	LAST_NAME = @P_LAST_NAME,
	MIDDLE_NAME = @P_MIDDLE_NAME,
	PREFERED_NAME = @P_PREFERED_NAME,
	EMAIL = @P_EMAIL,
	PWD = @P_PWD,
	FAILED_ATTEMPTS = @P_FAILED_ATTEMPTS,
	ADDED_DATE = @P_DATE_CREATED,
	UPDATED_DATE = @P_UPDATED_DATE,
	LAST_LOGIN = @P_LAST_LOGIN,
	TIME_ZONE = @P_TIME_ZONE,
	LOCATION = @P_LOCATION,
	ENABLE_NOTIFICATIONS = @P_ENABLENOTIFICATIONS
WHERE ACCOUNT_SEQ_ID = @P_ACCOUNT_SEQ_ID
END
IF @@ERROR = 0 RETURN 1
ELSE
RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE   PROCEDURE ZBP_UPDATE_ACCOUNT_RLS (
	@P_ACCOUNT_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ROLES NVARCHAR(1000),
	@P_ADDUPD_BY INT
)
AS
	DECLARE @ROLE_SEQ_ID 		INT,
		@BU_SECURITY_SEQ_ID 	INT,
		@ROLE_NAME		VARCHAR(50),
		@IS_SYSTEM 		INT,
		@Pos 			INT
		SET @P_ROLES = LTRIM(RTRIM(@P_ROLES))+ ','
		SET @Pos = CHARINDEX(',', @P_ROLES, 1)
	
	
		--NEED TO DELETE EXISTING ROLES ASSOCITAED BEFORE 
		-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
		-- IS MOVED FROM CODE			
	
		EXEC ZBP_DEL_ACCOUNT_RLS @P_ACCOUNT_SEQ_ID,@P_BUSINESS_UNIT_SEQ_ID,@P_ADDUPD_BY	
		PRINT '********DELETED'
		IF REPLACE(@P_ROLES, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @ROLE_NAME = LTRIM(RTRIM(LEFT(@P_ROLES, @Pos - 1)))
				IF @ROLE_NAME <> ''
				BEGIN
					
					SELECT  @ROLE_SEQ_ID = ROLE_SEQ_ID,
						@IS_SYSTEM   = IS_SYSTEM 
					FROM 	ZB_RLS
					WHERE ROLE_NAME = @ROLE_NAME	
				
					SELECT
						@BU_SECURITY_SEQ_ID = BU_SECURITY_SEQ_ID
					FROM
						ZB_BU_SECURITY
					WHERE
						ROLE_SEQ_ID = @ROLE_SEQ_ID AND
						GROUP_SEQ_ID IS NULL AND
						BUSINESS_UNIT_SEQ_ID = DBO.ZBF_CHOOSE_BUSINESS_UNIT_ID
									(@IS_SYSTEM,
									  @P_BUSINESS_UNIT_SEQ_ID)
				
					-- SELECT THE BUSINESS SEQID BASED FOR THE ROLE_SEQ_ID 
					-- AND THE BUSINESS UNIT ID (NOTE:BUSINESS_UNIT_ID 
					-- MIGHT CHANGE HERE BASED ON IS_SYSTEM PROPERTY FOR THE ROLE)
					IF @BU_SECURITY_SEQ_ID IS NOT NULL
					BEGIN
						
						INSERT ZB_ACCTS_SECURITY (
							BU_SECURITY_SEQ_ID,
							ACCOUNT_SEQ_ID
						)VALUES(
							@BU_SECURITY_SEQ_ID,
							@P_ACCOUNT_SEQ_ID
						)
				
						PRINT '*****************INSERTED INTO ZB_ACCTS_SECURITY'
					END
	
				END
				SET @P_ROLES = RIGHT(@P_ROLES, LEN(@P_ROLES) - @Pos)
				SET @Pos = CHARINDEX(',', @P_ROLES, 1)
	
			END
		END	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_BUSINESS_UNIT_PROFILE (
	@P_BUSINESS_UNIT_SEQ_ID 		INT,
	@P_NAME					NVARCHAR(50),
	@P_DESCRIPTION				NVARCHAR(50),
	@P_SKIN					NVARCHAR(50),
	@P_PARENT_BUSINESS_UNIT_SEQ_ID 		INT,
	@P_STATUS_SEQ_ID 			INT,
	@P_CONNECTION_STRING			NVARCHAR(50),
	@P_DAL					NVARCHAR(50),
	@P_ADDUPD_BY			INT
)
 AS
	UPDATE 
		ZB_BUSINESS_UNITS 
	SET 
		[NAME]	 = @P_NAME,
		[DESCRIPTION] = @P_DESCRIPTION,
		SKIN = @P_SKIN,
		PARENT_BUSINESS_UNIT_SEQ_ID =  @P_PARENT_BUSINESS_UNIT_SEQ_ID,
		STATUS_SEQ_ID = @P_STATUS_SEQ_ID
	WHERE
		BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_BU_PROFILE (
	@P_BUSINESS_UNIT_SEQ_ID 		INT,
	@P_NAME					NVARCHAR(50),
	@P_DESCRIPTION				NVARCHAR(50),
	@P_SKIN					NVARCHAR(50),
	@P_PARENT_BUSINESS_UNIT_SEQ_ID 		INT,
	@P_STATUS_SEQ_ID 			INT,
	@P_CONNECTION_STRING			NVARCHAR(50),
	@P_DAL					NVARCHAR(50),
	@P_ADDUPD_BY			INT
)
 AS
	UPDATE 
		ZB_BUSINESS_UNITS 
	SET 
		[NAME]	 = @P_NAME,
		[DESCRIPTION] = @P_DESCRIPTION,
		SKIN = @P_SKIN,
		PARENT_BUSINESS_UNIT_SEQ_ID =  @P_PARENT_BUSINESS_UNIT_SEQ_ID,
		STATUS_SEQ_ID = @P_STATUS_SEQ_ID
	WHERE
		BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_DROP_BOX
(
	@P_DROP_BOX_SEQ_ID	INT,
	@P_DESCRIPTION		NVARCHAR(255),
	@P_ADDUPD_BY			INT
)
AS
BEGIN
	-- UPDATE RECORD
	UPDATE ZB_DROP_BOXES SET
		DESCRIPTION = @P_DESCRIPTION
	WHERE
		DROP_BOX_SEQ_ID = @P_DROP_BOX_SEQ_ID
END
IF @@ERROR = 0 
	RETURN 1
ELSE
	IF @@ERROR = 1
		RETURN 2
	ELSE
		RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_DROP_BOX_DETAILS
@P_ID INT,
@P_CODE INT,
@P_VALUE VARCHAR(300),
@P_STATUS INT,
@P_DROP_BOX_SEQ_ID INT,
@P_ACCOUNT_SEQ_ID	INT,
@P_ADDUPD_BY	INT
AS
BEGIN
	IF EXISTS(
		SELECT
			DROP_BOX_DET_ID 
		FROM
			ZB_DROP_BOX_DETAIL
		WHERE
			ZB_DROP_BOX_DETAIL.DROP_BOX_DET_ID = @P_ID) 
	BEGIN
		UPDATE
			ZB_DROP_BOX_DETAIL SET 
			DROP_BOX_DET_CODE = @P_CODE,
			DROP_BOX_DET_VALUE = @P_VALUE,
			DROP_BOX_DET_STATUS = @P_STATUS,
		    	DROP_BOX_SEQ_ID   = @P_DROP_BOX_SEQ_ID,
		    	UPDATED_BY = @P_ACCOUNT_SEQ_ID,
			UPDATED_DATE = GETDATE()
		 WHERE
			ZB_DROP_BOX_DETAIL.DROP_BOX_DET_ID = @P_ID
		RETURN @@ERROR
	END
ELSE
	BEGIN
	  RETURN -1
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_UPDATE_GROUP (
	@P_GROUP_SEQ_ID		INT,
	@P_GROUP_NAME		NVARCHAR(50),
	@P_GROUP_DESCRIPTION	NVARCHAR(50),
	@P_ADDUPD_BY	INT
)
AS
	UPDATE ZB_GRPS SET
		GROUP_NAME = @P_GROUP_NAME,
		DESCRIPTION = @P_GROUP_DESCRIPTION
	WHERE
		GROUP_SEQ_ID = @P_GROUP_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_UPDATE_GROUP_INFO (
	@P_GROUP_SEQ_ID		INT,
	@P_GROUP_NAME		NVARCHAR(50),
	@P_GROUP_DESCRIPTION	NVARCHAR(50),
	@P_ADDUPD_BY	INT
)
AS
	UPDATE ZB_GRPS SET
		GROUP_NAME = @P_GROUP_NAME,
		DESCRIPTION = @P_GROUP_DESCRIPTION
	WHERE
		GROUP_SEQ_ID = @P_GROUP_SEQ_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE    PROCEDURE ZBP_UPDATE_GROUP_RLS (
	@P_GROUP_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_ROLES NVARCHAR(1000),
	@P_ADDUPD_BY    INT
)
AS
	DECLARE @ROLE_SEQ_ID 		INT,
		@BU_SECURITY_SEQ_ID 	INT,
		@ROLE_NAME		VARCHAR(50),
		@IS_SYSTEM 		INT,
		@Pos 			INT
		--NEED TO DELETE EXISTING ROLES ASSOCITAED BEFORE 
		-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
		-- IS MOVED FROM CODE			
		EXEC ZBP_DEL_GROUP_ROLES @P_GROUP_SEQ_ID,@P_BUSINESS_UNIT_SEQ_ID,@P_ADDUPD_BY	
		SET @P_ROLES = LTRIM(RTRIM(@P_ROLES))+ ','
		SET @Pos = CHARINDEX(',', @P_ROLES, 1)
	
		IF REPLACE(@P_ROLES, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @ROLE_NAME = LTRIM(RTRIM(LEFT(@P_ROLES, @Pos - 1)))
				IF @ROLE_NAME <> ''
				BEGIN
					--SELECT THE ROLE_SEQ_ID FROM THE ROLES
					--TABLE FOR ALL THE ROLES PASSED
					SELECT  @ROLE_SEQ_ID = ROLE_SEQ_ID,
						@IS_SYSTEM   = IS_SYSTEM 
					FROM 	ZB_RLS
					WHERE ROLE_NAME = @ROLE_NAME	
				
								
					/*
					INSERT THE ZB_BU_SECURITY
					WITH ROLES INFORMATION
					*/	
					IF @ROLE_SEQ_ID IS NOT NULL
					BEGIN
									
						INSERT ZB_BU_SECURITY (
							BUSINESS_UNIT_SEQ_ID,
							GROUP_SEQ_ID,
							ROLE_SEQ_ID
						)VALUES(
							@P_BUSINESS_UNIT_SEQ_ID,
							@P_GROUP_SEQ_ID,
							@ROLE_SEQ_ID
						)
				
						PRINT '*****************INSERTED INTO ZB_BU_SECURITY'
					END
	
				END
				SET @P_ROLES = RIGHT(@P_ROLES, LEN(@P_ROLES) - @Pos)
				SET @Pos = CHARINDEX(',', @P_ROLES, 1)
	
			END
		END	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_MESSAGE_PROFILE (
	@P_MESSAGE_SEQ_ID		INT,
	@P_BUSINESS_UNIT_SEQ_ID	INT,
	@P_NAME			NVARCHAR(50),
	@P_TITLE			NVARCHAR(100),
	@P_DESCRIPTION		NVARCHAR(300),
	@P_BODY			NTEXT,
	@P_ADDUPD_BY			INT
)
AS
	BEGIN
		-- UPDATE PROFILE
		UPDATE ZB_MESSAGES SET
			BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID,
			[NAME]  = @P_NAME,
			TITLE = @P_TITLE,
			[DESCRIPTION] = @P_DESCRIPTION,
			BODY = @P_BODY
		WHERE MESSAGE_SEQ_ID = @P_MESSAGE_SEQ_ID
	END
	IF @@ERROR = 0 RETURN 1
	ELSE
		RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_UPDATE_MODULE_GRPS (
	@P_MODULE_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_PERMISSIONS_SEQ_ID INT,
	@P_GRPS NVARCHAR(1000),
	@P_ADDUPD_BY	INT
)
AS
	DECLARE @GROUP_SEQ_ID 		INT,
		@BU_SECURITY_SEQ_ID 	INT,
		@GROUP_NAME		VARCHAR(50),
		@Pos 			INT
		SET @P_GRPS = LTRIM(RTRIM(@P_GRPS))+ ','
		SET @Pos = CHARINDEX(',', @P_GRPS, 1)
		--NEED TO DELETE EXISTING ROLES ASSOCITAED BEFORE 
		-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
		-- IS MOVED FROM CODE			
		EXEC ZBP_DEL_MODULE_SECURITY_BY_GROUP @P_MODULE_SEQ_ID,@P_PERMISSIONS_SEQ_ID,@P_BUSINESS_UNIT_SEQ_ID,@P_ADDUPD_BY	
		
		PRINT '********DELETED'
	
		IF REPLACE(@P_GRPS, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @GROUP_NAME = LTRIM(RTRIM(LEFT(@P_GRPS, @Pos - 1)))
				IF @GROUP_NAME <> ''
				BEGIN
					
					SELECT  @GROUP_SEQ_ID = GROUP_SEQ_ID
					FROM 	ZB_GRPS
					WHERE GROUP_NAME = @GROUP_NAME	
				
					SELECT
						@BU_SECURITY_SEQ_ID=BU_SECURITY_SEQ_ID
					FROM
						ZB_BU_SECURITY
					WHERE
					GROUP_SEQ_ID = @GROUP_SEQ_ID AND
					BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND 
					ROLE_SEQ_ID IS NULL
					IF NOT EXISTS(
							SELECT 
								BU_SECURITY_SEQ_ID 
							FROM 
								ZB_MODULES_SECURITY 
							WHERE 
							MODULE_SEQ_ID = @P_MODULE_SEQ_ID AND
							BU_SECURITY_SEQ_ID = @BU_SECURITY_SEQ_ID AND
							PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID
					)
					BEGIN
			--			PRINT('INSERT RECORD')
						INSERT ZB_MODULES_SECURITY (
							MODULE_SEQ_ID,
							BU_SECURITY_SEQ_ID,
							PERMISSIONS_SEQ_ID
						)
						VALUES (
							@P_MODULE_SEQ_ID,
							@BU_SECURITY_SEQ_ID,
							@P_PERMISSIONS_SEQ_ID
						)
					END
	
				END
				SET @P_GRPS = RIGHT(@P_GRPS, LEN(@P_GRPS) - @Pos)
				SET @Pos = CHARINDEX(',', @P_GRPS, 1)
	
			END
		END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_MODULE_PROFILE
(
	@P_MODULE_SEQ_ID	INT,
	@P_NAME			NVARCHAR(100),
	@P_DESCRIPTION		NVARCHAR(50),
	@P_SOURCE		NVARCHAR(512),
	@P_ENABLE_VIEW_STATE	BIT,
	@P_IS_NAV		BIT,
	@P_NAV_TYPE_SEQ_ID	INT,
	@P_PARENTID		INT,
	@P_ACTION		NVARCHAR(255),
	@P_ADDUPD_BY			INT
)
AS
BEGIN
	-- UPDATE PROFILE
	UPDATE ZB_MODULES SET
		NAME				=@P_NAME,
		DESCRIPTION			=@P_DESCRIPTION,
		SOURCE 				=@P_SOURCE,
		ENABLE_VIEW_STATE		=@P_ENABLE_VIEW_STATE,
		IS_NAV				=@P_IS_NAV,
		NAV_TYPE_SEQ_ID		=@P_NAV_TYPE_SEQ_ID,
		PARENT_MODULE_SEQ_ID	=@P_PARENTID,
		MODULE_ACTION				=@P_ACTION
	WHERE
		MODULE_SEQ_ID = @P_MODULE_SEQ_ID
END
IF @@ERROR = 0 
	RETURN 1
ELSE
	IF @@ERROR = 1
		RETURN 2
	ELSE
		RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE ZBP_UPDATE_MODULE_RLS (
	@P_ROLES NVARCHAR(1000),
	@P_MODULE_SEQ_ID INT,
	@P_BUSINESS_UNIT_SEQ_ID INT,
	@P_PERMISSIONS_SEQ_ID INT,
	@P_ADDUPD_BY			INT
) 
AS
	DECLARE @ROLE_SEQ_ID 		AS 	INT
	DECLARE @BU_SECURITY_SEQ_ID 	AS 	INT
	DECLARE @ROLE_NAME			VARCHAR(50)
	DECLARE @Pos 				INT
	--NEED TO DELETE EXISTING ROLES ASSOCITAED WITH THE MODULE  BEFORE 
	-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
	-- IS MOVED FROM CODE			
	EXEC ZBP_DEL_MODULE_SECTY_BY_ROLE @P_MODULE_SEQ_ID,@P_PERMISSIONS_SEQ_ID,@P_BUSINESS_UNIT_SEQ_ID,@P_ADDUPD_BY	
	
	SET @P_ROLES = LTRIM(RTRIM(@P_ROLES))+ ','
	SET @Pos = CHARINDEX(',', @P_ROLES, 1)
	IF REPLACE(@P_ROLES, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @ROLE_NAME = LTRIM(RTRIM(LEFT(@P_ROLES, @Pos - 1)))
			IF @ROLE_NAME <> ''
			BEGIN
				--select the role seq id first
				SELECT @ROLE_SEQ_ID = ZB_RLS.ROLE_SEQ_ID 
				FROM ZB_RLS 
				WHERE ROLE_NAME=@ROLE_NAME
 				SELECT
					@BU_SECURITY_SEQ_ID=BU_SECURITY_SEQ_ID
				FROM
					ZB_BU_SECURITY
				WHERE
					ROLE_SEQ_ID = @ROLE_SEQ_ID AND
					BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID AND
					GROUP_SEQ_ID IS NULL
					--PRINT('@BU_SECURITY_SEQ_ID = ' + CONVERT(VARCHAR,@BU_SECURITY_SEQ_ID))
				IF NOT EXISTS(
						SELECT 
							BU_SECURITY_SEQ_ID 
						FROM 
							ZB_MODULES_SECURITY 
						WHERE 
						MODULE_SEQ_ID = @P_MODULE_SEQ_ID AND
						BU_SECURITY_SEQ_ID = @BU_SECURITY_SEQ_ID AND
						PERMISSIONS_SEQ_ID = @P_PERMISSIONS_SEQ_ID
				)
				BEGIN
					--PRINT('INSERT RECORD')
					INSERT ZB_MODULES_SECURITY (
						MODULE_SEQ_ID,
						BU_SECURITY_SEQ_ID,
						PERMISSIONS_SEQ_ID
					)
					VALUES (
						@P_MODULE_SEQ_ID,
						@BU_SECURITY_SEQ_ID,
						@P_PERMISSIONS_SEQ_ID
					)
				END
		
			END
				SET @P_ROLES = RIGHT(@P_ROLES, LEN(@P_ROLES) - @Pos)
				SET @Pos = CHARINDEX(',', @P_ROLES, 1)
		
		END
	END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_ROLE (
	@P_ORIGINALROLENAME	NVARCHAR(50),
	@P_ROLE_NAME		NVARCHAR(50),
	@P_DESCRIPTION			NVARCHAR(20),
	@P_ADDUPD_BY			INT
)
AS
	UPDATE ZB_RLS SET
		ROLE_NAME = @P_ROLE_NAME,
		DESCRIPTION = @P_DESCRIPTION
	WHERE
		ROLE_NAME = @P_ORIGINALROLENAME

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_STATE_PROFILE (
	@P_STATE AS NVARCHAR(2),
	@P_LONGNAME AS NVARCHAR(25),
	@P_STATUS_SEQ_ID AS INT,
	@P_ADDUPD_BY			INT
)
 AS
	UPDATE 
		ZB_STATES 
	SET 
		DESCRIPTION = @P_LONGNAME,
		STATUS_SEQ_ID= @P_STATUS_SEQ_ID
	WHERE
		STATE = @P_STATE

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPDATE_WORK_FLOW_PROFILE(
	@P_WORK_FLOW_SEQ_ID AS INT,
	@P_ORDER_ID AS INT,
	@P_WORK_FLOW_NAME  AS NVARCHAR(50),
	@P_ACTION AS NVARCHAR(255),
  @P_ADDUPD_BY			INT
)
AS
BEGIN
-- UPDATE PROFILE
UPDATE ZB_WORK_FLOWS SET
	ORDER_ID = @P_ORDER_ID,
	WORK_FLOW_NAME = @P_WORK_FLOW_NAME,
	MODULE_SEQ_ID = @P_ACTION
WHERE WORK_FLOW_SEQ_ID = @P_WORK_FLOW_SEQ_ID
END
IF @@ERROR = 0 RETURN 1
ELSE
RETURN @@ERROR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ZBP_UPD_ALL_ACCTS_ROLE_BY_BU (
	@P_ROLE_SEQ_ID AS INT,
	@P_BUSINESS_UNIT_SEQ_ID AS INT,
	@P_ACCOUNT AS NVARCHAR(25),
	@P_ADDUPD_BY			INT
)
AS
	DECLARE @ACCOUNT_SEQ_ID AS INT
	DECLARE @BU_SECURITY_SEQ_ID AS INT
	SET @ACCOUNT_SEQ_ID = (SELECT ZB_ACCOUNTS.ACCOUNT_SEQ_ID FROM ZB_ACCOUNTS WHERE ACCOUNT = @P_ACCOUNT)
	SET @BU_SECURITY_SEQ_ID = (
			SELECT
				BU_SECURITY_SEQ_ID
			FROM
				ZB_BU_SECURITY
			WHERE
				ROLE_SEQ_ID = @P_ROLE_SEQ_ID AND
				BUSINESS_UNIT_SEQ_ID = @P_BUSINESS_UNIT_SEQ_ID
				AND GROUP_SEQ_ID IS NULL
		)
	INSERT INTO
		ZB_ACCTS_SECURITY(BU_SECURITY_SEQ_ID,ACCOUNT_SEQ_ID)
	VALUES(
		@BU_SECURITY_SEQ_ID,
		@ACCOUNT_SEQ_ID
	)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

