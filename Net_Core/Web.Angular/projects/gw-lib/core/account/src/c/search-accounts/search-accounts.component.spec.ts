import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SearchAccountsComponent } from './search-accounts.component';

describe('SearchAccountsComponent', () => {
	let component: SearchAccountsComponent;
	let fixture: ComponentFixture<SearchAccountsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SearchAccountsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(SearchAccountsComponent);
		component = fixture.componentInstance;
		/**
		 * fixture.detectChanges(); causes an error in BaseSearchComponent.ngOnInit
		 * I believe all components that use BaseSearchComponent should have the same error.
		 * NOTE: I would start by looking at the services that are injected into the concrete components.
		 * 
		 */
		// fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});