USE [master]
GO
IF DB_ID ( N'YourDatabaseName' ) IS NOT NULL
BEGIN
	ALTER DATABASE [YourDatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [YourDatabaseName];
END
CREATE DATABASE [YourDatabaseName]
GO
ALTER DATABASE [YourDatabaseName] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [YourDatabaseName].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [YourDatabaseName] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [YourDatabaseName] SET ANSI_NULLS ON 
GO
ALTER DATABASE [YourDatabaseName] SET ANSI_PADDING ON 
GO
ALTER DATABASE [YourDatabaseName] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [YourDatabaseName] SET ARITHABORT ON 
GO
ALTER DATABASE [YourDatabaseName] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [YourDatabaseName] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [YourDatabaseName] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [YourDatabaseName] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [YourDatabaseName] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [YourDatabaseName] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [YourDatabaseName] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [YourDatabaseName] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [YourDatabaseName] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [YourDatabaseName] SET  DISABLE_BROKER 
GO
ALTER DATABASE [YourDatabaseName] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [YourDatabaseName] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [YourDatabaseName] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [YourDatabaseName] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [YourDatabaseName] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [YourDatabaseName] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [YourDatabaseName] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [YourDatabaseName] SET RECOVERY FULL 
GO
ALTER DATABASE [YourDatabaseName] SET  MULTI_USER 
GO
ALTER DATABASE [YourDatabaseName] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [YourDatabaseName] SET DB_CHAINING OFF 
GO
ALTER DATABASE [YourDatabaseName] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [YourDatabaseName] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [YourDatabaseName] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'YourDatabaseName', N'ON'
GO
ALTER DATABASE [YourDatabaseName] SET QUERY_STORE = OFF
GO
USE [YourDatabaseName]
GO
/****** Object:  Schema [ZGWCoreWeb]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.schemas
WHERE name = N'ZGWCoreWeb')
EXEC sys.sp_executesql N'CREATE SCHEMA [ZGWCoreWeb]'
GO
/****** Object:  Schema [ZGWCoreWebApplication]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.schemas
WHERE name = N'ZGWCoreWebApplication')
EXEC sys.sp_executesql N'CREATE SCHEMA [ZGWCoreWebApplication]'
GO
/****** Object:  Schema [ZGWOptional]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.schemas
WHERE name = N'ZGWOptional')
EXEC sys.sp_executesql N'CREATE SCHEMA [ZGWOptional]'
GO
/****** Object:  Schema [ZGWSecurity]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.schemas
WHERE name = N'ZGWSecurity')
EXEC sys.sp_executesql N'CREATE SCHEMA [ZGWSecurity]'
GO
/****** Object:  Schema [ZGWSystem]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.schemas
WHERE name = N'ZGWSystem')
EXEC sys.sp_executesql N'CREATE SCHEMA [ZGWSystem]'
GO
/****** Object:  UserDefinedFunction [ZGWSecurity].[Get_Default_Entity_ID]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Default_Entity_ID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	execute dbo.sp_executesql @statement = N'CREATE FUNCTION [ZGWSecurity].[Get_Default_Entity_ID]()
RETURNS INT
AS
BEGIN
	DECLARE @V_Retval INT = (SELECT TOP 1 SecurityEntitySeqId FROM ZGWSecurity.Security_Entities ORDER BY SecurityEntitySeqId ASC)
	RETURN @V_Retval
END'
END
GO
/****** Object:  UserDefinedFunction [ZGWSecurity].[Get_Entity_Parents]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Entity_Parents]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	execute dbo.sp_executesql @statement = N'/*
Usage:
	DECLARE @PSecurityEntitySeqId INT = 1
	SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(0,@PSecurityEntitySeqId)
	SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description: Returns all "Parent" Secutriy_EntitySeqId''s
--	Given the SecurityEntitySeqId 
-- Note:
--	Works through recursion of MAXRECURSION 32767
-- =============================================
CREATE FUNCTION [ZGWSecurity].[Get_Entity_Parents]
(
	@P_IncludeParent bit, 
	@PSecurityEntitySeqId int
)
RETURNS @retParents TABLE 
(
	SecurityEntitySeqId int, 
	PARENTSecurityEntitySeqId int
)
AS
BEGIN
	IF (ZGWSystem.Inheritance_Enabled()=1)
		BEGIN
			;WITH tblParent(SecurityEntitySeqId, ParentSecurityEntitySeqId) AS
			(
				SELECT SecurityEntitySeqId, ParentSecurityEntitySeqId
					FROM ZGWSecurity.Security_Entities WITH(NOLOCK) WHERE SecurityEntitySeqId = @PSecurityEntitySeqId
				UNION ALL
				SELECT 
					SE.SecurityEntitySeqId, SE.ParentSecurityEntitySeqId
				FROM 
					ZGWSecurity.Security_Entities SE WITH(NOLOCK) 
					INNER JOIN tblParent
						ON SE.SecurityEntitySeqId = tblParent.ParentSecurityEntitySeqId
			)
			INSERT INTO @retParents(SecurityEntitySeqId)
			SELECT 
				tblParent.SecurityEntitySeqId
			FROM  
				tblParent
			WHERE SecurityEntitySeqId <> @PSecurityEntitySeqId
			OPTION(MAXRECURSION 32767)
			IF (@P_IncludeParent=1) INSERT INTO @retParents(SecurityEntitySeqId)VALUES(@PSecurityEntitySeqId);
		END
	ELSE
		BEGIN
			INSERT INTO @retParents VALUES(ZGWSecurity.Get_Default_Entity_ID(),1)
			IF (@P_IncludeParent=1) INSERT INTO @retParents VALUES(@PSecurityEntitySeqId,1)
		END
	-- END IF
	RETURN

END'
END
GO
/****** Object:  UserDefinedFunction [ZGWSecurity].[Get_View_PermissionSeqId]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_View_PermissionSeqId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	execute dbo.sp_executesql @statement = N'/*
Usage:
	DECLARE @V_Permission_ID INT
	SET @V_Permission_ID = ZGWSecurity.Get_View_PermissionSeqId()
	PRINT @V_Permission_ID
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description: Returns NVP_DetailSeqId associated
--	with the NVP_Detail_Name value = ''view''
-- Note:
--	Created to allow change in a single location
--	should the sequence id change.
-- =============================================
CREATE FUNCTION [ZGWSecurity].[Get_View_PermissionSeqId]
(

)
RETURNS INT
AS
BEGIN
	RETURN 1
END'
END
GO
/****** Object:  UserDefinedFunction [ZGWSystem].[Inheritance_Enabled]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Inheritance_Enabled]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	execute dbo.sp_executesql @statement = N'CREATE FUNCTION [ZGWSystem].[Inheritance_Enabled]()
RETURNS INT
AS
BEGIN
	DECLARE @V_RETURN_VAL INT
	SET @V_RETURN_VAL = (SELECT TOP 1 Enable_Inheritance FROM ZGWSystem.Database_Information ORDER BY Updated_Date DESC)
	RETURN @V_RETURN_VAL -- 0 = FALSE 1 = TRUE
END'
END
GO
/****** Object:  Table [ZGWCoreWeb].[Messages]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Messages]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWCoreWeb].[Messages]
	(
		[MessageSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[SecurityEntitySeqId] [int] NOT NULL,
		[Name] [varchar](50) NOT NULL,
		[Title] [varchar](100) NOT NULL,
		[Description] [varchar](512) SPARSE NULL,
		[Format_As_HTML] [int] NOT NULL,
		[Body] [varchar](max) NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZFO_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Accounts]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Accounts]
	(
		[AccountSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Account] [varchar](128) NOT NULL,
		[Email] [varchar](128) NULL,
		[Enable_Notifications] [int] NULL,
		[Is_System_Admin] [int] NOT NULL,
		[StatusSeqId] [int] NOT NULL,
		[Password_Last_Set] [datetime] NOT NULL,
		[Password] [varchar](256) NOT NULL,
		[Failed_Attempts] [int] NOT NULL,
		[First_Name] [varchar](35) NOT NULL,
		[Last_Login] [datetime] NULL,
		[Last_Name] [varchar](35) NOT NULL,
		[Location] [varchar](128) NULL,
		[Middle_Name] [varchar](35) NULL,
		[Preferred_Name] [varchar](50) NULL,
		[Time_Zone] [int] NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[AccountSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_Accounts] UNIQUE NONCLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  View [ZGWCoreWeb].[vwSearchMessages]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.views
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[vwSearchMessages]'))
EXEC dbo.sp_executesql @statement = N'/*
Usage:
	SELECT * FROM ZGWCoreWeb.vwSearchMessages WHERE SecurityEntitySeqId = 1
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the MessageSeqId.  If MessageSeqId = -1
--	all messages are returned.
-- =============================================
CREATE VIEW [ZGWCoreWeb].[vwSearchMessages] AS 
	SELECT 
		[MessageSeqId]
		,[SecurityEntitySeqId]
		,[Name]
		,[Title]
		,[Description]
		,[Format_As_HTML]
		,[Body]
		,(SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = msg.Added_By) AS [Added_By]
		,[Added_Date]
		,(SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = msg.Updated_By) AS [Updated_By]
		,[Updated_Date] 
	FROM 
		[ZGWCoreWeb].[Messages] msg WITH(NOLOCK)' 
GO
/****** Object:  Table [ZGWOptional].[States]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[States]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWOptional].[States]
	(
		[State] [char](2) NOT NULL,
		[Description] [varchar](128) NULL,
		[StatusSeqId] [int] NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWOptional_States] PRIMARY KEY CLUSTERED 
(
	[State] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSystem].[Statuses]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Statuses]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSystem].[Statuses]
	(
		[StatusSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Name] [char](25) NOT NULL,
		[Description] [varchar](512) NULL,
		[Added_By] [int] NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWSystem_Statuses] PRIMARY KEY CLUSTERED 
(
	[StatusSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  View [ZGWOptional].[vwSearchStates]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.views
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[vwSearchStates]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [ZGWOptional].[vwSearchStates]
	AS 
SELECT
	[State]
	, [Description]
	, [Status] = (SELECT TOP(1) [Name] FROM [ZGWSystem].[Statuses] WHERE [StatusSeqId] = States.[StatusSeqId])
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = States.Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = States.Updated_By) AS Updated_By
	, Updated_Date
FROM 
	[ZGWOptional].[States] States WITH(NOLOCK)' 
GO
/****** Object:  Table [ZGWSecurity].[Roles_Security_Entities]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Roles_Security_Entities]
	(
		[RolesSecurityEntitiesSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[SecurityEntitySeqId] [int] NOT NULL,
		[RoleSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		CONSTRAINT [PK_Roles_Security_Entities] PRIMARY KEY CLUSTERED 
(
	[RolesSecurityEntitiesSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Roles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Roles]
	(
		[RoleSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Name] [varchar](50) NOT NULL,
		[Description] [varchar](128) NOT NULL,
		[Is_System] [int] NOT NULL,
		[Is_System_Only] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWSecurity_Roles] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  View [ZGWSecurity].[vwSearchRoles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.views
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[vwSearchRoles]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [ZGWSecurity].[vwSearchRoles] AS 
	SELECT
		R.[RoleSeqId] AS ROLE_SEQ_ID,
		R.[Name],
		R.[Description],
		R.[Is_System],
		R.[Is_System_Only],
		Added_By = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = R.Added_By),
		R.Added_Date,
		[Updated_By] = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = R.Updated_By),
		R.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Roles R WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Roles_Security_Entities RSE WITH(NOLOCK)
			ON R.RoleSeqId = RSE.RoleSeqId' 
GO
/****** Object:  Table [ZGWSecurity].[Groups_Security_Entities]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups_Security_Entities]
	(
		[GroupsSecurityEntitiesSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[SecurityEntitySeqId] [int] NOT NULL,
		[GroupSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		CONSTRAINT [PK_ZFC_GRPS_SE] PRIMARY KEY CLUSTERED 
(
	[GroupsSecurityEntitiesSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZFC_GRPS_SE] UNIQUE NONCLUSTERED 
(
	[SecurityEntitySeqId] ASC,
	[GroupSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups]
	(
		[GroupSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Name] [varchar](128) NOT NULL,
		[Description] [varchar](512) NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWSecurity_Groups] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  View [ZGWSecurity].[vwSearchGroups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.views
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[vwSearchGroups]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [ZGWSecurity].[vwSearchGroups] AS
	SELECT
		G.[GroupSeqId] AS Group_SEQ_ID,
		G.[Name],
		G.[Description],
		Added_By = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_By),
		Added_Date = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_Date),
		G.[Updated_By],
		G.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Groups G WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Groups_Security_Entities RSE WITH(NOLOCK)
			ON G.GroupSeqId = RSE.GroupSeqId' 
GO
/****** Object:  Table [ZGWSystem].[Name_Value_Pairs]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Name_Value_Pairs]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSystem].[Name_Value_Pairs]
	(
		[NVPSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Schema_Name] [varchar](30) NOT NULL,
		[Static_Name] [varchar](30) NOT NULL,
		[Display] [varchar](128) NOT NULL,
		[Description] [varchar](256) NOT NULL,
		[StatusSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWSystem_Name_Value_Pairs] PRIMARY KEY CLUSTERED 
(
	[NVPSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  View [ZGWSystem].[vwSearchNVP]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.views
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[vwSearchNVP]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [ZGWSystem].[vwSearchNVP]
	AS 
SELECT
	NVPSeqId
	, Schema_Name + ''.'' + Static_Name AS Name
	, Description
	, Status = (SELECT TOP(1) Name FROM ZGWSystem.Statuses WHERE StatusSeqId = StatusSeqId)
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = NVP.Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = NVP.Updated_By) AS Updated_By
	, Updated_Date
FROM 
	ZGWSystem.Name_Value_Pairs  NVP WITH(NOLOCK)' 
GO
/****** Object:  Table [ZGWSecurity].[Functions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Functions]
	(
		[FunctionSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Action] [varchar](256) NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Controller] [varchar](512) SPARSE NULL,
		[Description] [varchar](512) NOT NULL,
		[Enable_Notifications] [int] NOT NULL,
		[Enable_View_State] [int] NOT NULL,
		[FunctionTypeSeqId] [int] NULL,
		[Is_Nav] [int] NOT NULL,
		[Link_Behavior] [int] NOT NULL,
		[Meta_Key_Words] [varchar](512) SPARSE NULL,
		[Name] [varchar](30) NOT NULL,
		[Navigation_Types_NVP_DetailSeqId] [int] NOT NULL,
		[Notes] [varchar](512) SPARSE NULL,
		[No_UI] [int] NOT NULL,
		[ParentSeqId] [int] NULL,
		[Redirect_On_Timeout] [int] NOT NULL,
		[Resolve] [varchar](max) SPARSE NULL,
		[Sort_Order] [int] NOT NULL,
		[Source] [varchar](512) SPARSE NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_Functions] PRIMARY KEY CLUSTERED 
(
	[FunctionSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWSecurity_Functions] UNIQUE NONCLUSTERED 
(
	[Action] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  View [ZGWSystem].[vwSearchFunctions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.views
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[vwSearchFunctions]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [ZGWSystem].[vwSearchFunctions]
	AS 
SELECT
	FunctionSeqId
	, Name
	, Description
	, Action
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = FUN.Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = FUN.Updated_By) AS Updated_By
	, Updated_Date
FROM 
	ZGWSecurity.Functions FUN WITH(NOLOCK)' 
GO
/****** Object:  Table [ZGWCoreWeb].[Account_Choices]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Account_Choices]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWCoreWeb].[Account_Choices]
	(
		[Account] [varchar](128) NOT NULL,
		[SecurityEntityID] [int] NULL,
		[SecurityEntityName] [varchar](256) NULL,
		[BackColor] [varchar](15) NULL,
		[LeftColor] [varchar](15) NULL,
		[HeadColor] [varchar](15) NULL,
		[SubHeadColor] [varchar](15) NULL,
		[ColorScheme] [varchar](15) NULL,
		[FavoriteAction] [varchar](50) NULL,
		[recordsPerPage] [int] NULL,
		[RowBackColor] [varchar](15) NULL,
		[AlternatingRowBackColor] [varchar](15) NULL,
		[HeaderForeColor] [varchar](15) NULL,
		CONSTRAINT [UK_ZGWCore_Account_Choices] UNIQUE NONCLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWCoreWeb].[Link_Behaviors]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Link_Behaviors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWCoreWeb].[Link_Behaviors]
	(
		[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[NVPSeqId] [int] NOT NULL,
		[NVP_Detail_Name] [varchar](50) NOT NULL,
		[NVP_Detail_Value] [varchar](300) NOT NULL,
		[StatusSeqId] [int] NOT NULL,
		[Sort_Order] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_DATE] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[UPDATED_DATE] [datetime] NULL,
		CONSTRAINT [PK_Link_Behaviors] PRIMARY KEY CLUSTERED 
(
	[NVP_DetailSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_Link_Behaviors] UNIQUE NONCLUSTERED 
(
	[NVP_Detail_Name] ASC,
	[NVP_Detail_Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWCoreWeb].[Notifications]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Notifications]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWCoreWeb].[Notifications]
	(
		[NotificationSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[SecurityEntitySeqId] [int] NOT NULL,
		[FunctionSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		CONSTRAINT [PK_ZGWCoreWeb_NOTIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[NotificationSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWCoreWeb].[Work_Flows]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Work_Flows]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWCoreWeb].[Work_Flows]
	(
		[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[NVPSeqId] [int] NOT NULL,
		[NVP_Detail_Name] [varchar](50) NOT NULL,
		[NVP_Detail_Value] [varchar](300) NOT NULL,
		[StatusSeqId] [int] NOT NULL,
		[Sort_Order] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWCoreWeb_Work_Flows] PRIMARY KEY CLUSTERED 
(
	[NVP_DetailSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWCoreWeb_Work_Flows] UNIQUE NONCLUSTERED 
(
	[NVP_Detail_Name] ASC,
	[NVP_Detail_Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWOptional].[Calendars]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Calendars]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWOptional].[Calendars]
	(
		[SecurityEntitySeqId] [int] NOT NULL,
		[Calendar_Name] [varchar](50) NOT NULL,
		[Entry_Date] [smalldatetime] NOT NULL,
		[Comment] [varchar](100) NOT NULL,
		[Active] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWOptional].[Directories]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Directories]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWOptional].[Directories]
	(
		[FunctionSeqId] [int] NOT NULL,
		[Directory] [varchar](255) NOT NULL,
		[Impersonate] [int] NOT NULL,
		[Impersonating_Account] [varchar](50) NULL,
		[Impersonating_Password] [varchar](50) NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_Directories] PRIMARY KEY CLUSTERED 
(
	[FunctionSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWOptional].[Zip_Codes]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Zip_Codes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWOptional].[Zip_Codes]
	(
		[State] [char](2) NOT NULL,
		[Zip_Code] [int] NOT NULL,
		[Area_Code] [int] NOT NULL,
		[City] [varchar](255) NULL,
		[Time_Zone] [varchar](255) NULL,
		CONSTRAINT [UK_ZGWOptional_Zip_Codes] UNIQUE NONCLUSTERED 
(
	[State] ASC,
	[Zip_Code] ASC,
	[City] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Function_Types]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Function_Types]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Function_Types]
	(
		[FunctionTypeSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Name] [varchar](50) NOT NULL,
		[Description] [varchar](512) NOT NULL,
		[Template] [varchar](512) SPARSE NULL,
		[Is_Content] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWCoreWeb_Function_Types] PRIMARY KEY CLUSTERED 
(
	[FunctionTypeSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWCoreWeb_Function_Types] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Groups_Security_Entities_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Accounts]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts]
	(
		[GroupsSecurityEntitiesSeqId] [int] NOT NULL,
		[AccountSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Groups_Security_Entities_Functions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Functions]
	(
		[GroupsSecurityEntitiesSeqId] [int] NOT NULL,
		[FunctionSeqId] [int] NOT NULL,
		[PermissionsNVPDetailSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		CONSTRAINT [UK_ZFC_FUNCT_PER_GRPS] UNIQUE NONCLUSTERED 
(
	[PermissionsNVPDetailSeqId] ASC,
	[GroupsSecurityEntitiesSeqId] ASC,
	[FunctionSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Groups_Security_Entities_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Groups]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Groups]
	(
		[GroupsSecurityEntitiesSeqId] [int] NOT NULL,
		[GroupSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		CONSTRAINT [UK_Groups_Security_Entities_Groups] UNIQUE NONCLUSTERED 
(
	[GroupsSecurityEntitiesSeqId] ASC,
	[GroupSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Groups_Security_Entities_Permissions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]
	(
		[GroupsSecurityEntitiesSeqId] [int] NOT NULL,
		[NVPSeqId] [int] NOT NULL,
		[PermissionsNVPDetailSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]
	(
		[GroupsSecurityEntitiesSeqId] [int] NOT NULL,
		[RolesSecurityEntitiesSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Navigation_Types]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Navigation_Types]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Navigation_Types]
	(
		[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[NVPSeqId] [int] NOT NULL,
		[NVP_Detail_Name] [varchar](50) NOT NULL,
		[NVP_Detail_Value] [varchar](300) NOT NULL,
		[StatusSeqId] [int] NOT NULL,
		[Sort_Order] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWSecurity_Navigation_Types] PRIMARY KEY CLUSTERED 
(
	[NVP_DetailSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWSecurity_Navigation_Types] UNIQUE NONCLUSTERED 
(
	[NVP_Detail_Name] ASC,
	[NVP_Detail_Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Permissions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Permissions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Permissions]
	(
		[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[NVPSeqId] [int] NOT NULL,
		[NVP_Detail_Name] [varchar](50) NOT NULL,
		[NVP_Detail_Value] [varchar](300) NOT NULL,
		[StatusSeqId] [int] NOT NULL,
		[Sort_Order] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[ADDED_DATE] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[UPDATED_DATE] [datetime] NULL,
		CONSTRAINT [PK_ZGWSecurity_Permissions] PRIMARY KEY CLUSTERED 
(
	[NVP_DetailSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
		CONSTRAINT [UK_ZGWSecurity_Permissions] UNIQUE NONCLUSTERED 
(
	[NVP_Detail_Name] ASC,
	[NVP_Detail_Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Roles_Security_Entities_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Accounts]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts]
	(
		[RolesSecurityEntitiesSeqId] [int] NOT NULL,
		[AccountSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Roles_Security_Entities_Functions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Functions]
	(
		[RolesSecurityEntitiesSeqId] [int] NOT NULL,
		[FunctionSeqId] [int] NOT NULL,
		[PermissionsNVPDetailSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		CONSTRAINT [UK_ZGWSecurity_Permissions_Roles_Security_Entities_Functions] UNIQUE NONCLUSTERED 
(
	[PermissionsNVPDetailSeqId] ASC,
	[RolesSecurityEntitiesSeqId] ASC,
	[FunctionSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSecurity].[Roles_Security_Entities_Permissions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Permissions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions]
	(
		[NVPSeqId] [int] NOT NULL,
		[RolesSecurityEntitiesSeqId] [int] NOT NULL,
		[PermissionsNVPDetailSeqId] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL
	) ON [PRIMARY]
END
GO
/****** Object:  Index [UIX_Roles_EntitesSeqId_PermissionsNVPDetailSeqId]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.indexes
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Permissions]') AND name = N'UIX_Roles_EntitesSeqId_PermissionsNVPDetailSeqId')
CREATE UNIQUE CLUSTERED INDEX [UIX_Roles_EntitesSeqId_PermissionsNVPDetailSeqId] ON [ZGWSecurity].[Roles_Security_Entities_Permissions]
(
	[NVPSeqId] ASC,
	[RolesSecurityEntitiesSeqId] ASC,
	[PermissionsNVPDetailSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [ZGWSecurity].[Security_Entities]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Security_Entities]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Security_Entities]
	(
		[SecurityEntitySeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[Name] [varchar](256) NOT NULL,
		[Description] [varchar](512) NULL,
		[URL] [varchar](128) NULL,
		[StatusSeqId] [int] NOT NULL,
		[DAL] [nchar](50) NOT NULL,
		[DAL_Name] [nchar](50) NOT NULL,
		[DAL_Name_Space] [varchar](256) NOT NULL,
		[DAL_String] [varchar](512) NOT NULL,
		[Skin] [nchar](25) NOT NULL,
		[Style] [varchar](25) NOT NULL,
		[Encryption_Type] [int] NOT NULL,
		[ParentSecurityEntitySeqId] [int] NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_Entities] PRIMARY KEY CLUSTERED 
(
	[SecurityEntitySeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSystem].[Data_Errors]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Data_Errors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSystem].[Data_Errors]
	(
		[ErrorSeqId] [int] IDENTITY(1,1) NOT NULL,
		[ErrorNumber] [int] NULL,
		[ErrorSeverity] [int] NULL,
		[ErrorState] [int] NULL,
		[ErrorProcedure] [varchar](max) NULL,
		[ErrorLine] [int] NULL,
		[ErrorMessage] [varchar](max) SPARSE NULL,
		[ErrorDate] [datetime] NOT NULL,
		[Parameters] [varchar](max) SPARSE NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSystem].[Database_Information]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Database_Information]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSystem].[Database_Information]
	(
		[Database_InformationSeqId] [int] IDENTITY(1,1) NOT NULL,
		[Version] [varchar](50) NOT NULL,
		[Enable_Inheritance] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL,
		CONSTRAINT [PK_ZGWSystem_Database_Information] PRIMARY KEY CLUSTERED 
(
	[Database_InformationSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [ZGWSystem].[Logging]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Logging]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSystem].[Logging]
	(
		[Account] [varchar](128) NOT NULL,
		[Component] [varchar](50) NOT NULL,
		[ClassName] [varchar](50) NOT NULL,
		[Level] [varchar](5) NOT NULL,
		[LogDate] [datetime] NULL,
		[LogSeqId] [int] IDENTITY(1,1) NOT NULL,
		[MethodName] [varchar](50) NOT NULL,
		[Msg] [varchar](max) NOT NULL,
		CONSTRAINT [CI_ZGWSystem.Logging] UNIQUE CLUSTERED 
(
	[LogDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Index [FK_IXSecurityEntitySeqId]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.indexes
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Messages]') AND name = N'FK_IXSecurityEntitySeqId')
CREATE NONCLUSTERED INDEX [FK_IXSecurityEntitySeqId] ON [ZGWCoreWeb].[Messages]
(
	[SecurityEntitySeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [FK_IX_Notification_Entity_Function]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.indexes
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Notifications]') AND name = N'FK_IX_Notification_Entity_Function')
CREATE NONCLUSTERED INDEX [FK_IX_Notification_Entity_Function] ON [ZGWCoreWeb].[Notifications]
(
	[NotificationSeqId] ASC,
	[SecurityEntitySeqId] ASC,
	[FunctionSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [FX_IX_Work_Flows]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.indexes
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Work_Flows]') AND name = N'FX_IX_Work_Flows')
CREATE NONCLUSTERED INDEX [FX_IX_Work_Flows] ON [ZGWCoreWeb].[Work_Flows]
(
	[NVPSeqId] ASC,
	[StatusSeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NC_ZGWSystem_Logging_LogDate_Level]    Script Date: 7/4/2022 10:50:33 AM ******/
IF NOT EXISTS (SELECT *
FROM sys.indexes
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Logging]') AND name = N'NC_ZGWSystem_Logging_LogDate_Level')
CREATE NONCLUSTERED INDEX [NC_ZGWSystem_Logging_LogDate_Level] ON [ZGWSystem].[Logging]
(
	[LogDate] ASC,
	[Level] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[DF_ZGWCoreWeb_Messages_Format_As_HTML]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWCoreWeb].[Messages] ADD  CONSTRAINT [DF_ZGWCoreWeb_Messages_Format_As_HTML]  DEFAULT ((0)) FOR [Format_As_HTML]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[DF__Messages__Added___00200768]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWCoreWeb].[Messages] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[DF_ZGWCoreWeb_NOTIFICATIONS_ADDED_DATE]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWCoreWeb].[Notifications] ADD  CONSTRAINT [DF_ZGWCoreWeb_NOTIFICATIONS_ADDED_DATE]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF_ZGWOptional_Calendar_Added_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[Calendars] ADD  CONSTRAINT [DF_ZGWOptional_Calendar_Added_By]  DEFAULT ((1)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF_ZGWOptional_Calendar_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[Calendars] ADD  CONSTRAINT [DF_ZGWOptional_Calendar_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF__Directori__Added__03F0984C]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[Directories] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF_ZGWOptional_States_Added_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[States] ADD  CONSTRAINT [DF_ZGWOptional_States_Added_By]  DEFAULT ((1)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF_ZGWOptional_States_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[States] ADD  CONSTRAINT [DF_ZGWOptional_States_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF_ZGWOptional_States_Updated_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[States] ADD  CONSTRAINT [DF_ZGWOptional_States_Updated_By]  DEFAULT ((1)) FOR [Updated_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[DF_ZGWOptional_States_Updated_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWOptional].[States] ADD  CONSTRAINT [DF_ZGWOptional_States_Updated_Date]  DEFAULT (getdate()) FOR [Updated_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_Accounts_IS_SYSTEM_ADMIN]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Accounts] ADD  CONSTRAINT [DF_Accounts_IS_SYSTEM_ADMIN]  DEFAULT ((0)) FOR [Is_System_Admin]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_Accounts_PASSWORD_LAST_SET]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Accounts] ADD  CONSTRAINT [DF_Accounts_PASSWORD_LAST_SET]  DEFAULT (getdate()) FOR [Password_Last_Set]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_Accounts_LAST_LOGIN]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Accounts] ADD  CONSTRAINT [DF_Accounts_LAST_LOGIN]  DEFAULT (getdate()) FOR [Last_Login]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_Accounts_Added_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Accounts] ADD  CONSTRAINT [DF_Accounts_Added_By]  DEFAULT ((1)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_Accounts_ADDED_DATE]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Accounts] ADD  CONSTRAINT [DF_Accounts_ADDED_DATE]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWCoreWeb_Function_Types_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Function_Types] ADD  CONSTRAINT [DF_ZGWCoreWeb_Function_Types_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Functions_ADDED_DATE]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Functions] ADD  CONSTRAINT [DF_ZGWSecurity_Functions_ADDED_DATE]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Functions_ENABLE_NOTIFICATIONS]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Functions] ADD  CONSTRAINT [DF_ZGWSecurity_Functions_ENABLE_NOTIFICATIONS]  DEFAULT ((0)) FOR [Enable_Notifications]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Functions_NO_UI]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Functions] ADD  CONSTRAINT [DF_ZGWSecurity_Functions_NO_UI]  DEFAULT ((0)) FOR [No_UI]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Functions_REDIRECT_ON_TIMEOUT]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Functions] ADD  CONSTRAINT [DF_ZGWSecurity_Functions_REDIRECT_ON_TIMEOUT]  DEFAULT ((1)) FOR [Redirect_On_Timeout]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Functions_Sort_Order]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Functions] ADD  CONSTRAINT [DF_ZGWSecurity_Functions_Sort_Order]  DEFAULT ((0)) FOR [Sort_Order]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF__Groups__Added_Da__1332DBDC]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF__Groups_Se__Added__14270015]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups_Security_Entities] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts] ADD  CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_By]  DEFAULT ((1)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts] ADD  CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_Groups_Security_Entities_Groups_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Groups] ADD  CONSTRAINT [DF_Groups_Security_Entities_Groups_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Groups_Security_Entities_Permissions_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] ADD  CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Permissions_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF__Groups_Se__Added__18EBB532]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Roles_IS_SYSTEM_ONLY]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Roles] ADD  CONSTRAINT [DF_ZGWSecurity_Roles_IS_SYSTEM_ONLY]  DEFAULT ((0)) FOR [Is_System_Only]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Roles_Added_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Roles] ADD  CONSTRAINT [DF_ZGWSecurity_Roles_Added_By]  DEFAULT ((2)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Roles_ADDED_DATE]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Roles] ADD  CONSTRAINT [DF_ZGWSecurity_Roles_ADDED_DATE]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Roles_Security_Entities_Accounts_Added_By]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] ADD  CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_Added_By]  DEFAULT ((1)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Roles_Security_Entities_Accounts_ADDED_DATE]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] ADD  CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_ADDED_DATE]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[DF_ZGWSecurity_Roles_Security_Entities_Functions_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions] ADD  CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Functions_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[DF_ZGWSystem.Data_Errors_ErrorDate]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSystem].[Data_Errors] ADD  CONSTRAINT [DF_ZGWSystem.Data_Errors_ErrorDate]  DEFAULT (getdate()) FOR [ErrorDate]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[DF__Database___Added__208CD6FA]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSystem].[Database_Information] ADD  DEFAULT ((1)) FOR [Added_By]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[DF__Database___Added__2180FB33]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSystem].[Database_Information] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[DF_ZGWSystem.Logging_LogDate]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSystem].[Logging] ADD  CONSTRAINT [DF_ZGWSystem.Logging_LogDate]  DEFAULT (getdate()) FOR [LogDate]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[DF__Name_Valu__Added__236943A5]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSystem].[Name_Value_Pairs] ADD  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[DF_ZGWSystem_Statuses_Added_Date]') AND type = 'D')
