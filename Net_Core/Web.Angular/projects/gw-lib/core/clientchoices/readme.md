The ClientChoices feature was created to allow a non-circular dependency between the GroupDetailsComponent and RoleDetailsComponent.

The GroupDetails and RoleDetails components for example are used in the AccountDetailsComponent so adding a reference or import to the "ClientChoices" in either of them results in a circular dependency.

By moving the IClientChoices and ClientChoices object and adding a service to return an ClientChoices object in to a seporate feature, we avoid this issue.

Management of the client choices data is still processed in the AccountService because ultimately the Client Choices are closly linked to the account.