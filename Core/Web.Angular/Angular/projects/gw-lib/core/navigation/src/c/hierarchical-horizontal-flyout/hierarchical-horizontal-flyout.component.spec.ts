import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HierarchicalHorizontalFlyoutComponent } from './hierarchical-horizontal-flyout.component';

describe('HierarchicalHorizontalFlyoutComponent', () => {
	let component: HierarchicalHorizontalFlyoutComponent;
	let fixture: ComponentFixture<HierarchicalHorizontalFlyoutComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [HierarchicalHorizontalFlyoutComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalHorizontalFlyoutComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