BEGIN
	ALTER TABLE [ZGWSystem].[Statuses] ADD  CONSTRAINT [DF_ZGWSystem_Statuses_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]
END
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Account_Choices]'))
ALTER TABLE [ZGWCoreWeb].[Account_Choices]  WITH CHECK ADD  CONSTRAINT [FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts] FOREIGN KEY([Account])
REFERENCES [ZGWSecurity].[Accounts] ([Account])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Account_Choices]'))
ALTER TABLE [ZGWCoreWeb].[Account_Choices] CHECK CONSTRAINT [FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Link_Behaviors]'))
ALTER TABLE [ZGWCoreWeb].[Link_Behaviors]  WITH CHECK ADD  CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Link_Behaviors]'))
ALTER TABLE [ZGWCoreWeb].[Link_Behaviors] CHECK CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Name_Value_Pairs]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Link_Behaviors]'))
ALTER TABLE [ZGWCoreWeb].[Link_Behaviors]  WITH CHECK ADD  CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Link_Behaviors]'))
ALTER TABLE [ZGWCoreWeb].[Link_Behaviors] CHECK CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_Messages_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Messages]'))
ALTER TABLE [ZGWCoreWeb].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Entities] FOREIGN KEY([SecurityEntitySeqId])
REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_Messages_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Messages]'))
ALTER TABLE [ZGWCoreWeb].[Messages] CHECK CONSTRAINT [FK_Messages_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_Notifications_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Notifications]'))
ALTER TABLE [ZGWCoreWeb].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Entities] FOREIGN KEY([SecurityEntitySeqId])
REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_Notifications_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Notifications]'))
ALTER TABLE [ZGWCoreWeb].[Notifications] CHECK CONSTRAINT [FK_Notifications_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_Notifications_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Notifications]'))
ALTER TABLE [ZGWCoreWeb].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Functions] FOREIGN KEY([FunctionSeqId])
REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_Notifications_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Notifications]'))
ALTER TABLE [ZGWCoreWeb].[Notifications] CHECK CONSTRAINT [FK_Notifications_Functions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Work_Flows_ZGWSystem_NVP]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Work_Flows]'))
ALTER TABLE [ZGWCoreWeb].[Work_Flows]  WITH CHECK ADD  CONSTRAINT [FK_ZGWCoreWeb_Work_Flows_ZGWSystem_NVP] FOREIGN KEY([NVPSeqId])
REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Work_Flows_ZGWSystem_NVP]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Work_Flows]'))
ALTER TABLE [ZGWCoreWeb].[Work_Flows] CHECK CONSTRAINT [FK_ZGWCoreWeb_Work_Flows_ZGWSystem_NVP]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Work_Flows_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Work_Flows]'))
ALTER TABLE [ZGWCoreWeb].[Work_Flows]  WITH CHECK ADD  CONSTRAINT [FK_ZGWCoreWeb_Work_Flows_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[FK_ZGWCoreWeb_Work_Flows_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWCoreWeb].[Work_Flows]'))
ALTER TABLE [ZGWCoreWeb].[Work_Flows] CHECK CONSTRAINT [FK_ZGWCoreWeb_Work_Flows_ZGWSystem_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_ZGWOptional_Calendar_ZGWSecurity_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Calendars]'))
ALTER TABLE [ZGWOptional].[Calendars]  WITH CHECK ADD  CONSTRAINT [FK_ZGWOptional_Calendar_ZGWSecurity_Entities] FOREIGN KEY([SecurityEntitySeqId])
REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_ZGWOptional_Calendar_ZGWSecurity_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Calendars]'))
ALTER TABLE [ZGWOptional].[Calendars] CHECK CONSTRAINT [FK_ZGWOptional_Calendar_ZGWSecurity_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Directories_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Directories]'))
ALTER TABLE [ZGWOptional].[Directories]  WITH CHECK ADD  CONSTRAINT [FK_Directories_Functions] FOREIGN KEY([FunctionSeqId])
REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Directories_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Directories]'))
ALTER TABLE [ZGWOptional].[Directories] CHECK CONSTRAINT [FK_Directories_Functions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_ZGWOptional_States_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[States]'))
ALTER TABLE [ZGWOptional].[States]  WITH CHECK ADD  CONSTRAINT [FK_ZGWOptional_States_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_ZGWOptional_States_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[States]'))
ALTER TABLE [ZGWOptional].[States] CHECK CONSTRAINT [FK_ZGWOptional_States_ZGWSystem_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_ZGWOptional_Zip_Codes_ZGWOptional_States]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Zip_Codes]'))
ALTER TABLE [ZGWOptional].[Zip_Codes]  WITH CHECK ADD  CONSTRAINT [FK_ZGWOptional_Zip_Codes_ZGWOptional_States] FOREIGN KEY([State])
REFERENCES [ZGWOptional].[States] ([State])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_ZGWOptional_Zip_Codes_ZGWOptional_States]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Zip_Codes]'))
ALTER TABLE [ZGWOptional].[Zip_Codes] CHECK CONSTRAINT [FK_ZGWOptional_Zip_Codes_ZGWOptional_States]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Accounts_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Accounts]'))
ALTER TABLE [ZGWSecurity].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Accounts_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Accounts]'))
ALTER TABLE [ZGWSecurity].[Accounts] CHECK CONSTRAINT [FK_Accounts_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Functions_Function_Types]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]'))
ALTER TABLE [ZGWSecurity].[Functions]  WITH CHECK ADD  CONSTRAINT [FK_Functions_Function_Types] FOREIGN KEY([FunctionTypeSeqId])
REFERENCES [ZGWSecurity].[Function_Types] ([FunctionTypeSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Functions_Function_Types]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]'))
ALTER TABLE [ZGWSecurity].[Functions] CHECK CONSTRAINT [FK_Functions_Function_Types]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Functions_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]'))
ALTER TABLE [ZGWSecurity].[Functions]  WITH CHECK ADD  CONSTRAINT [FK_Functions_Functions] FOREIGN KEY([ParentSeqId])
REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Functions_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]'))
ALTER TABLE [ZGWSecurity].[Functions] CHECK CONSTRAINT [FK_Functions_Functions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Functions_Navigation_Types]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]'))
ALTER TABLE [ZGWSecurity].[Functions]  WITH CHECK ADD  CONSTRAINT [FK_Functions_Navigation_Types] FOREIGN KEY([Navigation_Types_NVP_DetailSeqId])
REFERENCES [ZGWSecurity].[Navigation_Types] ([NVP_DetailSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Functions_Navigation_Types]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Functions]'))
ALTER TABLE [ZGWSecurity].[Functions] CHECK CONSTRAINT [FK_Functions_Navigation_Types]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Entities] FOREIGN KEY([SecurityEntitySeqId])
REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities] CHECK CONSTRAINT [FK_Groups_Security_Entities_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Groups_Security_Entities_ZGWSecurity_Groups]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_ZGWSecurity_Groups] FOREIGN KEY([GroupSeqId])
REFERENCES [ZGWSecurity].[Groups] ([GroupSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Groups_Security_Entities_ZGWSecurity_Groups]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities] CHECK CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_ZGWSecurity_Groups]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Accounts_Accounts]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Accounts_Accounts] FOREIGN KEY([AccountSeqId])
REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Accounts_Accounts]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts] CHECK CONSTRAINT [FK_Groups_Security_Entities_Accounts_Accounts]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Accounts_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Accounts_Groups_Security_Entities] FOREIGN KEY([GroupsSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Accounts_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts] CHECK CONSTRAINT [FK_Groups_Security_Entities_Accounts_Groups_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Functions_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Functions_Functions] FOREIGN KEY([FunctionSeqId])
REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Functions_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions] CHECK CONSTRAINT [FK_Groups_Security_Entities_Functions_Functions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Functions_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Functions_Groups_Security_Entities] FOREIGN KEY([GroupsSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Functions_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions] CHECK CONSTRAINT [FK_Groups_Security_Entities_Functions_Groups_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Functions_Permissions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Functions_Permissions] FOREIGN KEY([PermissionsNVPDetailSeqId])
REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Functions_Permissions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions] CHECK CONSTRAINT [FK_Groups_Security_Entities_Functions_Permissions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Groups_Groups]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Groups]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups] FOREIGN KEY([GroupSeqId])
REFERENCES [ZGWSecurity].[Groups] ([GroupSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Groups_Groups]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Groups]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Groups] CHECK CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Groups_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Groups]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups_Security_Entities] FOREIGN KEY([GroupsSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Groups_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Groups]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Groups] CHECK CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]  WITH CHECK ADD  CONSTRAINT [FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS] FOREIGN KEY([PermissionsNVPDetailSeqId])
REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] CHECK CONSTRAINT [FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities] FOREIGN KEY([GroupsSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] CHECK CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] CHECK CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities] FOREIGN KEY([GroupsSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities] CHECK CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities] FOREIGN KEY([RolesSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities] CHECK CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Navigation_Types_ZGWSystem_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Navigation_Types]'))
ALTER TABLE [ZGWSecurity].[Navigation_Types]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Navigation_Types_ZGWSystem_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Navigation_Types_ZGWSystem_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Navigation_Types]'))
ALTER TABLE [ZGWSecurity].[Navigation_Types] CHECK CONSTRAINT [FK_ZGWSecurity_Navigation_Types_ZGWSystem_Name_Value_Pairs]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Navigation_Types_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Navigation_Types]'))
ALTER TABLE [ZGWSecurity].[Navigation_Types]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Navigation_Types_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Navigation_Types_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Navigation_Types]'))
ALTER TABLE [ZGWSecurity].[Navigation_Types] CHECK CONSTRAINT [FK_ZGWSecurity_Navigation_Types_ZGWSystem_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Permissions_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Permissions]'))
ALTER TABLE [ZGWSecurity].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Permissions_Name_Value_Pairs]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Permissions]'))
ALTER TABLE [ZGWSecurity].[Permissions] CHECK CONSTRAINT [FK_Permissions_Name_Value_Pairs]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Permissions_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Permissions]'))
ALTER TABLE [ZGWSecurity].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Permissions_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_ZGWSecurity_Permissions_ZGWSystem_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Permissions]'))
ALTER TABLE [ZGWSecurity].[Permissions] CHECK CONSTRAINT [FK_ZGWSecurity_Permissions_ZGWSystem_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Entities] FOREIGN KEY([SecurityEntitySeqId])
REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities] CHECK CONSTRAINT [FK_Roles_Security_Entities_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Roles]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Roles] FOREIGN KEY([RoleSeqId])
REFERENCES [ZGWSecurity].[Roles] ([RoleSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Roles]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities] CHECK CONSTRAINT [FK_Roles_Security_Entities_Roles]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Accounts_Accounts]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Accounts_Accounts] FOREIGN KEY([AccountSeqId])
REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Accounts_Accounts]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] CHECK CONSTRAINT [FK_Roles_Security_Entities_Accounts_Accounts]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Accounts_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Accounts_Roles_Security_Entities] FOREIGN KEY([RolesSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Accounts_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Accounts]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] CHECK CONSTRAINT [FK_Roles_Security_Entities_Accounts_Roles_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Functions_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Functions_Functions] FOREIGN KEY([FunctionSeqId])
REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Functions_Functions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions] CHECK CONSTRAINT [FK_Roles_Security_Entities_Functions_Functions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Functions_Permissions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Functions_Permissions] FOREIGN KEY([PermissionsNVPDetailSeqId])
REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Functions_Permissions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions] CHECK CONSTRAINT [FK_Roles_Security_Entities_Functions_Permissions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Functions_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Functions_Roles_Security_Entities] FOREIGN KEY([RolesSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Functions_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Functions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions] CHECK CONSTRAINT [FK_Roles_Security_Entities_Functions_Roles_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Permissions_Permissions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Permissions_Permissions] FOREIGN KEY([PermissionsNVPDetailSeqId])
REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Permissions_Permissions]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions] CHECK CONSTRAINT [FK_Roles_Security_Entities_Permissions_Permissions]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Permissions_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Security_Entities_Permissions_Roles_Security_Entities] FOREIGN KEY([RolesSecurityEntitiesSeqId])
REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Roles_Security_Entities_Permissions_Roles_Security_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Roles_Security_Entities_Permissions]'))
ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions] CHECK CONSTRAINT [FK_Roles_Security_Entities_Permissions_Roles_Security_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Entities_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Entities_Entities] FOREIGN KEY([ParentSecurityEntitySeqId])
REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Entities_Entities]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Security_Entities] CHECK CONSTRAINT [FK_Entities_Entities]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Entities_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Security_Entities]  WITH CHECK ADD  CONSTRAINT [FK_Entities_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[FK_Entities_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSecurity].[Security_Entities]'))
ALTER TABLE [ZGWSecurity].[Security_Entities] CHECK CONSTRAINT [FK_Entities_Statuses]
GO
IF NOT EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[FK_Name_Value_Pairs_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSystem].[Name_Value_Pairs]'))
ALTER TABLE [ZGWSystem].[Name_Value_Pairs]  WITH CHECK ADD  CONSTRAINT [FK_Name_Value_Pairs_Statuses] FOREIGN KEY([StatusSeqId])
REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
GO
IF  EXISTS (SELECT *
FROM sys.foreign_keys
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[FK_Name_Value_Pairs_Statuses]') AND parent_object_id = OBJECT_ID(N'[ZGWSystem].[Name_Value_Pairs]'))
ALTER TABLE [ZGWSystem].[Name_Value_Pairs] CHECK CONSTRAINT [FK_Name_Value_Pairs_Statuses]
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Get_Account_Choice]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Get_Account_Choice]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Get_Account_Choice] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 0

	exec ZGWCoreWeb.Get_Account_Choice
		@P_Account ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description: Gets a record from xx
--	given the Account
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Get_Account_Choice]
	@P_Account VARCHAR(128),
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF EXISTS(SELECT Account
FROM ZGWCoreWeb.Account_Choices
WHERE Account = @P_Account)
		BEGIN
	IF @P_Debug = 1 PRINT 'Selecting client choices for ' + CONVERT(VARCHAR(25),@P_Account)
	SELECT
		Account AS ACCT
				, SecurityEntityID
				, SecurityEntityName
				, BackColor
				, LeftColor
				, HeadColor
				, HeaderForeColor
				, SubHeadColor
				, RowBackColor
				, AlternatingRowBackColor
				, ColorScheme
				, FavoriteAction
				, recordsPerPage
	FROM ZGWCoreWeb.Account_Choices
	WHERE
				Account = @P_Account
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Selecting client choices for the Anonymous account'
	SELECT
		Account AS ACCT
				, SecurityEntityID
				, SecurityEntityName
				, BackColor
				, LeftColor
				, HeadColor
				, HeaderForeColor
				, SubHeadColor
				, RowBackColor
				, AlternatingRowBackColor
				, ColorScheme
				, FavoriteAction
				, recordsPerPage
	FROM ZGWCoreWeb.Account_Choices
	WHERE
				[Account] = 'Anonymous'
END

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Get_Messages]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Get_Messages]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Get_Messages] AS'
END
GO

/*
Usage:
	DECLARE 	
		@P_MessageSeqId INT, 
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Messages
		@P_MessageSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the MessageSeqId.  If MessageSeqId = -1
--	all messages are returned.
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Get_Messages]
	@P_MessageSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWCoreWeb.Get_Messages'
	DECLARE @V_DefaultSecurityEntitySeqId INT = ZGWSecurity.Get_Default_Entity_ID()
	/*
		Don't like this ... need to think of a better way
		something along the lines of core defaul security entity message
		+ other security entity messages ...
	*/
	IF (SELECT COUNT(*)
FROM ZGWCoreWeb.[Messages]
WHERE SecurityEntitySeqId = @PSecurityEntitySeqId) = 0
		BEGIN
	IF (SELECT COUNT(*)
	FROM ZGWCoreWeb.[Messages]
	WHERE SecurityEntitySeqId = @V_DefaultSecurityEntitySeqId) > 0
				BEGIN
		INSERT INTO ZGWCoreWeb.[Messages]
		SELECT
			@PSecurityEntitySeqId
							, Name
							, Title
							, [Description]
							, Format_As_HTML
							, Body
							, Added_By
							, Added_Date
							, Updated_By
							, Updated_Date
		FROM
			ZGWCoreWeb.[Messages]
		WHERE SecurityEntitySeqId = @V_DefaultSecurityEntitySeqId
		IF @P_Debug = 1 PRINT 'Needed to add entries for all message for the requested Security_Entity'
	END
			ELSE
				IF @P_Debug = 1 PRINT 'There are no message as of yet stop trying to get them!'
	RETURN
--END IF
END
	--END IF

	IF @P_MessageSeqId <> -1
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting single message'
	SELECT
		MessageSeqId as MESSAGE_SEQ_ID
				, SecurityEntitySeqId as SecurityEntityID
				, NAME
				, TITLE
				, [Description]
				, Format_As_HTML
				, BODY
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWCoreWeb.[Messages]
	WHERE
				MessageSeqId = @P_MessageSeqId
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting all messages'
	SELECT
		MessageSeqId as MESSAGE_SEQ_ID
				, SecurityEntitySeqId as SecurityEntityID
				, NAME
				, TITLE
				, [Description]
				, Format_As_HTML
				, BODY
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWCoreWeb.[Messages]
	ORDER BY
				[Name]
END
	--END IF
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Get_Notification_Status]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Get_Notification_Status]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Get_Notification_Status] AS'
END
GO

/*
Usage:
	DECLARE
		@PSecurityEntitySeqId int = 1,
		@P_FunctionSeqId int = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Primary_Key INT = null,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Notification_Status
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_Account,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns single value of 0 or 1
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Get_Notification_Status]
	@PSecurityEntitySeqId int,
	@P_FunctionSeqId int,
	@P_Account VARCHAR(128),
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Notification_Status'
	-- GET AccountSeqId
	DECLARE @V_AccountSeqId Int
	SET @V_AccountSeqId = (SELECT AccountSeqId
FROM ZGWSecurity.Accounts
WHERE Account = @P_Account)

	IF EXISTS
		(SELECT
	Added_By
FROM
	ZGWCoreWeb.Notifications WITH(NOLOCK)
WHERE 
			SecurityEntitySeqId = @PSecurityEntitySeqId
	AND FunctionSeqId = @P_FunctionSeqId
	AND Added_By = @V_AccountSeqId)
		BEGIN
	SELECT @P_Primary_Key = 1
END
	ELSE
		BEGIN
	SELECT @P_Primary_Key = 0
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Notification_Status'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Get_Notifications]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Get_Notifications]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Get_Notifications] AS'
END
GO

/*
Usage:
	DECLARE
		@PSecurityEntitySeqId int,
		@P_FunctionSeqId int,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Notifications
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns single value of 0 or 1
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Get_Notifications]
	@PSecurityEntitySeqId int,
	@P_FunctionSeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWCoreWeb.Get_Notifications'
	DECLARE @V_Enable_Notifications Int
	SET @V_Enable_Notifications = 1 -- True

	SELECT
	Email
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK)
	INNER JOIN ZGWCoreWeb.Notifications Notifications WITH(NOLOCK)
	ON Accounts.AccountSeqId = Notifications.Added_By
WHERE
		Accounts.Enable_Notifications = @V_Enable_Notifications
	AND Notifications.FunctionSeqId = @P_FunctionSeqId
	AND Notifications.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		Email

	IF @P_Debug = 1 PRINT 'Ending ZGWCoreWeb.Get_Notifications'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Set_Account_Choices]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Set_Account_Choices]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Set_Account_Choices] AS'
END
GO

/*
Usage:
	EXEC ZGWCoreWeb.Set_Account_Choices
		@P_ACCT = N'Anonymous',
		@P_SecurityEntityID = 1,
		@P_SecurityEntityName = 'System',
		@P_BackColor = '#ffffff',
		@P_LeftColor = '#eeeeee',
		@P_HeadColor = '#C7C7C7',
		@P_HeaderForeColor = 'Black',
		@P_SubHeadColor = '#b6cbeb',
		@P_RowBackColor = '#b6cbeb',
		@P_AlternatingRowBackColor = '#6699cc',
		@P_ColorScheme = 'Blue',
		@P_FavoriteAction = 'Home',
		@P_recordsPerPage = 5
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_ACCT
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Set_Account_Choices]
	@P_ACCT VARCHAR(128),
	@P_SecurityEntityID int,
	@P_SecurityEntityName VARCHAR(256),
	@P_BackColor VARCHAR(15),
	@P_LeftColor VARCHAR(15),
	@P_HeadColor VARCHAR(15),
	@P_HeaderForeColor VARCHAR(15),
	@P_SubHeadColor VARCHAR(15),
	@P_RowBackColor VARCHAR(15),
	@P_AlternatingRowBackColor VARCHAR(15),
	@P_ColorScheme VARCHAR(15),
	@P_FavoriteAction VARCHAR(50),
	@P_recordsPerPage int
AS
-- INSERT a new row in the table.
	IF(SELECT COUNT(*)
FROM ZGWCoreWeb.Account_Choices
WHERE Account = @P_ACCT) <= 0
		BEGIN
	INSERT ZGWCoreWeb.Account_Choices
		(
		Account,
		SecurityEntityID,
		SecurityEntityName,
		BackColor,
		LeftColor,
		HeadColor,
		HeaderForeColor,
		SubHeadColor,
		RowBackColor,
		AlternatingRowBackColor,
		ColorScheme,
		FavoriteAction,
		recordsPerPage
		)
	VALUES
		(
			@P_ACCT,
			@P_SecurityEntityID,
			@P_SecurityEntityName,
			@P_BackColor,
			@P_LeftColor,
			@P_HeadColor,
			@P_HeaderForeColor,
			@P_SubHeadColor,
			@P_RowBackColor,
			@P_AlternatingRowBackColor,
			@P_ColorScheme,
			@P_FavoriteAction,
			@P_recordsPerPage
			)
END
	ELSE
		BEGIN
	UPDATE ZGWCoreWeb.Account_Choices
			SET
				SecurityEntityID = @P_SecurityEntityID,
				SecurityEntityName = @P_SecurityEntityName,
				BackColor =@P_BackColor ,
				LeftColor=@P_LeftColor,
				HeadColor=@P_HeadColor,
				HeaderForeColor=@P_HeaderForeColor,
				SubHeadColor=@P_SubHeadColor,
				RowBackColor=@P_RowBackColor,
				AlternatingRowBackColor=@P_AlternatingRowBackColor,
				ColorScheme=@P_ColorScheme,
				FavoriteAction=@P_FavoriteAction,
				recordsPerPage=@P_recordsPerPage
			WHERE
				Account=@P_ACCT
END
	-- END IF
-- Get the Error Code for the statement just executed.
--SELECT @P_ErrorCode=@@ERROR
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Set_Message]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Set_Message]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Set_Message] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_MessageSeqId INT = 1,
		@PSecurityEntitySeqId INT = 2,
		@P_Name VARCHAR(50) 'Test',
		@P_Title VARCHAR(100) = 'Just Testing',
		@P_Description VARCHAR(512) = 'Some description',
		@P_Body VARCHAR(MAX) = 'The body',
		@P_Format_As_HTML INT = 0,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Set_Message
		@P_MessageSeqId,
		@PSecurityEntitySeqId,
		@P_Name,
		@P_Title,
		@P_Description,
		@P_Body,
		@P_Format_As_HTML,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug

	PRINT 'Primay key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWCoreWeb.[Messages]
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Set_Message]
	@P_MessageSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Name VARCHAR(50),
	@P_Title VARCHAR(100),
	@P_Description VARCHAR(512),
	@P_Body VARCHAR(MAX),
	@P_Format_As_HTML INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Message'
	DECLARE @V_Now DATETIME = GETDATE()

	IF @P_MessageSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS( SELECT [Name]
	FROM ZGWCoreWeb.[Messages]
	WHERE [Name] = @P_Name AND
		SecurityEntitySeqId = @PSecurityEntitySeqId
			)
				BEGIN
		UPDATE ZGWCoreWeb.[Messages]
					SET
						SecurityEntitySeqId = @PSecurityEntitySeqId,
						[Name] = @P_Name,
						Title = @P_Title,
						[Description] = @P_Description,
						Format_As_HTML = @P_Format_As_HTML,
						Body = @P_Body,
						Updated_By = @P_Added_Updated_By,
						Updated_Date = GETDATE()
					WHERE
						MessageSeqId = @P_MessageSeqId
			AND SecurityEntitySeqId = @PSecurityEntitySeqId

		SELECT @P_Primary_Key = @P_MessageSeqId
	-- set the output id just in case.
	END
			ELSE
				BEGIN
		INSERT ZGWCoreWeb.[Messages]
			(
			SecurityEntitySeqId,
			[Name],
			Title,
			[Description],
			BODY,
			Format_As_HTML,
			Added_By,
			Added_Date
			)
		VALUES
			(
				@PSecurityEntitySeqId,
				@P_Name,
				@P_Title,
				@P_Description,
				@P_Body,
				@P_Format_As_HTML,
				@P_Added_Updated_By,
				@V_Now
					)
		SELECT @P_Primary_Key = SCOPE_IDENTITY()
	-- Get the IDENTITY value for the row just inserted.
	END
END
	ELSE
		BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS( SELECT [Name]
	FROM ZGWCoreWeb.[Messages]
	WHERE [Name] = @P_Name AND
		SecurityEntitySeqId = @PSecurityEntitySeqId
			)
			BEGIN
		RAISERROR ('The message you entered already exists in the database.',16,1)
		RETURN
	END

	INSERT ZGWCoreWeb.[Messages]
		(
		SecurityEntitySeqId,
		[Name],
		Title,
		[Description],
		Body,
		Format_As_HTML,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@PSecurityEntitySeqId,
			@P_Name,
			@P_Title,
			@P_Description,
			@P_Body,
			@P_Format_As_HTML,
			@P_Added_Updated_By,
			@V_Now
			)
	SELECT @P_Primary_Key = SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
	-- END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Message'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWCoreWeb].[Set_Notification]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWCoreWeb].[Set_Notification]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Set_Notification] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId int = 2,
		@P_FunctionSeqId int = 1,
		@P_Account Varchar(128) = 'Developer',
		@P_Status int = 1,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Set_Notification
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_Account,
		@P_Status,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/07/2011
-- Description:	Inserts or deletes from ZGWCoreWeb.Notifications
--	Status value 1 = insert, 0 = delete
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Set_Notification]
	@PSecurityEntitySeqId int,
	@P_FunctionSeqId int,
	@P_Account Varchar(128),
	@P_Status int,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Notification'
DECLARE @V_AccountSeqId int = (SELECT AccountSeqId
FROM ZGWSecurity.Accounts
WHERE Account = @P_Account)

IF @P_Status = 1
	BEGIN
	IF NOT EXISTS
		(
			SELECT
		Added_By
	FROM
		ZGWCoreWeb.Notifications
	WHERE 
				SecurityEntitySeqId = @PSecurityEntitySeqId
		AND FunctionSeqId = @P_FunctionSeqId
		AND Added_By = @V_AccountSeqId
		)
		BEGIN
		IF @P_Debug = 1 PRINT 'Insert'
		INSERT ZGWCoreWeb.Notifications
			(
			SecurityEntitySeqId,
			FunctionSeqId,
			Added_By
			)
		VALUES
			(
				@PSecurityEntitySeqId,
				@P_FunctionSeqId,
				@V_AccountSeqId
			)
	END
END
ELSE
	IF @P_Debug = 1 PRINT 'Delete'
	DELETE 
		ZGWCoreWeb.Notifications
	WHERE 
		SecurityEntitySeqId = @PSecurityEntitySeqId
	AND FunctionSeqId = @P_FunctionSeqId
	AND Added_By = @V_AccountSeqId
--END IF
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Notification'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWOptional].[Get_Directory]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Get_Directory]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Get_Directory] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWOptional.Get_Directory
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/11/2011
-- Description:	Selects directory infomation given
--	the FunctionSeqId. When FunctionSeqId = -1
--	all rows in the table are retruned.
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Get_Directory]
	@P_FunctionSeqId INT,
	@P_Debug INT = 0
AS
IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Get_Directory'
IF @P_FunctionSeqId = -1
	BEGIN
	IF @P_Debug = 1 PRINT 'Getting all'
	SELECT
		FunctionSeqId as FUNCTION_SEQ_ID
			, Directory
			, Impersonate
			, Impersonating_Account as IMPERSONATE_ACCOUNT
			, Impersonating_Password as IMPERSONATE_PWD
			, Added_By
			, Added_Date
			, Updated_By
			, Updated_Date
	FROM
		ZGWOptional.Directories WITH(NOLOCK)
	ORDER BY
			Directory
END
ELSE
	BEGIN
	IF @P_Debug = 1 PRINT 'Getting 1'
	SELECT
		FunctionSeqId as FUNCTION_SEQ_ID
			, Directory
			, Impersonate
			, Impersonating_Account as IMPERSONATE_ACCOUNT
			, Impersonating_Password as IMPERSONATE_PWD
			, Added_By
			, Added_Date
			, Updated_By
			, Updated_Date
	FROM
		ZGWOptional.Directories
	WHERE
			FunctionSeqId = @P_FunctionSeqId
END
-- end if
IF @P_Debug = 1 PRINT 'Ending ZGWOptional.Get_Directory'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWOptional].[Get_State]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Get_State]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Get_State] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_State AS varchar(2) = 'ca',
		@P_Debug INT = 1

	exec ZGWOptional.Get_State
		@P_State,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/23/2011
-- Description:	Retrieves State details
--	given the state
-- Note:
--	SeqID value of -1 will return all
--	security enties.
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Get_State]
	@P_State CHAR(2),
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Get_State'
	IF @P_State <> '-1'
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting a single record'
	SELECT
		[State]
				, [Description]
				, StatusSeqId
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWOptional.States
	WHERE [State] = @P_State
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting all single records'
	SELECT
		[State]
				, [Description]
				, StatusSeqId
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWOptional.States
	ORDER BY
				[State]
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWOptional.Get_State'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWOptional].[Set_Directory]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Set_Directory]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Set_Directory] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Directory VARCHAR(255) = '',
		@P_Impersonate INT = 0,
		@P_Impersonating_Account VARCHAR(50) = '',
		@P_Impersonating_Password VARCHAR(50) = '',
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key INT,
		@P_Debug INT = 0

	exec ZGWOptional.Set_Directory
		@P_FunctionSeqId,
		@P_Directory,
		@P_Impersonate,
		@P_Impersonating_Account,
		@P_Impersonating_Password,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWOptional.Directories
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Set_Directory]
	@P_FunctionSeqId INT,
	@P_Directory VARCHAR(255),
	@P_Impersonate INT,
	@P_Impersonating_Account VARCHAR(50),
	@P_Impersonating_Password VARCHAR(50),
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Set_Directory'
	DECLARE @V_Now DATETIME = GETDATE()
	IF (SELECT COUNT(*)
FROM ZGWOptional.Directories
WHERE FunctionSeqId = @P_FunctionSeqId) = 0
		BEGIN
	IF @P_Debug = 1 PRINT 'Insert Row'
	INSERT ZGWOptional.Directories
		(
		FunctionSeqId,
		Directory,
		Impersonate,
		Impersonating_Account,
		Impersonating_Password,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_FunctionSeqId,
			@P_Directory,
			@P_Impersonate,
			@P_Impersonating_Account,
			@P_Impersonating_Password,
			@P_Added_Updated_By,
			@V_Now
			)

	SELECT @P_Primary_Key = @P_FunctionSeqId
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Update Row'
	UPDATE ZGWOptional.Directories
			SET 
				FunctionSeqId = @P_FunctionSeqId,
				Directory = @P_Directory,
				Impersonate = @P_Impersonate,
				Impersonating_Account = @P_Impersonating_Account,
				Impersonating_Password = @P_Impersonating_Password,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				FunctionSeqId = @P_FunctionSeqId

	SELECT @P_Primary_Key = @P_FunctionSeqId
END
	--end if
	IF @P_Debug = 1 PRINT 'Ending Optional.Set_Directory'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWOptional].[Set_State]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Set_State]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Set_State] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_State VARCHAR(2) = 'MA',
		@P_Description VARCHAR(128) = 'Changed',
		@P_StatusSeqId INT = 1,
		@P_Updated_By INT = 1,
		@P_Primary_Key VARCHAR(2),
		@P_Debug INT = 0

	exec ZGWOptional.Set_State
		@P_State,
		@P_Description,
		@P_StatusSeqId,
		@P_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug
	PRINT '@P_Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Inserts into ZGWOptional.States
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Set_State]
	@P_State CHAR(2),
	@P_Description VARCHAR(128),
	@P_StatusSeqId INT,
	@P_Updated_By INT,
	@P_Primary_Key CHAR(2) OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	DECLARE @V_Now DATETIME = GETDATE()
	UPDATE
		ZGWOptional.States
	SET 
		[State] = @P_State,
		[Description] = @P_Description,
		StatusSeqId = @P_StatusSeqId,
		Updated_By = @P_Updated_By,
		Updated_Date = @V_Now
	WHERE
		[State] = @P_State

	SELECT @P_Primary_Key = @P_State
END

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Account]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Account]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Account] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_AccountSeqId int = 4,
		@P_Debug INT = 0

	exec  ZGWSecurity.Delete_Account
		@P_AccountSeqId ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from [ZGWSecurity].[Accounts]
--	given the AccountSeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Account]
	@P_AccountSeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Account]'
	-- DELETE an existing row from the table.
	DELETE FROM ZGWSecurity.Accounts
	WHERE
		AccountSeqId = @P_AccountSeqId
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Account]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Account_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Account_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Account_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_AccountSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_ErrorCode int

	exec  ZGWSecurity.Delete_Account_Groups
		@P_AccountSeqId,
		@PSecurityEntitySeqId,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts 
--	given the AccountSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Account_Groups]
	@P_AccountSeqId INT,
	@PSecurityEntitySeqId	INT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Account_Groups]'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Accounts 
	WHERE 
		GroupsSecurityEntitiesSeqId IN(SELECT GroupsSecurityEntitiesSeqId
		FROM ZGWSecurity.Groups_Security_Entities
		WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND AccountSeqId = @P_AccountSeqId
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Account_Groups]'
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Account_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Account_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Account_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_AccountSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_ErrorCode int

	exec ZGWSecurity.Delete_Account_Roles
		@P_AccountSeqId,
		@PSecurityEntitySeqId,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the AccountSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Account_Roles]
	@P_AccountSeqId INT,
	@PSecurityEntitySeqId	INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Start [ZGWSecurity].[Delete_Account_Roles]'
	DELETE FROM 
		ZGWSecurity.Roles_Security_Entities_Accounts 
	WHERE 
		RolesSecurityEntitiesSeqId IN(SELECT RolesSecurityEntitiesSeqId
		FROM ZGWSecurity.Roles_Security_Entities
		WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND AccountSeqId = @P_AccountSeqId
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Account_Roles]'
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Entity]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Entity]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Entity] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId int = 4,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Function
		@PSecurityEntitySeqId ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Deletes a record from ZGWSecurity.Security_Entities
--	given the SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Entity]
	@PSecurityEntitySeqId int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [ZGWSecurity].[Delete_Entity]'
	DELETE FROM ZGWSecurity.Security_Entities
	WHERE
		SecurityEntitySeqId = @PSecurityEntitySeqId
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Entity]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Function]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Function]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Function] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 4,
		@P_ErrorCode int

	exec ZGWSecurity.Delete_Function
		@P_FunctionSeqId ,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Functions
--	given the FunctionSeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Function]
	@P_FunctionSeqId int,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function'
	-- DELETE an existing row from the table.
	DELETE FROM ZGWSecurity.Functions WHERE	FunctionSeqId = @P_FunctionSeqId
	-- Get the Error Code for the statement just executed.
	SELECT @P_ErrorCode=@@ERROR
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function'
	RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Function_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Function_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Function_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_ErrorCode int,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Function_Groups
		@P_FunctionSeqId,
		@PSecurityEntitySeqId,
		@P_PermissionsNVPDetailSeqId,
		@P_ErrorCode OUT,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Functions 
--	given the FunctionSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Function_Groups]
	@P_FunctionSeqId INT,
	@PSecurityEntitySeqId	INT,
	@P_PermissionsNVPDetailSeqId INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function_Groups'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Functions
	WHERE 
		GroupsSecurityEntitiesSeqId IN(SELECT GroupsSecurityEntitiesSeqId
		FROM ZGWSecurity.Groups_Security_Entities
		WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND FunctionSeqId = @P_FunctionSeqId
		AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function_Groups'
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Function_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Function_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Function_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_ErrorCode int,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Function_Groups
		@P_FunctionSeqId,
		@PSecurityEntitySeqId,
		@P_PermissionsNVPDetailSeqId,
		@P_ErrorCode OUT,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Functions
--	given the FunctionSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Function_Roles]
	@P_FunctionSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_PermissionsNVPDetailSeqId INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function_Roles'
	DELETE FROM ZGWSecurity.Roles_Security_Entities_Functions
	WHERE 
		RolesSecurityEntitiesSeqId IN(SELECT RolesSecurityEntitiesSeqId
		FROM ZGWSecurity.Roles_Security_Entities
		WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND FunctionSeqId = @P_FunctionSeqId
		AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function_Groups'
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Group]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Group]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Group] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_GroupSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Group
		@P_GroupSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups
--	given the GroupSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Group]
	@P_GroupSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Group'
	/*
	NOTE : ** CASCADE DELETE SHOULD BE TURNED ON IN
		ZGWSecurity.Groups_Security_Entities FOR THIS TO WORK ELSE
		THIS MIGHT THROW AN ERROR
		**** 
	*/
	DECLARE @GROUP_COUNT INT
	BEGIN TRANSACTION
	BEGIN
		-- DELETE GROUP FROM ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
		IF @P_Debug = 1 PRINT 'Deleting rows from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities'
		DELETE ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			WHERE (GroupsSecurityEntitiesSeqId = 
						(SELECT
			GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities
		WHERE 
							GroupSeqId=@P_GroupSeqId
			AND SecurityEntitySeqId = @PSecurityEntitySeqId
						)
					)
	END

	BEGIN
		-- DELETE GROUP FROM ZGWSecurity.Groups_Security_Entities
		IF @P_Debug = 1 PRINT 'Deleting rows from ZGWSecurity.Groups_Security_Entities'
		DELETE ZGWSecurity.Groups_Security_Entities
			WHERE (
				GroupSeqId = @P_GroupSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId
				   )
	END

	BEGIN
		-- DELETE GROUP FROM ZGWSecurity.Groups_Security_Entities
		SET @GROUP_COUNT = (SELECT COUNT(*)
		FROM
			ZGWSecurity.Groups,
			ZGWSecurity.Groups_Security_Entities
		WHERE
						ZGWSecurity.Groups.GroupSeqId = ZGWSecurity.Groups_Security_Entities.GroupSeqId
			AND ZGWSecurity.Groups.GroupSeqId = @P_GroupSeqId)
		-- PRINT @GROUP_COUNT -- for debug
		IF @GROUP_COUNT = 0
				IF @P_Debug = 1 PRINT 'Role is not used by other entites'
		BEGIN
			DELETE ZGWSecurity.Groups
					WHERE (GroupSeqId = @P_GroupSeqId)
		END
	-- END IF
	END
	--  DELETE GROUP FROM ZGWSecurity.Groups
	IF @@ERROR <> 0
	 BEGIN
		-- Rollback the transaction
		ROLLBACK

		-- Raise an error and return
		RAISERROR ('Error in deleting group.', 16, 1)
		RETURN
	END
	COMMIT
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Group'
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Group_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Group_Accounts]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Group_Accounts] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_GroupSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Group_Accounts]
		@P_GroupSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts
