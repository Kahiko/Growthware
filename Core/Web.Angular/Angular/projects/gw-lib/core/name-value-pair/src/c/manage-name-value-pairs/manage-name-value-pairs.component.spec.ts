import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageNameValuePairsComponent } from './manage-name-value-pairs.component';

describe('ManageNameValuePairsComponent', () => {
	let component: ManageNameValuePairsComponent;
	let fixture: ComponentFixture<ManageNameValuePairsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ManageNameValuePairsComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(ManageNameValuePairsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
