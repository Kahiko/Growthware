-- Oracle Version_0.0.0.0.sql
-- This script is run after the PDB is created by DDatabaseManager.Create()
-- Set NLS parameters for consistent behavior
BEGIN
    EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_LANGUAGE = ''AMERICAN''';
    EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_TERRITORY = ''AMERICA''';
    EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_DATE_FORMAT = ''YYYY-MM-DD HH24:MI:SS''';
    EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_TIMESTAMP_FORMAT = ''YYYY-MM-DD HH24:MI:SS.FF''';
    EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_TIMESTAMP_TZ_FORMAT = ''YYYY-MM-DD HH24:MI:SS.FF TZR''';
END;
/

-- Create all schemas with minimal permissions first
DECLARE
    v_count NUMBER;
    v_tablespace VARCHAR2(100) := 'YourUpperDatabaseName_users';
    v_password VARCHAR2(100) := 'NotUsed123!';
    
    -- List of all schemas to create
    TYPE t_schemas IS TABLE OF VARCHAR2(30);
    l_schemas t_schemas := t_schemas(
        'ZGWSystem',
        'ZGWOptional',
        'ZGWSecurity',
        'ZGWCoreWeb'
    );
BEGIN
    -- Create each schema if it doesn't exist
    FOR i IN 1..l_schemas.COUNT LOOP
        BEGIN
            -- Check if user exists
            SELECT COUNT(*) INTO v_count 
            FROM dba_users 
            WHERE username = l_schemas(i);
            
            IF v_count = 0 THEN
                -- Create the user
                EXECUTE IMMEDIATE 
                    'CREATE USER ' || l_schemas(i) || ' IDENTIFIED BY "' || v_password || '" ' ||
                    'DEFAULT TABLESPACE ' || v_tablespace || ' ' ||
                    'TEMPORARY TABLESPACE temp ' ||
                    'QUOTA UNLIMITED ON ' || v_tablespace || ' ' ||
                    'ACCOUNT UNLOCK';
                
                -- Grant basic permissions
                EXECUTE IMMEDIATE 
                    'GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, ' ||
                    'CREATE PROCEDURE, CREATE SEQUENCE, CREATE TRIGGER ' ||
                    'TO ' || l_schemas(i);
                    
                DBMS_OUTPUT.PUT_LINE('Created schema: ' || l_schemas(i));
            END IF;
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Error creating schema ' || l_schemas(i) || ': ' || SQLERRM);
                RAISE;
        END;
    END LOOP;
END;
/

-- Grant cross-schema permissions
BEGIN
    -- Grant full access between all schemas
    FOR schema_rec IN (
        SELECT username 
        FROM dba_users 
        WHERE username IN ('ZGWSystem', 'ZGWSecurity', 'ZGWCoreWeb')
    ) LOOP
        -- Allow each schema to create objects in other schemas
        EXECUTE IMMEDIATE 
            'GRANT CREATE ANY TABLE, CREATE ANY VIEW, CREATE ANY PROCEDURE, ' ||
            'CREATE ANY SEQUENCE, CREATE ANY TRIGGER, DROP ANY TABLE, ' ||
            'DROP ANY VIEW, DROP ANY PROCEDURE, DROP ANY SEQUENCE, ' ||
            'DROP ANY TRIGGER TO ' || schema_rec.username;
    END LOOP;
END;
/

-- =============================================
-- Oracle Database Creation Script
-- Version: 0.0.0.0
-- Description: Creates all necessary tables for YourDatabaseName
-- =============================================

-- 1. ZGWSystem.Database_Information (already exists)
-- This table is created by the initial script
CREATE TABLE ZGWSystem.Database_Information (
    Database_InformationSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Version VARCHAR2(50) NOT NULL,
    Enable_Inheritance NUMBER(1) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_ZGWSystem_Database_Information PRIMARY KEY (Database_InformationSeqId)
)
/
-- 2. ZGWCoreWeb.Messages
CREATE TABLE ZGWCoreWeb.Messages (
    MessageSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    SecurityEntitySeqId NUMBER(10) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Title VARCHAR2(100) NOT NULL,
    Description VARCHAR2(512),
    Format_As_HTML NUMBER(1) NOT NULL,
    Body CLOB NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_ZFO_Messages PRIMARY KEY (MessageSeqId)
)
/

-- 3. ZGWSecurity.Accounts
CREATE TABLE ZGWSecurity.Accounts (
    AccountSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Account VARCHAR2(128) NOT NULL,
    Email VARCHAR2(128),
    Enable_Notifications NUMBER(1),
    Is_System_Admin NUMBER(1) NOT NULL,
    StatusSeqId NUMBER(10) NOT NULL,
    Password_Last_Set TIMESTAMP NOT NULL,
    Password VARCHAR2(256) NOT NULL,
    ResetToken CLOB,
    ResetTokenExpires TIMESTAMP,
    Failed_Attempts NUMBER(10) NOT NULL,
    First_Name VARCHAR2(35) NOT NULL,
    Last_Login TIMESTAMP,
    Last_Name VARCHAR2(35) NOT NULL,
    Location VARCHAR2(128),
    Middle_Name VARCHAR2(35),
    Preferred_Name VARCHAR2(50),
    Time_Zone NUMBER(10),
    VerificationToken CLOB,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Accounts PRIMARY KEY (AccountSeqId),
    CONSTRAINT UK_Accounts UNIQUE (Account)
)
/

-- 4. ZGWSecurity.RefreshTokens
CREATE TABLE ZGWSecurity.RefreshTokens (
    RefreshTokenId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    AccountSeqId NUMBER(10) NOT NULL,
    Token CLOB,
    Expires TIMESTAMP NOT NULL,
    Created TIMESTAMP NOT NULL,
    CreatedByIp VARCHAR2(25),
    Revoked CLOB,
    RevokedByIp VARCHAR2(25),
    ReplacedByToken CLOB,
    ReasonRevoked VARCHAR2(512),
    CONSTRAINT PK_RefreshTokens PRIMARY KEY (RefreshTokenId),
    CONSTRAINT FK_RefreshTokens_Accounts FOREIGN KEY (AccountSeqId) 
        REFERENCES ZGWSecurity.Accounts(AccountSeqId) ON DELETE CASCADE
)
/

-- 5. ZGWOptional.States
CREATE TABLE ZGWOptional.States (
    State CHAR(2) NOT NULL,
    Description VARCHAR2(128),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_States PRIMARY KEY (State)
)
/

-- 6. ZGWSystem.Statuses
CREATE TABLE ZGWSystem.Statuses (
    StatusSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Name CHAR(25) NOT NULL,
    Description VARCHAR2(128),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    Status VARCHAR2(20) NOT NULL,
    CONSTRAINT PK_Statuses PRIMARY KEY (StatusSeqId),
    CONSTRAINT UK_Statuses_Name UNIQUE (Name)
)
/

-- 7. ZGWSecurity.Roles_Security_Entities
CREATE TABLE ZGWSecurity.Roles_Security_Entities (
    RolesSecurityEntitiesSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    SecurityEntitySeqId NUMBER(10) NOT NULL,
    RoleSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Roles_Security_Entities PRIMARY KEY (RolesSecurityEntitiesSeqId)
)
/

-- 8. ZGWSecurity.Roles
CREATE TABLE ZGWSecurity.Roles (
    RoleSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(128),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Roles PRIMARY KEY (RoleSeqId),
    CONSTRAINT UK_Roles_Name UNIQUE (Name)
)
/

-- 9. ZGWSecurity.Groups_Security_Entities
CREATE TABLE ZGWSecurity.Groups_Security_Entities (
    GroupsSecurityEntitiesSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    SecurityEntitySeqId NUMBER(10) NOT NULL,
    GroupSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups_Security_Entities PRIMARY KEY (GroupsSecurityEntitiesSeqId)
)
/

-- 10. ZGWSecurity.Groups
CREATE TABLE ZGWSecurity.Groups (
    GroupSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Name VARCHAR2(128) NOT NULL,
    Description VARCHAR2(512),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups PRIMARY KEY (GroupSeqId),
    CONSTRAINT UK_Groups_Name UNIQUE (Name)
)
/

-- 11. ZGWSystem.Name_Value_Pairs
CREATE TABLE ZGWSystem.Name_Value_Pairs (
    NVPSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Schema_Name VARCHAR2(30) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Value VARCHAR2(4000),
    Description VARCHAR2(512),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Name_Value_Pairs PRIMARY KEY (NVPSeqId)
)
/

-- 12. ZGWSecurity.Functions (SQL Server to Oracle 1:1 conversion)
CREATE TABLE ZGWSecurity.Functions (
    FunctionSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Action VARCHAR2(256) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Controller VARCHAR2(512),
    Description VARCHAR2(512) NOT NULL,
    Enable_Notifications NUMBER(10) NOT NULL,
    Enable_View_State NUMBER(10) NOT NULL,
    FunctionTypeSeqId NUMBER(10),
    Is_Nav NUMBER(10) NOT NULL,
    Link_Behavior NUMBER(10) NOT NULL,
    Meta_Key_Words VARCHAR2(512),
    Name VARCHAR2(30) NOT NULL,
    Navigation_Types_NVP_DetailSeqId NUMBER(10) NOT NULL,
    Notes VARCHAR2(512),
    No_UI NUMBER(10) NOT NULL,
    ParentSeqId NUMBER(10),
    Redirect_On_Timeout NUMBER(10) NOT NULL,
    Resolve CLOB,
    Sort_Order NUMBER(10) NOT NULL,
    Source VARCHAR2(512),
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Functions PRIMARY KEY (FunctionSeqId),
    CONSTRAINT UK_ZGWSecurity_Functions UNIQUE (Action)
)
/

-- 13. ZGWCoreWeb.Account_Choices
CREATE TABLE ZGWCoreWeb.Account_Choices (
    Account VARCHAR2(128) NOT NULL,
    SecurityEntityID NUMBER(10),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Account_Choices PRIMARY KEY (Account, SecurityEntityID)
)
/

-- 14. ZGWCoreWeb.Link_Behaviors
CREATE TABLE ZGWCoreWeb.Link_Behaviors (
    NVP_DetailSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    NVPSeqId NUMBER(10) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Value VARCHAR2(4000),
    Sort_Order NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Link_Behaviors PRIMARY KEY (NVP_DetailSeqId)
)
/

-- 15. ZGWCoreWeb.Notifications
CREATE TABLE ZGWCoreWeb.Notifications (
    NotificationSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    SecurityEntitySeqId NUMBER(10) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Title VARCHAR2(100) NOT NULL,
    Body CLOB NOT NULL,
    Format_As_HTML NUMBER(1) NOT NULL,
    Is_System NUMBER(1) NOT NULL,
    Is_System_Only NUMBER(1) NOT NULL,
    StatusSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Notifications PRIMARY KEY (NotificationSeqId)
)
/

-- 16. ZGWCoreWeb.Work_Flows
CREATE TABLE ZGWCoreWeb.Work_Flows (
    NVP_DetailSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    NVPSeqId NUMBER(10) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Value VARCHAR2(4000),
    Sort_Order NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Work_Flows PRIMARY KEY (NVP_DetailSeqId)
)
/

-- 17. ZGWOptional.Calendars
CREATE TABLE ZGWOptional.Calendars (
    SecurityEntitySeqId NUMBER(10) NOT NULL,
    Calendar_Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Is_System NUMBER(1) NOT NULL,
    Is_System_Only NUMBER(1) NOT NULL,
    StatusSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Calendars PRIMARY KEY (SecurityEntitySeqId, Calendar_Name)
)
/

-- 18. ZGWOptional.Directories
CREATE TABLE ZGWOptional.Directories (
    FunctionSeqId NUMBER(10) NOT NULL,
    Directory VARCHAR2(255) NOT NULL,
    Description VARCHAR2(512),
    Is_System NUMBER(1) NOT NULL,
    Is_System_Only NUMBER(1) NOT NULL,
    StatusSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Directories PRIMARY KEY (FunctionSeqId, Directory)
)
/

-- 19. ZGWOptional.Zip_Codes
CREATE TABLE ZGWOptional.Zip_Codes (
    State CHAR(2) NOT NULL,
    Zip_Code NUMBER(10) NOT NULL,
    City VARCHAR2(50) NOT NULL,
    Area_Code NUMBER(10),
    FIPS_Code NUMBER(10),
    County_Name VARCHAR2(50),
    State_Abbr CHAR(2) NOT NULL,
    County_Code NUMBER(10),
    Latitude FLOAT,
    Longitude FLOAT,
    Time_Zone NUMBER(10),
    Day_Light_Saving NUMBER(1),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Zip_Codes PRIMARY KEY (State, Zip_Code)
)
/

-- 20. ZGWSecurity.Function_Types
CREATE TABLE ZGWSecurity.Function_Types (
    FunctionTypeSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Function_Types PRIMARY KEY (FunctionTypeSeqId),
    CONSTRAINT UK_Function_Types_Name UNIQUE (Name)
)
/

-- 21. ZGWSecurity.Groups_Security_Entities_Accounts
CREATE TABLE ZGWSecurity.Groups_Security_Entities_Accounts (
    GroupsSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    AccountSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups_Security_Entities_Accounts PRIMARY KEY (GroupsSecurityEntitiesSeqId, AccountSeqId),
    CONSTRAINT FK_Groups_Security_Entities_Accounts_Accounts FOREIGN KEY (AccountSeqId) 
        REFERENCES ZGWSecurity.Accounts(AccountSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Groups_Security_Entities_Accounts_Groups_Security_Entities FOREIGN KEY (GroupsSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Groups_Security_Entities(GroupsSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 22. ZGWSecurity.Groups_Security_Entities_Functions
CREATE TABLE ZGWSecurity.Groups_Security_Entities_Functions (
    GroupsSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    FunctionSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups_Security_Entities_Functions PRIMARY KEY (GroupsSecurityEntitiesSeqId, FunctionSeqId),
    CONSTRAINT FK_Groups_Security_Entities_Functions_Functions FOREIGN KEY (FunctionSeqId) 
        REFERENCES ZGWSecurity.Functions(FunctionSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Groups_Security_Entities_Functions_Groups_Security_Entities FOREIGN KEY (GroupsSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Groups_Security_Entities(GroupsSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 23. ZGWSecurity.Groups_Security_Entities_Groups
CREATE TABLE ZGWSecurity.Groups_Security_Entities_Groups (
    GroupsSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    GroupSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups_Security_Entities_Groups PRIMARY KEY (GroupsSecurityEntitiesSeqId, GroupSeqId),
    CONSTRAINT FK_Groups_Security_Entities_Groups_Groups FOREIGN KEY (GroupSeqId) 
        REFERENCES ZGWSecurity.Groups(GroupSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Groups_Security_Entities_Groups_Groups_Security_Entities FOREIGN KEY (GroupsSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Groups_Security_Entities(GroupsSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 24. ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
CREATE TABLE ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities (
    GroupsSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    RolesSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups_Security_Entities_Roles_Security_Entities PRIMARY KEY (GroupsSecurityEntitiesSeqId, RolesSecurityEntitiesSeqId),
    CONSTRAINT FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities FOREIGN KEY (GroupsSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Groups_Security_Entities(GroupsSecurityEntitiesSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities FOREIGN KEY (RolesSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Roles_Security_Entities(RolesSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 25. ZGWSecurity.Groups_Security_Entities_Permissions
CREATE TABLE ZGWSecurity.Groups_Security_Entities_Permissions (
    GroupsSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    NVPSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Groups_Security_Entities_Permissions PRIMARY KEY (GroupsSecurityEntitiesSeqId, NVPSeqId),
    -- CONSTRAINT FK_Groups_Security_Entities_Permissions_Name_Value_Pairs FOREIGN KEY (NVPSeqId) 
    --     REFERENCES ZGWSystem.Name_Value_Pairs(NVPSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Groups_Security_Entities_Permissions_Groups_Security_Entities FOREIGN KEY (GroupsSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Groups_Security_Entities(GroupsSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 26. ZGWSecurity.Navigation_Types
CREATE TABLE ZGWSecurity.Navigation_Types (
    NVP_DetailSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    NVPSeqId NUMBER(10) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Value VARCHAR2(4000),
    Sort_Order NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Navigation_Types PRIMARY KEY (NVP_DetailSeqId)
)
/

-- 27. ZGWSecurity.Permissions
CREATE TABLE ZGWSecurity.Permissions (
    NVP_DetailSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    NVPSeqId NUMBER(10) NOT NULL,
    Name VARCHAR2(50) NOT NULL,
    Description VARCHAR2(512),
    Value VARCHAR2(4000),
    Sort_Order NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Permissions PRIMARY KEY (NVP_DetailSeqId)
)
/

-- 28. ZGWSecurity.Roles_Security_Entities_Accounts
CREATE TABLE ZGWSecurity.Roles_Security_Entities_Accounts (
    RolesSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    AccountSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Roles_Security_Entities_Accounts PRIMARY KEY (RolesSecurityEntitiesSeqId, AccountSeqId),
    CONSTRAINT FK_Roles_Security_Entities_Accounts_Accounts FOREIGN KEY (AccountSeqId) 
        REFERENCES ZGWSecurity.Accounts(AccountSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Roles_Security_Entities_Accounts_Roles_Security_Entities FOREIGN KEY (RolesSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Roles_Security_Entities(RolesSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 29. ZGWSecurity.Roles_Security_Entities_Functions
CREATE TABLE ZGWSecurity.Roles_Security_Entities_Functions (
    RolesSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    FunctionSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Roles_Security_Entities_Functions PRIMARY KEY (RolesSecurityEntitiesSeqId, FunctionSeqId),
    CONSTRAINT FK_Roles_Security_Entities_Functions_Functions FOREIGN KEY (FunctionSeqId) 
        REFERENCES ZGWSecurity.Functions(FunctionSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Roles_Security_Entities_Functions_Roles_Security_Entities FOREIGN KEY (RolesSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Roles_Security_Entities(RolesSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 30. ZGWSecurity.Roles_Security_Entities_Permissions
CREATE TABLE ZGWSecurity.Roles_Security_Entities_Permissions (
    RolesSecurityEntitiesSeqId NUMBER(10) NOT NULL,
    NVPSeqId NUMBER(10) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    StatusSeqId NUMBER(10) NOT NULL,
    CONSTRAINT PK_Roles_Security_Entities_Permissions PRIMARY KEY (RolesSecurityEntitiesSeqId, NVPSeqId),
    -- CONSTRAINT FK_Roles_Security_Entities_Permissions_Name_Value_Pairs FOREIGN KEY (NVPSeqId) 
    --     REFERENCES ZGWSystem.Name_Value_Pairs(NVPSeqId) ON DELETE CASCADE,
    CONSTRAINT FK_Roles_Security_Entities_Permissions_Roles_Security_Entities FOREIGN KEY (RolesSecurityEntitiesSeqId) 
        REFERENCES ZGWSecurity.Roles_Security_Entities(RolesSecurityEntitiesSeqId) ON DELETE CASCADE
)
/

-- 31. ZGWSystem.Data_Errors
CREATE TABLE ZGWSystem.Data_Errors (
    DataErrorSeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY NOT NULL,
    Error_Number NUMBER(10) NOT NULL,
    Error_Severity NUMBER(10),
    Error_State NUMBER(10),
    Error_Procedure VARCHAR2(128),
    Error_Line NUMBER(10),
    Error_Message VARCHAR2(4000),
    Error_Date TIMESTAMP NOT NULL,
    User_Name VARCHAR2(128),
    Host_Name VARCHAR2(128),
    Program_Name VARCHAR2(128),
    CONSTRAINT PK_Data_Errors PRIMARY KEY (DataErrorSeqId)
)
/
-- 32. ZGWSecurity.Security_Entities - Create table without foreign keys first
CREATE TABLE ZGWSecurity.Security_Entities
(
    SecurityEntitySeqId NUMBER(10) GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
    Name VARCHAR2(255) NOT NULL,
    Description VARCHAR2(4000),
    ParentSecurityEntitySeqId NUMBER(10),
    StatusSeqId NUMBER(10) NOT NULL,
    AddedBy VARCHAR2(50) NOT NULL,
    AddedDate TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,
    UpdatedBy VARCHAR2(50),
    UpdatedDate TIMESTAMP,
    CONSTRAINT PK_Security_Entities PRIMARY KEY (SecurityEntitySeqId)
)
/

-- Create index for better performance
CREATE INDEX IDX_Security_Entities_Parent ON ZGWSecurity.Security_Entities(ParentSecurityEntitySeqId)
/

-- Commit after table creation to ensure it's available for foreign keys
COMMIT
/

-- Add the self-referencing foreign key as DEFERRABLE
-- ALTER TABLE ZGWSecurity.Security_Entities
-- ADD CONSTRAINT FK_Security_Entities_Parent 
-- FOREIGN KEY (ParentSecurityEntitySeqId) 
-- REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- DEFERRABLE INITIALLY DEFERRED
-- /

-- Add status foreign key (commented out as per original)
-- ALTER TABLE ZGWSecurity.Security_Entities
-- ADD CONSTRAINT FK_Security_Entities_Statuses 
-- FOREIGN KEY (StatusSeqId) 
-- REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Add comments for the table and columns
COMMENT ON TABLE ZGWSecurity.Security_Entities IS 'Stores security entities (organizations, departments, etc.) for access control'
/
COMMENT ON COLUMN ZGWSecurity.Security_Entities.SecurityEntitySeqId IS 'Primary key'
/
COMMENT ON COLUMN ZGWSecurity.Security_Entities.Name IS 'Name of the security entity'
/
COMMENT ON COLUMN ZGWSecurity.Security_Entities.Description IS 'Description of the security entity'
/
COMMENT ON COLUMN ZGWSecurity.Security_Entities.ParentSecurityEntitySeqId IS 'Self-referencing foreign key for hierarchical relationships'
/
COMMENT ON COLUMN ZGWSecurity.Security_Entities.StatusSeqId IS 'Foreign key to Statuses table'
/

-- Commit after all Security_Entities related DDL
COMMIT
/

-- Debug: Check if table exists and is accessible
DECLARE
    v_count NUMBER;
BEGIN
    EXECUTE IMMEDIATE 'SELECT COUNT(*) FROM ZGWSecurity.Security_Entities' INTO v_count;
    DBMS_OUTPUT.PUT_LINE('Security_Entities table exists and is accessible. Row count: ' || v_count);
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error accessing Security_Entities: ' || SQLERRM);
        -- Re-raise the exception to fail the script
        RAISE;
END;
/

-- Grant REFERENCES on tables
DECLARE
    v_sql VARCHAR2(4000);
    v_grant_list SYS.ODCIVARCHAR2LIST := SYS.ODCIVARCHAR2LIST(
        -- Format: 'source_schema.table,target_schema'
        'ZGWCoreWeb.Messages, ZGWSecurity.Security_Entities',
        'ZGWCoreWeb.Messages,ZGWSecurity',
        'ZGWSystem.Statuses,ZGWCoreWeb',
        'ZGWSecurity.Accounts,ZGWCoreWeb',
        'ZGWSystem.Name_Value_Pairs,ZGWCoreWeb',
        'ZGWSystem.Statuses,ZGWSecurity'
    );
BEGIN
    DBMS_OUTPUT.PUT_LINE('Starting GRANT REFERENCES process...');
    
    FOR i IN 1..v_grant_list.COUNT LOOP
        DECLARE
            v_source VARCHAR2(100) := SUBSTR(v_grant_list(i), 1, INSTR(v_grant_list(i), ',') - 1);
            v_target VARCHAR2(100) := SUBSTR(v_grant_list(i), INSTR(v_grant_list(i), ',') + 1);
        BEGIN
            v_sql := 'GRANT REFERENCES ON ' || v_source || ' TO ' || v_target;
            DBMS_OUTPUT.PUT_LINE('Executing: ' || v_sql);
            EXECUTE IMMEDIATE v_sql;
            DBMS_OUTPUT.PUT_LINE('  - Success');
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('  - FAILED: ' || SQLERRM);
                -- Continue with next grant even if one fails
        END;
    END LOOP;
    
    DBMS_OUTPUT.PUT_LINE('GRANT REFERENCES process completed.');
END;
/

-- =============================================
-- Add Foreign Key Constraints
-- =============================================

-- Foreign Keys for ZGWCoreWeb.Messages
-- ALTER TABLE ZGWCoreWeb.Messages
-- ADD CONSTRAINT FK_Messages_Security_Entities FOREIGN KEY (SecurityEntitySeqId)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /
-- ALTER TABLE ZGWCoreWeb.Messages
-- ADD CONSTRAINT FK_Messages_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWSecurity.Accounts
-- ALTER TABLE ZGWSecurity.Accounts
-- ADD CONSTRAINT FK_Accounts_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWSecurity.Roles_Security_Entities
-- ALTER TABLE ZGWSecurity.Roles_Security_Entities
-- ADD CONSTRAINT FK_Roles_Security_Entities_Security_Entities FOREIGN KEY (SecurityEntitySeqId)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /
ALTER TABLE ZGWSecurity.Roles_Security_Entities
ADD CONSTRAINT FK_Roles_Security_Entities_Roles FOREIGN KEY (RoleSeqId)
    REFERENCES ZGWSecurity.Roles(RoleSeqId)
/

-- Foreign Keys for ZGWSecurity.Groups_Security_Entities
-- ALTER TABLE ZGWSecurity.Groups_Security_Entities
-- ADD CONSTRAINT FK_Groups_Security_Entities_Security_Entities FOREIGN KEY (SecurityEntitySeqId)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /
ALTER TABLE ZGWSecurity.Groups_Security_Entities
ADD CONSTRAINT FK_Groups_Security_Entities_Groups FOREIGN KEY (GroupSeqId)
    REFERENCES ZGWSecurity.Groups(GroupSeqId)
/

-- Foreign Keys for ZGWSecurity.Functions
ALTER TABLE ZGWSecurity.Functions
ADD CONSTRAINT FK_Functions_Function_Types FOREIGN KEY (FunctionTypeSeqId)
    REFERENCES ZGWSecurity.Function_Types(FunctionTypeSeqId)
/
-- ALTER TABLE ZGWSecurity.Functions
-- ADD CONSTRAINT FK_Functions_Navigation_Types FOREIGN KEY (NavigationTypeSeqId)
--     REFERENCES ZGWSecurity.Navigation_Types(NVP_DetailSeqId)
-- /
-- ALTER TABLE ZGWSecurity.Functions
-- ADD CONSTRAINT FK_Functions_Functions FOREIGN KEY (ParentSeqId)
--     REFERENCES ZGWSecurity.Functions(FunctionSeqId)
-- /
-- ALTER TABLE ZGWSecurity.Functions
-- ADD CONSTRAINT FK_Functions_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWCoreWeb.Account_Choices
ALTER TABLE ZGWCoreWeb.Account_Choices
ADD CONSTRAINT FK_Account_Choices_Accounts FOREIGN KEY (Account)
    REFERENCES ZGWSecurity.Accounts(Account)
/
-- ALTER TABLE ZGWCoreWeb.Account_Choices
-- ADD CONSTRAINT FK_Account_Choices_Security_Entities FOREIGN KEY (SecurityEntityID)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /

-- Foreign Keys for ZGWCoreWeb.Link_Behaviors
ALTER TABLE ZGWCoreWeb.Link_Behaviors
ADD CONSTRAINT FK_Link_Behaviors_Name_Value_Pairs FOREIGN KEY (NVPSeqId)
    REFERENCES ZGWSystem.Name_Value_Pairs(NVPSeqId)
/
-- ALTER TABLE ZGWCoreWeb.Link_Behaviors
-- ADD CONSTRAINT FK_Link_Behaviors_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWCoreWeb.Notifications
-- ALTER TABLE ZGWCoreWeb.Notifications
-- ADD CONSTRAINT FK_Notifications_Security_Entities FOREIGN KEY (SecurityEntitySeqId)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /
-- ALTER TABLE ZGWCoreWeb.Notifications
-- ADD CONSTRAINT FK_Notifications_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWCoreWeb.Work_Flows
ALTER TABLE ZGWCoreWeb.Work_Flows
ADD CONSTRAINT FK_Work_Flows_Name_Value_Pairs FOREIGN KEY (NVPSeqId)
    REFERENCES ZGWSystem.Name_Value_Pairs(NVPSeqId)
/
-- ALTER TABLE ZGWCoreWeb.Work_Flows
-- ADD CONSTRAINT FK_Work_Flows_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWOptional.Calendars
-- ALTER TABLE ZGWOptional.Calendars
-- ADD CONSTRAINT FK_Calendars_Security_Entities FOREIGN KEY (SecurityEntitySeqId)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /
-- ALTER TABLE ZGWOptional.Calendars
-- ADD CONSTRAINT FK_Calendars_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWOptional.Directories
-- ALTER TABLE ZGWOptional.Directories
-- ADD CONSTRAINT FK_Directories_Functions FOREIGN KEY (FunctionSeqId)
--     REFERENCES ZGWSecurity.Functions(FunctionSeqId)
-- /
-- ALTER TABLE ZGWOptional.Directories
-- ADD CONSTRAINT FK_Directories_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWOptional.Zip_Codes
ALTER TABLE ZGWOptional.Zip_Codes
ADD CONSTRAINT FK_Zip_Codes_States FOREIGN KEY (State)
    REFERENCES ZGWOptional.States(State)
/
-- ALTER TABLE ZGWOptional.Zip_Codes
-- ADD CONSTRAINT FK_Zip_Codes_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWSecurity.Function_Types
-- ALTER TABLE ZGWSecurity.Function_Types
-- ADD CONSTRAINT FK_Function_Types_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWSecurity.Groups
-- ALTER TABLE ZGWSecurity.Groups
-- ADD CONSTRAINT FK_Groups_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWSecurity.Roles
-- ALTER TABLE ZGWSecurity.Roles
-- ADD CONSTRAINT FK_Roles_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /

-- Foreign Keys for ZGWSecurity.Security_Entities
-- ALTER TABLE ZGWSecurity.Security_Entities
-- ADD CONSTRAINT FK_Security_Entities_Parent_Security_Entities FOREIGN KEY (Parent_Security_Entity_SeqID)
--     REFERENCES ZGWSecurity.Security_Entities(SecurityEntitySeqId)
-- /
-- ALTER TABLE ZGWSecurity.Security_Entities
-- ADD CONSTRAINT FK_Security_Entities_Statuses FOREIGN KEY (StatusSeqId)
--     REFERENCES ZGWSystem.Statuses(StatusSeqId)
-- /
-- =============================================
-- Create Indexes for better performance
-- =============================================

-- Indexes for ZGWCoreWeb.Messages
CREATE INDEX IX_Messages_SecurityEntitySeqId ON ZGWCoreWeb.Messages(SecurityEntitySeqId)
/

-- Indexes for ZGWSecurity.Accounts
CREATE INDEX IX_Accounts_Email ON ZGWSecurity.Accounts(Email)
/
CREATE INDEX IX_Accounts_StatusSeqId ON ZGWSecurity.Accounts(StatusSeqId)
/

-- Indexes for ZGWSecurity.RefreshTokens
CREATE INDEX IX_RefreshTokens_AccountSeqId ON ZGWSecurity.RefreshTokens(AccountSeqId)
/

-- Indexes for ZGWOptional.States
CREATE INDEX IX_States_StatusSeqId ON ZGWOptional.States(StatusSeqId)
/

-- Indexes for ZGWSecurity.Roles_Security_Entities
CREATE INDEX IX_Roles_Security_Entities_SecurityEntitySeqId ON ZGWSecurity.Roles_Security_Entities(SecurityEntitySeqId)
/
CREATE INDEX IX_Roles_Security_Entities_RoleSeqId ON ZGWSecurity.Roles_Security_Entities(RoleSeqId)
/

-- Indexes for ZGWSecurity.Groups_Security_Entities
CREATE INDEX IX_Groups_Security_Entities_SecurityEntitySeqId ON ZGWSecurity.Groups_Security_Entities(SecurityEntitySeqId)
/
CREATE INDEX IX_Groups_Security_Entities_GroupSeqId ON ZGWSecurity.Groups_Security_Entities(GroupSeqId)
/

-- Indexes for ZGWSystem.Name_Value_Pairs
CREATE INDEX IX_Name_Value_Pairs_Schema_Name ON ZGWSystem.Name_Value_Pairs(Schema_Name)
/
CREATE INDEX IX_Name_Value_Pairs_Name ON ZGWSystem.Name_Value_Pairs(Name)
/

-- Indexes for ZGWSecurity.Functions
CREATE INDEX IX_Functions_Navigation_Types_NVP_DetailSeqId ON ZGWSecurity.Functions(Navigation_Types_NVP_DetailSeqId)
/
-- Indexes for ZGWCoreWeb.Link_Behaviors
CREATE INDEX IX_Link_Behaviors_NVPSeqId ON ZGWCoreWeb.Link_Behaviors(NVPSeqId)
/
CREATE INDEX IX_Link_Behaviors_StatusSeqId ON ZGWCoreWeb.Link_Behaviors(StatusSeqId)
/

-- Indexes for ZGWCoreWeb.Notifications
CREATE INDEX IX_Notifications_SecurityEntitySeqId ON ZGWCoreWeb.Notifications(SecurityEntitySeqId)
/
CREATE INDEX IX_Notifications_StatusSeqId ON ZGWCoreWeb.Notifications(StatusSeqId)
/

-- Indexes for ZGWCoreWeb.Work_Flows
CREATE INDEX IX_Work_Flows_NVPSeqId ON ZGWCoreWeb.Work_Flows(NVPSeqId)
/
CREATE INDEX IX_Work_Flows_StatusSeqId ON ZGWCoreWeb.Work_Flows(StatusSeqId)
/

-- Indexes for ZGWOptional.Calendars
CREATE INDEX IX_Calendars_StatusSeqId ON ZGWOptional.Calendars(StatusSeqId)
/

-- Indexes for ZGWOptional.Directories
CREATE INDEX IX_Directories_StatusSeqId ON ZGWOptional.Directories(StatusSeqId)
/

-- Indexes for ZGWOptional.Zip_Codes
CREATE INDEX IX_Zip_Codes_StateAbbr ON ZGWOptional.Zip_Codes(State_Abbr)
/
CREATE INDEX IX_Zip_Codes_City ON ZGWOptional.Zip_Codes(City)
/
CREATE INDEX IX_Zip_Codes_County_Name ON ZGWOptional.Zip_Codes(County_Name)
/

-- Indexes for ZGWSecurity.Function_Types
CREATE INDEX IX_Function_Types_StatusSeqId ON ZGWSecurity.Function_Types(StatusSeqId)
/

-- Indexes for ZGWSecurity.Groups_Security_Entities_Accounts
CREATE INDEX IX_Groups_Security_Entities_Accounts_AccountSeqId ON ZGWSecurity.Groups_Security_Entities_Accounts(AccountSeqId)
/
CREATE INDEX IX_Groups_Security_Entities_Accounts_StatusSeqId ON ZGWSecurity.Groups_Security_Entities_Accounts(StatusSeqId)
/

-- Indexes for ZGWSecurity.Groups_Security_Entities_Functions
CREATE INDEX IX_Groups_Security_Entities_Functions_FunctionSeqId ON ZGWSecurity.Groups_Security_Entities_Functions(FunctionSeqId)
/
CREATE INDEX IX_Groups_Security_Entities_Functions_StatusSeqId ON ZGWSecurity.Groups_Security_Entities_Functions(StatusSeqId)
/

-- Indexes for ZGWSecurity.Groups_Security_Entities_Groups
CREATE INDEX IX_Groups_Security_Entities_Groups_GroupSeqId ON ZGWSecurity.Groups_Security_Entities_Groups(GroupSeqId)
/
CREATE INDEX IX_Groups_Security_Entities_Groups_StatusSeqId ON ZGWSecurity.Groups_Security_Entities_Groups(StatusSeqId)
/

-- Indexes for ZGWSecurity.Groups_Security_Entities_Permissions
CREATE INDEX IX_Groups_Security_Entities_Permissions_NVPSeqId ON ZGWSecurity.Groups_Security_Entities_Permissions(NVPSeqId)
/
CREATE INDEX IX_Groups_Security_Entities_Permissions_StatusSeqId ON ZGWSecurity.Groups_Security_Entities_Permissions(StatusSeqId)
/

-- Indexes for ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
CREATE INDEX IX_Groups_Security_Entities_Roles_Security_Entities_RolesSecurityEntitiesSeqId ON ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities(RolesSecurityEntitiesSeqId)
/
CREATE INDEX IX_Groups_Security_Entities_Roles_Security_Entities_StatusSeqId ON ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities(StatusSeqId)
/

-- Indexes for ZGWSecurity.Navigation_Types
CREATE INDEX IX_Navigation_Types_NVPSeqId ON ZGWSecurity.Navigation_Types(NVPSeqId)
/
CREATE INDEX IX_Navigation_Types_StatusSeqId ON ZGWSecurity.Navigation_Types(StatusSeqId)
/

-- Indexes for ZGWSecurity.Permissions
CREATE INDEX IX_Permissions_NVPSeqId ON ZGWSecurity.Permissions(NVPSeqId)
/
CREATE INDEX IX_Permissions_StatusSeqId ON ZGWSecurity.Permissions(StatusSeqId)
/

-- Indexes for ZGWSecurity.Roles_Security_Entities_Accounts
CREATE INDEX IX_Roles_Security_Entities_Accounts_AccountSeqId ON ZGWSecurity.Roles_Security_Entities_Accounts(AccountSeqId)
/
CREATE INDEX IX_Roles_Security_Entities_Accounts_StatusSeqId ON ZGWSecurity.Roles_Security_Entities_Accounts(StatusSeqId)
/

-- Indexes for ZGWSecurity.Roles_Security_Entities_Functions
CREATE INDEX IX_Roles_Security_Entities_Functions_FunctionSeqId ON ZGWSecurity.Roles_Security_Entities_Functions(FunctionSeqId)
/
CREATE INDEX IX_Roles_Security_Entities_Functions_StatusSeqId ON ZGWSecurity.Roles_Security_Entities_Functions(StatusSeqId)
/

-- Indexes for ZGWSecurity.Roles_Security_Entities_Permissions
CREATE INDEX IX_Roles_Security_Entities_Permissions_NVPSeqId ON ZGWSecurity.Roles_Security_Entities_Permissions(NVPSeqId)
/
CREATE INDEX IX_Roles_Security_Entities_Permissions_StatusSeqId ON ZGWSecurity.Roles_Security_Entities_Permissions(StatusSeqId)
/
-- =============================================
-- Add Comments for Documentation
-- =============================================

-- Comments for ZGWCoreWeb.Messages
COMMENT ON TABLE ZGWCoreWeb.Messages IS 'Stores system messages with HTML formatting support'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.MessageSeqId IS 'Primary key for the Messages table'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.SecurityEntitySeqId IS 'Foreign key to Security_Entities table'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Name IS 'Unique name identifier for the message'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Title IS 'Display title of the message'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Description IS 'Description of the message'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Format_As_HTML IS 'Flag indicating if the message body contains HTML'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Body IS 'The actual message content'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Added_By IS 'User ID who added the record'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Added_Date IS 'Date and time when the record was added'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Updated_By IS 'User ID who last updated the record'
/
COMMENT ON COLUMN ZGWCoreWeb.Messages.Updated_Date IS 'Date and time when the record was last updated'
/

-- Comments for ZGWSecurity.Accounts
COMMENT ON TABLE ZGWSecurity.Accounts IS 'Stores user account information'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.AccountSeqId IS 'Primary key for the Accounts table'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Account IS 'Unique username'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Email IS 'User''s email address'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Enable_Notifications IS 'Flag indicating if notifications are enabled for the user'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Is_System_Admin IS 'Flag indicating if the user has system administrator privileges'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.StatusSeqId IS 'Foreign key to Statuses table'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Password_Last_Set IS 'Date and time when the password was last changed'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Password IS 'Hashed password'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.ResetToken IS 'Token for password reset'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.ResetTokenExpires IS 'Expiration date and time for the reset token'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Failed_Attempts IS 'Number of failed login attempts'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.First_Name IS 'User''s first name'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Last_Login IS 'Date and time of the last successful login'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Last_Name IS 'User''s last name'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Location IS 'User''s location'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Middle_Name IS 'User''s middle name'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Preferred_Name IS 'User''s preferred name'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.Time_Zone IS 'User''s time zone'
/
COMMENT ON COLUMN ZGWSecurity.Accounts.VerificationToken IS 'Token for email verification'
/

-- Comments for ZGWSecurity.RefreshTokens
COMMENT ON TABLE ZGWSecurity.RefreshTokens IS 'Stores refresh tokens for user sessions'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.RefreshTokenId IS 'Primary key for the RefreshTokens table'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.AccountSeqId IS 'Foreign key to Accounts table'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.Token IS 'The refresh token value'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.Expires IS 'Expiration date and time of the token'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.Created IS 'Date and time when the token was created'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.CreatedByIp IS 'IP address from which the token was created'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.Revoked IS 'Date and time when the token was revoked'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.RevokedByIp IS 'IP address from which the token was revoked'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.ReplacedByToken IS 'Token that replaced this one'
/
COMMENT ON COLUMN ZGWSecurity.RefreshTokens.ReasonRevoked IS 'Reason for revoking the token'
/

-- Comments for ZGWOptional.States
COMMENT ON TABLE ZGWOptional.States IS 'Stores US state information'
/
COMMENT ON COLUMN ZGWOptional.States.State IS 'Two-letter state code (primary key)'
/
COMMENT ON COLUMN ZGWOptional.States.Description IS 'Full state name'
/
COMMENT ON COLUMN ZGWOptional.States.Added_By IS 'User ID who added the record'
/
COMMENT ON COLUMN ZGWOptional.States.Added_Date IS 'Date and time when the record was added'
/
COMMENT ON COLUMN ZGWOptional.States.Updated_By IS 'User ID who last updated the record'
/
COMMENT ON COLUMN ZGWOptional.States.Updated_Date IS 'Date and time when the record was last updated'
/
COMMENT ON COLUMN ZGWOptional.States.StatusSeqId IS 'Foreign key to Statuses table'
/

-- Comments for ZGWSystem.Statuses
COMMENT ON TABLE ZGWSystem.Statuses IS 'Defines status values used throughout the system'
/
COMMENT ON COLUMN ZGWSystem.Statuses.StatusSeqId IS 'Primary key for the Statuses table'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Name IS 'Unique name of the status'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Description IS 'Description of the status'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Added_By IS 'User ID who added the record'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Added_Date IS 'Date and time when the record was added'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Updated_By IS 'User ID who last updated the record'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Updated_Date IS 'Date and time when the record was last updated'
/
COMMENT ON COLUMN ZGWSystem.Statuses.Status IS 'Current status of the record (Active, Inactive, etc.)'
/

-- =============================================
-- End of Script
-- =============================================

-- Create a simple procedure to update database information
CREATE OR REPLACE PROCEDURE ZGWSystem.Set_DataBase_Information(
    p_Version IN VARCHAR2,
    p_Enable_Inheritance IN NUMBER,
    p_Updated_By IN NUMBER
) AS
BEGIN
    UPDATE ZGWSystem.Database_Information
    SET Version = p_Version,
        Enable_Inheritance = p_Enable_Inheritance,
        Updated_By = p_Updated_By,
        Updated_Date = SYSTIMESTAMP
    WHERE Database_InformationSeqId = 1;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- Grant execute on the procedure
GRANT EXECUTE ON ZGWSystem.Set_DataBase_Information TO ZGWSystem, ZGWSecurity;
/

-- Create a public synonym for easier access
CREATE OR REPLACE PUBLIC SYNONYM Set_DataBase_Information FOR ZGWSystem.Set_DataBase_Information;
/

COMMIT;