--	given theGroupSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Group_Accounts]
	@P_GroupSeqId AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Group_Accounts]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Accounts
	WHERE
		GroupsSecurityEntitiesSeqId IN (
			SELECT
	GroupsSecurityEntitiesSeqId
FROM
	ZGWSecurity.Groups_Security_Entities
WHERE 
				GroupSeqId = @P_GroupSeqId
	AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Group_Accounts]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Group_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Group_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Group_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_GroupSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Function_Groups
		@P_GroupSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/29/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
--	given the GroupSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Group_Roles]
	@P_GroupSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Group_Roles]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
	WHERE
		GroupsSecurityEntitiesSeqId IN (SELECT GroupsSecurityEntitiesSeqId
FROM ZGWSecurity.Groups_Security_Entities
WHERE SecurityEntitySeqId=@PSecurityEntitySeqId
	AND GroupSeqId = @P_GroupSeqId)
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Group_Roles]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Groups_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Groups_Accounts]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Groups_Accounts] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_GroupsSecurityEntitiesSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Groups_Accounts]
		@P_GroupsSecurityEntitiesSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts
--	given the GroupsSecurityEntitiesSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Groups_Accounts]
	@P_GroupsSecurityEntitiesSeqId AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Groups_Accounts]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Accounts
	WHERE
		GroupsSecurityEntitiesSeqId IN (
			SELECT
	GroupsSecurityEntitiesSeqId
FROM
	ZGWSecurity.Groups_Security_Entities
WHERE 
				GroupsSecurityEntitiesSeqId = @P_GroupsSecurityEntitiesSeqId
	AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Groups_Accounts]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Role]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Role]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Role] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Name AS VARCHAR(50) = 'MyRole',
		@PSecurityEntitySeqId AS INT = 1

	exec ZGWSecurity.Delete_Role
		@P_Name
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Roles,
--	0 to x records from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities and ZGWSecurity.Roles_Security_Entities
--	given the roles name and the SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Role]
	@P_Name VARCHAR (50),
	@PSecurityEntitySeqId	INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Role'
	/*
	NOTE : ** CASCADE DELETE SHOULD BE TURNED ON IN
		ZGWSecurity.Roles_Security_Entities FOR THIS TO WORK ELSE
		THIS MIGHT THROW AN ERROR
		**** 
	*/
	DECLARE @V_RolesSeqId INT
			
	SET @V_RolesSeqId = (SELECT RoleSeqId
FROM ZGWSecurity.Roles
WHERE [Name] = @P_Name)

	BEGIN TRANSACTION
		BEGIN
	-- DELETE ROLE FROM Groups_Security_Entities_Roles_Security_Entities
	/*
				Note:  This should not be necessary ... cascade delete and triggers should
				handle this and deleting the record from ZGWSecurity.Roles should be sufficient
				... this would be "overkill" and or for other datastores that
				don't support cascade delete or triggers.
				... no i don't know of one off hand and yes i know this is for sql server :)
			*/
	IF @P_Debug = 1 PRINT 'Deleting roles from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities'
	DELETE ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			WHERE (RolesSecurityEntitiesSeqId = 
						(SELECT
		RolesSecurityEntitiesSeqId
	FROM
		ZGWSecurity.Roles_Security_Entities
	WHERE 
							RoleSeqId = @V_RolesSeqId
		AND SecurityEntitySeqId = @PSecurityEntitySeqId
						)
					)
END 

		BEGIN
	-- DELETE ROLE FROM ZGWSecurity.Roles_Security_Entities
	IF @P_Debug = 1 PRINT 'Deleting roles from ZGWSecurity.Roles_Security_Entities'
	DELETE ZGWSecurity.Roles_Security_Entities
			WHERE (
				RoleSeqId= @V_RolesSeqId AND
		SecurityEntitySeqId = @PSecurityEntitySeqId
				   )
END 
		BEGIN
	-- Delete the role from ZGWSecurity.Roles if no other entites are using the role
	IF @P_Debug = 1 PRINT 'Deleting role from ZGWSecurity.Roles'
	IF (SELECT COUNT(*)
	FROM
		ZGWSecurity.Roles Roles,
		ZGWSecurity.Roles_Security_Entities RoleEntities
	WHERE
				Roles.RoleSeqId = RoleEntities.RoleSeqId
		AND Roles.RoleSeqId = @V_RolesSeqId) = 0
			BEGIN
		IF @P_Debug = 1 PRINT 'Role is not used by other entites'
		DELETE ZGWSecurity.Roles
				WHERE (RoleSeqId = @V_RolesSeqId)
	END
END
	IF @@ERROR <> 0
	 BEGIN
	-- Rollback the transaction
	ROLLBACK
	-- Raise an error and return
	RAISERROR ('Error in deleting role in ZGWSecurity.Roles.', 16, 1)
	RETURN 1
END
	COMMIT
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Role'
	RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Roles_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Roles_Accounts]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Accounts] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_RolesSecurityEntitiesSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Accounts]
		@P_RolesSecurityEntitiesSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the RolesSecurityEntitiesSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Roles_Accounts]
	@P_ROLE_SEQ_ID AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
	DECLARE @V_RolesSecurityEntitiesSeqId AS INT = (SELECT RolesSecurityEntitiesSeqId
FROM ZGWSecurity.Roles_Security_Entities
WHERE RoleSeqId = @P_ROLE_SEQ_ID)
	DELETE
		ZGWSecurity.Roles_Security_Entities_Accounts
	WHERE
		RolesSecurityEntitiesSeqId IN (
			SELECT
	RolesSecurityEntitiesSeqId
FROM
	ZGWSecurity.Roles_Security_Entities
WHERE 
				RolesSecurityEntitiesSeqId = @V_RolesSecurityEntitiesSeqId
	AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_RoleSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
		@P_RoleSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the GroupsSecurityEntitiesSeqId and SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
	@P_RoleSeqId AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
	DELETE
		ZGWSecurity.Roles_Security_Entities_Accounts
	WHERE
		RolesSecurityEntitiesSeqId IN (
			SELECT
	RolesSecurityEntitiesSeqId
FROM
	ZGWSecurity.Roles_Security_Entities
WHERE 
				RoleSeqId = @P_RoleSeqId
	AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Account]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Account]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Account] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Is_System_Admin bit = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec  ZGWSecurity.Get_Account
		@P_Is_System_Admin,
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Selects 1 or all records from ZGWSecurity.Get_Account
--	from ZGWSecurity.Accounts
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Account]
	@P_Is_System_Admin Bit,
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	-- SELECT all rows from the table.
	IF LEN(RTRIM(LTRIM(@P_Account))) = 0
		BEGIN
	IF @P_Is_System_Admin = 1
				BEGIN
		IF @P_Debug = 1 PRINT 'Selecting all accounts'
		SELECT
			AccountSeqId AS ACCT_SEQ_ID
						, Account AS ACCT
						, Email
						, Enable_Notifications
						, Is_System_Admin
						, StatusSeqId AS STATUS_SEQ_ID
						, Password_Last_Set
						, [Password] AS PWD
						, Failed_Attempts
						, First_Name
						, Last_Login
						, Last_Name
						, Location
						, Middle_Name
						, Preferred_Name
						, Time_Zone
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
		FROM
			[ZGWSecurity].[Accounts] WITH(NOLOCK)
		ORDER BY 
						[Account] ASC
	END
			ELSE
				BEGIN
		IF @P_Debug = 1 PRINT 'Selecting all accounts for Entity ' + CONVERT(VARCHAR(MAX),@PSecurityEntitySeqId)
		DECLARE @V_Accounts TABLE (
			AccountSeqId INT
						,
			Account VARCHAR(100)
						,
			Email VARCHAR(100)
						,
			Enable_Notifications BIT
						,
			Is_System_Admin INT
						,
			StatusSeqId INT
						,
			Password_Last_Set DATETIME
						,
			[Password] VARCHAR(256)
						,
			Failed_Attempts INT
						,
			First_Name VARCHAR(30)
						,
			Last_Login DATETIME
						,
			Last_Name VARCHAR(30)
						,
			Location VARCHAR(100)
						,
			Middle_Name VARCHAR(30)
						,
			Preferred_Name VARCHAR(100)
						,
			Time_Zone INT
						,
			Added_By INT
						,
			Added_Date DATETIME
						,
			Updated_By INT
						,
			Updated_Date DATETIME)
		INSERT INTO @V_Accounts
					SELECT -- Roles via roles
				Accounts.AccountSeqId
						, Accounts.Account
						, Accounts.Email
						, Accounts.Enable_Notifications
						, Accounts.Is_System_Admin
						, Accounts.StatusSeqId
						, Accounts.Password_Last_Set
						, Accounts.[Password]
						, Accounts.Failed_Attempts
						, Accounts.First_Name
						, Accounts.Last_Login
						, Accounts.Last_Name
						, Accounts.Location
						, Accounts.Middle_Name
						, Accounts.Preferred_Name
						, Accounts.Time_Zone
						, Accounts.Added_By
						, Accounts.Added_Date
						, Accounts.Updated_By
						, Accounts.Updated_Date
			FROM
				ZGWSecurity.Accounts AS Accounts WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
						Roles_Security_Entities_Accounts.AccountSeqId = Accounts.AccountSeqId
				AND Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
				AND Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		UNION
			SELECT -- Roles via groups
				Accounts.AccountSeqId
						, Accounts.Account
						, Accounts.Email
						, Accounts.Enable_Notifications
						, Accounts.Is_System_Admin
						, Accounts.StatusSeqId
						, Accounts.Password_Last_Set
						, Accounts.[Password]
						, Accounts.Failed_Attempts
						, Accounts.First_Name
						, Accounts.Last_Login
						, Accounts.Last_Name
						, Accounts.Location
						, Accounts.Middle_Name
						, Accounts.Preferred_Name
						, Accounts.Time_Zone
						, Accounts.Added_By
						, Accounts.Added_Date
						, Accounts.Updated_By
						, Accounts.Updated_Date
			FROM
				ZGWSecurity.Accounts AS Accounts WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
						ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = Accounts.AccountSeqId
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
				AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
				AND Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId

		SELECT DISTINCT
			AccountSeqId AS ACCT_SEQ_ID
						, Account AS ACCT
						, Email
						, Enable_Notifications
						, Is_System_Admin
						, StatusSeqId AS STATUS_SEQ_ID
						, Password_Last_Set
						, [Password] AS PWD
						, Failed_Attempts
						, First_Name
						, Last_Login
						, Last_Name
						, Location
						, Middle_Name
						, Preferred_Name
						, Time_Zone
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
		FROM
			@V_Accounts
		ORDER BY
						Account
	END
-- END IF
END
	ELSE
		BEGIN
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Selecting single account'
	-- SELECT an existing row from the table.
	SELECT
		AccountSeqId AS ACCT_SEQ_ID
				, Account AS ACCT
				, Email
				, Enable_Notifications
				, Is_System_Admin
				, StatusSeqId AS STATUS_SEQ_ID
				, Password_Last_Set
				, [Password] AS PWD
				, Failed_Attempts
				, First_Name
				, Last_Login
				, Last_Name
				, Location
				, Middle_Name
				, Preferred_Name
				, Time_Zone
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM ZGWSecurity.Accounts WITH(NOLOCK)
	WHERE
				[Account] = @P_Account
END
	-- END IF
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Account_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Account_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Account_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Groups
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all groups for a given Account and Entity
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Account_Groups]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	SELECT
	ZGWSecurity.Groups.[Name] AS Groups
FROM
	ZGWSecurity.Accounts WITH(NOLOCK),
	ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
	ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
	ZGWSecurity.Groups WITH(NOLOCK)
WHERE
		ZGWSecurity.Accounts.Account = @P_Account
	AND ZGWSecurity.Accounts.AccountSeqId = ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId
	AND ZGWSecurity.Groups_Security_Entities_Accounts.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
	AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
	AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		GROUPS

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Account_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Account_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Account_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Roles
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all groups for a given Account and Entity
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Account_Roles]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	SELECT
	ZGWSecurity.Roles.[Name] AS Roles
FROM
	ZGWSecurity.Accounts WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
	ZGWSecurity.Roles WITH(NOLOCK)
WHERE
		ZGWSecurity.Accounts.Account = @P_Account
	AND ZGWSecurity.Accounts.AccountSeqId = ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId
	AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
	AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
	AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		ROLES

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Account_Security]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Account_Security]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Account_Security] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Security
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all derived roles given the account
--	and Entity.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Account_Security]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
		SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account
		AND ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId
		AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
		AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
UNION
	SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account AND
		ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId AND
		ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Accounts.GroupsSecurityEntitiesSeqId AND
		ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId)) AND
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId AND
		ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId AND
		ZGWSecurity.Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
ORDER BY
		Roles

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Accounts_In_Group]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Accounts_In_Group]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_In_Group] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_GroupSeqId INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_In_Group
		@PSecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/10/2011
-- Description:	Selects all accounts in a group
--	given the SecurityEntitySeqId and GroupSeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Accounts_In_Group]
	@PSecurityEntitySeqId INT,
	@P_GroupSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Group'
	SELECT
	Accounts.Account AS ACCT
		, Accounts.Email AS Email
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK),
	ZGWSecurity.Groups_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
	ZGWSecurity.Groups_Security_Entities Security WITH(NOLOCK),
	ZGWSecurity.Groups Groups WITH(NOLOCK)
WHERE
		Accounts.AccountSeqId = AcctSecurity.AccountSeqId
	AND AcctSecurity.GroupsSecurityEntitiesSeqId = Security.GroupsSecurityEntitiesSeqId
	AND Security.GroupSeqId = Groups.GroupSeqId
	AND Accounts.StatusSeqId <> 2
	AND Groups.GroupSeqId = @P_GroupSeqId
	AND Security.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Group'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Accounts_In_Role]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Accounts_In_Role]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_In_Role] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_RoleSeqId INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_In_Role
		@PSecurityEntitySeqId,
		@P_RoleSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all accounts in a role
--	given the SecurityEntitySeqId and RoleSeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Accounts_In_Role]
	@PSecurityEntitySeqId INT,
	@P_RoleSeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Role'
	SELECT
	Accounts.Account AS ACCT
		, Accounts.Email AS Email
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities [Security] WITH(NOLOCK),
	ZGWSecurity.Roles Roles WITH(NOLOCK)
WHERE
		Accounts.AccountSeqId = AcctSecurity.AccountSeqId
	AND AcctSecurity.RolesSecurityEntitiesSeqId = Security.RolesSecurityEntitiesSeqId
	AND [Security].RoleSeqId = Roles.RoleSeqId
	AND Accounts.StatusSeqId <> 2
	AND Roles.RoleSeqId = @P_RoleSeqId
	AND [Security].SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Role'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Accounts_Not_In_Group]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Accounts_Not_In_Group]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Group] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_GroupSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Accounts_Not_In_Group
		@PSecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all accounts not in a group
--	given the SecurityEntitySeqId and GroupSeqId
-- Note: This should not be needed by the CoreWebApplication anymore
--	and was left for others that may need it.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Group]
	@PSecurityEntitySeqId INT,
	@P_GroupSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Group'
	SELECT
	Accounts.Account AS ACCT
FROM
	Accounts
WHERE
		Account NOT IN(SELECT
	Accounts.Account
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK),
	ZGWSecurity.Groups_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
	ZGWSecurity.Groups_Security_Entities [Security] WITH(NOLOCK),
	ZGWSecurity.Groups Groups WITH(NOLOCK)
WHERE
						Accounts.AccountSeqId = AcctSecurity.AccountSeqId
	AND AcctSecurity.GroupsSecurityEntitiesSeqId = Security.GroupsSecurityEntitiesSeqId
	AND Security.GroupSeqId = Groups.GroupSeqId
	AND Accounts.StatusSeqId <> 2
	AND Groups.GroupSeqId = @P_GroupSeqId
	AND [Security].SecurityEntitySeqId = @PSecurityEntitySeqId
					)
ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Group'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Accounts_Not_In_Role]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Accounts_Not_In_Role]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Role] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_RoleSeqId INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_Not_In_Role
		@PSecurityEntitySeqId,
		@P_RoleSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/11/2011
-- Description:	Selects all accounts not in a role
--	given the SecurityEntitySeqId and RoleSeqId
-- Note: This should not be needed by the CoreWebApplication anymore
--	and was left for others that may need it.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Role]
	@PSecurityEntitySeqId INT,
	@P_RoleSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Role'
	SELECT
	Accounts.Account AS ACCT
FROM
	Accounts
WHERE
		Account NOT IN(
					SELECT
	Accounts.Account
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities [Security] WITH(NOLOCK),
	ZGWSecurity.Roles Roles WITH(NOLOCK)
WHERE
						Accounts.AccountSeqId = AcctSecurity.AccountSeqId
	AND AcctSecurity.RolesSecurityEntitiesSeqId = Security.RolesSecurityEntitiesSeqId
	AND [Security].RoleSeqId = Roles.RoleSeqId
	AND Accounts.StatusSeqId <> 2
	AND Roles.RoleSeqId = @P_RoleSeqId
	AND [Security].SecurityEntitySeqId = @PSecurityEntitySeqId
					)
ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Role'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Function]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function
		@P_FunctionSeqId
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/12/2011
-- Description:	Selects function given
--	the FunctionSeqId. When FunctionSeqId = -1
--	all rows in the table are retruned.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function]
	@P_FunctionSeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function'
	IF @P_FunctionSeqId <> -1
		BEGIN
	-- SELECT an existing row from the table.
	IF @P_Debug = 1 PRINT 'Selecting single record'
	SELECT
		FunctionSeqId AS FUNCTION_SEQ_ID
				, [Name]
				, [Description]
				, FunctionTypeSeqId AS FUNCTION_TYPE_SEQ_ID
				, [Source]
				, [Controller]
				, [Resolve]
				, Enable_View_State
				, Enable_Notifications
				, Redirect_On_Timeout
				, Is_Nav
				, Link_Behavior
				, No_UI
				, Navigation_Types_NVP_DetailSeqId AS NAVIGATION_NVP_SEQ_DET_ID
				, Meta_Key_Words
				, [Action]
				, ParentSeqId AS PARENT_FUNCTION_SEQ_ID
				, Notes
				, Sort_Order
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Functions WITH(NOLOCK)
	WHERE
				FunctionSeqId = @P_FunctionSeqId
	ORDER BY 
				[Name] ASC
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Selecting all records'
	SELECT
		FunctionSeqId AS FUNCTION_SEQ_ID
				, [Name]
				, [Description]
				, FunctionTypeSeqId AS FUNCTION_TYPE_SEQ_ID
				, [Source]
				, [Controller]
				, [Resolve]
				, Enable_View_State
				, Enable_Notifications
				, Redirect_On_Timeout
				, Is_Nav
				, Link_Behavior
				, No_UI
				, Navigation_Types_NVP_DetailSeqId AS NAVIGATION_NVP_SEQ_DET_ID
				, Meta_Key_Words
				, [Action]
				, ParentSeqId AS PARENT_FUNCTION_SEQ_ID
				, Notes
				, Sort_Order
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Functions WITH(NOLOCK)
	ORDER BY 
				[Name] ASC
END
	-- END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Function_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_FunctionSeqId INT = 1,
		@P_PermissionsSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Groups
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_PermissionsSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/12/2011
-- Description:	Selects groups given the security entity
--	function and permission.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Groups]
	@PSecurityEntitySeqId INT,
	@P_FunctionSeqId INT,
	@P_PermissionsSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Groups'
	IF @P_FunctionSeqId > 0
		BEGIN
	SELECT
		ZGWSecurity.Groups.[Name] AS Groups
	FROM
		ZGWSecurity.Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups WITH(NOLOCK)
	WHERE
				ZGWSecurity.Functions.FunctionSeqId = @P_FunctionSeqId
		AND ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId
		AND ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId = @P_PermissionsSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
				Groups
