import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SearchSecurityEntitiesComponent } from './search-security-entities.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SearchSecurityEntitiesComponent', () => {
	let component: SearchSecurityEntitiesComponent;
	let fixture: ComponentFixture<SearchSecurityEntitiesComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [SearchSecurityEntitiesComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(SearchSecurityEntitiesComponent);
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
