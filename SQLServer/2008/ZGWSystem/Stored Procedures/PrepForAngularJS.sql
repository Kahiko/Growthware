/*
Usage:
	DECLARE 
		@P_UseAngular BIT = 0

	exec ZGWSystem.PrepForAngularJS
		@P_UseAngular
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 06/25/2016
-- Description:	Setups up data for the use of AngularJS for the frontend
--	or for .APSX
-- =============================================
CREATE PROCEDURE [ZGWSystem].[PrepForAngularJS]
	@P_UseAngular BIT = 1
AS
	if @P_UseAngular = 1
		BEGIN
			UPDATE [ZGWSecurity].[Functions] SET [Controller] = 'SearchController', [Source] = 'Functions/System/Search/SearchPage.aspx'
			WHERE [Action] like 'search%' or [Action] = 'Manage_Groups'

			UPDATE [ZGWSecurity].[Functions] SET [Controller] = 'AddEditFunctionController'
			WHERE [Action] IN('AddFunctions', 'EditFunctions');


			UPDATE [ZGWSecurity].[Functions] SET [Controller] = 'AddEditAccountController'
			WHERE [Action] IN('EditAccount', 'EditOtherAccount', 'AddAccount');

		END
	ELSE
		BEGIN
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/Accounts/SearchAccounts.aspx' WHERE [Action] = 'Search_Accounts'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/Functions/SearchFunctions.aspx' WHERE [Action] = 'Search_Functions'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/Messages/SearchMessages.aspx' WHERE [Action] = 'Search_Messages'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/NVP/SearchNVP.aspx' WHERE [Action] = 'Search_Name_Value_Pairs'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/Roles/SearchRoles.aspx' WHERE [Action] = 'Search_Roles'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/SecurityEntities/SearchSecurityEntities.aspx' WHERE [Action] = 'Search_Security_Entities'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/States/SearchStates.aspx' WHERE [Action] = 'Search_States'
			UPDATE [ZGWSecurity].[Functions] SET [Source] = 'Functions/System/Administration/Groups/SearchGroups.aspx' WHERE [Action] = 'Manage_Groups'
		END
	--END IF
RETURN 0