END
	ELSE
		BEGIN
	SELECT
		ZGWSecurity.Functions.FunctionSeqId AS 'FUNCTION_SEQ_ID'
				, ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId AS 'PERMISSIONS_SEQ_ID'
				, ZGWSecurity.Groups.[Name] AS [Group]
	FROM
		ZGWSecurity.Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups WITH(NOLOCK)
	WHERE
				ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId
		AND ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
				FUNCTION_SEQ_ID
				, PERMISSIONS_SEQ_ID
				, [Group]
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Groups'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Function_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_FunctionSeqId INT = 1,
		@P_PermissionsSeqId INT = 1
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Function_Roles
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_PermissionsSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Selects roles given the security entity
--	function and permission.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Roles]
	@PSecurityEntitySeqId INT,
	@P_FunctionSeqId INT,
	@P_PermissionsSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Roles'
	IF @P_FunctionSeqId > 0
		BEGIN
	SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Functions WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
				ZGWSecurity.Functions.FunctionSeqId = @P_FunctionSeqId
		AND ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Roles_Security_Entities_Functions.FunctionSeqId
		AND ZGWSecurity.Roles_Security_Entities_Functions.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities_Functions.PermissionsNVPDetailSeqId = @P_PermissionsSeqId
		AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
				Roles
END
	ELSE
		BEGIN
	SELECT
		ZGWSecurity.Functions.FunctionSeqId AS 'Function_Seq_ID'
				, ZGWSecurity.Roles_Security_Entities_Functions.PermissionsNVPDetailSeqId AS 'PERMISSIONS_SEQ_ID'
				, ZGWSecurity.Roles.[Name] AS Role
	FROM
		ZGWSecurity.Functions WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
				ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Roles_Security_Entities_Functions.FunctionSeqId
		AND ZGWSecurity.Roles_Security_Entities_Functions.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
				ZGWSecurity.Functions.FunctionSeqId
				,ZGWSecurity.Roles_Security_Entities_Functions.PermissionsNVPDetailSeqId
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Roles'

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Function_Security]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Security]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Security] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Security
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Returns all Roles for all functions
--	given the SecurityEntitySeqId and NVP_DetailSeqId from
--	ZGWSecurity.Permissions or PermissionsNVPDetailSeqId
--	from ZGWSecurity.Groups_Security_Entities_Functions and 
--	ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Security]
	@PSecurityEntitySeqId int = -1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Security'
	DECLARE @V_AvalibleItems TABLE (FUNCTION_SEQ_ID INT,
	PERMISSIONS_SEQ_ID INT,
	ROLE VARCHAR(50))
	INSERT INTO @V_AvalibleItems
	SELECT DISTINCT -- Directly assigned Roles
		Functions.FunctionSeqId,
		[Permissions].NVP_DetailSeqId,
		Roles.[Name] AS [ROLE]
	FROM
		ZGWSecurity.Roles_Security_Entities Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles Roles WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Functions [Security] WITH(NOLOCK),
		ZGWSecurity.Functions WITH(NOLOCK),
		ZGWSecurity.[Permissions] WITH(NOLOCK)
	WHERE
			Roles_Security_Entities.RoleSeqId = Roles.RoleSeqId
		AND [Security].RolesSecurityEntitiesSeqId = Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND [Security].FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
		AND Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
UNION
	SELECT DISTINCT -- Roles assigned via groups
		Functions.FunctionSeqId,
		[Permissions].NVP_DetailSeqId,
		Roles.[Name] AS [ROLE]
	FROM
		ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles Roles,
		ZGWSecurity.Functions WITH(NOLOCK),
		ZGWSecurity.[Permissions] WITH(NOLOCK)
	WHERE
			ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
		AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))

	IF (SELECT COUNT(*)
FROM @V_AvalibleItems) > 0
		BEGIN
	SELECT
		*
	FROM
		@V_AvalibleItems
	ORDER BY
				FUNCTION_SEQ_ID
				,[ROLE]

	EXEC ZGWSecurity.Get_Function_Roles @PSecurityEntitySeqId, -1, -1, @P_Debug

	EXEC ZGWSecurity.Get_Function_Groups @PSecurityEntitySeqId, -1, -1, @P_Debug

END
	ELSE
		BEGIN
	IF @P_Debug = 1 
				BEGIN
		PRINT 'No Security Information was not found '
		PRINT 'Now settings the ParentSecurityEntitySeqId '
		PRINT 'the defaul Security_Entity and executing '
		PRINT 'ZGWSecurity.Get_Function_Security'
	END
	--END IF
	UPDATE ZGWSecurity.Security_Entities
				SET 
					ParentSecurityEntitySeqId = ZGWSecurity.Get_Default_Entity_ID()
				WHERE
					SecurityEntitySeqId = @PSecurityEntitySeqId
	EXEC ZGWSecurity.Get_Function_Security @PSecurityEntitySeqId, NULL
END
	-- END IF
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Security'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Function_Sort]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Sort]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Sort] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Sort
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Returns sorted function information
--	for related functions given the funtionSeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	DECLARE @V_Parent_ID INT
	DECLARE @V_NAV_TYPE_ID INT
	SET @V_Parent_ID = (SELECT ParentSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	SET @V_NAV_TYPE_ID = (SELECT Navigation_Types_NVP_DetailSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	SELECT
	FunctionSeqId as FUNCTION_SEQ_ID,
	[Name],
	[Action],
	Sort_Order
FROM
	ZGWSecurity.Functions WITH(NOLOCK)
WHERE
		ParentSeqId = @V_PARENT_ID
	AND Is_Nav = 1
	AND Navigation_Types_NVP_DetailSeqId = @V_NAV_TYPE_ID
	AND ParentSeqId <> 1
ORDER BY
		Sort_Order ASC

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Function_Types]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Types]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Types] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Types
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/16/2011
-- Description:	Returns all function types
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Types]
	@P_FunctionTypeSeqId int = -1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Types'
	IF @P_FunctionTypeSeqId = -1
		BEGIN
	IF @P_Debug = -1 PRINT 'Seleting all Function_Types'
	SELECT
		FunctionTypeSeqId as FUNCTION_TYPE_SEQ_ID
				, Name
				, [Description]
				, Template
				, Is_Content
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Function_Types WITH(NOLOCK)
	ORDER BY
				[Name]
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Seleting single Function_Type'
	SELECT
		FunctionTypeSeqId as FUNCTION_TYPE_SEQ_ID
				, Name
				, [Description]
				, Template
				, Is_Content
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Function_Types WITH(NOLOCK)
	WHERE
				FunctionTypeSeqId = @P_FunctionTypeSeqId
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Types'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Group]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Group]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Group] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId AS INT,
		@P_GroupSeqId AS INT,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Group
		@PSecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves one or more groups given the 
--	SecurityEntitySeqId and GroupSeqId.
-- Note:
--	If GroupSeqId is -1 all groups will be returned.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Group]
	@PSecurityEntitySeqId AS INT,
	@P_GroupSeqId AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Group'
	
	IF @P_GroupSeqId > -1
		BEGIN
	IF @P_Debug = 1 PRINT 'SELECT an existing row from the table.'
	SELECT
		ZGWSecurity.Groups.GroupSeqId as GROUP_SEQ_ID
				, ZGWSecurity.Groups.Name
				, ZGWSecurity.Groups.[Description]
				, ZGWSecurity.Groups.Added_By
				, ZGWSecurity.Groups.Added_Date
				, ZGWSecurity.Groups.Updated_By
				, ZGWSecurity.Groups.Updated_Date
	FROM
		ZGWSecurity.Groups WITH(NOLOCK)
	WHERE
				GroupSeqId = @P_GroupSeqId

END
	ELSE --
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting all groups for a given Security Entity'
	SELECT
		ZGWSecurity.Groups.GroupSeqId as GROUP_SEQ_ID
				, ZGWSecurity.Groups.Name
				, ZGWSecurity.Groups.[Description]
				, ZGWSecurity.Groups.Added_By
				, ZGWSecurity.Groups.Added_Date
				, ZGWSecurity.Groups.Updated_By
				, ZGWSecurity.Groups.Updated_Date
	FROM
		ZGWSecurity.Groups WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK)
	WHERE
				ZGWSecurity.Groups.GroupSeqId = ZGWSecurity.Groups_Security_Entities.GroupSeqId
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
				ZGWSecurity.Groups.Name
END
	-- END IF		
	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Group'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Group_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Group_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Group_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId AS INT,
		@P_GroupSeqId AS INT,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Group_Roles
		@PSecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrievs all roles given the 
--	group id and secruity entity id
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Group_Roles]
	@PSecurityEntitySeqId AS INT,
	@P_GroupSeqId AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Group_Roles'

	SELECT
	[Name] AS [Role]
FROM
	ZGWSecurity.Roles WITH(NOLOCK)
WHERE 
		RoleSeqId IN 
			(SELECT
	RoleSeqId
FROM
	ZGWSecurity.Roles_Security_Entities WITH(NOLOCK)
WHERE 
				RolesSecurityEntitiesSeqId IN 
				(SELECT
	RolesSecurityEntitiesSeqId
FROM
	ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK)
WHERE GroupsSecurityEntitiesSeqId IN 
					(SELECT
	GroupsSecurityEntitiesSeqId
FROM
	ZGWSecurity.Groups_Security_Entities WITH(NOLOCK)
WHERE 
						SecurityEntitySeqId = @PSecurityEntitySeqId AND GroupSeqId = @P_GroupSeqId)))
ORDER BY
		[Role]

	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Group_Roles'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Menu_Data]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Menu_Data]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Menu_Data] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId AS INT = 1,
		@P_Navigation_Types_NVP_DetailSeqId AS INT = 3,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Menu_Data
		@PSecurityEntitySeqId,
		@P_Navigation_Types_NVP_DetailSeqId,
		@P_Account,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves menu data given the
--	Account, Security Entity ID and the Navigation type.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Menu_Data]
	@PSecurityEntitySeqId INT,
	@P_Navigation_Types_NVP_DetailSeqId INT,
	@P_Account VARCHAR(128),
	@P_Debug INT = 1
AS
	SET NOCOUNT ON
	DECLARE @V_Permission_Id INT
	SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()
	DECLARE @V_AvalibleItems TABLE ([ID] INT,
	Title VARCHAR(30),
	[Description] VARCHAR(256),
	URL VARCHAR(256),
	Parent INT,
	Sort_Order INT,
	[Role] VARCHAR(50),
	FunctionTypeSeqId INT)
	INSERT INTO @V_AvalibleItems
	SELECT -- Menu items via roles
		[FUNCTIONS].FunctionSeqId AS [ID],
		[FUNCTIONS].[Name] AS Title,
		[FUNCTIONS].[Description],
		[FUNCTIONS].[Action] AS URL,
		[FUNCTIONS].ParentSeqId AS Parent,
		[FUNCTIONS].Sort_Order AS Sort_Order,
		ROLES.[Name] AS ROLE,
		[FUNCTIONS].FunctionTypeSeqId
	FROM
		ZGWSecurity.Roles_Security_Entities SE_ROLES WITH(NOLOCK),
		ZGWSecurity.Roles ROLES WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Functions [SECURITY] WITH(NOLOCK),
		ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
		ZGWSecurity.[Permissions] [Permissions] WITH(NOLOCK)
	WHERE
			SE_ROLES.RoleSeqId = ROLES.RoleSeqId
		AND [SECURITY].RolesSecurityEntitiesSeqId = SE_ROLES.RolesSecurityEntitiesSeqId
		AND [SECURITY].FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
		AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
		AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
		AND [FUNCTIONS].Is_Nav = 1
		AND SE_ROLES.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
UNION ALL
	SELECT -- Menu items via groups
		[FUNCTIONS].FunctionSeqId AS [ID],
		[FUNCTIONS].[Name] AS Title,
		[FUNCTIONS].[Description],
		[FUNCTIONS].[Action] AS URL,
		[FUNCTIONS].ParentSeqId AS Parent,
		[FUNCTIONS].Sort_Order AS Sort_Order,
		ROLES.[Name] AS ROLE,
		[FUNCTIONS].FunctionTypeSeqId
	FROM
		ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles ROLES WITH(NOLOCK),
		ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
		ZGWSecurity.[Permissions] [Permissions] WITH(NOLOCK)
	WHERE
			ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
		AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId
		AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
		AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
		AND [FUNCTIONS].Is_Nav = 1
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))

	--SELECT * FROM @V_AvalibleMenuItems -- DEBUG

	DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
	INSERT INTO @V_AccountRoles
EXEC ZGWSecurity.Get_Account_Security @P_Account, @PSecurityEntitySeqId, @P_Debug

	--SELECT * FROM @V_AccountRoles -- DEBUG
	DECLARE @V_AllMenuItems TABLE ([ID] INT,
	Title VARCHAR(30),
	[Description] VARCHAR(256),
	URL VARCHAR(256),
	Parent INT,
	Sort_Order INT,
	ROLE VARCHAR(50),
	FunctionTypeSeqId INT)
	INSERT INTO @V_AllMenuItems
SELECT -- Last but not least get the menu items when there are matching account roles.
	[ID],
	Title,
	[Description],
	URL,
	Parent,
	Sort_Order,
	[Role],
	FunctionTypeSeqId
FROM
	@V_AvalibleItems
WHERE
			ROLE IN (SELECT DISTINCT Roles
FROM @V_AccountRoles)

	DECLARE @V_DistinctItems TABLE ([ID] INT,
	TITLE VARCHAR(30),
	[Description] VARCHAR(256),
	URL VARCHAR(256),
	Parent INT,
	Sort_Order INT,
	FunctionTypeSeqId INT)
	INSERT INTO @V_DistinctItems
SELECT DISTINCT
	[ID],
	Title,
	[Description],
	URL,
	Parent,
	Sort_Order,
	FunctionTypeSeqId
FROM
	@V_AllMenuItems
	IF EXISTS (SELECT TOP(1)
	1
FROM @V_DistinctItems
WHERE [TITLE] = 'Favorite')
		BEGIN
	DECLARE @V_FavoriteAction VARCHAR(256)
	SET @V_FavoriteAction = (SELECT [FavoriteAction]
	FROM [ZGWCoreWeb].[Account_Choices]
	WHERE [Account] = @P_Account);
	IF @V_FavoriteAction IS NOT NULL
				BEGIN
		UPDATE @V_DistinctItems SET [URL] = @V_FavoriteAction WHERE [TITLE] = 'Favorite';
	END
--END IF
END
	--END IF

	SELECT
	ID as MenuID,
	TITLE AS Title,
	[Description],
	URL,
	Parent as ParentID,
	Sort_Order,
	FunctionTypeSeqId as Function_Type_Seq_ID
FROM
	@V_DistinctItems
ORDER BY
		Parent,
		Sort_Order,
		Title,
		ID


RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Name_Value_Pair_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Name_Value_Pair_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Groups] AS'
END
GO

/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Name_Value_Pair_Groups
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/22/2011
-- Description:	Returns groups associated with
--	Name Value Pairs 
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Groups]
	@P_NVPSeqId int = 1,
	@PSecurityEntitySeqId int = 1,
	@P_Debug INT = 1
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Groups'
	SELECT
	ZGWSecurity.Groups.[Name] AS GROUPS
FROM
	ZGWSecurity.Groups_Security_Entities_Permissions,
	ZGWSecurity.Groups_Security_Entities,
	ZGWSecurity.Groups
WHERE
		ZGWSecurity.Groups_Security_Entities_Permissions.NVPSeqId = @P_NVPSeqId
	AND ZGWSecurity.Groups_Security_Entities_Permissions.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
	AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
	AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		GROUPS
	IF @P_Debug = 1 PRINT 'End Get_Name_Value_Pair_Groups'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Name_Value_Pair_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Name_Value_Pair_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Roles] AS'
END
GO

/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Name_Value_Pair_Roles
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/22/2011
-- Description:	Returns roles associated with
--	Name Value Pairs 
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Roles]
	@P_NVPSeqId int = 1,
	@PSecurityEntitySeqId int = 1,
	@P_Debug INT = 1
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Roles'
	SELECT
	ZGWSecurity.Roles.[Name] AS ROLES
FROM
	ZGWSecurity.Roles_Security_Entities_Permissions,
	ZGWSecurity.Roles_Security_Entities,
	ZGWSecurity.Roles
WHERE
		ZGWSecurity.Roles_Security_Entities_Permissions.NVPSeqId = @P_NVPSeqId
	AND ZGWSecurity.Roles_Security_Entities_Permissions.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
	AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
	AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
		ROLES
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Roles'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Navigation_Types]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Navigation_Types]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Navigation_Types] AS'
END
GO

/*
Usage:
	exec ZGWSecurity.Get_Navigation_Types
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns all navigation types
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Navigation_Types]
AS
	SET NOCOUNT ON
	SELECT
	NVP_DetailSeqId AS NVP_SEQ_DET_ID
		, NVPSeqId AS NVP_SEQ_ID
		, NVP_Detail_Name AS NVP_DET_VALUE
		, NVP_Detail_Value AS NVP_DET_TEXT
		, StatusSeqId AS STATUS_SEQ_ID
		, Sort_Order
		, Added_By
		, Added_Date
		, Updated_By
		, Updated_Date
FROM
	ZGWSecurity.Navigation_Types
ORDER BY
		NVP_Detail_Name

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Role]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Role]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Role] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_RoleSeqId AS INT = -1,
		@PSecurityEntitySeqId AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Role
		@P_RoleSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves roles given the
--	the RoleSeqId and SecurityEntitySeqId
-- Note:
--	RoleSeqId of -1 returns all roles.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Role]
	@P_RoleSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start ZGWSecurity.Get_Role and SELECT an existing row from the table.'
	IF @P_RoleSeqId > -1 -- SELECT an existing row from the table.
		SELECT
	ZGWSecurity.Roles.[RoleSeqId] AS ROLE_SEQ_ID,
	ZGWSecurity.Roles.[Name],
	ZGWSecurity.Roles.[Description],
	ZGWSecurity.Roles.[Is_System],
	ZGWSecurity.Roles.[Is_System_Only],
	ZGWSecurity.Roles.[Added_By],
	ZGWSecurity.Roles.[Added_Date],
	ZGWSecurity.Roles.[Updated_By],
	ZGWSecurity.Roles.[Updated_Date]
FROM
	ZGWSecurity.Roles
WHERE
			RoleSeqId = @P_RoleSeqId
	ELSE -- GET ALL ROLES FOR A GIVEN Security Entity
		IF @P_Debug = 1 PRINT 'GET ALL ROLES FOR A GIVEN Security Entity.'
		SELECT
	ZGWSecurity.Roles.[RoleSeqId] AS ROLE_SEQ_ID,
	ZGWSecurity.Roles.[Name],
	ZGWSecurity.Roles.[Description],
	ZGWSecurity.Roles.[Is_System],
	ZGWSecurity.Roles.[Is_System_Only],
	ZGWSecurity.Roles.[Added_By],
	ZGWSecurity.Roles.[Added_Date],
	ZGWSecurity.Roles.[Updated_By],
	ZGWSecurity.Roles.[Updated_Date]
FROM
	ZGWSecurity.Roles,
	ZGWSecurity.Roles_Security_Entities
WHERE
			ZGWSecurity.Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
	AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
ORDER BY
			ZGWSecurity.Roles.[Name]
	-- END IF		
	IF @P_Debug = 1 PRINT 'End ZGWSecurity.Get_Role'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Security_Entity]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Security_Entity]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Security_Entity] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Security_Entity
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/23/2011
-- Description:	Retrieves security entity details
--	given the SecurityEntitySeqId
-- Note:
--	SeqID value of -1 will return all
--	security enties.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Security_Entity]
	@PSecurityEntitySeqId AS INT = 1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Security_Entity'
	IF @PSecurityEntitySeqId = -1
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting all Security_Enties'
	SELECT
		SecurityEntitySeqId as SecurityEntityID
				, Name
				, [Description]
				, URL
				, StatusSeqId as STATUS_SEQ_ID
				, DAL
				, DAL_Name
				, DAL_Name_Space
				, DAL_String
				, Skin
				, Style
				, Encryption_Type
				, ParentSecurityEntitySeqId as PARENT_SecurityEntityID
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Security_Entities
	ORDER BY 
				NAME ASC
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting 1 row from Security_Enties'
	SELECT
		SecurityEntitySeqId as SecurityEntityID
				, Name
				, [Description]
				, URL
				, StatusSeqId as STATUS_SEQ_ID
				, DAL
				, DAL_Name
				, DAL_Name_Space
				, DAL_String
				, Skin
				, Style
				, Encryption_Type
				, ParentSecurityEntitySeqId as PARENT_SecurityEntityID
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Security_Entities
	WHERE
				SecurityEntitySeqId = @PSecurityEntitySeqId
END
	--End IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Security_Entity'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Get_Valid_Security_Entity]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Valid_Security_Entity]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Valid_Security_Entity] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'developer',
		@P_Is_Se_Admin INT = 1,
		@PSecurityEntitySeqId AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Valid_Security_Entity
		@P_Account,
		@P_Is_Se_Admin,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/23/2011
-- Description:	Retrieves valid security entity details
--	for a given account.
-- Note:
--	SeqID value of -1 will return all
--	security enties.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Valid_Security_Entity]
	@P_Account VARCHAR(128),
	@P_Is_Se_Admin INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Valid_Security_Entity'
	DECLARE @V_Active_Status VARCHAR(50)
	DECLARE @T_Valic_Se TABLE (SecurityEntitySeqId INT)
	DECLARE @V_Is_Sys_Admin INT
	SET @V_Active_Status = (SELECT [StatusSeqId]
FROM ZGWSystem.Statuses
WHERE UPPER([Name]) = 'ACTIVE')
	SET @V_Is_Sys_Admin = (SELECT Is_System_Admin
FROM ZGWSecurity.Accounts
WHERE UPPER(Account) = UPPER(@P_Account))
	IF @V_Is_Sys_Admin = 0
		BEGIN
	INSERT INTO @T_Valic_Se
			SELECT -- Security Entitys via roles
			ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId
		FROM
			ZGWSecurity.Accounts,
			ZGWSecurity.Roles_Security_Entities_Accounts,
			ZGWSecurity.Roles_Security_Entities,
			ZGWSecurity.Roles
		WHERE
					ZGWSecurity.Accounts.Account = @P_Account
			AND ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId
			AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
			AND ZGWSecurity.Roles.Is_System_Only = 0
	UNION
		SELECT -- Security Entitys via groups
			ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId
		FROM
			ZGWSecurity.Accounts,
			ZGWSecurity.Groups_Security_Entities_Accounts,
			ZGWSecurity.Groups_Security_Entities,
			ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
			ZGWSecurity.Roles_Security_Entities,
			ZGWSecurity.Roles
		WHERE
					ZGWSecurity.Accounts.Account = @P_Account
			AND ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId
			AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
	IF @P_Is_Se_Admin = 0 -- FALSE
					BEGIN
		SELECT
			SecurityEntitySeqId AS SecurityEntityID,
			[Name],
			[Description]
		FROM
			ZGWSecurity.Security_Entities
		WHERE
							ZGWSecurity.Security_Entities.SecurityEntitySeqId IN (SELECT *
			FROM @T_Valic_Se)
			AND ZGWSecurity.Security_Entities.StatusSeqId = @V_Active_Status
		ORDER BY
							[Name]
	END
				ELSE
					BEGIN
		SELECT
			SecurityEntitySeqId AS SecurityEntityID,
			[Name],
			[Description]
		FROM
			ZGWSecurity.Security_Entities
		WHERE
							ZGWSecurity.Security_Entities.SecurityEntitySeqId IN (SELECT *
			FROM @T_Valic_Se)
			OR ZGWSecurity.Security_Entities.ParentSecurityEntitySeqId = @PSecurityEntitySeqId
		ORDER BY
							[Name]
	END
-- END IF
END
	ELSE
		BEGIN
	SELECT
		SecurityEntitySeqId AS SecurityEntityID,
		[Name],
		[Description]
	FROM
		ZGWSecurity.Security_Entities
	ORDER BY
				[Name]
END
	--END IF

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Account]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Account]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Account] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_AccountSeqId int = -1,
		@P_StatusSeqId int = 1,
		@P_Account VARCHAR(128) = 'test',
		@P_First_Name VARCHAR(15) = 'test',
		@P_Last_Name VARCHAR(15) = 'test',
		@P_Middle_Name VARCHAR(15) = 'test',
		@P_Preferred_Name VARCHAR(50) = 'test',
		@P_Email VARCHAR(128) = 'test@test.com',
		@P_Password VARCHAR(256) = 'test',
		@P_Password_Last_Set datetime = GETDATE(),
		@P_Failed_Attempts int = 0,
		@P_Added_Updated_By int = 1,
		@P_Last_Login datetime = GETDATE(),
		@P_Time_Zone int = -5,
		@P_Location VARCHAR(50) = 'desk',
		@P_Enable_Notifications int = 0,
		@P_Is_System_Admin int = 0,
		@P_Debug INT = 1
--Insert new
	exec ZGWSecurity.Set_Account 
		@P_AccountSeqId,
		@P_StatusSeqId,
		@P_Account,
		@P_First_Name,
		@P_Last_Name,
		@P_Middle_Name,
		@P_Preferred_Name,
		@P_Email,
		@P_Password,
		@P_Password_Last_Set,
		@P_Failed_Attempts,
		@P_Added_Updated_By,
		@P_Last_Login,
		@P_Time_Zone,
		@P_Location,
		@P_Enable_Notifications,
		@P_Is_System_Admin,
		@P_Debug
--Update
	SET @P_AccountSeqId = (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = 'test')
	exec ZGWSecurity.Set_Account
		@P_AccountSeqId,
		@P_StatusSeqId,
		@P_Account,
		@P_First_Name,
		@P_Last_Name,
		@P_Middle_Name,
		@P_Preferred_Name,
		@P_Email,
		@P_Password,
		@P_Password_Last_Set,
		@P_Failed_Attempts,
		@P_Added_Updated_By,
		@P_Last_Login,
		@P_Time_Zone,
		@P_Location,
		@P_Enable_Notifications,
		@P_Is_System_Admin,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSystem].[Account]
--	@P_StatusSeqId's value determines insert/update
--	a value of -1 is insert > -1 performs update
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Account]
	@P_AccountSeqId int output,
	@P_StatusSeqId int,
	@P_Account VARCHAR(128),
	@P_First_Name VARCHAR(35),
	@P_Last_Name VARCHAR(35),
	@P_Middle_Name VARCHAR(35),
	@P_Preferred_Name VARCHAR(50),
	@P_Email VARCHAR(128),
	@P_Password VARCHAR(256),
	@P_Password_Last_Set datetime,
	@P_Failed_Attempts int,
	@P_Added_Updated_By int,
	@P_Last_Login datetime,
	@P_Time_Zone int,
	@P_Location VARCHAR(50),
	@P_Enable_Notifications int,
	@P_Is_System_Admin int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start Set_Account'
	DECLARE @VSecurityEntitySeqId VARCHAR(1),
		@V_SecurityEntityName VARCHAR(50),
		@V_BackColor VARCHAR(15),
		@V_LeftColor VARCHAR(15),
		@V_HeadColor VARCHAR(15),
		@V_HeaderForeColor VARCHAR(15),
		@V_SubHeadColor VARCHAR(15),
		@V_RowBackColor VARCHAR(15),
		@V_AlternatingRowBackColor VARCHAR(15),
		@V_ColorScheme VARCHAR(15),
		@V_FavoriteAction VARCHAR(25),
		@V_recordsPerPage VARCHAR(1000),
		@V_Default_Account VARCHAR(50),
		@V_Now DATETIME = GETDATE()
	
	
	IF @P_AccountSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	IF @P_Debug = 1 PRINT 'UPDATE [ZGWSecurity].[Accounts]'
	UPDATE [ZGWSecurity].[Accounts]
			SET 
				StatusSeqId = @P_StatusSeqId,
				Account = @P_Account,
				First_Name = @P_First_Name,
				Last_Name = @P_Last_Name,
				Middle_Name = @P_Middle_Name,
				Preferred_Name = @P_Preferred_Name,
				Email = @P_Email,
				Password_Last_Set = @P_Password_Last_Set,
				[Password] = @P_Password,
				Failed_Attempts = @P_Failed_Attempts,
				Last_Login = @P_Last_Login,
				Time_Zone = @P_Time_Zone,
				Location = @P_Location,
				Is_System_Admin = @P_Is_System_Admin,
				Enable_Notifications = @P_Enable_Notifications,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				AccountSeqId = @P_AccountSeqId

END
	ELSE
		BEGIN
	-- INSERT a new row in the table.
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'INSERT [ZGWSecurity].[Accounts]'
	INSERT [ZGWSecurity].[Accounts]
		(
		StatusSeqId,
		Account,
		First_Name,
		Last_Name,
		Middle_Name,
		Preferred_Name,
		Email,
		Password_Last_Set,
		[Password],
		FAILED_ATTEMPTS,
		IS_SYSTEM_ADMIN,
		Added_By,
		Added_Date,
		LAST_LOGIN,
		TIME_ZONE,
		Location,
		Enable_Notifications
		)
	VALUES
		(
			@P_StatusSeqId,
			@P_Account,
			@P_First_Name,
			@P_Last_Name,
			@P_Middle_Name,
			@P_Preferred_Name,
			@P_Email,
			@P_Password_Last_Set,
			@P_Password,
			@P_Failed_Attempts,
			@P_Is_System_Admin,
			@P_Added_Updated_By,
			@V_Now,
			@P_Last_Login,
			@P_Time_Zone,
			@P_Location,
			@P_Enable_Notifications
			)
	SET @P_AccountSeqId = SCOPE_IDENTITY()
	IF EXISTS (SELECT 1
	FROM [ZGWSecurity].[Accounts]
	WHERE AccountSeqId = @P_AccountSeqId)

			exec ZGWSecurity.Set_Account_Roles
				@P_Account,
				1,
				'Authenticated',
				@P_Added_Updated_By,
				@P_Debug

	BEGIN
		/*add an entry to account choice table*/
		IF  EXISTS (SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_NAME = 'Account_Choices' AND TABLE_SCHEMA = 'ZGWCoreWeb')		
					BEGIN
			SELECT @V_Default_Account=Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = @P_Added_Updated_By

			IF @V_Default_Account = NULL SET @V_Default_Account = 'ANONYMOUS'

			IF EXISTS (SELECT 1
			FROM [ZGWCoreWeb].Account_Choices
			WHERE Account = @V_Default_Account)
							BEGIN
				-- Populate values from Account_Choices from the Anonymous account
				IF @P_Debug = 1 PRINT 'Populating default values from the database for account ' + CONVERT(VARCHAR(MAX),@V_Default_Account)
				SELECT -- FILL THE DEFAULT VALUES
					@VSecurityEntitySeqId = SecurityEntityID,
					@V_SecurityEntityName = SecurityEntityName,
					@V_BackColor = BackColor,
					@V_LeftColor = LeftColor,
					@V_HeadColor = HeadColor,
					@V_HeaderForeColor = HeaderForeColor,
					@V_SubHeadColor = SubHeadColor,
					@V_RowBackColor = RowBackColor,
					@V_AlternatingRowBackColor = AlternatingRowBackColor,
					@V_ColorScheme = ColorScheme,
					@V_FavoriteAction = FavoriteAction,
					@V_recordsPerPage = recordsPerPage
				FROM
					[ZGWCoreWeb].Account_Choices
				WHERE 
									Account = @V_Default_Account
			END
						ELSE
							BEGIN
				IF @P_Debug = 1 PRINT 'Populating default values minimum values'
				SET @VSecurityEntitySeqId = (SELECT MIN(SecurityEntitySeqId)
				FROM ZGWSecurity.Security_Entities)
				SET @V_SecurityEntityName = (SELECT [Name]
				FROM ZGWSecurity.Security_Entities
				WHERE SecurityEntitySeqId = @VSecurityEntitySeqId)
				IF @VSecurityEntitySeqId = NULL SET @VSecurityEntitySeqId = 1
				IF @V_SecurityEntityName = NULL SET @V_SecurityEntityName = 'System'
			END
			--END IF
			IF @P_Debug = 1 PRINT 'Executing ZGWCoreWeb.Set_Account_Choices'
			EXEC ZGWCoreWeb.Set_Account_Choices
							@P_Account,
							@VSecurityEntitySeqId,
							@V_SecurityEntityName,
							@V_BackColor,
							@V_LeftColor,
							@V_HeadColor,
							@V_HeaderForeColor,
							@V_SubHeadColor,
							@V_RowBackColor,
							@V_AlternatingRowBackColor,
							@V_ColorScheme ,
							@V_FavoriteAction,
							@V_recordsPerPage
		END
	--END IF
	END
