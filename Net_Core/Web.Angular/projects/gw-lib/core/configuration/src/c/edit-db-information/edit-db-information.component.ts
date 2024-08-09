import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { IKeyValuePair } from '@growthware/common/interfaces';
// Feature
import { ConfigurationService } from '../../configuration.service';
import { DBInformation, IDBInformation } from '../../db-information.model';

@Component({
	selector: 'gw-core-edit-db-information',
	standalone: true,
	imports: [
		FormsModule,
		
		MatButtonModule,
		MatSelectModule,
		MatTabsModule,

		ReactiveFormsModule
	],
	templateUrl: './edit-db-information.component.html',
	styleUrl: './edit-db-information.component.scss'
})
export class EditDbInformationComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {	
	private _Profile: IDBInformation = new DBInformation();

	validInheritanceTypes: IKeyValuePair[] = [
		{key: 0, value: 'False'},
		{key: 1, value: 'True'}
	];

	selectedInheritanceType: number = 1;

	version: string = '';

	constructor(
		private configurationService: ConfigurationService,
		private _FormBuilder: FormBuilder,
	) {
		/** 
		 * The component is use full when your a testing security aspects of the system.
		 * Really this is over engineering and a simple T-SQL update statement
		 * is all that is needed.
		*/
		super();
		this._ProfileSvc = configurationService;
	}

	ngOnInit(): void {
		// console.log('EditDbInformationComponent.ngOnInit', 'Called');
		this._ProfileSvc.getDBInformation().then((response: IDBInformation) => {
			this._Profile = response;
			this.populateForm();
		}).catch((error: unknown) => {
			console.error('EditDbInformationComponent.ngOnInit', error);
		});
		this.createForm();
	}

	override delete(): void {
		throw new Error('Method not implemented.');
	}

	/**
	 * Creates this.frmProfile
	 */
	override createForm(): void {
		// in this simple case this is not necessary b/c we don't actually use the form, but 
		// it's here to adher to the BaseDetailComponent interface
		this.frmProfile = this._FormBuilder.group({});
	}

	populateForm(): void {
		this.selectedInheritanceType = this._Profile.enableInheritance;
	}

	/**
	 * Populates this._Profile with data from the from, called by BaseDetailComponent.onSubmit
	 */
	override populateProfile(): void {
		this._Profile.enableInheritance = this.selectedInheritanceType;
	}

	/**
	 * Saves the data, called by BaseDetailComponent.onSubmit
	 */
	override save(): void {
		// console.log('EditDbInformationComponent.save', this._Profile);
		this._ProfileSvc.save(this._Profile.enableInheritance);
	}

}
