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

-- Create the schemas (users) with minimal permissions
-- They won't be used for login, just as namespaces for objects
DECLARE
    v_count NUMBER;
BEGIN
    -- Create ZGWSystem schema
    SELECT COUNT(*) INTO v_count FROM dba_users WHERE username = 'ZGWSystem';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE USER ZGWSystem IDENTIFIED BY "NotUsed123!" 
                          DEFAULT TABLESPACE YourUpperDatabaseName_users
                          TEMPORARY TABLESPACE temp
                          QUOTA UNLIMITED ON YourUpperDatabaseName_users
                          ACCOUNT LOCK';
        EXECUTE IMMEDIATE 'GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, 
                          CREATE PROCEDURE, CREATE SEQUENCE, CREATE TRIGGER 
                          TO ZGWSystem';
    END IF;

    -- Create ZGWOptional schema
    SELECT COUNT(*) INTO v_count FROM dba_users WHERE username = 'ZGWOptional';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE USER ZGWOptional IDENTIFIED BY "NotUsed123!" 
                          DEFAULT TABLESPACE YourUpperDatabaseName_users
                          TEMPORARY TABLESPACE temp
                          QUOTA UNLIMITED ON YourUpperDatabaseName_users
                          ACCOUNT LOCK';
        EXECUTE IMMEDIATE 'GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, 
                          CREATE PROCEDURE, CREATE SEQUENCE, CREATE TRIGGER 
                          TO ZGWOptional';
    END IF;

    -- Create ZGWSecurity schema
    SELECT COUNT(*) INTO v_count FROM dba_users WHERE username = 'ZGWSecurity';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE USER ZGWSecurity IDENTIFIED BY "NotUsed123!" 
                          DEFAULT TABLESPACE YourUpperDatabaseName_users
                          TEMPORARY TABLESPACE temp
                          QUOTA UNLIMITED ON YourUpperDatabaseName_users
                          ACCOUNT LOCK';
        EXECUTE IMMEDIATE 'GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, 
                          CREATE PROCEDURE, CREATE SEQUENCE, CREATE TRIGGER 
                          TO ZGWSecurity';
    END IF;

    -- Create ZGWCoreWeb schema
    SELECT COUNT(*) INTO v_count FROM dba_users WHERE username = 'ZGWCoreWeb';
    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE USER ZGWCoreWeb IDENTIFIED BY "NotUsed123!" 
                          DEFAULT TABLESPACE YourUpperDatabaseName_users
                          TEMPORARY TABLESPACE temp
                          QUOTA UNLIMITED ON YourUpperDatabaseName_users
                          ACCOUNT LOCK';
        EXECUTE IMMEDIATE 'GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, 
                          CREATE PROCEDURE, CREATE SEQUENCE, CREATE TRIGGER 
                          TO ZGWCoreWeb';
    END IF;
END;
/

-- Create the Database_Information table
CREATE TABLE ZGWSystem.Database_Information (
    Database_InformationSeqId NUMBER GENERATED ALWAYS AS IDENTITY,
    Version VARCHAR2(50) NOT NULL,
    Enable_Inheritance NUMBER(1) NOT NULL,
    Added_By NUMBER(10) NOT NULL,
    Added_Date TIMESTAMP NOT NULL,
    Updated_By NUMBER(10),
    Updated_Date TIMESTAMP,
    CONSTRAINT PK_Database_Information PRIMARY KEY (Database_InformationSeqId)
);
/

-- Insert initial data
BEGIN
    INSERT INTO ZGWSystem.Database_Information (
        Version, 
        Enable_Inheritance, 
        Added_By, 
        Added_Date
    ) VALUES (
        '0.0.0.0',  -- Initial version
        1,          -- Enable inheritance by default
        1,          -- System user
        SYSTIMESTAMP
    );
    COMMIT;
END;
/

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