END-- Get the Error Code for the statement just executed.
	IF @P_Debug = 1 PRINT '@P_AccountSeqId = '
	IF @P_Debug = 1 PRINT @P_AccountSeqId
/* -- GOING BACK TO USING AN OUTPUT PARAMETER.
	SELECT
		AccountSeqId
		, Account
		, Email
		, Enable_Notifications
		, Is_System_Admin
		, StatusSeqId
		, Password_Last_Set
		, Password
		, Failed_Attempts
		, First_Name
		, Last_Login
		, Last_Name
		, Location
		, Middle_Name
		, Preferred_Name
		, Time_Zone
		, Added_By
		, Added_Date
		, Updated_By
		, Updated_Date
	FROM 
		[ZGWSecurity].[Accounts] 
	WHERE 
		AccountSeqId = @P_AccountSeqId
*/
	IF @P_Debug = 1 PRINT 'End Set_Account'
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Account_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Account_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Account_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Groups VARCHAR(max) = 'Everyone',
		@P_Added_Updated_By INT = 2,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Account_Groups
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Groups,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Set's the Groups associated
--	with an account.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Account_Groups]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Groups VARCHAR(max),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Account_Groups'
	DECLARE @V_ErrorCode INT
	DECLARE @V_ErrorMsg VARCHAR(MAX)

	BEGIN TRAN
		DECLARE @AccountSeqId INT
		SET @AccountSeqId = (SELECT AccountSeqId
FROM ZGWSecurity.Accounts
WHERE Account = @P_Account)
		-- Deleting old records before inseting any new ones.
		IF @P_Debug = 1 PRINT 'Calling ZGWSecurity.Delete_Account_Groups'
		EXEC ZGWSecurity.Delete_Account_Groups @AccountSeqId, @PSecurityEntitySeqId, @P_Debug
		IF @@ERROR <> 0
			BEGIN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Account_Groups' + CHAR(10)
	RAISERROR(@V_ErrorMsg,16,1)
	GOTO ABEND
END
		--END IF
		DECLARE @V_GroupSeqId AS 	INT
		DECLARE @V_SecurityEntity_GroupSeqID AS 	INT
		DECLARE @V_Group_Name AS	VARCHAR(50)
		DECLARE @V_Pos AS	INT
		SET @P_Groups = LTRIM(RTRIM(@P_Groups))+ ','
		SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
		IF REPLACE(@P_Groups, ',', '') <> ''
			WHILE @V_Pos > 0
			BEGIN
	SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
	IF @V_Group_Name <> ''
				BEGIN
		--select the role seq id first
		SELECT @V_GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		FROM ZGWSecurity.Groups
		WHERE [Name]=@V_Group_Name

		SELECT
			@V_SecurityEntity_GroupSeqID=GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities
		WHERE
						GroupSeqId = @V_GroupSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId
		IF @P_Debug = 1 PRINT ('@V_SecurityEntity_GroupSeqID = ' + CONVERT(VARCHAR,@V_SecurityEntity_GroupSeqID))
		IF NOT EXISTS(
							SELECT
			GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities_Accounts
		WHERE 
							AccountSeqId = @AccountSeqId
			AND GroupsSecurityEntitiesSeqId = @V_SecurityEntity_GroupSeqID
					)
					BEGIN TRY
						IF @P_Debug = 1 PRINT 'Inserting records'
						INSERT ZGWSecurity.Groups_Security_Entities_Accounts
			(
			AccountSeqId,
			GroupsSecurityEntitiesSeqId,
			Added_By
			)
		VALUES
			(
				@AccountSeqId,
				@V_SecurityEntity_GroupSeqID,
				@P_Added_Updated_By
						)
					END TRY
					BEGIN CATCH
						GOTO ABEND
					END CATCH
	END
	SET @P_Groups = RIGHT(@P_Groups, LEN(@P_Groups) - @V_Pos)
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
END
		IF @@ERROR <> 0 GOTO ABEND
	COMMIT TRAN
	GOTO DONE
ABEND:
	BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Account_Groups' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
DONE:
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Account_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Account_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Account_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Roles VARCHAR(max) = 'Developer',
		@P_Added_Updated_By INT = 2,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Account_Roles
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Roles,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/24/2011
-- Description:	Set's the roles associated
--	with an account.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Account_Roles]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Roles VARCHAR(max),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Account_Roles'
	DECLARE @V_ErrorCode INT
	DECLARE @V_ErrorMsg VARCHAR(MAX)

	BEGIN TRAN
		DECLARE @AccountSeqId INT
		SET @AccountSeqId = (SELECT AccountSeqId
FROM ZGWSecurity.Accounts
WHERE Account = @P_Account)
		-- Deleting old records before inseting any new ones.
		IF @P_Debug = 1 PRINT 'Calling ZGWSecurity.Delete_Account_Roles'
		EXEC ZGWSecurity.Delete_Account_Roles @AccountSeqId, @PSecurityEntitySeqId, @V_ErrorCode, @P_Debug
		IF @V_ErrorCode <> 0
			BEGIN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Account_Roles' + CHAR(10)
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
		--END IF
		DECLARE @V_RoleSeqId AS 	INT
		DECLARE @V_SE_RLS_SECURITY_ID AS 	INT
		DECLARE @V_Role_Name AS	VARCHAR(50)
		DECLARE @V_Pos AS	INT
		SET @P_Roles = LTRIM(RTRIM(@P_Roles))+ ','
		SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
		IF REPLACE(@P_Roles, ',', '') <> ''
			WHILE @V_Pos > 0
			BEGIN
	SET @V_Role_Name = LTRIM(RTRIM(LEFT(@P_Roles, @V_Pos - 1)))
	IF @V_Role_Name <> ''
				BEGIN
		--select the role seq id first
		SELECT @V_RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		FROM ZGWSecurity.Roles
		WHERE [Name]=@V_ROLE_NAME

		SELECT
			@V_SE_RLS_SECURITY_ID=RolesSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Roles_Security_Entities
		WHERE
						RoleSeqId = @V_RoleSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId
		IF @P_Debug = 1 PRINT ('@V_SE_RLS_SECURITY_ID = ' + CONVERT(VARCHAR,@V_SE_RLS_SECURITY_ID))
		IF NOT EXISTS(
							SELECT
			RolesSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Roles_Security_Entities_Accounts
		WHERE 
							AccountSeqId = @AccountSeqId
			AND RolesSecurityEntitiesSeqId = @V_SE_RLS_SECURITY_ID
					)
					BEGIN TRY
						IF @P_Debug = 1 PRINT 'Inserting records'
						INSERT ZGWSecurity.Roles_Security_Entities_Accounts
			(
			AccountSeqId,
			RolesSecurityEntitiesSeqId,
			Added_By
			)
		VALUES
			(
				@AccountSeqId,
				@V_SE_RLS_SECURITY_ID,
				@P_Added_Updated_By
						)
					END TRY
					BEGIN CATCH
						GOTO ABEND
					END CATCH
	END
	SET @P_Roles = RIGHT(@P_Roles, LEN(@P_Roles) - @V_Pos)
	SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
END
		IF @@ERROR <> 0 GOTO ABEND
	COMMIT TRAN
	RETURN 0
ABEND:
	IF @@ERROR <> 0
		BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Account_Roles' + CHAR(10)
	IF @P_Debug = 1 PRINT @V_ErrorMsg
	--RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
	--END IF
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Function]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Function]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Function] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = -1,
		@P_Name VARCHAR(30) = 'Testing',
		@P_Description VARCHAR(512) = 'Testing',
		@P_FunctionTypeSeqId INT = 1,
		@P_Source VARCHAR(512) = '',
		@P_Controller VARCHAR(512) = '',
		@P_Enable_View_State int = 0,
		@P_Enable_Notifications int = 0,
		@P_Redirect_On_Timeout int = 0,
		@P_Is_Nav int = 0,
		@P_Link_Behavior int 0,
		@P_NO_UI int = 0,
		@P_NAV_TYPE_ID int = 1,
		@P_Action VARCHAR(256) = 'testing',
		@P_Meta_Key_Words VARCHAR(512) = '',
		@P_ParentSeqId int = 1,
		@P_Notes VARCHAR(512) = '',
		@P_Added_Updated_By INT = 1
		@P_Debug INT = 0

	exec ZGWSecurity.Set_Function
		@P_FunctionSeqId,
		@P_Name,
		@P_Description,
		@P_FunctionTypeSeqId,
		@P_Source,
		@P_Controller,
		@P_Enable_View_State,
		@P_Enable_Notifications,
		@P_Redirect_On_Timeout,
		@P_Is_Nav,
		@P_Link_Behavior,
		@P_NO_UI,
		@P_NAV_TYPE_ID,
		@P_Action,
		@P_Meta_Key_Words,
		@P_ParentSeqId,
		@P_Notes,
		@P_Added_Updated_By
		@P_Debug
		
	PRINT 'Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_FunctionSeqId)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWSecurity.Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function]
	@P_FunctionSeqId int OUTPUT,
	@P_Name VARCHAR(30),
	@P_Description VARCHAR(512),
	@P_FunctionTypeSeqId INT,
	@P_Source VARCHAR(512),
	@P_Controller VARCHAR(512) = NULL,
	@P_Resolve VARCHAR(MAX) = NULL,
	@P_Enable_View_State int,
	@P_Enable_Notifications int,
	@P_Redirect_On_Timeout int,
	@P_Is_Nav int,
	@P_Link_Behavior int,
	@P_NO_UI int,
	@P_NAV_TYPE_ID int,
	@P_Action VARCHAR(256),
	@P_Meta_Key_Words VARCHAR(512),
	@P_ParentSeqId int,
	@P_Notes VARCHAR(512),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Function'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_FunctionSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE ZGWSecurity.Functions
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				FunctionTypeSeqId = @P_FunctionTypeSeqId,
				[Source] = @P_Source,
				[Controller] = @P_Controller,
				[Resolve] = @P_Resolve,
				Enable_View_State = @P_Enable_View_State,
				Enable_Notifications = @P_Enable_Notifications,
				Redirect_On_Timeout = @P_Redirect_On_Timeout,
				Is_Nav = @P_Is_Nav,
				Link_Behavior = @P_Link_Behavior,
				No_UI = @P_NO_UI,
				Navigation_Types_NVP_DetailSeqId = @P_NAV_TYPE_ID,
				Meta_Key_Words = @P_Meta_Key_Words,
				ParentSeqId = @P_ParentSeqId,
				Notes = @P_Notes,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				FunctionSeqId = @P_FunctionSeqId
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Inserting new row'
	IF EXISTS( SELECT [Action]
	FROM ZGWSecurity.Functions
	WHERE [Action] = @P_Action
			)
			BEGIN
		RAISERROR ('THE FUNCTION YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
		RETURN
	END
	INSERT ZGWSecurity.Functions
		(
		[Name],
		[Description],
		FunctionTypeSeqId,
		[Source],
		[Controller],
		[Resolve],
		Enable_View_State,
		Enable_Notifications,
		Redirect_On_Timeout,
		Is_Nav,
		Link_Behavior,
		NO_UI,
		Navigation_Types_NVP_DetailSeqId,
		Meta_Key_Words,
		[Action],
		ParentSeqId,
		Notes,
		Sort_Order,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_Name,
			@P_Description,
			@P_FunctionTypeSeqId,
			@P_Source,
			@P_Controller,
			@P_Resolve,
			@P_Enable_View_State,
			@P_Enable_Notifications,
			@P_Redirect_On_Timeout,
			@P_Is_Nav,
			@P_Link_Behavior,
			@P_NO_UI,
			@P_NAV_TYPE_ID,
			@P_Meta_Key_Words,
			@P_Action,
			@P_ParentSeqId,
			@P_Notes,
			0,
			@P_Added_Updated_By,
			@V_Now
			)
	SELECT @P_FunctionSeqId=SCOPE_IDENTITY()
	-- Get the IDENTITY value for the row just inserted.
	DECLARE @V_Sort_Order INT
	SET @V_Sort_Order = (SELECT MAX(Sort_Order)
	FROM ZGWSecurity.Functions
	WHERE ParentSeqId = @P_ParentSeqId) + 1
	UPDATE ZGWSecurity.Functions SET Sort_Order = ISNULL(@V_Sort_Order,0) WHERE FunctionSeqId = @P_FunctionSeqId

END
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Function'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Function_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Function_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Function_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Groups VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Groups
		@P_FunctionSeqId,
		@PSecurityEntitySeqId,
		@P_Groups,
		@P_PermissionsNVPDetailSeqId,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/26/2011
-- Description:	Delete and inserts into ZGWSecurity.Groups_Security_Entities_Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function_Groups]
	@P_FunctionSeqId int,
	@PSecurityEntitySeqId INT,
	@P_Groups VARCHAR(MAX),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
BEGIN TRANSACTION
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Groups_Security_Entities_Functions'
	-- NEED TO DELETE EXISTING Group ASSOCITAED WITH THE FUNCTION BEFORE 
	-- INSERTING NEW ONES.
	
	DECLARE @V_ErrorCodde INT
			,@V_GroupSeqId INT
			,@V_GroupsSecurityEntitiesSeqId AS INT
			,@V_Group_Name VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
			,@V_Now DATETIME = GETDATE()

	EXEC ZGWSecurity.Delete_Function_Groups @P_FunctionSeqId,@PSecurityEntitySeqId,@P_PermissionsNVPDetailSeqId,@P_Added_Updated_By,@V_ErrorCodde
	IF @@ERROR <> 0
	BEGIN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Function_Groups' + CHAR(10)
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
	SET @P_Groups = LTRIM(RTRIM(@P_Groups))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
	IF LEN(REPLACE(@P_Groups, ',', '')) > 0
		WHILE @V_Pos > 0
			BEGIN
	-- go through all the Groups and add if necessary
	SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
	IF @V_Group_Name <> ''
				BEGIN
		--select the Group seq id first
		SELECT
			@V_GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		FROM
			ZGWSecurity.Groups
		WHERE 
						[Name]=@V_Group_Name

		--select the GroupsSecurityEntitiesSeqId
		SELECT
			@V_GroupsSecurityEntitiesSeqId=GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities
		WHERE
						GroupSeqId = @V_GroupSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId

		IF @P_Debug = 1 PRINT('@V_GroupsSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_GroupsSecurityEntitiesSeqId))
		IF NOT EXISTS(
							SELECT
			GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities_Functions
		WHERE 
							FunctionSeqId = @P_FunctionSeqId
			AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
			AND GroupsSecurityEntitiesSeqId = @V_GroupsSecurityEntitiesSeqId)
						BEGIN TRY-- INSERT RECORD
							INSERT ZGWSecurity.Groups_Security_Entities_Functions
			(
			FunctionSeqId,
			GroupsSecurityEntitiesSeqId,
			PermissionsNVPDetailSeqId,
			Added_By,
			Added_Date
			)
		VALUES
			(
				@P_FunctionSeqId,
				@V_GroupsSecurityEntitiesSeqId,
				@P_PermissionsNVPDetailSeqId,
				@P_Added_Updated_By,
				@V_Now
							)
						END TRY
						BEGIN CATCH
							GOTO ABEND
						END CATCH
	--END IF
	END
	SET @P_Groups = RIGHT(@P_Groups, LEN(@P_Groups) - @V_Pos)
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
END
		--END WHILE
	IF @@error <> 0 GOTO ABEND
Commit Transaction
RETURN 0
ABEND:
	IF @@error <> 0
		BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Account_Roles' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
	--END IF
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Function_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Function_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Function_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Roles VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Roles
		@P_FunctionSeqId,
		@PSecurityEntitySeqId,
		@P_Roles,
		@P_PermissionsNVPDetailSeqId,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/01/2011
-- Description:	Delete and inserts into ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function_Roles]
	@P_FunctionSeqId int,
	@PSecurityEntitySeqId INT,
	@P_Roles VARCHAR(MAX),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
BEGIN TRANSACTION
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Function_Roles'
	-- NEED TO DELETE EXISTING ROLE ASSOCITAED WITH THE FUNCTION BEFORE 
	-- INSERTING NEW ONES.
	
	DECLARE @V_ErrorCodde INT
			,@V_RoleSeqId AS INT
			,@V_RolesSecurityEntitiesSeqId AS INT
			,@V_Role_Name AS VARCHAR(50)
			,@V_Pos AS INT
			,@V_ErrorMsg VARCHAR(MAX)
	EXEC ZGWSecurity.Delete_Function_Roles @P_FunctionSeqId,@PSecurityEntitySeqId,@P_PermissionsNVPDetailSeqId,@P_Added_Updated_By,@V_ErrorCodde
	IF @@ERROR <> 0
		BEGIN
	GOTO ABEND
END
	--END IF	

	SET @P_Roles = LTRIM(RTRIM(@P_Roles))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
	IF LEN(REPLACE(@P_Roles, ',', '')) > 0
		WHILE @V_Pos > 0
			BEGIN
	-- go through all the roles and add if necessary
	SET @V_Role_Name = LTRIM(RTRIM(LEFT(@P_Roles, @V_Pos - 1)))
	IF @V_Role_Name <> ''
				BEGIN
		--select the role seq id first
		SELECT
			@V_RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		FROM
			ZGWSecurity.Roles
		WHERE 
						[Name]=@V_Role_Name

		--select the RolesSecurityEntitiesSeqId
		SELECT
			@V_RolesSecurityEntitiesSeqId=RolesSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Roles_Security_Entities
		WHERE
						RoleSeqId = @V_RoleSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId

		IF @P_Debug = 1 PRINT('@V_RolesSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_RolesSecurityEntitiesSeqId))
		IF NOT EXISTS(
							SELECT
			RolesSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Roles_Security_Entities_Functions
		WHERE 
							FunctionSeqId = @P_FunctionSeqId
			AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
			AND RolesSecurityEntitiesSeqId = @V_RolesSecurityEntitiesSeqId)
						BEGIN TRY
							IF @P_Debug = 1 PRINT 'Insert new record'
							INSERT ZGWSecurity.Roles_Security_Entities_Functions
			(
			FunctionSeqId,
			RolesSecurityEntitiesSeqId,
			PermissionsNVPDetailSeqId,
			Added_By
			)
		VALUES
			(
				@P_FunctionSeqId,
				@V_RolesSecurityEntitiesSeqId,
				@P_PermissionsNVPDetailSeqId,
				@P_Added_Updated_By
							)
						END TRY
						BEGIN CATCH
							GOTO ABEND
						END CATCH
	--END IF
	END
	SET @P_Roles = RIGHT(@P_Roles, LEN(@P_Roles) - @V_Pos)
	SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
END
		--END WHILE
	IF @@error <> 0 GOTO ABEND
Commit Transaction
RETURN 0
ABEND:
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Function_Roles' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Function_Sort]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Function_Sort]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Function_Sort] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Direction INT = 1,
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Sort
		@P_FunctionSeqId,
		@P_Direction,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Updates ZGWSecurity.Functions Sort_Order column
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Direction INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @V_Current_Sort_Order int
			,@V_Sort_Order_Move int
			,@V_ParentSeqId INT
			,@V_Updated_Date DATETIME = GETDATE()
	-- Get the parent ID so only the menu items here can be effected
	SET @V_ParentSeqId = (SELECT ParentSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	-- Get Current Sort Order
	SELECT
	@V_Current_Sort_Order = Sort_Order
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId
	
	-- Get Sort Order for Section Above
	IF @P_Direction = 0 -- Down
		BEGIN
	SELECT @V_Sort_Order_Move = MIN( Sort_Order )
	FROM ZGWSecurity.Functions
	WHERE Sort_Order > @V_Current_Sort_Order
END
	ELSE -- up
		BEGIN
	SELECT @V_Sort_Order_Move = MAX( Sort_Order )
	FROM ZGWSecurity.Functions
	WHERE Sort_Order < @V_Current_Sort_Order
END
	-- END IF
	-- If no row to move, exit
	IF @V_Sort_Order_Move IS NULL
		return
	
	-- Otherwise, switch sort orders
	UPDATE ZGWSecurity.Functions SET
	  Sort_Order = @V_Current_Sort_Order
	  WHERE Sort_Order = @V_Sort_Order_Move
	
	UPDATE ZGWSecurity.Functions SET
	  Sort_Order = @V_Sort_Order_Move,
	  Updated_By = @P_Added_Updated_By,
	  Updated_Date = @V_Updated_Date
	  WHERE FunctionSeqId = @P_FunctionSeqId

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Function_Types]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Function_Types]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Function_Types] AS'
END
GO

/*
Usage:
	DECLARE 
			@P_FunctionTypeSeqId INT = -1,
			@P_Name VARCHAR(50) = 'Name',
			@P_Description VARCHAR(512) = 'Description',
			@P_Template VARCHAR(512) = null,
			@P_Is_Content INT = 0,
			@P_Added_Updated_BY	INT = 2,
			@P_Primary_Key INT = NULL,
			@P_ErrorCode INT = NULL
--Insert new
	exec [ZGWSecurity].[Set_Function_Types]
			@P_FunctionTypeSeqId,
			@P_Name,
			@P_Description,
			@P_Template,
			@P_Is_Content,
			@P_Added_Updated_BY,
			@P_Primary_Key,
			@P_ErrorCode
--Update
	SET @P_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM [ZGWSecurity].[Function_Types] WHERE [Name] = @P_Name)
	exec [ZGWSecurity].[Set_Function_Types]
			@P_FunctionTypeSeqId,
			@P_Name,
			@P_Description,
			@P_Template,
			@P_Is_Content,
			@P_Added_Updated_BY,
			@P_Primary_Key,
			@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSecurity].[Function_Types]
--	Given the FunctionTypeSeqId
--	a value of -1 is insert > -1 performs update
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function_Types]
	@P_FunctionTypeSeqId INT,
	@P_Name VARCHAR(50),
	@P_Description VARCHAR(512),
	@P_Template VARCHAR(512),
	@P_Is_Content INT,
	@P_Added_Updated_BY	INT,
	@P_Primary_Key int OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [Set_Function_Types]'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_FunctionTypeSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	IF @P_Debug = 1 PRINT 'Updating ZGWSecurity.Function_Types'
	UPDATE ZGWSecurity.Function_Types
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Template = @P_Template,
				Is_Content = @P_Is_Content,
				Updated_By = @P_Added_Updated_BY,
				Updated_Date =@V_Now
			WHERE
				FunctionTypeSeqId = @P_FunctionTypeSeqId

	SELECT @P_Primary_Key = @P_FunctionTypeSeqId
END
	ELSE
	BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS( SELECT @P_Description
	FROM [ZGWSecurity].[Function_Types]
	WHERE [Name] = @P_Name
			)
			BEGIN
		RAISERROR ('THE FUNCTION TYPE YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
		RETURN
	END
	IF @P_Debug = 1 PRINT 'Inserting record into ZGWSecurity.Function_Types'
	INSERT ZGWSecurity.Function_Types
		(
		[Name],
		[Description],
		Template,
		Is_Content,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_Name,
			@P_Description,
			@P_Template,
			@P_Is_Content,
			@P_Added_Updated_BY,
			@V_Now
			)
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
	-- Get the Error Code for the statement just executed.
	SELECT @P_ErrorCode=@@ERROR
	IF @P_Debug = 1 PRINT 'End [Set_Function_Types]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Group]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Group]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Group] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_GroupSeqId INT = 1,
		@P_Name VARCHAR(128) = 'Test',
		@P_Description VARCHAR(512) = ' ',
		@PSecurityEntitySeqId INT = 1,
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Group
		@P_GroupSeqId,
		@P_Name,
		@P_Description,
		@PSecurityEntitySeqId,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
	PRINT 'Primary key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWSecurity.Groups
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Group]
	@P_GroupSeqId INT,
	@P_Name VARCHAR(128),
	@P_Description VARCHAR(512),
	@PSecurityEntitySeqId INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @RLS_SEQ_ID INT
			,@V_Added_Updated_Date DATETIME = GETDATE()

	IF @P_GroupSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE ZGWSecurity.Groups
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Added_Updated_Date
			WHERE
				GroupSeqId = @P_GroupSeqId

	SELECT @P_Primary_Key = @P_GroupSeqId
END
	ELSE
		BEGIN
	-- INSERT a new row in the table.
	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF NOT EXISTS( SELECT [Name]
	FROM ZGWSecurity.Groups
	WHERE [Name] = @P_Name)
				BEGIN
		INSERT ZGWSecurity.Groups
			(
			[Name],
			[Description],
			Added_By,
			Added_Date
			)
		VALUES
			(
				@P_Name,
				@P_Description,
				@P_Added_Updated_By,
				@V_Added_Updated_Date
					)
		SELECT @P_Primary_Key=SCOPE_IDENTITY()
	-- Get the IDENTITY value for the row just inserted.
	END
			ELSE
				--PRINT 'ENTERING SECURITY INFORMATION FOR THE GROUP'
				SET @P_Primary_Key = (SELECT GroupSeqId
	FROM ZGWSecurity.Groups
	WHERE [Name] = @P_Name)
-- END IF
END
	-- END IF
	IF(SELECT COUNT(*)
FROM ZGWSecurity.Groups_Security_Entities
WHERE SecurityEntitySeqId = @PSecurityEntitySeqId AND GroupSeqId = @P_Primary_Key) = 0 
	BEGIN
	-- ADD GROUP REFERENCE TO SE_SECURITY
	INSERT ZGWSecurity.Groups_Security_Entities
		(
		SecurityEntitySeqId,
		GroupSeqId,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@PSecurityEntitySeqId,
			@P_Primary_Key,
			@P_Added_Updated_By,
			@V_Added_Updated_Date
			)
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Group_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Group_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Group_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_GroupSeqId INT = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Roles VARCHAR(MAX) = 'Anonymous, Authenticated',
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Group_Roles
		@P_GroupSeqId,
		@PSecurityEntitySeqId,
		@P_Roles,
		@P_Added_Updated_By,
		@P_Debug

*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Deletes and inserts ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Group_Roles]
	@P_GroupSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Roles VARCHAR(MAX),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Group_Roles'
	DECLARE @V_RolesSecurityEntitiesSeqId INT
			,@V_Role_Name VARCHAR(50)
			,@V_Is_System INT
			,@V_POS INT
			,@V_GroupsSecurityEntitiesSeqId INT
			,@V_Now DATETIME = GETDATE()
		--NEED TO DELETE EXISTING Roles ASSOCITAED BEFORE 
		-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
		-- IS MOVED FROM CODE			
		EXEC ZGWSecurity.Delete_Group_Roles @P_GroupSeqId,@PSecurityEntitySeqId, @P_Debug	
		SET @P_Roles = LTRIM(RTRIM(@P_Roles))+ ','
		SET @V_POS = CHARINDEX(',', @P_Roles, 1)
	
		IF REPLACE(@P_Roles, ',', '') <> ''
		BEGIN
	WHILE @V_POS > 0
			BEGIN
		SET @V_Role_Name = LTRIM(RTRIM(LEFT(@P_Roles, @V_POS - 1)))
		IF @V_Role_Name <> ''
				IF @P_Debug = 1 PRINT @V_Role_Name
		-- DEBUG
		BEGIN
			--SELECT THE RoleSeqId FROM THE Roles
			--TABLE FOR ALL THE Roles PASSED
			SELECT
				@V_RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
			FROM
				ZGWSecurity.Roles_Security_Entities
			WHERE 
						ZGWSecurity.Roles_Security_Entities.RoleSeqId = (SELECT RoleSeqId
				FROM ZGWSecurity.Roles
				WHERE ZGWSecurity.Roles.[Name] = @V_Role_Name)
				AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
			IF @P_Debug = 1 PRINT @V_RolesSecurityEntitiesSeqId

			SELECT
				@V_GroupsSecurityEntitiesSeqId = GroupsSecurityEntitiesSeqId
			FROM
				ZGWSecurity.Groups_Security_Entities
			WHERE
						SecurityEntitySeqId = @PSecurityEntitySeqId
				AND GroupSeqId = @P_GroupSeqId

			IF @P_Debug = 1 PRINT @V_GroupsSecurityEntitiesSeqId
			-- DEBUG
			/*
					INSERT THE ZGWSecurity.Groups_Security_Entities_Roles_Entities
					WITH Roles INFORMATION
					*/
			IF @V_RolesSecurityEntitiesSeqId IS NOT NULL
					BEGIN

				INSERT ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
					(
					GroupsSecurityEntitiesSeqId,
					RolesSecurityEntitiesSeqId,
					Added_By,
					Added_Date
					)
				VALUES(
						@V_GroupsSecurityEntitiesSeqId,
						@V_RolesSecurityEntitiesSeqId,
						@P_Added_Updated_By,
						@V_Now
						)

				IF @P_Debug = 1 PRINT 'Inserted into ZGWSecurity.Groups_Security_Entities_Roles_Entities'
			END

		END
		SET @P_Roles = RIGHT(@P_Roles, LEN(@P_Roles) - @V_POS)
		SET @V_POS = CHARINDEX(',', @P_Roles, 1)

	END
END	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Group_Roles'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Name_Value_Pair_Groups]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Name_Value_Pair_Groups]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Groups] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Groups VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug int = 1

	exec ZGWSecurity.Set_Name_Value_Pair_Groups
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_Groups,
		@P_PermissionsNVPDetailSeqId,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/26/2011
