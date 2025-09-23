-- Oracle downgrade script to remove the PDB (similar to SQL Server's DROP DATABASE)
-- All commands must end in a forward slash, the forward slash will be removed before being executed.

-- First, connect to the root container
-- First, connect to the root container
ALTER SESSION SET CONTAINER = CDB$ROOT;
/

-- Check if PDB exists and drop it
DECLARE
    v_pdb_exists NUMBER;
BEGIN
    -- Check if PDB exists
    SELECT COUNT(*) INTO v_pdb_exists 
    FROM cdb_pdbs 
    WHERE UPPER(pdb_name) = UPPER('YourDatabaseName');
    
    IF v_pdb_exists > 0 THEN
        -- Close the PDB first
        BEGIN
            EXECUTE IMMEDIATE 'ALTER PLUGGABLE DATABASE YourDatabaseName CLOSE IMMEDIATE';
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Warning: Could not close PDB: ' || SQLERRM);
        END;
        
        -- Drop the PDB
        BEGIN
            EXECUTE IMMEDIATE 'DROP PLUGGABLE DATABASE YourDatabaseName INCLUDING DATAFILES';
            DBMS_OUTPUT.PUT_LINE('PDB YourDatabaseName has been dropped successfully.');
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Error dropping PDB: ' || SQLERRM);
                RAISE;
        END;
    ELSE
        DBMS_OUTPUT.PUT_LINE('PDB YourDatabaseName does not exist.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
        RAISE;
END;
/