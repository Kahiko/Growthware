import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageValuePairDetailsComponent } from './manage-value-pair-details.component';

describe('ManageValuePairDetailsComponent', () => {
	let component: ManageValuePairDetailsComponent;
	let fixture: ComponentFixture<ManageValuePairDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ManageValuePairDetailsComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(ManageValuePairDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
