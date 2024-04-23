import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { AccountDetailsComponent } from './account-details.component';

describe('AccountDetailsComponent', () => {
	let component: AccountDetailsComponent;
	let fixture: ComponentFixture<AccountDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				AccountDetailsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();

		fixture = TestBed.createComponent(AccountDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
