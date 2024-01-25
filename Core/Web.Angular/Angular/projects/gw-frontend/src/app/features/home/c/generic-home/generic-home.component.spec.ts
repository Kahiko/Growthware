import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericHomeComponent } from './generic-home.component';

describe('GenericHomeComponent', () => {
	let component: GenericHomeComponent;
	let fixture: ComponentFixture<GenericHomeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [ GenericHomeComponent ]
		})
			.compileComponents();

		fixture = TestBed.createComponent(GenericHomeComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
