import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HierarchicalHorizontalComponent } from './hierarchical-horizontal.component';

describe('HierarchicalHorizontalComponent', () => {
	let component: HierarchicalHorizontalComponent;
	let fixture: ComponentFixture<HierarchicalHorizontalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				HierarchicalHorizontalComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalHorizontalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
