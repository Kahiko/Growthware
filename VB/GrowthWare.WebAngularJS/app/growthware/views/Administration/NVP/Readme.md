This will require some more though ... If memeory serves this is a two search process:
	1.) You "search" the the name value pair "table" 
	2.) You click on the "edit" link you need to search for the values in the "table"
From the second search when you click on the "edit" link the AddEditNVPDetails.html would be
displayed in a modal

This all comes from the fact that creating a "Name Value Pair" creates an actual table in the
database, and it's that table that contains the actual name value pairs:

SELECT
	 [NVP_Detail_SeqID]
	,[NVP_SeqID]
	,[NVP_Detail_Name]
	,[NVP_Detail_Value]
	,[Status_SeqID]
	,[Sort_Order]
	,[Added_By]
	,[Added_Date]
	,[Updated_By]
	,[Updated_Date]
FROM [ZGWSecurity].[Navigation_Types]

NVP_Detail_SeqID	NVP_SeqID	NVP_Detail_Name	NVP_Detail_Value	Status_SeqID	Sort_Order	Added_By	Added_Date				Updated_By	Updated_Date
1					1			Horizontal		Horizontal			1				0			2			2021-11-06 07:24:00.000	NULL		NULL
2					1			Vertical		Vertical			1				0			2			2021-11-06 07:24:00.000	NULL		NULL
3					1			Hierarchical	Hierarchical		1				0			2			2021-11-06 07:24:00.000	NULL		NULL

At the time of this writing I was considering adding a data element in the Search.config.json file where
we could define the navigation type (transition or modal for example)
at that point we could then transition to the secondary "search" and that could bring
up the normal modal popup "add/edit" page

hmm there my be a need to add what to transition to after the edit as well.... just thinking