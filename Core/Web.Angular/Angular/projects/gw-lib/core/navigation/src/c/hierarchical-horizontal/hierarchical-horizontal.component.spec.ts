import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HierarchicalHorizontalComponent } from './hierarchical-horizontal.component';

describe('HierarchicalHorizontalComponent', () => {
	let component: HierarchicalHorizontalComponent;
	let fixture: ComponentFixture<HierarchicalHorizontalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [HierarchicalHorizontalComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalHorizontalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
