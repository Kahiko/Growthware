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
    for each of the files in the given upgrade or downgrade directory for the databsase technology

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