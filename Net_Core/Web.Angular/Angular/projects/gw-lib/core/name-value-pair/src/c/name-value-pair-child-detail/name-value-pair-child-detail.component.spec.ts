import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NameValuePairChildDetailComponent } from './name-value-pair-child-detail.component';

describe('NameValuePairChildDetailComponent', () => {
	let component: NameValuePairChildDetailComponent;
	let fixture: ComponentFixture<NameValuePairChildDetailComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [NameValuePairChildDetailComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(NameValuePairChildDetailComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
