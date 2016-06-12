Hi,

Here is how the ClientChoices project works.

1.) The web.config file initiates the ClientChoicesModule
	<httpModules>
		<!--Initiates the ClientChoices Modules when application loades." /-->
		<add name="ClientChoices" type="BaseClientChoices.ClientChoicesModule,BaseClientChoices" />
	</httpModules>
2.) Pages/Modules that need acces to this create an instance to the
	ClientChoicesState by either inheriting the appropriate
	ClientChoices ControlModule/PageModule or by creating a direct
	instance:

		Imports BLL.Base.ClientChoices

		Dim myState as string = string.empty
		Dim myClientChoices As New ClientChoicesState(context.Current.User.Identity.Name)
		myState = myClientChoices(MClientChoices.SelectedState)
NOTE:::
Before you choose to create your own instance the the ClientChoicesState
keep in mind that by inheriting the ClientChoicesState you also gain session
time out handeling through the BasePage.vb file

MClientChoices.vb from the DALModel project acts as a buffer to the items
contained in the ClientChoicesState by having string value properties
that represent column names in the TBL_CLIENT_CHOICES table of the data store.

This is not completely necessary you could just do one of the following:
ClientChoicesState("key")
ClientChoicesState.item("key")

The MClientChoices just makes it easier to change a column name.  Should 
the column name change and you have used MClientChoices then all that's
necessary is to change the MClientChoices.vb file

Here are the store proceedures necessary:
add_Account_Choices
get_Account_ChoicesInfo
update_Client_Choices
Though it is possible to find this information out by examining the DALSQLServer
The store proceedures (in particular the get_Client_ChoicesInfo, 
update_Client_Choices) are most important to adding choices.
Add the choice to the table in the form of a column
change the store proceedures for adding/updating the data store,
change the MClientChoices and that's it.

The BasePage is a way of adding something that is necessary to all pages
in the application in a single location.  Currently this does not do much.