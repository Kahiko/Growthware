import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { ChangePasswordComponent } from './change-password.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('ChangePasswordComponent', () => {
	let component: ChangePasswordComponent;
	let fixture: ComponentFixture<ChangePasswordComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [ChangePasswordComponent,
        RouterTestingModule.withRoutes([]),
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
			.compileComponents();
    
		fixture = TestBed.createComponent(ChangePasswordComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
