-- Upgrade Populate all the tables (DML);
-- All command must end in a semicolon, the semicolon will be removed before being executed.;

-- Insert the version
ALTER SESSION SET current_schema=ZGWSystem;
INSERT INTO ZGWSystem.Database_Information( 
    Version, 
    Enable_Inheritance, 
    Added_By, 
    Added_Date 
) VALUES ( 
    '1.0.0.0', 
    1, 
    2, 
    SYSDATE
);
COMMIT;