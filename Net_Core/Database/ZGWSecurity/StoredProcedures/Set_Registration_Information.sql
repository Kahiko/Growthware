/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId int = 1,
        @P_SecurityEntitySeqId_Owner int = 1,
        @P_AccountChoices [varchar](128) = 'Mike',
        @P_AddAccount [int] = 1,
        @P_Groups [varchar](max) = 'Everyone',
        @P_Roles [varchar](max) = 'Authenticated',
        @P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	EXECUTE [ZGWSecurity].[Set_Registration_Information]
		@P_SecurityEntitySeqId,
        @P_SecurityEntitySeqId_Owner,
        @P_AccountChoices,
        @P_AddAccount,
        @P_Groups,
        @P_Roles,
        @P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/10/2024
-- Description:	Inserts or updates [ZGWSecurity].[Set_Registration_Information]
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Registration_Information]
	@P_SecurityEntitySeqId int,
    @P_SecurityEntitySeqId_Owner int,
    @P_AccountChoices [varchar](128) NULL,
    @P_AddAccount [varchar](128) NULL,
    @P_Groups [varchar](max) NULL,
    @P_Roles [varchar](max) NULL,
    @P_Added_Updated_By [int],
	@P_Debug INT = 0
AS
	SET NOCOUNT ON;
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Registration_Information';
    DECLARE @V_Now DATETIME = GETDATE();
    IF NOT EXISTS (SELECT NULL FROM [ZGWSecurity].[Security_Entities] WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
    BEGIN
        RAISERROR ('SecurityEntitySeqId does not exist in table [ZGWSecurity].[Security_Entities]', 16, 1);
        RETURN 1
    END
    IF EXISTS (SELECT NULL FROM [ZGWSecurity].[Registration_Information] WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
        BEGIN
			IF @P_Debug = 1 PRINT 'Updating Record';
            UPDATE [ZGWSecurity].[Registration_Information] SET 
                [SecurityEntitySeqId_Owner] = @P_SecurityEntitySeqId_Owner,
                [AccountChoices] = @P_AccountChoices,
                [AddAccount] = @P_AddAccount,
                [Groups] = @P_Groups,
                [Roles] = @P_Roles,
                [Updated_By] = @P_Added_Updated_By,
                [Updated_Date] = @V_Now
            WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId;
        END
    ELSE
        BEGIN
			IF @P_Debug = 1 PRINT 'Inserting Record';
            INSERT INTO [ZGWSecurity].[Registration_Information] (
                [SecurityEntitySeqId],
                [SecurityEntitySeqId_Owner],
                [AccountChoices],
                [AddAccount],
                [Groups],
                [Roles],
                [Added_By],
                [Added_Date]
            ) VALUES (
                @P_SecurityEntitySeqId,
                @P_SecurityEntitySeqId_Owner,
                @P_AccountChoices,
                @P_AddAccount,
                @P_Groups,
                @P_Roles,
                @P_Added_Updated_By,
                @V_Now
            );
        END
    --END IF
    SELECT
         RI.[SecurityEntitySeqId]
        ,RI.[SecurityEntitySeqId_Owner]
        ,RI.[AccountChoices]
        ,RI.[AddAccount]
        ,RI.[Groups]
        ,RI.[Roles]
        ,RI.[Added_By]
        ,RI.[Added_Date]
        ,RI.[Updated_By]
        ,RI.[Updated_Date]
    FROM 
        [ZGWSecurity].[Registration_Information] RI
    WHERE
        RI.[SecurityEntitySeqId] = @P_SecurityEntitySeqId;
    SET NOCOUNT OFF;
    IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Registration_Information';
RETURN 0

GO