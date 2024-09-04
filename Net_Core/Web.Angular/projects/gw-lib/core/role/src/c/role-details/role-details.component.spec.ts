import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { RoleDetailsComponent } from './role-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('RoleDetailsComponent', () => {
	let component: RoleDetailsComponent;
	let fixture: ComponentFixture<RoleDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [RoleDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(RoleDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
