import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultFooterComponent } from './default-footer.component';

describe('DefaultFooterComponent', () => {
	let component: DefaultFooterComponent;
	let fixture: ComponentFixture<DefaultFooterComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [ DefaultFooterComponent ]
		})
			.compileComponents();

		fixture = TestBed.createComponent(DefaultFooterComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});