-- Description:	Delete and inserts into ZGWSecurity.Groups_Security_Entities_Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Groups]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Groups VARCHAR(1000),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT('Starting ZGWSecurity.Set_Name_Value_Pair_Groups')
BEGIN TRAN
	DECLARE @V_GroupSeqId INT
			,@V_GroupsSecurityEntitiesSeqId INT
			,@V_GROUP_NAME VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
	
	IF @P_Debug = 1 PRINT 'Deleting existing Groups associated with the name value pair before inseting new ones.'
	EXEC ZGWSystem.Delete_Groups_Security_Entities_Permissions @P_NVPSeqId,@PSecurityEntitySeqId,@P_PermissionsNVPDetailSeqId, @P_Debug
	IF @@ERROR <> 0
		BEGIN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Groups_Security_Entities_Permissions' + CHAR(10)
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
	--END IF	
	SET @P_Groups = LTRIM(RTRIM(@P_Groups))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
	IF REPLACE(@P_Groups, ',', '') <> ''
		WHILE @V_Pos > 0
		BEGIN
	SET @V_GROUP_NAME = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
	IF @V_GROUP_NAME <> ''
			BEGIN
		IF @P_Debug = 1 PRINT 'select the GROUP seq id first'
		SELECT @V_GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		FROM ZGWSecurity.Groups
		WHERE [Name]=@V_GROUP_NAME

		SELECT
			@V_GroupsSecurityEntitiesSeqId=GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities
		WHERE
					GroupSeqId = @V_GroupSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId
		IF @P_Debug = 1 PRINT('@V_GroupsSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_GroupsSecurityEntitiesSeqId))
		IF NOT EXISTS(
						SELECT
			GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities_Permissions
		WHERE 
						NVPSeqId = @P_NVPSeqId
			AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
			AND GroupsSecurityEntitiesSeqId = @V_GroupsSecurityEntitiesSeqId
				)
				BEGIN TRY
					IF @P_Debug = 1 PRINT('Inserting record')
					INSERT ZGWSecurity.Groups_Security_Entities_Permissions
			(
			NVPSeqId,
			GroupsSecurityEntitiesSeqId,
			PermissionsNVPDetailSeqId,
			Added_By
			)
		VALUES
			(
				@P_NVPSeqId,
				@V_GroupsSecurityEntitiesSeqId,
				@P_PermissionsNVPDetailSeqId,
				@P_Added_Updated_By
					)
				END TRY
				BEGIN CATCH
					GOTO ABEND
				END CATCH
	END
	SET @P_Groups = RIGHT(@P_Groups, LEN(@P_Groups) - @V_Pos)
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
END
	--END IF
IF @@ERROR = 0
	BEGIN
	COMMIT TRAN
	IF @P_Debug = 1 PRINT('Ending ZGWSecurity.Set_Name_Value_Pair_Groups')
	RETURN 0
END
ABEND:
BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Name_Value_Pair_Groups' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Name_Value_Pair_Roles]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Name_Value_Pair_Roles]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Roles] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Role VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug int = 1

	exec ZGWSecurity.Set_Name_Value_Pair_Roles
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_Role,
		@P_PermissionsNVPDetailSeqId,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Delete and inserts into ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Roles]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Role VARCHAR(1000),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT('Starting ZGWSecurity.Set_Name_Value_Pair_Role')
BEGIN TRAN
	DECLARE @V_RoleSeqId INT
			,@V_RolesSecurityEntitiesSeqId INT
			,@V_Group_Name VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
			,@V_Now DATETIME = GETDATE()
	
	IF @P_Debug = 1 PRINT 'Deleting existing Role associated with the name value pair before inseting new ones.'
	EXEC ZGWSystem.Delete_Roles_Security_Entities_Permissions @P_NVPSeqId,@PSecurityEntitySeqId,@P_PermissionsNVPDetailSeqId, @P_Debug
	IF @@ERROR <> 0
		BEGIN
	GOTO ABEND
END
	--END IF	
	SET @P_Role = LTRIM(RTRIM(@P_Role))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Role, 1)
	IF REPLACE(@P_Role, ',', '') <> ''
		WHILE @V_Pos > 0
		BEGIN
	SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Role, @V_Pos - 1)))
	IF @V_Group_Name <> ''
			BEGIN
		IF @P_Debug = 1 PRINT 'select the RoleSeqId first'
		SELECT @V_RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		FROM ZGWSecurity.Roles
		WHERE [Name]=@V_Group_Name

		SELECT
			@V_RolesSecurityEntitiesSeqId=RolesSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Roles_Security_Entities
		WHERE
					RoleSeqId = @V_RoleSeqId AND
			SecurityEntitySeqId = @PSecurityEntitySeqId
		IF @P_Debug = 1 PRINT('@V_RolesSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_RolesSecurityEntitiesSeqId))
		IF NOT EXISTS(
						SELECT
			RolesSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Roles_Security_Entities_Permissions
		WHERE 
						NVPSeqId = @P_NVPSeqId
			AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
			AND RolesSecurityEntitiesSeqId = @V_RolesSecurityEntitiesSeqId
				)
				BEGIN TRY
					IF @P_Debug = 1 PRINT('Inserting record')
					INSERT ZGWSecurity.Roles_Security_Entities_Permissions
			(
			NVPSeqId,
			RolesSecurityEntitiesSeqId,
			PermissionsNVPDetailSeqId,
			Added_By,
			Added_Date
			)
		VALUES
			(
				@P_NVPSeqId,
				@V_RolesSecurityEntitiesSeqId,
				@P_PermissionsNVPDetailSeqId,
				@P_Added_Updated_By,
				@V_Now
					)
				END TRY
				BEGIN CATCH
					GOTO ABEND
				END CATCH
	END
	SET @P_Role = RIGHT(@P_Role, LEN(@P_Role) - @V_Pos)
	SET @V_Pos = CHARINDEX(',', @P_Role, 1)
END
	--END IF
IF @@ERROR = 0
	BEGIN
	COMMIT TRAN
	IF @P_Debug = 1 PRINT('Ending ZGWSecurity.Set_Name_Value_Pair_Role')
	RETURN 0
END
ABEND:
BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Name_Value_Pair_Role' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Role]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Role]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Role] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_RoleSeqId INT = -1,
		@P_Name VARCHAR(50) = 'Test',
		@P_Description VARCHAR(128) = 'Testing',
		@P_Is_System INT = 0,
		@P_Is_System_Only INT = 0,
		@PSecurityEntitySeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Role
		@P_RoleSeqId,
		@P_Name,
		@P_Description,
		@P_Is_System,
		@P_Is_System_Only,
		@PSecurityEntitySeqId,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug
		
	PRINT '@P_Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_Primary_Key)
	
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Inserts or updates ZGWSecurity.Roles and
--	ZGWSecurity.Roles_Security_Entities
-- Note: @P_RoleSeqId value of -1 inserts a new record
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Role]
	@P_RoleSeqId INT,
	@P_Name VARCHAR(50),
	@P_Description VARCHAR(128),
	@P_Is_System INT,
	@P_Is_System_Only INT,
	@PSecurityEntitySeqId INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Roles'
BEGIN TRAN
	DECLARE @V_RLS_SEQ_ID INT
			,@V_Message AS VARCHAR(128)
			,@V_Now DATETIME = GETDATE()
			,@V_ErrorMsg VARCHAR(MAX)

	IF (SELECT COUNT(*)
FROM ZGWSecurity.Roles
WHERE Is_System_Only = 1 AND [Name] = @P_Name) > 0
	BEGIN
	SET @V_Message = 'The role you entered ' + @P_Name + ' is for system use only.'
	RAISERROR (@V_Message,16,1)
	RETURN
END

	IF @P_RoleSeqId > -1
		BEGIN
	IF @P_Debug = 1 PRINT 'Updating role in ZGWSecurity.Roles'
	UPDATE ZGWSecurity.Roles
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Is_System = @P_Is_System,
				Is_System_Only = @P_Is_System_Only,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				RoleSeqId = @P_RoleSeqId

	SELECT @P_Primary_Key = @P_RoleSeqId
END
	ELSE
		BEGIN TRY -- INSERT a new row in the table.
			-- CHECK FOR DUPLICATE Name BEFORE INSERTING
			IF NOT EXISTS( SELECT [Name]
FROM ZGWSecurity.Roles
WHERE [Name] = @P_Name)
				BEGIN
	IF @P_Debug = 1 PRINT 'Add role to ZGWSecurity.Roles'
	INSERT ZGWSecurity.Roles
		(
		[Name],
		[Description],
		Is_System,
		Is_System_Only,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_Name,
			@P_Description,
			@P_Is_System,
			@P_Is_System_Only,
			@P_Added_Updated_By,
			@V_Now
					)
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
			ELSE
				SET @P_Primary_Key = (SELECT RoleSeqId
FROM ZGWSecurity.Roles
WHERE [Name] = @P_Name)
			-- END IF
		END TRY
		BEGIN CATCH
			GOTO ABEND		
		END CATCH
	-- END IF
	IF(SELECT COUNT(*)
FROM ZGWSecurity.Roles_Security_Entities
WHERE SecurityEntitySeqId = @PSecurityEntitySeqId AND RoleSeqId = @P_Primary_Key) = 0 
	BEGIN TRY  -- ADD ROLE REFERENCE TO SE_SECURITY
			IF @P_Debug = 1 PRINT 'Add role reference to ZGWSecurity.Roles_Security_Entities'
			INSERT ZGWSecurity.Roles_Security_Entities
	(
	SecurityEntitySeqId
	, RoleSeqId
	, Added_By
	, Added_Date
	)
VALUES
	(
		@PSecurityEntitySeqId,
		@P_Primary_Key,
		@P_Added_Updated_By,
		@V_Now
			)
	END TRY
	BEGIN CATCH
		GOTO ABEND	
	END CATCH
COMMIT TRAN
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Roles'
RETURN 0

ABEND:
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Role' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Roles'
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Role_Accounts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Role_Accounts]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Role_Accounts] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_RoleSeqId INT = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Role_Accounts
		@P_RoleSeqId,
		@PSecurityEntitySeqId,
		@P_Account,
		@P_Added_Updated_By,
		@P_Debug

*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Inserts into ZGWSecurity.Roles_Security_Entities_Accounts
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Role_Accounts]
	@P_RoleSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Account VARCHAR(128),
	@P_Added_Updated_By INT,
	@P_Debug INT = 1
AS
IF @P_Debug = 1	PRINT 'Starting ZGWSecurity.Set_Role_Accounts'
DECLARE @V_AccountSeqId AS INT
		,@V_RolesSecurityEntitiesSeqId AS INT
		,@V_ErrorMsg VARCHAR(MAX)
