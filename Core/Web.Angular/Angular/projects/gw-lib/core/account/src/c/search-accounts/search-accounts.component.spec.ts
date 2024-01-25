import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchAccountsComponent } from './search-accounts.component';

describe('SearchAccountsComponent', () => {
	let component: SearchAccountsComponent;
	let fixture: ComponentFixture<SearchAccountsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SearchAccountsComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SearchAccountsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
