To use the picklist add the following to your component
    you will need to import PickListModule in either your xx.module or your component if it is a standalone
Add the following to your HTML:
    <gw-lib-pick-list #rolePickList
        id="roles"
        name="{{rolesPickListName}}"
        allItemsText="Available Roles"
        header="Assign roles"
        pickListTableHelp="Select roles from either the 'Available Roles' or the 'Selected Roles' then use the center arrow buttons to move the role for one list to the other."
        selectedItemsText="Selected Roles"
        size=8
        width=120>
    </gw-lib-pick-list>
You'll need to do the following in your component class
    1.) Import the data service from the library:
        import { DataService } from '@Growthware/shared/services';

    2.) Import Subscription from rxjs
        import { Subscription } from 'rxjs';

    3.) Implement OnInit and subscribe to the dataservices's dataChanged
            this._Subscription.add(this._DataSvc.dataChanged.subscribe((data) => {
            // console.log('GroupDetailsComponent.ngOnInit',data.name.toLowerCase()); // used to determine the data name 
            switch (data.name.toLowerCase()) {
                case 'roles':
                    // set the paload to whatever you are using to track the "selected" items
                    this._GroupProfile.rolesInGroup = data.payLoad;
                    break;
                default:
                    break;
            }
            }));

    4.) Implement OnDestroy and unsubscribe to the subscription
            ngOnDestroy(): void {
                // this will unsubscribe to all registered subscriptions in a single go.
                this._Subscription.unsubscribe();
            }
