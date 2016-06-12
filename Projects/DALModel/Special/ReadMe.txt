The special folder contains the objects (MClientChoices and MAccountProfileInfo),
though they are key to the base applcation they are also expected to change
with the business requirements.

Should the MAccountProfileInfo object need to change then 
BaseDALSQLServer.DClient.PopulateProfileFromSqlDataReader()
method must also reflect the changes.

Currently ClientChoicesState only contains string types or nvarchar of a length
of 1000, being so the code to update loops through all the keys
and creates the sql parameter and value.  The MClientChoices only aids a 
developer with typeing by allowing them to choose what they by
reading the property rather than having to always know the exact key
name they are looking for.