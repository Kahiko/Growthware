import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectSecurityEntityComponent } from './select-security-entity.component';

describe('SelectSecurityEntityComponent', () => {
	let component: SelectSecurityEntityComponent;
	let fixture: ComponentFixture<SelectSecurityEntityComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SelectSecurityEntityComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SelectSecurityEntityComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
