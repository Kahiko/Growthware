# DataAccess Project

This project contains the core data access classes.  The classes are used to access any data store.  We accomplish this by having an abastract class that adheres to the IDBInteraction interface.  Apart from the abstract class, this project contains a number of concrete classes that implement the a "table" specific class desinged to perform any "table" specific operations.

The Business Logic project is responsable for "loading" the correct concrete "table" class.  The loading of the object is accomplished by using the ObjectFactory class found in the GrowthWare.Framework project.

# Directory Structure (Best viewed with a plain text editor)
DataAccess<br />
├── Interfaces/<br />
├── Oracle/ - DataStore<br />
│   └── Base/<br />
│       └── ADBinteraction.cs       - Contains most of the code necessary to interact with the data store<br />
└── SQLServer/ - DataStore<br />
    └── Base/<br />
        └── ADBinteraction.cs       - Contains most of the code necessary to interact with the data store<br />

# Directories
## Interfaces<br />
&nbsp;&nbsp;&nbsp;&nbsp;The interfaces directory contains all of the interfaces for the project.  Most notablely the IDBInteraction interface followed by all of the "table" specific interfaces.

## "DataStore"<br />
&nbsp;&nbsp;&nbsp;&nbsp;The "DataStore" directory contains all of the concrete classes that implement the interfaces found in the Interfaces directory for the give data store. Currently only SQL Server (SQLServer) and Oracle (Oracle) are supported.

### Base<br />
&nbsp;&nbsp;&nbsp;&nbsp;The Base directory contains the ADBInteraction abstract class that performs the bulk of data store interaction.<br />
&nbsp;&nbsp;&nbsp;&nbsp;You will find a small amout of code that touches the data store in DDatabaseManager class.  At the time of writing DDatabaseManager stood as an outlying class and it's need to access the data store was unique so the code was not brought into the ADBInteraction base class.
