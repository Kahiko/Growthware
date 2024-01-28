import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchRolesComponent } from './search-roles.component';

describe('SearchRolesComponent', () => {
	let component: SearchRolesComponent;
	let fixture: ComponentFixture<SearchRolesComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SearchRolesComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SearchRolesComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
