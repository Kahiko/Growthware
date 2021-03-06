﻿CREATE PROCEDURE [ZFP_GET_NVP]
	@P_NVP_SEQ_ID int,
	@P_ACCT_SEQ_ID int,
	@P_SE_SEQ_ID int,
	@P_ErrorCode int OUTPUT
AS
	IF @P_NVP_SEQ_ID > -1
		BEGIN
			SELECT
				*
			FROM
				ZFC_NVP
			WHERE
				ZFC_NVP.NVP_SEQ_ID = @P_NVP_SEQ_ID
		END
	ELSE
		BEGIN
			IF @P_ACCT_SEQ_ID > -1
				BEGIN -- get only valid NVP for the given account
					DECLARE @V_PERMISSION_ID INT
					SET @V_PERMISSION_ID = dbo.ZFF_GET_VIEW_PERMISSION_ID()
					--DECLARE @V_AvalibleItems TABLE ([ID] INT, TITLE VARCHAR(30), [DESCRIPTION] VARCHAR(256), URL VARCHAR(256), PARENT INT, SORT_ORDER INT, ROLE VARCHAR(50),FUNCTION_TYPE_SEQ_ID INT)
					DECLARE @V_AvalibleItems TABLE ([NVP_SEQ_ID] int,[STATIC_NAME] varchar(30),
					[DISPLAY] varchar(128),
					[DESCRIPTION] varchar(256),
					[ADDED_BY] int,
					[ADDED_DATE] datetime,
					[UPDATED_BY] int,
					[UPDATED_DATE] datetime,
					[ROLE] VARCHAR(50))
					INSERT INTO @V_AvalibleItems
					SELECT -- Items via roles
						ZFC_NVP.NVP_SEQ_ID,
						ZFC_NVP.STATIC_NAME,
						ZFC_NVP.DISPLAY,
						ZFC_NVP.[DESCRIPTION],
						ZFC_NVP.ADDED_BY,
						ZFC_NVP.ADDED_DATE,
						ZFC_NVP.UPDATED_BY,
						ZFC_NVP.UPDATED_DATE,
						ROLES.NAME AS [ROLE]
					FROM
						ZFC_SECURITY_RLS_SE SE_ROLES,
						ZFC_SECURITY_RLS ROLES,
						ZFC_SECURITY_NVP_RLS [SECURITY],
						ZFC_NVP,
						ZFC_PERMISSIONS [PERMISSIONS]
					WHERE
						SE_ROLES.ROLE_SEQ_ID = ROLES.ROLE_SEQ_ID
						AND SECURITY.RLS_SE_SEQ_ID = SE_ROLES.RLS_SE_SEQ_ID
						AND SECURITY.NVP_SEQ_ID = ZFC_NVP.NVP_SEQ_ID
						AND [PERMISSIONS].NVP_SEQ_DET_ID = SECURITY.PERMISSIONS_NVP_SEQ_DET_ID
						AND [PERMISSIONS].NVP_SEQ_DET_ID = @V_PERMISSION_ID
						AND SE_ROLES.SE_SEQ_ID IN (SELECT SE_SEQ_ID FROM dbo.ZFF_GET_SE_PARENTS(1,@P_SE_SEQ_ID))
					INSERT INTO @V_AvalibleItems
					SELECT -- Items via groups
						ZFC_NVP.NVP_SEQ_ID,
						ZFC_NVP.STATIC_NAME,
						ZFC_NVP.DISPLAY,
						ZFC_NVP.[DESCRIPTION],
						ZFC_NVP.ADDED_BY,
						ZFC_NVP.ADDED_DATE,
						ZFC_NVP.UPDATED_BY,
						ZFC_NVP.UPDATED_DATE,
						ROLES.NAME AS [ROLE]
					FROM
						ZFC_SECURITY_NVP_GRPS,
						ZFC_SECURITY_GRPS_SE,
						ZFC_SECURITY_GRPS_RLS,
						ZFC_SECURITY_RLS_SE,
						ZFC_SECURITY_RLS ROLES,
						ZFC_NVP,
						ZFC_PERMISSIONS [PERMISSIONS]
					WHERE
						ZFC_SECURITY_NVP_GRPS.NVP_SEQ_ID = ZFC_NVP.NVP_SEQ_ID
						AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_NVP_GRPS.GRPS_SE_SEQ_ID
						AND ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID
						AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
						AND ROLES.ROLE_SEQ_ID = ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID
						AND [PERMISSIONS].NVP_SEQ_DET_ID = ZFC_SECURITY_NVP_GRPS.PERMISSIONS_NVP_SEQ_DET_ID
						AND [PERMISSIONS].NVP_SEQ_DET_ID = @V_PERMISSION_ID
						AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN (SELECT SE_SEQ_ID FROM dbo.ZFF_GET_SE_PARENTS(1,@P_SE_SEQ_ID))

					DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
					INSERT INTO @V_AccountRoles
					SELECT -- Roles via roles
						ZFC_SECURITY_RLS.[NAME] AS Roles
					FROM
						ZFC_ACCTS,
						ZFC_SECURITY_ACCTS_RLS,
						ZFC_SECURITY_RLS_SE,
						ZFC_SECURITY_RLS
					WHERE
						ZFC_SECURITY_ACCTS_RLS.ACCT_SEQ_ID = @P_ACCT_SEQ_ID
						AND ZFC_SECURITY_ACCTS_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
						AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID IN (SELECT SE_SEQ_ID FROM dbo.ZFF_GET_SE_PARENTS(1,@P_SE_SEQ_ID))
						AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
					UNION
					SELECT -- Roles via groups
						ZFC_SECURITY_RLS.[NAME] AS Roles
					FROM
						ZFC_ACCTS,
						ZFC_SECURITY_ACCTS_GRPS,
						ZFC_SECURITY_GRPS_SE,
						ZFC_SECURITY_GRPS_RLS,
						ZFC_SECURITY_RLS_SE,
						ZFC_SECURITY_RLS
					WHERE
						ZFC_SECURITY_ACCTS_GRPS.ACCT_SEQ_ID = @P_ACCT_SEQ_ID
						AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN (SELECT SE_SEQ_ID FROM dbo.ZFF_GET_SE_PARENTS(1,@P_SE_SEQ_ID))
						AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID
						AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
						AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID

					DECLARE @V_AllItems TABLE ([NVP_SEQ_ID] int,[STATIC_NAME] varchar(30),
					[DISPLAY] varchar(128),
					[DESCRIPTION] varchar(256),
					[ADDED_BY] int,
					[ADDED_DATE] datetime,
					[UPDATED_BY] int,
					[UPDATED_DATE] datetime)
					INSERT INTO @V_AllItems
					SELECT -- Last but not least get the menu items when there are matching account roles.
						NVP_SEQ_ID,
						STATIC_NAME,
						DISPLAY,
						[DESCRIPTION],
						ADDED_BY,
						ADDED_DATE,
						UPDATED_BY,
						UPDATED_DATE
					FROM 
						@V_AvalibleItems
					WHERE
						ROLE IN (SELECT DISTINCT * FROM @V_AccountRoles)

					DECLARE @V_DistinctItems TABLE ([NVP_SEQ_ID] int,[STATIC_NAME] varchar(30),
					[DISPLAY] varchar(128),
					[DESCRIPTION] varchar(256),
					[ADDED_BY] int,
					[ADDED_DATE] datetime,
					[UPDATED_BY] int,
					[UPDATED_DATE] datetime)
					INSERT INTO @V_DistinctItems
					SELECT DISTINCT
						NVP_SEQ_ID,
						STATIC_NAME,
						DISPLAY,
						[DESCRIPTION],
						ADDED_BY,
						ADDED_DATE,
						UPDATED_BY,
						UPDATED_DATE
					FROM
						@V_AllItems


					SELECT
						*
					FROM
						@V_DistinctItems
				END
			ELSE
				BEGIN -- get only valid NVP for the given account
					SELECT
						*
					FROM
						ZFC_NVP
				END
		END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
