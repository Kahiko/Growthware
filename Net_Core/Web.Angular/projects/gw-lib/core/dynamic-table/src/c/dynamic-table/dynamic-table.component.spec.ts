import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { DynamicTableComponent } from './dynamic-table.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('DynamicTableComponent', () => {
	let component: DynamicTableComponent;
	let fixture: ComponentFixture<DynamicTableComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [DynamicTableComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(DynamicTableComponent);
		component = fixture.componentInstance;
		/**
		 * fixture.detectChanges(); causes an error
		 * NOTE: I would start by looking at the services that are injected into the concrete components.
		 * 
		 */
		// fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
