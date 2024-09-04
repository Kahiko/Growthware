import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { AccountDetailsComponent } from './account-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('AccountDetailsComponent', () => {
	let component: AccountDetailsComponent;
	let fixture: ComponentFixture<AccountDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [AccountDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();

		fixture = TestBed.createComponent(AccountDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
