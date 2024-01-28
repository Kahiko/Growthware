import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecurityEntityDetailsComponent } from './security-entity-details.component';

describe('SecurityEntityDetailsComponent', () => {
	let component: SecurityEntityDetailsComponent;
	let fixture: ComponentFixture<SecurityEntityDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SecurityEntityDetailsComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SecurityEntityDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
