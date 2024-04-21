import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HierarchicalVerticalFlyoutComponent } from './hierarchical-vertical-flyout.component';

describe('HierarchicalVerticalFlyoutComponent', () => {
	let component: HierarchicalVerticalFlyoutComponent;
	let fixture: ComponentFixture<HierarchicalVerticalFlyoutComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				HierarchicalVerticalFlyoutComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalVerticalFlyoutComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
