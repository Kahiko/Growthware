import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArcFooterComponent } from './arc-footer.component';

describe('ArcFooterComponent', () => {
	let component: ArcFooterComponent;
	let fixture: ComponentFixture<ArcFooterComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [ ArcFooterComponent ]
		})
			.compileComponents();

		fixture = TestBed.createComponent(ArcFooterComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
