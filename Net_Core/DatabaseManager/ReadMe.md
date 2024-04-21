DatabaseManager
    Description: The database manage will create, update, downgrade or drop a database for any RDBMS.
    Requires:
        GrowthWare.DataAccess.Interfaces
        GrowthWare.Framework
        GrowthWare.json
        Directories:
            /Scripts/Downgrade/SQLServer: 
                Conatins the .sql files/scripts to downgrade the database to the required version for the database type
                Version_1.0.0.0.sql is unique in that it just drops the database
            /Scripts/Upgrade/SQLServer:
                Conatins the .sql files/scripts to create/upgrade the database to the required version for the database type
                Version_0.0.0.0.sql and Version_1.0.0.0.sql are unique b/c they are both needed in order to create the database:
                    Version_0.0.0.0.sql contains the first DDL for your database
                    Version_1.0.0.0.sql is where I added the data for the first DB release.
                Version_1.x.x.x.sql should contain any scripting needed to support that version of the software, to be clear both DML and DDL
    Parameters:
        Version as --Version X.X.X.X where X.X.X.X is the version number of the database
            A version number of 0.0.0.0 will drop the database

Basic flow:
    1.) Read the desired version from the --Version parameter
    2.) Get the following from GrothWare.json using the ConfigSettings
            DataAccessLayerAssemblyName
            DataAccessLayerNamespace
            ConnectionString (not only used to connect to the sql server but the database name is parsed as well)
    3.) Get an instance of "DDatabaseManager" by loading it from the DataAccessLayerAssemblyName/DataAccessLayerNamespace
            By loading the instance it's possible to support multiple data store technologies.  At the time of writing I had only written the code to support Microsoft SQL Server.
    4.) If needed drop the database specified in the ConnectionString by executing the Version_1.0.0.0.sql script
            Stop executing
    5.) If needed create the database specified in the ConnectionString
    6.) Determine if the database needs to be upgraded or downgraded
    7.) Execute the scripts in the upgrade/downgrade (up to or down to) the specified version

Script file naming convention:
    1.) When upgrading the name of the file should be the version you are attempting upgrade to.
    2.) When downgrading the name of the file should be the version you are downgrading from
    Eample:
        When upgrading to version 2.0.0.0 your file should be named Version_2.0.0.0.sql.
        When downgrading from version 2.0.0.0 your file should be named Version_2.0.0.0.sql.
Example upgrade to version 2.0.0.0:
    -- Upgrade

    /* All scripting code this version of the DB requires */

    -- Update the version
    UPDATE [ZGWSystem].[Database_Information] SET
        [Version] = '2.0.0.0',
        [Updated_By] = 3,
        [Updated_Date] = getdate()

Example downgrade from version 2.0.0.0:
    -- Downgrade

    /* All scripting code this version of the DB requires */

    -- Update the version
    -- B/C your downgrading from 2.0.0.0 the [Version] needs to be set to the previous DB release
    -- normally this will be the file name just before your new file
    UPDATE [ZGWSystem].[Database_Information] SET
        [Version] = '1.0.0.0',
        [Updated_By] = null,
        [Updated_Date] = null
