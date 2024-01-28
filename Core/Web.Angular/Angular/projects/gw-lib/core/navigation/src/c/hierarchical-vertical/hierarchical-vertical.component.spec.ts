import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HierarchicalVerticalComponent } from './hierarchical-vertical.component';

describe('HierarchicalVerticalComponent', () => {
	let component: HierarchicalVerticalComponent;
	let fixture: ComponentFixture<HierarchicalVerticalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [HierarchicalVerticalComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalVerticalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
