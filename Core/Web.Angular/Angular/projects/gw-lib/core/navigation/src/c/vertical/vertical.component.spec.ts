import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerticalComponent } from './vertical.component';

describe('VerticalComponent', () => {
	let component: VerticalComponent;
	let fixture: ComponentFixture<VerticalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [VerticalComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(VerticalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
