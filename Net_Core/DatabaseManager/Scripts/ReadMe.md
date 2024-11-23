                                General DatabaseManager information

The DatabaseManager relys on the following:
    Scripts directory in the same directory as the executable
    GrowthWare.Database.dll
    GrowthWare.Framework.dll
    GrowthWare.json
    A connection string with 'database=yourdatabasename' in it
        If your database technology does not allow of this then you will need
        to modify the code appropriately to set the DatabaseManager.ConnectionString appropriately

The 'Downgrade' directory contains all of your scripts used to downgrade the database
The 'Upgrade' directory contains (you guessed it) all of your scripts used to upgrade the database

The DatabaseManager was intended to be able to upgrade a database regardless of database technology.  We 
accomplish this by loading a class that conforms to 'GrowthWare.DataAccess.Interfaces.IDatabaseManager'.  The
loaded object will be specific to the database technology (IE SQLServer, Orcle, MySQL ...).  At the writing of
this only SqlServer is supported.

Script directory structure:
    direction/database technology/files
    Direction           - Upgrade or Downgrade
    Database technology - This is clear text in the _DAL entry in Growthware.json
    Files               - Version_1.0.0.0.sql

Naming:
    The file names should file the xx_x.x.x.x.sql, the processing code (program.cs/Main) will
    split the file name buy the underscore then remove the '.sql' and create Version objects
    for each of the files in the given upgrade or downgrade directory for the databsase technology.

    When creating a new version of a SQL server database and going from 2.0.0.0 to 3.0.0.0 then you need to create two files both named Version_3.0.0.0.sql, one in the Upgrade directory and one in the Downgrade directory.

    Downgrade/SQLServer/Version_3.0.0.0.sql contains any sql code that is needed to downgrade the database and updates the [Version] column in the [ZGWSystem].[Database_Information] table to the previous version.  The previous version value can always be found the next lower version file.  In our example the version number you need would be found in Downgrade/SQLServer/Version_2.0.0.0.sql (the version can also be deduced by the version number in the name of the file as well).

    Upgrade/SQLServer/Version_3.0.0.0.sql contains any sql code that is needed to upgrade the database and updates the [Version] column in the [ZGWSystem].[Database_Information] table to the desired version.  In our example the version number would be 3.0.0.0.

    When upgrading or downgrading the database be sure to your scripts are defensive in that will not create an error if ran multiple times.  Check for the existing object before creating or dropping it.  Check existing data before inserting new data.  Personally I will open both the upgrade and downgrade scripts in SSMS and run them in order multiple times.  Remember you have no idea who will decide to run they once that leave your development environment.


Versioning:
    Upgrade example:
{
    /* 
        Any code that is necessary to upgrade your database
        both DDL and DML
        The version will be the number you are going to
    */
    UPDATE [ZGWSystem].[Database_Information] SET [Version] = '2.0.0.0';
}
    Downgrade example:
{
    /* 
        Any code that is necessary to downgrade your database
        both DDL and DML
    */
    /*
        Requested version       - 2.0.0.1
        This file Name          - Version_3.0.0.0.sql
        The next lower file     - Version_2.0.0.1.sql
        The update statement    - 2.0.0.1
    */
    UPDATE [ZGWSystem].[Database_Information] SET [Version] = '2.0.0.1';
}

The following files are a bit special and should never be run more than once
    Upgrade/SQLServer/Version_0.0.0.0.sql   - DDL: Creates the datbase
    Upgrade/SQLServer/Version_1.0.0.0.sql   - DML: Populates the newly created database
    Downgrade/SQLServer/Version_1.0.0.0/sql - DDL: Delete the database (executed with requested version is 0.0.0.0)