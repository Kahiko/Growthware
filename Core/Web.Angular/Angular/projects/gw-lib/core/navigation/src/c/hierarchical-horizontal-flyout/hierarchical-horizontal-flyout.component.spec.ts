import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HierarchicalHorizontalFlyoutComponent } from './hierarchical-horizontal-flyout.component';

describe('HierarchicalHorizontalFlyoutComponent', () => {
	let component: HierarchicalHorizontalFlyoutComponent;
	let fixture: ComponentFixture<HierarchicalHorizontalFlyoutComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				HierarchicalHorizontalFlyoutComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalHorizontalFlyoutComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
