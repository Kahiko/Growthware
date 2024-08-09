import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { IKeyValuePair, KeyValuePair } from '@growthware/common/interfaces';
import { SecurityEntityService } from '@growthware/core/security-entities';
// Feature
import { FunctionService } from '../../function.service';

@Component({
	selector: 'gw-core-copy-function-security',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		MatSelectModule,
		MatTabsModule
	],
	templateUrl: './copy-function-security.component.html',
	styleUrls: ['./copy-function-security.component.scss']
})
export class CopyFunctionSecurityComponent implements OnInit {

	frmProfile!: FormGroup;
	selectedSource: number = -1;
	selectedTarget: number = -1;

	validSourceEntities: IKeyValuePair[] = [];
	validTargetEntities: IKeyValuePair[] = [];

	constructor(
    private _FormBuilder: FormBuilder,
    private _FunctionSvc: FunctionService,
    private _LoggingSvc: LoggingService,
    private _SecuritySvc: SecurityEntityService
	) { 
		this.createForm();
	}

	ngOnInit(): void {
		this._SecuritySvc.getValidParents(1).then((securityEntities) => {
			this.validSourceEntities = JSON.parse(JSON.stringify(securityEntities));
			this.validTargetEntities = JSON.parse(JSON.stringify(securityEntities));
			const mSystem: IKeyValuePair = new KeyValuePair();
			mSystem.key = 1;
			mSystem.value = 'System';
			this.validSourceEntities.push(mSystem);
			this.validSourceEntities = GWCommon.sortArray(this.validSourceEntities, 'value', 'desc');
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'CopyFunctionSecurityComponent', 'ngOnInit');
		});
	}

	createForm(): void {
		this.frmProfile = this._FormBuilder.group({});
	}

	onSubmit(): void {
		if(this.selectedSource != this.selectedTarget) {
			this._FunctionSvc.copyFunctionSecurity(this.selectedSource, this.selectedTarget).then((response: boolean) => {
				if (response) {
					this._LoggingSvc.toast('Function Security has been copied', 'Copy Function:', LogLevel.Success);
				} else {
					this._LoggingSvc.toast('Function Security could not be copied!', 'Copy Function:', LogLevel.Error);
				}
			});
		} else {
			this._LoggingSvc.toast('The source can not be equal to the target!', 'Copy Function:', LogLevel.Error);
		}
	}
}
