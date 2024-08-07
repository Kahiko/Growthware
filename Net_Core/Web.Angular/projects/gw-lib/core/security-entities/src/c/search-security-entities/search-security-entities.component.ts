import { Component } from '@angular/core';

// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { ConfigurationService } from '@growthware/core/configuration';
import { DynamicTableComponent, DynamicTableService } from '@growthware/core/dynamic-table';
import { DataService } from '@growthware/common/services';
import { SearchService } from '@growthware/core/search';
import { ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { SecurityEntityDetailsComponent } from '../security-entity-details/security-entity-details.component';
import { SecurityEntityService } from '../../security-entity.service';

@Component({
	selector: 'gw-core-search-security-entities',
	standalone: true,
	imports: [
		DynamicTableComponent
	],
	templateUrl: './search-security-entities.component.html',
	styleUrls: ['./search-security-entities.component.scss']
})
export class SearchSecurityEntitiesComponent extends BaseSearchComponent {

	securityEntityTranslation: string = 'Security Entity';

	constructor(
    private _ConfigurationSvc: ConfigurationService,
    theFeatureSvc: SecurityEntityService,
    dataSvc: DataService,
    dynamicTableSvc: DynamicTableService,
    modalSvc: ModalService,
    searchSvc: SearchService,
	) { 
		super();
		this.configurationName = 'Security_Entities';
		this._TheFeatureName = 'Security Entity';
		this._TheApi = 'GrowthwareSecurityEntity/Security_Entities';
		this._TheComponent = SecurityEntityDetailsComponent;
		this._TheWindowSize = new WindowSize(630,775);
		this._TheService = theFeatureSvc;
		this._DataSvc = dataSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}

	override init(): void {
		this._Subscription.add(
			this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { this.securityEntityTranslation = val;})
		);
	}
}
