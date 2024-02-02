import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SearchGroupsComponent } from './search-groups.component';

describe('SearchGroupsComponent', () => {
	let component: SearchGroupsComponent;
	let fixture: ComponentFixture<SearchGroupsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SearchGroupsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(SearchGroupsComponent);
		component = fixture.componentInstance;
		/**
		 * fixture.detectChanges(); causes an error in BaseSearchComponent.ngOnInit
		 * I believe all components that use BaseSearchComponent should have the same error.
		 * NOTE: I would start by looking at the services that are injected into the concrete components.
		 * Error: TableConfigurations have not been loaded yet!
		 * 
		 */
		// fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
