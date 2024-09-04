import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SecurityEntityDetailsComponent } from './security-entity-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SecurityEntityDetailsComponent', () => {
	let component: SecurityEntityDetailsComponent;
	let fixture: ComponentFixture<SecurityEntityDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [SecurityEntityDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(SecurityEntityDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
