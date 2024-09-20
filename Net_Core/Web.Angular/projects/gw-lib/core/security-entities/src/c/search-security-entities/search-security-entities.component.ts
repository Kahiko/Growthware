import { Component, computed } from '@angular/core';
// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { ConfigurationService } from '@growthware/core/configuration';
import { DynamicTableComponent, DynamicTableService } from '@growthware/core/dynamic-table';
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

	securityEntityTranslation = computed(() => this._ConfigurationSvc.securityEntityTranslation());

	constructor(
		private _ConfigurationSvc: ConfigurationService,
		theFeatureSvc: SecurityEntityService,
		dynamicTableSvc: DynamicTableService,
		modalSvc: ModalService,
		searchSvc: SearchService,
	) {
		super();
		this.configurationName = 'Security_Entities';
		this._TheFeatureName = 'Security Entity';
		this._TheApi = 'GrowthwareSecurityEntity/Security_Entities';
		this._TheComponent = SecurityEntityDetailsComponent;
		this._TheWindowSize = new WindowSize(630, 775);
		this._TheService = theFeatureSvc;
		this._DynamicTableSvc = dynamicTableSvc;
		this._ModalSvc = modalSvc;
		this._SearchSvc = searchSvc;
	}

	override init(): void {
		super.init();
	}
}