BEGIN TRAN
	SET NOCOUNT OFF;
	SET @V_AccountSeqId = (SELECT ZGWSecurity.Accounts.AccountSeqId
FROM ZGWSecurity.Accounts
WHERE Account = @P_Account)
	SET @V_RolesSecurityEntitiesSeqId = (
			SELECT
	RolesSecurityEntitiesSeqId
FROM
	ZGWSecurity.Roles_Security_Entities
WHERE
				RoleSeqId = @P_RoleSeqId
	AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	BEGIN TRY
		INSERT INTO
			ZGWSecurity.Roles_Security_Entities_Accounts
	(RolesSecurityEntitiesSeqId,AccountSeqId,Added_By)
VALUES(
		@V_RolesSecurityEntitiesSeqId,
		@V_AccountSeqId,
		@P_Added_Updated_By
		)
	END TRY
	BEGIN CATCH
		GOTO ABEND
	END CATCH
COMMIT TRAN
IF @P_Debug = 1	PRINT 'Ending ZGWSecurity.Set_Role_Accounts'
RETURN 0
ABEND:
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Role' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Roles'
GO
/****** Object:  StoredProcedure [ZGWSecurity].[Set_Security_Entity]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Security_Entity]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Security_Entity] AS'
END
GO

/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId int = -1,
		@P_Name VARCHAR(256) = 'System',
		@P_Description VARCHAR(512) = 'System',
		@P_URL VARCHAR(128) = '',
		@P_StatusSeqId int = 1,
		@P_DAL VARCHAR(50) = '',
		@P_DAL_Name VARCHAR(50) = '',
		@P_DAL_Name_SPACE VARCHAR(256) = '',
		@P_DAL_String VARCHAR(512) = '',
		@P_Skin char(25) = '',
		@P_Style VARCHAR(25) = '',
		@P_Encryption_Type INT = 1,
		@P_ParentSecurityEntitySeqId int = 1,
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Security_Entity
		@PSecurityEntitySeqId,
		@P_Name,
		@P_Description,
		@P_URL,
		@P_StatusSeqId,
		@P_DAL,
		@P_DAL_Name,
		@P_DAL_Name_SPACE,
		@P_DAL_String,
		@P_Skin,
		@P_Style,
		@P_Encryption_Type,
		@P_ParentSecurityEntitySeqId,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug

	PRINT 'Primay key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWSecurity.Security_Entities
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Security_Entity]
	@PSecurityEntitySeqId int,
	@P_Name VARCHAR(256),
	@P_Description VARCHAR(512),
	@P_URL VARCHAR(128),
	@P_StatusSeqId int,
	@P_DAL VARCHAR(50),
	@P_DAL_Name VARCHAR(50),
	@P_DAL_Name_SPACE VARCHAR(256),
	@P_DAL_String VARCHAR(512),
	@P_Skin char(25),
	@P_Style VARCHAR(25),
	@P_Encryption_Type INT,
	@P_ParentSecurityEntitySeqId int,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @V_Now DATETIME = GETDATE()
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Security_Entity'
	IF @P_ParentSecurityEntitySeqId = @PSecurityEntitySeqId or @P_ParentSecurityEntitySeqId = -1 SET @P_ParentSecurityEntitySeqId = NULL
	IF @PSecurityEntitySeqId > -1
		BEGIN
	IF @P_Debug = 1 PRINT 'Update'
	UPDATE ZGWSecurity.Security_Entities
			SET 
				Name = @P_Name,
				[Description] = @P_Description,
				URL = @P_URL,
				StatusSeqId = @P_StatusSeqId,
				DAL = @P_DAL,
				DAL_Name = @P_DAL_Name,
				DAL_Name_Space = @P_DAL_Name_SPACE,
				DAL_String = @P_DAL_String,
				Skin = @P_Skin,
				Style = @P_Style,
				Encryption_Type = @P_Encryption_Type,
				ParentSecurityEntitySeqId = @P_ParentSecurityEntitySeqId,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				SecurityEntitySeqId = @PSecurityEntitySeqId

	SELECT @P_Primary_Key = @PSecurityEntitySeqId
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Insert'
	-- INSERT a new row in the table.
	INSERT ZGWSecurity.Security_Entities
		(
		[Name],
		[Description],
		[URL],
		StatusSeqId,
		DAL,
		DAL_Name,
		DAL_Name_SPACE,
		DAL_STRING,
		Skin,
		Style,
		Encryption_Type,
		ParentSecurityEntitySeqId,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_Name,
			@P_Description,
			@P_URL,
			@P_StatusSeqId,
			@P_DAL,
			@P_DAL_Name,
			@P_DAL_Name_SPACE,
			@P_DAL_String,
			@P_Skin,
			@P_Style,
			@P_Encryption_Type,
			@P_ParentSecurityEntitySeqId,
			@P_Added_Updated_By,
			@V_Now
			)
	-- Get the IDENTITY value for the row just inserted.
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
END
-- End if
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Security_Entity'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Add_Data_Files]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Add_Data_Files]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Add_Data_Files] AS'
END
GO
/*
Usage:
	EXEC [ZGWSystem].[Add_Data_Files] 
		@P_DB_Name = 'tempdb'
		, @P_Debug = 1
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 2017/05/03
-- Description:	Adds X number of data files to
--	a database given the database name.  
--	Based on number of cores:
--		< 8 1 per core
--		between 8 & 32 inclusive then 1/2 the number of cores
--		> 32 then the number of cores / 4
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Add_Data_Files]
	@P_DB_Name SYSNAME -- Name of the database
	,
	@P_Debug BIT = 0
AS
BEGIN
	SET NOCOUNT ON

	IF @P_Debug = 1 PRINT '-- Instance name: ' + @@servername + ' ;
/* 
	Version: ' + @@version + '*/'

	-- Variables

	DECLARE @V_BITS BIGINT						-- Affinty Mask
			, @V_NUMPROCS SMALLINT				-- Number of cores addressed by instance
			, @V_DB_File_Count Int				-- Number of exisiting datafiles
			, @V_DB_Location NVARCHAR(4000)		-- Location of DB primary datafile
			, @V_File_LogicalName NVARCHAR(4000)-- The logical file name
			, @V_FileGrowthType NVARCHAR(400)	-- The type of file growth MB or %
			, @V_FileGrowth NVARCHAR(400)		-- The amount the file grows at
			, @V_Count INT						-- Counter
			, @V_SQLStatement NVARCHAR(max)		-- Dynamic SQL holder
			, @V_NewFile_Size_MB INT			-- Size of the new files,in Megabytes
			, @V_NewFile_Growth_MB INT			-- New files growth rate,in Megabytes
			, @V_NewFileLocation NVARCHAR(4000) -- New files path
			, @V_OS NVARCHAR(256)				-- The OS Type for us Windows or Linux (Ubuntu reported as)
			, @V_DBFileName VARCHAR(MAX);
	-- Used to remove the database filename if needed "Azmuth.mdf"

	-- Initialize variables

	SELECT @V_Count = 1, @V_BITS = 1
			, @V_OS = (SELECT host_platform
		FROM sys.dm_os_host_info)
			, @V_DBFileName = @P_DB_Name + '.mdf';
	SELECT
		@V_NewFile_Size_MB = 4096		-- Four Gbytes, it's easy to increase that after file creation but harder to shrink.
		, @V_NewFile_Growth_MB = 512	-- 512 Mbytes, can be easily shrunk
		, @V_NewFileLocation = NULL;
	-- NULL means create in same location as primary file.

	IF OBJECT_ID('tempdb..#SVer') IS NOT NULL
		BEGIN
		DROP TABLE #SVer;
	END
	--END IF

	CREATE TABLE #SVer
	(
		[ID] INT,
		[Name] sysname,
		[Internal_Value] INT,
		[Value] NVARCHAR(512)
	);

	INSERT #SVer
	EXEC master.dbo.xp_msver ProcessorCount;

	-- Get total number of Cores detected by the Operating system

	SELECT @V_NUMPROCS = Internal_Value
	FROM #SVer;
	IF @P_Debug = 1 PRINT '-- TOTAL numbers of CPU cores on server: ' + cast(@V_NUMPROCS as varchar(5));
	SET @V_NUMPROCS  = 0;

	-- Get number of Cores addressed by instance.

	WHILE @V_Count <= (SELECT Internal_Value
		FROM #SVer ) AND @V_Count <=32
		BEGIN
		SELECT @V_NUMPROCS =
				CASE 
					WHEN  CAST (VALUE AS INT) & @V_BITS > 0 THEN @V_NUMPROCS + 1 
					ELSE @V_NUMPROCS 
				END
		FROM sys.configurations
		WHERE NAME = 'AFFINITY MASK'
		SET  @V_BITS = (@V_BITS * 2)
		SET @V_Count = @V_Count + 1
	END
	--END WHILE

	IF (SELECT Internal_Value
	FROM #SVer) > 32
		BEGIN
		WHILE @V_Count <= (SELECT Internal_Value
		FROM #SVer )
				BEGIN
			SELECT @V_NUMPROCS = 
						CASE 
							WHEN  CAST (VALUE AS INT) & @V_BITS > 0 THEN @V_NUMPROCS + 1 
							ELSE @V_NUMPROCS 
						END
			FROM sys.configurations
			WHERE [NAME] = 'AFFINITY64 MASK';
			SET  @V_BITS = (@V_BITS * 2);
			SET @V_Count = @V_Count + 1;
		END
	--END WHILE
	END
	--END IF

	If @V_NUMPROCS = 0 SELECT @V_NUMPROCS=  Internal_Value
	FROM #SVer;

	IF @P_Debug = 1 PRINT '-- Number of CPU cores Configured for usage by instance: ' + cast(@V_NUMPROCS as varchar(5));

	-------------------------------------------------------------------------------------
	-- Here you define how many files should exist per core ; Feel free to change
	-------------------------------------------------------------------------------------

	-- IF cores < 8 then no change , IF between 8 & 32 inclusive then 1/2 of cores number
	IF @V_NUMPROCS >8 AND @V_NUMPROCS <=32
		SELECT @V_NUMPROCS = @V_NUMPROCS /2;
	--END IF

	-- IF cores > 32 then files should be 1/4 of cores number
	If @V_NUMPROCS >32
		SELECT @V_NUMPROCS = @V_NUMPROCS /4;
	--END IF
	Create Table ##temp
	(
		DatabaseName sysname,
		LogicalName sysname,
		Location nvarchar(500),
		SizeMB decimal (18,2),
		Growth INT
	)
	SET @V_SQLStatement = '
Use [' + @P_DB_Name + '];
Insert Into ##temp (DatabaseName, LogicalName, Location, SizeMB, Growth)
    SELECT 
		  [DatabaseName] = DB_NAME()
		, [LogicalName] = [name]
		, [Location] = REPLACE(physical_name, DB_NAME() + ''.mdf'', '''')
		, [SizeMB] = Cast(Cast(Round(cast(size as decimal) * 8.0/1024.0,2) as decimal(18,2)) as nvarchar)
		, [Growth] = Growth
    FROM sys.database_files WHERE [type]=0
'
	EXEC sp_executesql @V_SQLStatement;
	SELECT TOP(1)
		@V_DB_Location = [Location]
	, @V_NewFile_Size_MB = [SizeMB]
	, @V_File_LogicalName = [LogicalName]
	, @V_FileGrowth = [Growth]
	FROM ##temp
	-- IF CHARINDEX('Linux',@V_OS) > 0 --&& CHARINDEX('/',@V_DB_Location) <> 1
	-- 	BEGIN
	-- 		SET @V_DB_Location = '/' + @V_DB_Location;
	-- 	END
	-- --END IF
	IF CHARINDEX(@V_DBFileName,@V_DB_Location) > 0
	BEGIN
		SET @V_DB_Location = REPLACE(@V_DB_Location, @V_DBFileName, '');
	End
	--END IF
	IF @P_Debug = 1 PRINT '@V_DB_Location IS:';
	IF @P_Debug = 1 PRINT @V_DB_Location;
	SELECT @V_DB_File_Count=COUNT(*)
	FROM ##temp;
	DROP TABLE ##temp

	SELECT TOP(1)
		@V_FileGrowthType =  
		CASE mf.is_percent_growth
			WHEN 1 THEN '%'
			WHEN 0 THEN ' MB'
		END
	FROM sys.master_files mf
	WHERE DB_NAME(mf.database_id) = @P_DB_Name and [type]=0;

	IF @P_Debug = 1 PRINT '-- Current Number of ' + @P_DB_Name + ' datafiles: ' + cast(@V_DB_File_Count as varchar(5));

	-- Determine IF we already have enough datafiles
	If @V_DB_File_Count >= @V_NUMPROCS
		BEGIN
		PRINT '--****Number of Recommedned datafiles is already exists****';
		RETURN;
	End
	--END IF

	SET @V_NewFileLocation= Isnull(@V_NewFileLocation,@V_DB_Location);
	PRINT @V_NewFileLocation;

	-- Determine IF the new location exists or not
	DECLARE @file_results table(file_exists int,
		file_is_a_directory int,
		parent_directory_exists int);

	INSERT INTO @file_results
		(file_exists, file_is_a_directory, parent_directory_exists)
	EXEC [master].[dbo].xp_fileexist @V_NewFileLocation;
	SELECT *
	FROM @file_results;
	IF (SELECT file_is_a_directory
	FROM @file_results ) = 0
		BEGIN
		PRINT '-- New files Directory Does NOT exist , please specify a correct folder!';
		RETURN;
	END
	--END IF

	-- Determine IF we have enough free space on the destination drive

	DECLARE @FreeSpace Table (Drive char(1),
		MB_Free BIGINT)
	INSERT INTO @FreeSpace
	exec master..xp_fixeddrives

	IF (SELECT MB_Free
	FROM @FreeSpace
	WHERE drive = LEFT(@V_NewFileLocation,1) ) < @V_NUMPROCS * @V_NewFile_Size_MB
		BEGIN
		PRINT '-- WARNING: Not enough free space on ' + Upper(LEFT(@V_NewFileLocation,1)) + ':\ to accomodate the new files. Around '+ cast(@V_NUMPROCS * @V_NewFile_Size_MB as varchar(10))+ ' Mbytes are needed; Please add more space or choose a new location!';
		RETURN;
	END
	--END IF

	-- Determine IF any of the exisiting datafiles have different size than proposed ones.
	If exists
	(
		SELECT
		(CONVERT (BIGINT, size) * 8)/1024
	FROM tempdb.sys.database_files
	WHERE 
			type_desc= 'Rows'
		AND (CONVERT (BIGINT, size) * 8)/1024  <> @V_NewFile_Size_MB
	)

	IF @P_Debug = 1 PRINT
	'
/*
WARNING: Some Existing datafile(s) do NOT have the same size as new ones.
It''s recommended IF ALL datafiles have same size for optimal proportional-fill performance.Use ALTER DATABASE AND DBCC SHRINKFILE to resize files
 
Optimizing ' + @P_DB_Name + ' Performance : http://msdn.microsoft.com/en-us/library/ms175527.aspx
	'

	IF @P_Debug = 1 PRINT '****Proposed New ' + @P_DB_Name + ' Datafiles, PLEASE REVIEW CODE BEFORE RUNNIG  *****/
	------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 
	'
	-- Generate the statements
	WHILE @V_DB_File_Count < @V_NUMPROCS
		BEGIN
		SELECT @V_SQLStatement = 
'ALTER DATABASE [' + @P_DB_Name + '] ADD FILE (NAME = N'''+ @V_File_LogicalName + '_' + CAST(@V_DB_File_Count +1 AS VARCHAR (5))+''',FILENAME = N'''+ @V_NewFileLocation + @P_DB_Name + '_' + CAST (@V_DB_File_Count +1 AS VARCHAR(5)) +'.ndf'',SIZE = ' + CAST(@V_NewFile_Size_MB AS VARCHAR(15)) + 'MB,FILEGROWTH = ' + CAST(@V_FileGrowth AS VARCHAR(15)) + @V_FileGrowthType +' )';
		IF @P_Debug = 1 
				PRINT @V_SQLStatement;
			ELSE
				EXEC sp_executesql @V_SQLStatement;
		--END IF
		SET @V_DB_File_Count += 1;
	END
--END WHILE
END
GO
/****** Object:  StoredProcedure [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Delete_Groups_Security_Entities_Permissions]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Delete_Groups_Security_Entities_Permissions] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVP_DetailSeqId INT = 4,
		@P_NVPSeqId int = 1,
		@P_Debug INT = 0

	exec [ZGWSystem].[Delete_Name_Value_Pair_Group]
		@P_NVP_DetailSeqId,
		@P_NVPSeqId,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a records from ZGWSecurity.Groups_Security_Entities_Permissions
--	given the NVPSeqId, SecurityEntitySeqId, and PermissionsNVPDetailSeqId
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_PermissionsNVPDetailSeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Permissions
	WHERE 
		GroupsSecurityEntitiesSeqId IN(SELECT GroupsSecurityEntitiesSeqId
	FROM ZGWSecurity.Groups_Security_Entities
	WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
	AND NVPSeqId = @P_NVPSeqId
	AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
	IF @P_Debug = 1 PRINT 'End [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Delete_Name_Value_Pair]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Delete_Name_Value_Pair]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVPSeqId int = 3,
		@PSecurityEntitySeqId	INT = 1,
		@P_ErrorCode int

	exec ZGWSystem.Delete_Name_Value_Pair
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSystem.Name_Value_Pairs
--	given the NVPSeqId
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId	INT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Name_Value_Pair'
	DELETE FROM ZGWSystem.Name_Value_Pairs
	WHERE 
		NVPSeqId = @P_NVPSeqId
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Name_Value_Pair'
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Delete_Name_Value_Pair_Detail]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Delete_Name_Value_Pair_Detail]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair_Detail] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVP_DetailSeqId INT = 4,
		@P_NVPSeqId int = 1,
		@P_Debug INT = 0

	exec ZGWSystem.Delete_Name_Value_Pair_Detail
		@P_NVP_DetailSeqId,
		@P_NVPSeqId,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a records from xx where xx is the static_name column
--	from ZGWSystem.Name_Value_Pairs given the NVP_DetailSeqId and NVPSeqId
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair_Detail]
	@P_NVP_DetailSeqId INT,
	@P_NVPSeqId int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Name_Value_Pair'
	DECLARE @V_Statement NVARCHAR(4000),
			@V_Static_Name VARCHAR(30)

	SET @V_Static_Name = (SELECT Static_Name
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)

	SET @V_Statement= 'DELETE 
		   FROM ' + CONVERT(VARCHAR,@V_Static_Name) + '
		   WHERE NVP_DetailSeqId= ''' + CONVERT(VARCHAR,@P_NVP_DetailSeqId) + ''''
	EXECUTE sp_executesql @V_Statement
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Name_Value_Pair'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Delete_Roles_Security_Entities_Permissions]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Delete_Roles_Security_Entities_Permissions]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Delete_Roles_Security_Entities_Permissions] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVP_DetailSeqId INT = 4,
		@P_NVPSeqId int = 1,
		@P_Debug INT = 0

	exec [ZGWSystem].[Delete_Name_Value_Pair_Group]
		@P_NVP_DetailSeqId,
		@P_NVPSeqId,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a records from ZGWSecurity.Roles_Security_Entities_Permissions
--	given the NVPSeqId, SecurityEntitySeqId, and PermissionsNVPDetailSeqId
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Delete_Roles_Security_Entities_Permissions]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_PermissionsNVPDetailSeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSystem.Delete_Roles_Security_Entities_Permissions'
	DELETE FROM 
		ZGWSecurity.Roles_Security_Entities_Permissions
	WHERE 
		RolesSecurityEntitiesSeqId IN(SELECT RolesSecurityEntitiesSeqId
	FROM ZGWSecurity.Roles_Security_Entities
	WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
	AND NVPSeqId = @P_NVPSeqId
	AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
	IF @P_Debug = 1 PRINT 'Ending ZGWSystem.Delete_Roles_Security_Entities_Permissions'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[GenerateInserts]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[GenerateInserts]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[GenerateInserts] AS'
END
GO

/***********************************************************************************************************
Procedure:	ZGWSystem.GenerateInserts  (Build 22) 
		(Copyright � 2002 Narayana Vyas Kondreddi. All rights reserved.)
                                          
Purpose:	To generate INSERT statements from existing data. 
		These INSERTS can be executed to regenerate the data at some other location.
		This procedure is also useful to create a database setup, where in you can 
		script your data along with your table definitions.

Written by:	Narayana Vyas Kondreddi
	        http://vyaskn.tripod.com

Acknowledgements:
		Divya Kalra	-- For beta testing
		Mark Charsley	-- For reporting a problem with scripting uniqueidentifier columns with NULL values
		Artur Zeygman	-- For helping me simplify a bit of code for handling non-dbo owned tables
		Joris Laperre   -- For reporting a regression bug in handling text/ntext columns

Tested on: 	SQL Server 7.0 and SQL Server 2000 and SQL Server 2005

Date created:	January 17th 2001 21:52 GMT

Date modified:	May 1st 2002 19:50 GMT

Email: 		vyaskn@hotmail.com

NOTE:		This procedure may not work with tables with too many columns.
		Results can be unpredictable with huge text columns or SQL Server 2000's sql_variant data types
		Whenever possible, Use @include_column_list parameter to ommit column list in the INSERT statement, for better results
		IMPORTANT: This procedure is not tested with internation data (Extended characters or Unicode). If needed
		you might want to convert the datatypes of character variables in this procedure to their respective unicode counterparts
		like nchar and nvarchar

		ALSO NOTE THAT THIS PROCEDURE IS NOT UPDATED TO WORK WITH NEW DATA TYPES INTRODUCED IN SQL SERVER 2005 / YUKON
		

Example 1:	To generate INSERT statements for table 'titles':
		
		EXEC ZGWSystem.GenerateInserts 'titles'

Example 2: 	To ommit the column list in the INSERT statement: (Column list is included by default)
		IMPORTANT: If you have too many columns, you are advised to ommit column list, as shown below,
		to avoid erroneous results
		
		EXEC ZGWSystem.GenerateInserts 'titles', @include_column_list = 0

Example 3:	To generate INSERT statements for 'titlesCopy' table from 'titles' table:

		EXEC ZGWSystem.GenerateInserts 'titles', 'titlesCopy'

Example 4:	To generate INSERT statements for 'titles' table for only those titles 
		which contain the word 'Computer' in them:
		NOTE: Do not complicate the FROM or WHERE clause here. It's assumed that you are good with T-SQL if you are using this parameter

		EXEC ZGWSystem.GenerateInserts 'titles', @from = "from titles where title like '%Computer%'"

Example 5: 	To specify that you want to include TIMESTAMP column's data as well in the INSERT statement:
		(By default TIMESTAMP column's data is not scripted)

		EXEC ZGWSystem.GenerateInserts 'titles', @include_timestamp = 1

Example 6:	To print the debug information:
  
		EXEC ZGWSystem.GenerateInserts 'titles', @debug_mode = 1

Example 7: 	If you are not the owner of the table, use @owner parameter to specify the owner name
		To use this option, you must have SELECT permissions on that table

		EXEC ZGWSystem.GenerateInserts Nickstable, @owner = 'Nick'

Example 8: 	To generate INSERT statements for the rest of the columns excluding images
		When using this otion, DO NOT set @include_column_list parameter to 0.

		EXEC ZGWSystem.GenerateInserts imgtable, @ommit_images = 1

Example 9: 	To generate INSERT statements excluding (ommiting) IDENTITY columns:
		(By default IDENTITY columns are included in the INSERT statement)

		EXEC ZGWSystem.GenerateInserts mytable, @ommit_identity = 1

Example 10: 	To generate INSERT statements for the TOP 10 rows in the table:
		
		EXEC ZGWSystem.GenerateInserts mytable, @top = 10

Example 11: 	To generate INSERT statements with only those columns you want:
		
		EXEC ZGWSystem.GenerateInserts titles, @cols_to_include = "'title','title_id','au_id'"

Example 12: 	To generate INSERT statements by omitting certain columns:
		
		EXEC ZGWSystem.GenerateInserts titles, @cols_to_exclude = "'title','title_id','au_id'"

Example 13:	To avoid checking the foreign key constraints while loading data with INSERT statements:
		
		EXEC ZGWSystem.GenerateInserts titles, @disable_constraints = 1

Example 14: 	To exclude computed columns from the INSERT statement:
		EXEC ZGWSystem.GenerateInserts MyTable, @ommit_computed_cols = 1
***********************************************************************************************************/

ALTER PROCEDURE [ZGWSystem].[GenerateInserts]
	@table_name varchar(776),
	-- The table/view for which the INSERT statements will be generated using the existing data
	@target_table varchar(776) = NULL,
	-- Use this parameter to specify a different table name into which the data will be inserted
	@include_column_list bit = 1,
	-- Use this parameter to include/ommit column list in the generated INSERT statement
	@from varchar(800) = NULL,
	-- Use this parameter to filter the rows based on a filter condition (using WHERE)
	@include_timestamp bit = 0,
	-- Specify 1 for this parameter, if you want to include the TIMESTAMP/ROWVERSION column's data in the INSERT statement
	@debug_mode bit = 0,
	-- If @debug_mode is set to 1, the SQL statements constructed by this procedure will be printed for later examination
	@owner varchar(64) = NULL,
	-- Use this parameter if you are not the owner of the table
	@ommit_images bit = 0,
	-- Use this parameter to generate INSERT statements by omitting the 'image' columns
	@ommit_identity bit = 0,
	-- Use this parameter to ommit the identity columns
	@top int = NULL,
	-- Use this parameter to generate INSERT statements only for the TOP n rows
	@cols_to_include varchar(8000) = NULL,
	-- List of columns to be included in the INSERT statement
	@cols_to_exclude varchar(8000) = NULL,
	-- List of columns to be excluded from the INSERT statement
	@disable_constraints bit = 0,
	-- When 1, disables foreign key constraints and enables them after the INSERT statements
	@ommit_computed_cols bit = 0
-- When 1, computed columns will not be included in the INSERT statement	
AS
BEGIN
	SET NOCOUNT ON

	--Making sure user only uses either @cols_to_include or @cols_to_exclude
	IF ((@cols_to_include IS NOT NULL) AND (@cols_to_exclude IS NOT NULL))
		BEGIN
		RAISERROR('Use either @cols_to_include or @cols_to_exclude. Do not use both the parameters at once',16,1)
		RETURN -1
	--Failure. Reason: Both @cols_to_include and @cols_to_exclude parameters are specified
	END

	--Making sure the @cols_to_include and @cols_to_exclude parameters are receiving values in proper format
	IF ((@cols_to_include IS NOT NULL) AND (PATINDEX('''%''',@cols_to_include) = 0))
		BEGIN
		RAISERROR('Invalid use of @cols_to_include property',16,1)
		PRINT 'Specify column names surrounded by single quotes and separated by commas'
		PRINT 'Eg: EXEC ZGWSystem.GenerateInserts titles, @cols_to_include = "''title_id'',''title''"'
		RETURN -1
	--Failure. Reason: Invalid use of @cols_to_include property
	END

	IF ((@cols_to_exclude IS NOT NULL) AND (PATINDEX('''%''',@cols_to_exclude) = 0))
		BEGIN
		RAISERROR('Invalid use of @cols_to_exclude property',16,1)
		PRINT 'Specify column names surrounded by single quotes and separated by commas'
		PRINT 'Eg: EXEC ZGWSystem.GenerateInserts titles, @cols_to_exclude = "''title_id'',''title''"'
		RETURN -1
	--Failure. Reason: Invalid use of @cols_to_exclude property
	END


	--Checking to see if the database name is specified along wih the table name
	--Your database context should be local to the table for which you want to generate INSERT statements
	--specifying the database name is not allowed
	IF (PARSENAME(@table_name,3)) IS NOT NULL
		BEGIN
		RAISERROR('Do not specify the database name. Be in the required database and just specify the table name.',16,1)
		RETURN -1
	--Failure. Reason: Database name is specified along with the table name, which is not allowed
	END

	--Checking for the existence of 'user table' or 'view'
	--This procedure is not written to work on system tables
	--To script the data in system tables, just create a view on the system tables and script the view instead

	IF @owner IS NULL
		BEGIN
		IF ((OBJECT_ID(@table_name,'U') IS NULL) AND (OBJECT_ID(@table_name,'V') IS NULL)) 
				BEGIN
			RAISERROR('User table or view not found.',16,1)
			PRINT 'You may see this error, if you are not the owner of this table or view. In that case use @owner parameter to specify the owner name.'
			PRINT 'Make sure you have SELECT permission on that table or view.'
			RETURN -1
		--Failure. Reason: There is no user table or view with this name
		END
	END
	ELSE
		BEGIN
		IF NOT EXISTS (SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_NAME = @table_name AND (TABLE_TYPE = 'BASE TABLE' OR TABLE_TYPE = 'VIEW') AND TABLE_SCHEMA = @owner)
				BEGIN
			RAISERROR('User table or view not found.',16,1)
			PRINT 'You may see this error, if you are not the owner of this table. In that case use @owner parameter to specify the owner name.'
			PRINT 'Make sure you have SELECT permission on that table or view.'
			RETURN -1
		--Failure. Reason: There is no user table or view with this name		
		END
	END

	--Variable declarations
	DECLARE		@Column_ID int, 		
			@Column_List varchar(8000), 
			@Column_Name varchar(128), 
			@Start_Insert varchar(786), 
			@Data_Type varchar(128), 
			@Actual_Values varchar(8000),	--This is the string that will be finally executed to generate INSERT statements
			@IDN varchar(128)
	--Will contain the IDENTITY column's name in the table

	--Variable Initialization
	SET @IDN = ''
	SET @Column_ID = 0
	SET @Column_Name = ''
	SET @Column_List = ''
	SET @Actual_Values = ''

	IF @owner IS NULL 
		BEGIN
		SET @Start_Insert = 'INSERT INTO ' + '[' + RTRIM(COALESCE(@target_table,@table_name)) + ']'
	END
	ELSE
		BEGIN
		SET @Start_Insert = 'INSERT ' + '[' + LTRIM(RTRIM(@owner)) + '].' + '[' + RTRIM(COALESCE(@target_table,@table_name)) + ']'
	END


	--To get the first column's ID

	SELECT @Column_ID = MIN(ORDINAL_POSITION)
	FROM INFORMATION_SCHEMA.COLUMNS (NOLOCK)
	WHERE 	TABLE_NAME = @table_name AND
		(@owner IS NULL OR TABLE_SCHEMA = @owner)



	--Loop through all the columns of the table, to get the column names and their data types
	WHILE @Column_ID IS NOT NULL
		BEGIN
		SELECT @Column_Name = QUOTENAME(COLUMN_NAME),
			@Data_Type = DATA_TYPE
		FROM INFORMATION_SCHEMA.COLUMNS (NOLOCK)
		WHERE 	ORDINAL_POSITION = @Column_ID AND
			TABLE_NAME = @table_name AND
			(@owner IS NULL OR TABLE_SCHEMA = @owner)



		IF @cols_to_include IS NOT NULL --Selecting only user specified columns
			BEGIN
			IF CHARINDEX( '''' + SUBSTRING(@Column_Name,2,LEN(@Column_Name)-2) + '''',@cols_to_include) = 0 
				BEGIN
				GOTO SKIP_LOOP
			END
		END

		IF @cols_to_exclude IS NOT NULL --Selecting only user specified columns
			BEGIN
			IF CHARINDEX( '''' + SUBSTRING(@Column_Name,2,LEN(@Column_Name)-2) + '''',@cols_to_exclude) <> 0 
				BEGIN
				GOTO SKIP_LOOP
			END
		END

		--Making sure to output SET IDENTITY_INSERT ON/OFF in case the table has an IDENTITY column
		IF (SELECT COLUMNPROPERTY( OBJECT_ID(QUOTENAME(COALESCE(@owner,USER_NAME())) + '.' + @table_name),SUBSTRING(@Column_Name,2,LEN(@Column_Name) - 2),'IsIdentity')) = 1 
			BEGIN
			IF @ommit_identity = 0 --Determing whether to include or exclude the IDENTITY column
					SET @IDN = @Column_Name
				ELSE
					GOTO SKIP_LOOP
		END

		--Making sure whether to output computed columns or not
		IF @ommit_computed_cols = 1
			BEGIN
			IF (SELECT COLUMNPROPERTY( OBJECT_ID(QUOTENAME(COALESCE(@owner,USER_NAME())) + '.' + @table_name),SUBSTRING(@Column_Name,2,LEN(@Column_Name) - 2),'IsComputed')) = 1 
				BEGIN
				GOTO SKIP_LOOP
			END
		END

		--Tables with columns of IMAGE data type are not supported for obvious reasons
		IF(@Data_Type in ('image'))
				BEGIN
			IF (@ommit_images = 0)
						BEGIN
				RAISERROR('Tables with image columns are not supported.',16,1)
				PRINT 'Use @ommit_images = 1 parameter to generate INSERTs for the rest of the columns.'
				PRINT 'DO NOT ommit Column List in the INSERT statements. If you ommit column list using @include_column_list=0, the generated INSERTs will fail.'
				RETURN -1
			--Failure. Reason: There is a column with image data type
			END
					ELSE
						BEGIN
				GOTO SKIP_LOOP
			END
		END

		--Determining the data type of the column and depending on the data type, the VALUES part of
		--the INSERT statement is generated. Care is taken to handle columns with NULL values. Also
		--making sure, not to lose any data from flot, real, money, smallmomey, datetime columns
		SET @Actual_Values = @Actual_Values  +
			CASE 
				WHEN @Data_Type IN ('char','varchar','nchar','nvarchar') 
					THEN 
						'COALESCE('''''''' + REPLACE(RTRIM(' + @Column_Name + '),'''''''','''''''''''')+'''''''',''NULL'')'
				WHEN @Data_Type IN ('datetime','smalldatetime') 
					THEN 
						'COALESCE('''''''' + RTRIM(CONVERT(char,' + @Column_Name + ',109))+'''''''',''NULL'')'
				WHEN @Data_Type IN ('uniqueidentifier') 
					THEN  
						'COALESCE('''''''' + REPLACE(CONVERT(char(255),RTRIM(' + @Column_Name + ')),'''''''','''''''''''')+'''''''',''NULL'')'
				WHEN @Data_Type IN ('text','ntext') 
					THEN  
						'COALESCE('''''''' + REPLACE(CONVERT(char(8000),' + @Column_Name + '),'''''''','''''''''''')+'''''''',''NULL'')'					
				WHEN @Data_Type IN ('binary','varbinary') 
					THEN  
						'COALESCE(RTRIM(CONVERT(char,' + 'CONVERT(int,' + @Column_Name + '))),''NULL'')'  
				WHEN @Data_Type IN ('timestamp','rowversion') 
					THEN  
						CASE 
							WHEN @include_timestamp = 0 
								THEN 
									'''DEFAULT''' 
								ELSE 
									'COALESCE(RTRIM(CONVERT(char,' + 'CONVERT(int,' + @Column_Name + '))),''NULL'')'  
						END
				WHEN @Data_Type IN ('float','real','money','smallmoney')
					THEN
						'COALESCE(LTRIM(RTRIM(' + 'CONVERT(char, ' +  @Column_Name  + ',2)' + ')),''NULL'')' 
				ELSE 
					'COALESCE(LTRIM(RTRIM(' + 'CONVERT(char, ' +  @Column_Name  + ')' + ')),''NULL'')' 
			END   + '+' +  ''',''' + ' + '

		--Generating the column list for the INSERT statement
		SET @Column_List = @Column_List +  @Column_Name + ','

		SKIP_LOOP:
		--The label used in GOTO

		SELECT @Column_ID = MIN(ORDINAL_POSITION)
		FROM INFORMATION_SCHEMA.COLUMNS (NOLOCK)
		WHERE 	TABLE_NAME = @table_name AND
			ORDINAL_POSITION > @Column_ID AND
			(@owner IS NULL OR TABLE_SCHEMA = @owner)


	--Loop ends here!
	END

	--To get rid of the extra characters that got concatenated during the last run through the loop
	SET @Column_List = LEFT(@Column_List,len(@Column_List) - 1)
	SET @Actual_Values = LEFT(@Actual_Values,len(@Actual_Values) - 6)

	IF LTRIM(@Column_List) = '' 
		BEGIN
		RAISERROR('No columns to select. There should at least be one column to generate the output',16,1)
		RETURN -1
	--Failure. Reason: Looks like all the columns are ommitted using the @cols_to_exclude parameter
	END

	--Forming the final string that will be executed, to output the INSERT statements
	IF (@include_column_list <> 0)
		BEGIN
		SET @Actual_Values = 
				'SELECT ' +  
				CASE WHEN @top IS NULL OR @top < 0 THEN '' ELSE ' TOP ' + LTRIM(STR(@top)) + ' ' END + 
				'''' + RTRIM(@Start_Insert) + 
				' ''+' + '''(' + RTRIM(@Column_List) +  '''+' + ''')''' + 
				' +''VALUES(''+ ' +  @Actual_Values  + '+'')''' + ' ' + 
				COALESCE(@from,' FROM ' + CASE WHEN @owner IS NULL THEN '' ELSE '[' + LTRIM(RTRIM(@owner)) + '].' END + '[' + rtrim(@table_name) + ']' + '(NOLOCK)')
	END
	ELSE IF (@include_column_list = 0)
		BEGIN
		SET @Actual_Values = 
				'SELECT ' + 
				CASE WHEN @top IS NULL OR @top < 0 THEN '' ELSE ' TOP ' + LTRIM(STR(@top)) + ' ' END + 
				'''' + RTRIM(@Start_Insert) + 
				' '' +''VALUES(''+ ' +  @Actual_Values + '+'')''' + ' ' + 
				COALESCE(@from,' FROM ' + CASE WHEN @owner IS NULL THEN '' ELSE '[' + LTRIM(RTRIM(@owner)) + '].' END + '[' + rtrim(@table_name) + ']' + '(NOLOCK)')
	END

	--Determining whether to ouput any debug information
	IF @debug_mode =1
		BEGIN
		PRINT '/*****START OF DEBUG INFORMATION*****'
		PRINT 'Beginning of the INSERT statement:'
		PRINT @Start_Insert
		PRINT ''
		PRINT 'The column list:'
		PRINT @Column_List
		PRINT ''
		PRINT 'The SELECT statement executed to generate the INSERTs'
		PRINT @Actual_Values
		PRINT ''
		PRINT '*****END OF DEBUG INFORMATION*****/'
		PRINT ''
	END

	PRINT '--INSERTs generated by ''ZGWSystem.GenerateInserts'' stored procedure written by Vyas'
	PRINT '--Build number: 22'
	PRINT '--Problems/Suggestions? Contact Vyas @ vyaskn@hotmail.com'
	PRINT '--http://vyaskn.tripod.com'
	PRINT ''
	PRINT 'SET NOCOUNT ON'
	PRINT ''


	--Determining whether to print IDENTITY_INSERT or not
	IF (@IDN <> '')
		BEGIN
		PRINT 'SET IDENTITY_INSERT ' + QUOTENAME(COALESCE(@owner,USER_NAME())) + '.' + QUOTENAME(@table_name) + ' ON'
		PRINT 'GO'
		PRINT ''
	END


	IF @disable_constraints = 1 AND (OBJECT_ID(QUOTENAME(COALESCE(@owner,USER_NAME())) + '.' + @table_name, 'U') IS NOT NULL)
		BEGIN
		IF @owner IS NULL
				BEGIN
			SELECT 'ALTER TABLE ' + QUOTENAME(COALESCE(@target_table, @table_name)) + ' NOCHECK CONSTRAINT ALL' AS '--Code to disable constraints temporarily'
		END
			ELSE
				BEGIN
			SELECT 'ALTER TABLE ' + QUOTENAME(@owner) + '.' + QUOTENAME(COALESCE(@target_table, @table_name)) + ' NOCHECK CONSTRAINT ALL' AS '--Code to disable constraints temporarily'
		END

		PRINT 'GO'
	END

	PRINT ''
	PRINT 'PRINT ''Inserting values into ' + '[' + RTRIM(COALESCE(@target_table,@table_name)) + ']' + ''''


	--All the hard work pays off here!!! You'll get your INSERT statements, when the next line executes!
	EXEC (@Actual_Values)

	PRINT 'PRINT ''Done'''
	PRINT ''


	IF @disable_constraints = 1 AND (OBJECT_ID(QUOTENAME(COALESCE(@owner,USER_NAME())) + '.' + @table_name, 'U') IS NOT NULL)
		BEGIN
		IF @owner IS NULL
				BEGIN
			SELECT 'ALTER TABLE ' + QUOTENAME(COALESCE(@target_table, @table_name)) + ' CHECK CONSTRAINT ALL'  AS '--Code to enable the previously disabled constraints'
		END
			ELSE
				BEGIN
			SELECT 'ALTER TABLE ' + QUOTENAME(@owner) + '.' + QUOTENAME(COALESCE(@target_table, @table_name)) + ' CHECK CONSTRAINT ALL' AS '--Code to enable the previously disabled constraints'
		END

		PRINT 'GO'
	END

	PRINT ''
	IF (@IDN <> '')
		BEGIN
		PRINT 'SET IDENTITY_INSERT ' + QUOTENAME(COALESCE(@owner,USER_NAME())) + '.' + QUOTENAME(@table_name) + ' OFF'
		PRINT 'GO'
	END

	PRINT 'SET NOCOUNT OFF'


	SET NOCOUNT OFF
	RETURN 0
--Success. We are done!
END
GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_Database_Information]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Database_Information]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Database_Information] AS'
END
GO

/*
Usage:
	exec ZGWSystem.Get_Database_Information
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrievs the database information from
--	ZGWSystem.Database_Information
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Database_Information]
AS
	SET NOCOUNT ON
	SELECT TOP 1
	Database_InformationSeqId as Information_SEQ_ID
		, [Version]
		, Enable_Inheritance
		, Added_By
		, Added_Date
		, Updated_By
		, Updated_Date
FROM
	ZGWSystem.Database_Information WITH(NOLOCK)
ORDER BY
		Updated_Date DESC
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_JSON]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_JSON]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_JSON] AS'
END
GO

/*
Usage:
	DECLARE
		@P_SQL VARCHAR(MAX) = 'SELECT * FROM ZGWOptional.Calendars',
		@P_Debug INT = 1

	exec ZGWSystem.Get_JSON
		@P_SQL,
		@P_Debug
*/
-- =============================================
-- Author:		karuta
-- Create date: 05/25/2013
-- Description:	JSON from SQL Statement 
-- Note:
--  Found at
--	http://karuta.wordpress.com/2011/08/31/gerar-json-em-qualquer-clausula-sql/
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_JSON]
	@P_SQL VARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	DECLARE @SQL NVARCHAR(MAX)
	DECLARE @XMLString VARCHAR(MAX)
	DECLARE @XML XML
	DECLARE @Paramlist NVARCHAR(1000)
	SET @Paramlist = N'@XML XML OUTPUT'
	SET @SQL = 'WITH PrepareTable (XMLString) '
	SET @SQL = @SQL + 'AS ( '
	SET @SQL = @SQL + @P_SQL+ ' FOR XML RAW, TYPE, ELEMENTS '
	SET @SQL = @SQL + ') '
	SET @SQL = @SQL + 'SELECT @XML = XMLString FROM PrepareTable '
	EXEC sp_executesql @SQL, @Paramlist, @XML=@XML OUTPUT
	SET @XMLString = CAST(@XML AS VARCHAR(MAX))

	DECLARE @JSON VARCHAR(MAX)
	DECLARE @Row VARCHAR(MAX)
	DECLARE @RowStart INT
	DECLARE @RowEnd INT
	DECLARE @FieldStart INT
	DECLARE @FieldEnd INT
	DECLARE @KEY VARCHAR(MAX)
	DECLARE @Value VARCHAR(MAX)

	DECLARE @StartRoot VARCHAR(100);
	SET @StartRoot = '<row>'
	DECLARE @EndRoot VARCHAR(100);
	SET @EndRoot = '</row>'
	DECLARE @StartField VARCHAR(100);
	SET @StartField = '<'
	DECLARE @EndField VARCHAR(100);
	SET @EndField = '>'

	SET @RowStart = CharIndex(@StartRoot, @XMLString, 0)
	SET @JSON = ''
	WHILE @RowStart > 0
	BEGIN
		SET @RowStart = @RowStart+Len(@StartRoot)
		SET @RowEnd = CharIndex(@EndRoot, @XMLString, @RowStart)
		SET @Row = SubString(@XMLString, @RowStart, @RowEnd-@RowStart)
		SET @JSON = @JSON+'{'

		-- for each row
		SET @FieldStart = CharIndex(@StartField, @Row, 0)
		WHILE @FieldStart > 0
		BEGIN
			-- parse node key
			SET @FieldStart = @FieldStart+Len(@StartField)
			SET @FieldEnd = CharIndex(@EndField, @Row, @FieldStart)
			SET @KEY = SubString(@Row, @FieldStart, @FieldEnd-@FieldStart)
			SET @JSON = @JSON+'"'+@KEY+'":'

			-- parse node value
			SET @FieldStart = @FieldEnd+1
			SET @FieldEnd = CharIndex('</', @Row, @FieldStart)
			SET @Value = SubString(@Row, @FieldStart, @FieldEnd-@FieldStart)
			SET @JSON = @JSON+'"'+@Value+'",'

			SET @FieldStart = @FieldStart+Len(@StartField)
			SET @FieldEnd = CharIndex(@EndField, @Row, @FieldStart)
			SET @FieldStart = CharIndex(@StartField, @Row, @FieldEnd)
		END
		IF LEN(@JSON)>0 SET @JSON = SubString(@JSON, 0, LEN(@JSON))
		SET @JSON = @JSON+'},'
		--/ for each row

		SET @RowStart = CharIndex(@StartRoot, @XMLString, @RowEnd)
	END
	IF LEN(@JSON)>0 SET @JSON = SubString(@JSON, 0, LEN(@JSON))
	SET @JSON = '[' + @JSON + ']'
	SELECT @JSON

END
GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_Log]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Log]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Log] AS'
END
GO

