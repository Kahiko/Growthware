-- Downgrade
-- All command must end in a forward slash, the forward slash will be removed before being executed.;

ALTER SESSION SET CONTAINER = CDB$ROOT/
ALTER PLUGGABLE DATABASE YourDatabaseName CLOSE IMMEDIATE INSTANCES=all/
DROP PLUGGABLE DATABASE YourDatabaseName INCLUDING DATAFILES/