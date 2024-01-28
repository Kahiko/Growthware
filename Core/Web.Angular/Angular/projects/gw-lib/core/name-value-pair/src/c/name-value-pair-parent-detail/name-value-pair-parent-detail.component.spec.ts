import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NameValuePairParentDetailComponent } from './name-value-pair-parent-detail.component';

describe('NameValuePairParentDetailComponent', () => {
	let component: NameValuePairParentDetailComponent;
	let fixture: ComponentFixture<NameValuePairParentDetailComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [NameValuePairParentDetailComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(NameValuePairParentDetailComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