/*
Usage:
    DECLARE
		@P_LogSeqId int = 1

	exec ZGWSystem.Get_Log @P_LogSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/04/2022
-- Description:	Retrievs a row from the [ZGWSystem].[Logging]
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Log]
	@P_LogSeqId int,
	@P_StartDate VARCHAR(10) = NULL,
	@P_EndDate VARCHAR(10) = NULL
AS
    SET NOCOUNT ON;
    SELECT TOP 1
	[Account]
        , [Component]
        , [ClassName]
        , [Level]
        , [LogDate]
        , [LogSeqId]
        , [MethodName]
        , [Msg]
FROM
	[ZGWSystem].[Logging] WITH(NOLOCK)
WHERE
		[LogSeqId] = @P_LogSeqId;

    RETURN 0;

GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_Name_Value_Pair]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair] AS'
END
GO

/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@P_AccountSeqId int = 2,
		@PSecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair
		@P_NVPSeqId,
		@P_AccountSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pairs 
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_AccountSeqId int,
	@PSecurityEntitySeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSystem.Get_Name_Value_Pair'
	IF @P_NVPSeqId > -1
		BEGIN
	SELECT
		NVPSeqId as NVP_SEQ_ID
				, [Schema_Name]
				, Static_Name
				, Display
				, [Description]
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSystem.Name_Value_Pairs
	WHERE
				ZGWSystem.Name_Value_Pairs.NVPSeqId = @P_NVPSeqId
	ORDER BY
				Static_Name
END
	ELSE
		BEGIN
	IF @P_AccountSeqId > -1
				BEGIN
		-- get only valid NVP for the given account
		IF @P_Debug = 1 PRINT 'get only valid NVP for the given account'
		DECLARE @V_Permission_Id INT
		SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()
		DECLARE @V_AvalibleItems TABLE ([NVPSeqId] int,
			[Schema_Name] varchar(30),
			[Static_Name] varchar(30),
			[Display] varchar(128),
			[Description] varchar(256),
			[StatusSeqId] int,
			[Added_By] int,
			[Added_Date] datetime,
			[Updated_By] int,
			[Updated_Date] datetime,
			[Role] VARCHAR(50))
		IF @P_Debug = 1 PRINT 'Geting items via roles'
		INSERT INTO @V_AvalibleItems
		SELECT -- Items via roles
			ZGWSystem.Name_Value_Pairs.NVPSeqId,
			ZGWSystem.Name_Value_Pairs.[Schema_Name],
			ZGWSystem.Name_Value_Pairs.Static_Name,
			ZGWSystem.Name_Value_Pairs.Display,
			ZGWSystem.Name_Value_Pairs.[Description],
			ZGWSystem.Name_Value_Pairs.StatusSeqId,
			ZGWSystem.Name_Value_Pairs.Added_By,
			ZGWSystem.Name_Value_Pairs.Added_Date,
			ZGWSystem.Name_Value_Pairs.Updated_By,
			ZGWSystem.Name_Value_Pairs.Updated_Date,
			ROLES.Name AS [Role]
		FROM
			ZGWSecurity.Roles_Security_Entities SE_ROLES,
			ZGWSecurity.Roles ROLES,
			ZGWSecurity.Roles_Security_Entities_Permissions [SECURITY],
			ZGWSystem.Name_Value_Pairs,
			ZGWSecurity.[Permissions] [Permissions]
		WHERE
						SE_ROLES.RoleSeqId = ROLES.RoleSeqId
			AND SECURITY.RolesSecurityEntitiesSeqId = SE_ROLES.RolesSecurityEntitiesSeqId
			AND SECURITY.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
			AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND SE_ROLES.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
			FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
		IF @P_Debug = 1 PRINT 'Getting items via groups'
		INSERT INTO @V_AvalibleItems
		SELECT -- Items via groups
			ZGWSystem.Name_Value_Pairs.NVPSeqId
						, ZGWSystem.Name_Value_Pairs.[Schema_Name]
						, ZGWSystem.Name_Value_Pairs.Static_Name
						, ZGWSystem.Name_Value_Pairs.Display
						, ZGWSystem.Name_Value_Pairs.[Description]
						, ZGWSystem.Name_Value_Pairs.StatusSeqId
						, ZGWSystem.Name_Value_Pairs.Added_By
						, ZGWSystem.Name_Value_Pairs.Added_Date
						, ZGWSystem.Name_Value_Pairs.Updated_By
						, ZGWSystem.Name_Value_Pairs.Updated_Date
						, ROLES.[Name] AS [Role]
		FROM
			ZGWSecurity.Groups_Security_Entities_Permissions,
			ZGWSecurity.Groups_Security_Entities,
			ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
			ZGWSecurity.Roles_Security_Entities,
			ZGWSecurity.Roles ROLES,
			ZGWSystem.Name_Value_Pairs,
			ZGWSecurity.[Permissions] [Permissions]
		WHERE
						ZGWSecurity.Groups_Security_Entities_Permissions.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
			AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.PermissionsNVPDetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
			FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))

		DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30))
		-- Roles belonging to the account
		IF @P_Debug = 1 PRINT 'Getting roles for account and roles via groups'
		INSERT INTO @V_AccountRoles
					SELECT -- Roles via roles
				ZGWSecurity.Roles.[Name] AS Roles
			FROM
				ZGWSecurity.Accounts,
				ZGWSecurity.Roles_Security_Entities_Accounts,
				ZGWSecurity.Roles_Security_Entities,
				ZGWSecurity.Roles
			WHERE
						ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = @P_AccountSeqId
				AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
				AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		UNION
			SELECT -- Roles via groups
				ZGWSecurity.Roles.[Name] AS Roles
			FROM
				ZGWSecurity.Accounts,
				ZGWSecurity.Groups_Security_Entities_Accounts,
				ZGWSecurity.Groups_Security_Entities,
				ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
				ZGWSecurity.Roles_Security_Entities,
				ZGWSecurity.Roles
			WHERE
						ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = @P_AccountSeqId
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
				AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId

		DECLARE @V_AllItems TABLE ([NVPSeqId] int,
			[Schema_Name] varchar(30),
			[Static_Name] varchar(30),
			[Display] varchar(128),
			[Description] varchar(256),
			[Added_By] int,
			[Added_Date] datetime,
			[Updated_By] int,
			[Updated_Date] datetime)
		IF @P_Debug = 1 PRINT 'Putting all items into tabable variable'
		INSERT INTO @V_AllItems
		SELECT -- Last but not least get the menu items when there are matching account roles.
			NVPSeqId
							, [Schema_Name]
							, Static_Name
							, Display
							, [Description]
							, Added_By
							, Added_Date
							, Updated_By
							, Updated_Date
		FROM
			@V_AvalibleItems
		WHERE
							[Role] IN (SELECT DISTINCT *
		FROM @V_AccountRoles)

		DECLARE @V_DistinctItems TABLE ([NVPSeqId] int,
			[Schema_Name] varchar(30),
			[Static_Name] varchar(30),
			[Display] varchar(128),
			[Description] varchar(256),
			[Added_By] int,
			[Added_Date] datetime,
			[Updated_By] int,
			[Updated_Date] datetime)
		IF @P_Debug = 1 PRINT 'Getting disting items into table variable'
		INSERT INTO @V_DistinctItems
		SELECT DISTINCT
			NVPSeqId,
			[Schema_Name],
			Static_Name,
			Display,
			[Description],
			Added_By,
			Added_Date,
			Updated_By,
			Updated_Date
		FROM
			@V_AllItems

		IF @P_Debug = 1 PRINT 'Selecting all distint items for account'
		SELECT
			NVPSeqId as NVP_SEQ_ID
						, [Schema_Name]
						, Static_Name
						, Display
						, [Description]
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
		FROM
			@V_DistinctItems
		ORDER BY
						Static_Name
	END
			ELSE
				BEGIN
		-- get only valid NVP for the given account
		IF @P_Debug = 1 PRINT 'get only valid NVP for the given account'
		SELECT
			NVPSeqId as NVP_SEQ_ID
						, [Schema_Name]
						, Static_Name
						, Display
						, [Description]
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
		FROM
			ZGWSystem.Name_Value_Pairs
		ORDER BY
						Static_Name
	END
END
		IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Roles'
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Detail]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair_Detail]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Detail] AS'
END
GO

/*
Usage:
	DECLARE
		@P_NVPSeqId INT = 1,
		@P_NVP_DetailSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Detail
		@P_NVPSeqId,
		@P_NVP_DetailSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/11/2011
-- Description:	Returns name value pair detail
-- Note:
--	This not the most effecient however this should
--	not be called very often ... it is intended for the
--	front end to cache the information and only get called
--	when needed
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Detail]
	@P_NVPSeqId INT,
	@P_NVP_DetailSeqId INT,
	@P_Debug INT = 0
AS
	DECLARE @V_TableName VARCHAR(30)
	DECLARE @V_Statement nvarchar(4000)
	SET @V_TableName = (SELECT [Schema_Name] + '.' + Static_Name
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)
	SET @V_Statement = 'SELECT NVPSeqId as NVP_SEQ_ID, [Schema_Name], Static_Name, Display, Description, StatusSeqId as STATUS_SEQ_ID, Added_By, Added_Date, Updated_By, Updated_Date FROM ' + CONVERT(VARCHAR,@V_TableName) + '
	WHERE
		NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId) + ' ORDER BY Static_Name'

	EXECUTE dbo.sp_executesql @statement = @V_Statement
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Details]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair_Details]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Details] AS'
END
GO

/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Details
		@P_NVPSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pair details 
-- Note:
--	This not the most effecient however this should
--	not be called very often ... it is intended for the
--	front end to cache the information and only get called
--	when needed
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Details]
	@P_NVPSeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Name_Value_Pair_Details'

	CREATE TABLE #NVP_DETAILS
(
	NVP_DetailSeqId INT
								,
	NVPSeqId INT
								,
	NVP_Detail_Name VARCHAR(50)
								,
	NVP_Detail_Value VARCHAR(300)
								,
	StatusSeqId INT
								,
	Sort_Order INT
								,
	Added_By INT
								,
	Added_Date DATETIME
								,
	Updated_By INT
								,
	Updated_Date DATETIME
) 
	DECLARE @V_NVPSeqId INT
			,@V_Static_Name VARCHAR(30)
			,@V_Schema_Name VARCHAR(30)
			,@V_Statement nvarchar(max)
	SET @V_Statement = 'SELECT * FROM '
	DECLARE V_Name_Value_Pairs CURSOR STATIC LOCAL FOR
		SELECT
	NVPSeqId,
	Static_Name,
	[Schema_Name]
FROM
	ZGWSystem.Name_Value_Pairs

	OPEN V_Name_Value_Pairs
		FETCH NEXT FROM V_Name_Value_Pairs
		INTO 
			@V_NVPSeqId,  
			@V_Static_Name,
			@V_Schema_Name
		WHILE (@@FETCH_STATUS = 0)
			BEGIN
	SET @V_Statement =  @V_Statement + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + ' UNION ALL SELECT * FROM '
	FETCH NEXT FROM V_Name_Value_Pairs INTO @V_NVPSeqId, @V_Static_Name, @V_Schema_Name
END
	CLOSE V_Name_Value_Pairs
	DEALLOCATE V_Name_Value_Pairs
	SET @V_Statement = SUBSTRING(@V_Statement, 0, LEN(@V_Statement) - 23)
	IF @P_Debug = 1 PRINT @V_Statement
	INSERT INTO #NVP_DETAILS
EXECUTE dbo.sp_executesql @statement = @V_Statement

	IF @P_NVPSeqId = -1
		SELECT
	#NVP_DETAILS.NVP_DetailSeqId as NVP_SEQ_DET_ID,
	#NVP_DETAILS.NVPSeqId as NVP_SEQ_ID,
	ZGWSystem.Name_Value_Pairs.[Schema_Name] + '.' + ZGWSystem.Name_Value_Pairs.Static_Name as [Table_Name],
	#NVP_DETAILS.NVP_Detail_Name as NVP_DET_VALUE,
	#NVP_DETAILS.NVP_Detail_Value as NVP_DET_TEXT,
	#NVP_DETAILS.StatusSeqId as STATUS_SEQ_ID,
	#NVP_DETAILS.Sort_Order,
	(SELECT TOP(1)
		Account
	FROM ZGWSecurity.Accounts
	WHERE AccountSeqId = #NVP_DETAILS.Added_By) AS Added_By,
	#NVP_DETAILS.Added_Date,
	(SELECT TOP(1)
		Account
	FROM ZGWSecurity.Accounts
	WHERE AccountSeqId = #NVP_DETAILS.Updated_By) AS Updated_By,
	#NVP_DETAILS.Updated_Date
FROM
	#NVP_DETAILS,
	ZGWSystem.Name_Value_Pairs
WHERE
			#NVP_DETAILS.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
ORDER BY
			ZGWSystem.Name_Value_Pairs.Static_Name,
			#NVP_DETAILS.NVP_Detail_Value
	ELSE
		SELECT
	#NVP_DETAILS.NVP_DetailSeqId as NVP_SEQ_DET_ID,
	#NVP_DETAILS.NVPSeqId as NVP_SEQ_ID,
	ZGWSystem.Name_Value_Pairs.[Schema_Name] + '.' + ZGWSystem.Name_Value_Pairs.Static_Name as [Table_Name],
	#NVP_DETAILS.NVP_Detail_Name as NVP_DET_VALUE,
	#NVP_DETAILS.NVP_Detail_Value as NVP_DET_TEXT,
	#NVP_DETAILS.StatusSeqId as STATUS_SEQ_ID,
	#NVP_DETAILS.Sort_Order,
	(SELECT TOP(1)
		Account
	FROM ZGWSecurity.Accounts
	WHERE AccountSeqId = #NVP_DETAILS.Added_By) AS Added_By,
	#NVP_DETAILS.Added_Date,
	(SELECT TOP(1)
		Account
	FROM ZGWSecurity.Accounts
	WHERE AccountSeqId = #NVP_DETAILS.Updated_By) AS Updated_By,
	#NVP_DETAILS.Updated_Date
FROM
	#NVP_DETAILS,
	ZGWSystem.Name_Value_Pairs
WHERE
			#NVP_DETAILS.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
	AND ZGWSystem.Name_Value_Pairs.NVPSeqId = @P_NVPSeqId
ORDER BY
			ZGWSystem.Name_Value_Pairs.Static_Name,
			#NVP_DETAILS.NVP_Detail_Value

	DROP TABLE #NVP_DETAILS

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Get_Paginated_Data]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Paginated_Data]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Paginated_Data] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_TableOrView nvarchar(50) = 'ZGWSecurity.Functions',              
		@P_SelectedPage int = 1,
		@P_PageSize int = 10,
		@P_Columns nvarchar(500) = 'FunctionSeqId, Name, Description, Action, Added_By, Added_Date, Updated_By, Updated_Date',
		@P_OrderByColumn nvarchar(100) = 'Action',
		@P_OrderByDirection nvarchar(4) = 'ASC',
		@P_WhereClause nvarchar(500)

	exec ZGWSystem.Get_Paginated_Data
		@P_TableOrView,              
		@P_SelectedPage,
		@P_PageSize,
		@P_Columns,
		@P_OrderByColumn,
		@P_OrderByDirection,
		@P_WhereClause
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2012
-- Description:	Gets paginated data
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Paginated_Data]
	@P_TableOrView nvarchar (50),
	@P_SelectedPage int,
	@P_PageSize int,
	@P_Columns nvarchar(500),
	@P_OrderByColumn nvarchar(100),
	@P_OrderByDirection nvarchar(4),
	@P_WhereClause nvarchar(500),
	@P_Debug bit = 0
AS
	DECLARE @ReturnedRecords int, 
			@ParmDefinition NVARCHAR(500),
			@SqlQuery nvarchar(4000), 
			@P_ConOrderByDirection nvarchar(4),
			@ReturnCount INT, 
			@TotalPages int, 
			@TotalRecords int,
			@LastPageReturn INT

	SET @P_WhereClause = ISNULL(@P_WhereClause,'1 = 1')
	IF @P_SelectedPage = 0 SET @P_SelectedPage = 1
	IF Upper(@P_OrderByDirection) = 'ASC'
	  BEGIN
	SET @P_ConOrderByDirection = 'DESC'
END
	ELSE
	  BEGIN
	SET @P_ConOrderByDirection = 'ASC'
END
 
	IF @P_WhereClause <> ''
	  BEGIN
	SET @P_WhereClause = ' WHERE ' + @P_WhereClause
END

	SET @ReturnedRecords = (@P_PageSize * @P_SelectedPage)
	-- Get the total number of rows that can be returned
	SET @SqlQuery = N'SELECT @CountOUT = COUNT(*) FROM @TableOrView @WhereClause'
	SET @ParmDefinition = N'@CountOUT INT OUTPUT'
	SET @SqlQuery = REPLACE(@SqlQuery , '@WhereClause' , @P_WhereClause )
	SET @SqlQuery = REPLACE(@SqlQuery , '@TableOrView' , @P_TableOrView )
	
	PRINT @SqlQuery
	-- Get the requested data
	EXECUTE sp_executesql
		@SqlQuery,
		@ParmDefinition,
		@CountOUT=@ReturnCount OUTPUT

	PRINT @ReturnCount
	SET @TotalRecords = @ReturnCount
	-- Finds number of pages
	SET @ReturnedRecords = (@P_PageSize * @P_SelectedPage)
	SET @TotalPages = @ReturnCount / @P_PageSize
	IF @TotalRecords % @P_PageSize > 0
	  BEGIN
	SET @TotalPages = @TotalPages + 1
END
	PRINT '@TotalPages: ' + CONVERT(VARCHAR(20),@TotalPages)
	--SELECT @ReturnCount as TotalRecords
	
	SET @ParmDefinition = N'@ReturnCount INT'

	SET NOCOUNT ON

	-- Checks if current page is last page
	IF @P_SelectedPage != @TotalPages
		BEGIN
	-- Current page is not last page
	IF @P_Debug = 1 PRINT 'Current page is not last page'
	SET @SqlQuery = N'SELECT @ReturnCount as TotalRecords, * FROM
			(SELECT TOP ' + CAST(@P_PageSize as varchar(10)) + ' *  FROM
			  (SELECT TOP ' + CAST(@ReturnedRecords as varchar(10)) + ' ' + @P_Columns +
				' FROM ' + @P_TableOrView + @P_WhereClause + '
				ORDER BY ' + @P_OrderByColumn + ' ' + @P_OrderByDirection + ') AS T1
			  ORDER BY ' + @P_OrderByColumn + ' ' + @P_ConOrderByDirection + ') AS T2
			ORDER BY ' + @P_OrderByColumn + ' ' + @P_OrderByDirection
END
	ELSE
		BEGIN
	-- Current page is last page
	IF @P_Debug = 1 PRINT 'Current page is last page'
	IF (@ReturnCount % @P_PageSize) = 0 
				BEGIN
		SET @LastPageReturn = @P_PageSize
	END
			ELSE
				BEGIN
		SET @LastPageReturn = @ReturnCount % @P_PageSize
	END
	--END IF
	SET @SqlQuery = N'SELECT @ReturnCount as TotalRecords, * FROM (SELECT TOP (' + CAST((@LastPageReturn) as varchar(10)) + ')'
				+ ' *  FROM (SELECT TOP ' + CAST(@ReturnedRecords as varchar(10)) + ' ' + @P_Columns
				+ ' FROM ' + @P_TableOrView + @P_WhereClause 
				+ ' ORDER BY ' + @P_OrderByColumn + ' ' + @P_OrderByDirection 
				+ ') AS T1 ORDER BY ' + @P_OrderByColumn + ' ' + @P_ConOrderByDirection
				+ ') AS T2 ORDER BY ' + @P_OrderByColumn + ' ' + @P_OrderByDirection
END
	--END IF
	 
	IF @P_Debug = 1 PRINT @SqlQuery

	EXECUTE sp_executesql
		@SqlQuery,
		@ParmDefinition,
		@ReturnCount
	SET NOCOUNT OFF

RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Log_Error_Info]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Log_Error_Info]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Log_Error_Info] AS'
END
GO

ALTER PROCEDURE [ZGWSystem].[Log_Error_Info]
	@P_Return_Row bit = 0
AS
	DECLARE @V_ErrorSeqId INT
	INSERT ZGWSystem.Data_Errors
	(
	[ErrorNumber],
	[ErrorSeverity],
	[ErrorState],
	[ErrorProcedure],
	[ErrorLine],
	[ErrorMessage],
	[ErrorDate]
	)
VALUES
	(
		ERROR_NUMBER(),
		ERROR_SEVERITY(),
		ERROR_STATE(),
		ERROR_PROCEDURE(),
		ERROR_LINE(),
		ERROR_MESSAGE(),
		GETDATE()
	)
	IF @P_Return_Row = 1
		BEGIN
	SELECT @V_ErrorSeqId = SCOPE_IDENTITY()

	SELECT
		[ErrorNumber],
		[ErrorSeverity],
		[ErrorState],
		[ErrorProcedure],
		[ErrorLine],
		[ErrorMessage],
		[ErrorDate]
	FROM
		ZGWSystem.Data_Errors
	WHERE
				ErrorSeqId = @V_ErrorSeqId
END
	-- END IF
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[PrepForAngularJS]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[PrepForAngularJS]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[PrepForAngularJS] AS'
END
GO

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
ALTER PROCEDURE [ZGWSystem].[PrepForAngularJS]
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
GO
/****** Object:  StoredProcedure [ZGWSystem].[Set_DataBase_Information]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Set_DataBase_Information]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_DataBase_Information] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_Database_InformationSeqId INT = 1,
		@P_Version VARCHAR(15) = '3',
		@P_Enable_Inheritance INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSystem.Set_DataBase_Information
		@P_Database_InformationSeqId,
		@P_Version,
		@P_Enable_Inheritance,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Inserts or updates a record from [ZGWSystem].[Set_DataBase_Information]
--	given the Database_InformationSeqId
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_DataBase_Information]
	@P_Database_InformationSeqId INT,
	@P_Version VARCHAR(15),
	@P_Enable_Inheritance INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @V_Now DATETIME = GETDATE()
	IF (SELECT COUNT(*)
	FROM [ZGWSystem].[Database_Information]) = 0
		BEGIN
		-- INSERT
		INSERT [ZGWSystem].[Database_Information]
			(
			Version,
			Enable_Inheritance,
			Added_By,
			Added_Date
			)
		VALUES
			(
				@P_Version,
				@P_Enable_Inheritance,
				@P_Added_Updated_By,
				@V_Now
			)
		SELECT @P_Primary_Key = SCOPE_IDENTITY()-- Get the IDENTITY value for the row just inserted.
	END
	ELSE-- UPDATE
		BEGIN
		UPDATE [ZGWSystem].[Database_Information]
			SET 
				[Version] = @P_Version,
				Enable_Inheritance = @P_Enable_Inheritance,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				Database_InformationSeqId = @P_Database_InformationSeqId

		SET @P_Primary_Key = @P_Database_InformationSeqId

	END
-- END IF
END
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Set_Log]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Set_Log]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_Log] AS'
END
GO
/*
Usage:
    DECLARE
        @P_Account       VARCHAR (128) = 'System'
        , @P_Component     VARCHAR (50)  = 'UI'
        , @P_ClassName     VARCHAR (50)  = 'TestClass'
        , @P_Level         VARCHAR (5)   = 'Debug'
        , @P_MethodName    VARCHAR (50)  = 'TestMethod'
        , @P_Msg           VARCHAR (MAX) = 'Just testing'
        , @P_Primary_Key int

    EXEC [ZGWSystem].[Set_Log]
            @P_Account
            , @P_Component
            , @P_ClassName
            , @P_Level
            , @P_MethodName
            , @P_Msg
            , @P_Primary_Key OUTPUT
    PRINT @P_Primary_Key
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/04/2022
-- Description:	Inserts a row into the [ZGWSystem].[Logging] table
--	ZGWSystem.Get_Log
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_Log]
	@P_Account       VARCHAR (128),
	@P_Component     VARCHAR (50),
	@P_ClassName     VARCHAR (50),
	@P_Level         VARCHAR (5),
	@P_MethodName    VARCHAR (50),
	@P_Msg           VARCHAR (MAX),
	@P_Primary_Key int OUTPUT
AS
    SET NOCOUNT ON;
    INSERT [ZGWSystem].[Logging](
		[Account]
		, [Component]
		, [ClassName]
		, [Level]
		, [LogDate]
		, [MethodName]
		, [Msg]
	)VALUES(
		@P_Account
        , @P_Component
        , @P_ClassName
        , @P_Level
        , GETDATE()
        , @P_MethodName
        , @P_Msg
    )
    SELECT @P_Primary_Key = SCOPE_IDENTITY()-- Get the IDENTITY value for the row just inserted.
    RETURN 0;

GO
/****** Object:  StoredProcedure [ZGWSystem].[Set_Name_Value_Pair]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Set_Name_Value_Pair]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair] AS'
END
GO

/*
Usage:
	DECLARE 
		@P_NVPSeqId int = -1,
		@P_Schema_Name VARCHAR(30) = 'dbo',
		@P_Static_Name VARCHAR(30) = 'Testing',
		@P_Display VARCHAR(128) = 'TestingNVP',
		@P_Description VARCHAR(256) = 'Just Testing the Name value Pair',
		@P_StatusSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key INT = null,
		@P_ErrorCode int = null,
		@P_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair
		@P_NVPSeqId,
		@P_Schema_Name,
		@P_Static_Name,
		@P_Display,
		@P_Description,
		@P_StatusSeqId,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_ErrorCode,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_Schema_Name VARCHAR(30),
	@P_Static_Name VARCHAR(30),
	@P_Display VARCHAR(128),
	@P_Description VARCHAR(256),
	@P_StatusSeqId INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair]'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_NVPSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE ZGWSystem.Name_Value_Pairs
			SET 
				[Display] = @P_Display,
				[Description] = @P_Description,
				StatusSeqId = @P_StatusSeqId,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				NVPSeqId = @P_NVPSeqId

	SELECT @P_Primary_Key = @P_NVPSeqId
END
	ELSE
	BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	IF EXISTS(SELECT Static_Name
	FROM ZGWSystem.Name_Value_Pairs
	WHERE Static_Name = @P_Static_Name)
				BEGIN
		RAISERROR ('The name value pair already exists in the database.',16,1)
		SELECT @P_ErrorCode=1
		RETURN
	END
	IF NOT EXISTS (SELECT *
	FROM dbo.sysobjects
	WHERE id = OBJECT_ID('[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR(MAX),@P_Static_Name) + ']') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
			BEGIN
		-- Create the new table to hold the details for the name value pair
		DECLARE @V_Statement nvarchar(4000)

		set @V_Statement = 'CREATE TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '](
					[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
					[NVPSeqId] [int] NOT NULL,
					[NVP_Detail_Name] [varchar](50) NOT NULL,
					[NVP_Detail_Value] [varchar](300) NOT NULL,
					[StatusSeqId] [int] NOT NULL,
					[Sort_Order] [int] NOT NULL,
					[Added_By] [int] NOT NULL,
					[Added_DATE] [datetime] NOT NULL,
					[Updated_By] [int] NULL,
					[Updated_Date] [datetime] NULL,
					 CONSTRAINT [PK_' + CONVERT(VARCHAR,@P_Static_Name) + '] PRIMARY KEY CLUSTERED 
					(
						[NVP_DetailSeqId] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
					 CONSTRAINT [UK_' + CONVERT(VARCHAR,@P_Static_Name) + '] UNIQUE NONCLUSTERED 
					(
						[NVP_Detail_Name] ASC,	
						[NVP_Detail_Value] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] WITH CHECK ADD CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
					REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Statuses]
					ALTER TABLE[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + ']  WITH CHECK ADD  CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
					REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Name_Value_Pairs]
					'
		IF @P_Debug = 1 PRINT  @V_Statement
		EXECUTE dbo.sp_executesql @statement = @V_Statement

	END
	INSERT ZGWSystem.Name_Value_Pairs
		(
		[Schema_Name],
		Static_Name,
		[Display],
		[Description],
		StatusSeqId,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_Schema_Name,
			@P_Static_Name,
			@P_Display,
			@P_Description,
			@P_StatusSeqId,
			@P_Added_Updated_By,
			@V_Now
			)
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
IF @P_Debug = 1 PRINT 'End [ZGWSystem].[Set_Name_Value_Pair]'
GO
/****** Object:  StoredProcedure [ZGWSystem].[Set_Name_Value_Pair_Detail]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Set_Name_Value_Pair_Detail]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair_Detail] AS'
END
GO

/*
Usage:
	DECLARE 
		@V_NVP_DetailSeqId INT = -1,
		@V_NVPSeqId int = (SELECT NVPSeqId FROM ZGWSystem.Name_Value_Pairs WHERE STATIC_NAME = 'Navigation_Types') ,
		@V_NVP_Detail_Name VARCHAR(50) = 'Test',
		@V_NVP_Detail_Value VARCHAR(300) = 'Test value',
		@V_StatusSeqId INT = 1,
		@V_Sort_Order INT = 1,
		@V_Added_Updated_BY INT = 1,
		@V_Primary_Key INT = null,
		@V_ErrorCode int = null,
		@V_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair_Detail
		@V_NVP_DetailSeqId,
		@V_NVPSeqId,
		@V_NVP_Detail_Name,
		@V_NVP_Detail_Value,
		@V_StatusSeqId,
		@V_Sort_Order,
		@V_Added_Updated_BY,
		@V_Primary_Key,
		@V_ErrorCode,
		@V_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_Name_Value_Pair_Detail]
	@P_NVP_DetailSeqId INT,
	@P_NVPSeqId int,
	@P_NVP_Detail_Name VARCHAR(50),
	@P_NVP_Detail_Value VARCHAR(300),
	@P_StatusSeqId INT,
	@P_Sort_Order INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair_Detail]'
	DECLARE 
		@V_Static_Name VARCHAR(30) = (SELECT Static_Name
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)
		,@V_Schema_Name VARCHAR(30) = (SELECT [Schema_Name]
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)
		,@V_Statement NVARCHAR(4000)
		,@V_Now DATETIME = GETDATE()

	IF @P_NVP_DetailSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	SET @V_Statement = 'UPDATE ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
			SET 
				NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
				NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
				StatusSeqId = ' + CONVERT(VARCHAR,@P_StatusSeqId) + ',
				Sort_Order = ' + CONVERT(VARCHAR,@P_Sort_Order) + ',
				Updated_By = ' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
				UPDATED_DATE = ''' + CONVERT(VARCHAR,@V_Now) + '''
			WHERE
				NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId)
	IF @P_Debug = 1 PRINT @V_Statement
	EXECUTE dbo.sp_executesql @statement = @V_Statement
	SELECT @P_Primary_Key = @P_NVPSeqId
END
	ELSE
		BEGIN
	-- INSERT a new row in the table.
	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	DECLARE @V_COUNT INT
	SET @V_Statement= 'SET @V_COUNT = (SELECT COUNT(*)
				   FROM ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
				   WHERE NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''' AND NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''')'
	IF @P_Debug = 1 PRINT @V_Statement
	EXECUTE sp_executesql @V_Statement,N'@V_COUNT int output',@V_COUNT output
	IF @V_COUNT > 0
				BEGIN
		RAISERROR ('The entry already exists in the database.',16,1)
		RETURN
	END
	SET @V_Statement = 'INSERT INTO ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '(
					NVPSeqId,
					NVP_Detail_Name,
					NVP_Detail_Value,
					StatusSeqId,
					Sort_Order,
					Added_By,
					ADDED_DATE
				)
				VALUES
				(
					' + CONVERT(VARCHAR,@P_NVPSeqId) + ',
					''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
					''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
					' + CONVERT(VARCHAR,@P_StatusSeqId) + ',
					' + CONVERT(VARCHAR,@P_Sort_Order) + ',
					' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
					''' + CONVERT(VARCHAR,@V_Now) + '''
				)'
	IF @P_Debug = 1 PRINT @V_Statement
	EXECUTE dbo.sp_executesql @statement = @V_Statement
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
--PRINT 'DONE ADDING'
END
-- Get the Error Code for the statement just executed.
--PRINT 'SETTING ERROR CODE'
SELECT @P_ErrorCode=@@ERROR
RETURN 0
GO
/****** Object:  StoredProcedure [ZGWSystem].[Set_System_Status]    Script Date: 7/4/2022 10:50:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Set_System_Status]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_System_Status] AS'
END
GO

/*
Usage:
	DECLARE @V_StatusSeqId INT,
			@V_Added_Updated_By INT,
			@V_PRIMARY_KEY INT,
			@V_ErrorCode INT
	SET @V_StatusSeqId = -1
	SET @V_Added_Updated_By = 1
	SET @V_PRIMARY_KEY = NULL -- Not needed when setup up the database
	SET @V_ErrorCode = NULL -- Not needed when setup up the database
Insert new
	exec [ZGWSystem].[Set_System_Status] @V_StatusSeqId,'Active','Active Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
Update
	SET @V_StatusSeqId = 1
	exec [ZGWSystem].[Set_System_Status] @V_StatusSeqId,'Active','Active Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSystem].[Statuses]
--	@P_StatusSeqId's value determines insert/update
--	a value of -1 is insert > -1 performs update
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_System_Status]
	@P_StatusSeqId int,
	@P_Name VARCHAR(25),
	@P_Description VARCHAR(512) = null,
	@P_Added_Updated_By int,
	@P_Primary_Key int OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_StatusSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE [ZGWSystem].[Statuses]
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = GETDATE()
			WHERE
				StatusSeqId = @P_StatusSeqId

	SELECT @P_Primary_Key = @P_StatusSeqId
END
	ELSE
	BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	IF EXISTS( SELECT 1
	FROM [ZGWSystem].[Statuses]
	WHERE [Name] = @P_Name)
				BEGIN
		RAISERROR ('THE STATUS YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
		RETURN
	END
	-- END IF
	INSERT [ZGWSystem].[Statuses]
		(
		[Name],
		[Description],
		Added_By ,
		Added_Date
		)
	VALUES
		(
			@P_Name,
			@P_Description,
			@P_Added_Updated_By ,
			GETDATE() 
			)
	SELECT @P_StatusSeqId = SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
GO
IF NOT EXISTS (SELECT *
FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'ZGWSecurity', N'TABLE',N'Functions', N'COLUMN',N'Controller'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Used by AnjularJs for building routes' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'Controller'
GO
IF NOT EXISTS (SELECT *
FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'ZGWSecurity', N'TABLE',N'Functions', N'COLUMN',N'Resolve'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Used by AnjularJs for building routes' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'Resolve'
GO
USE [master]
GO
ALTER DATABASE [YourDatabaseName] SET  READ_WRITE 
GO
USE [YourDatabaseName]
GO

-- Handle security for the local service
IF SUSER_ID (N'NT AUTHORITY\LOCAL SERVICE') IS NULL
	BEGIN
		PRINT 'ADDING [NT AUTHORITY\LOCAL SERVICE] TO THE DB'
		CREATE LOGIN [NT AUTHORITY\LOCAL SERVICE] FROM WINDOWS WITH DEFAULT_DATABASE = [YourDatabaseName];
	END
--END IF
IF NOT EXISTS(select * FROM SYS.DATABASE_PRINCIPALS WHERE NAME = 'NT AUTHORITY\LOCAL SERVICE')
BEGIN
	CREATE USER [NT AUTHORITY\LOCAL SERVICE] FOR LOGIN [NT AUTHORITY\LOCAL SERVICE];
END
EXEC sp_addrolemember 'db_datareader', 'NT AUTHORITY\LOCAL SERVICE'
EXEC sp_addrolemember 'db_datawriter', 'NT AUTHORITY\LOCAL SERVICE'


GRANT EXECUTE ON SCHEMA::ZGWCoreWeb to [NT AUTHORITY\LOCAL SERVICE]
GO
GRANT EXECUTE ON SCHEMA::ZGWOptional to [NT AUTHORITY\LOCAL SERVICE]
GO
GRANT EXECUTE ON SCHEMA::ZGWSecurity to [NT AUTHORITY\LOCAL SERVICE]
GO
GRANT EXECUTE ON SCHEMA::ZGWSystem to [NT AUTHORITY\LOCAL SERVICE]
GO