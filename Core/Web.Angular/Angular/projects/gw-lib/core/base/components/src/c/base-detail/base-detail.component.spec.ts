import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseDetailComponent } from './base-detail.component';

describe('BaseDetailComponent', () => {
	let component: BaseDetailComponent;
	let fixture: ComponentFixture<BaseDetailComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [BaseDetailComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(BaseDetailComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
