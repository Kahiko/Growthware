import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SearchfunctionsComponent } from './searchfunctions.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SearchfunctionsComponent', () => {
	let component: SearchfunctionsComponent;
	let fixture: ComponentFixture<SearchfunctionsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [SearchfunctionsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
			.compileComponents();
    
		fixture = TestBed.createComponent(SearchfunctionsComponent);
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
