import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSessionComponent } from './update-session.component';

describe('UpdateSessionComponent', () => {
	let component: UpdateSessionComponent;
	let fixture: ComponentFixture<UpdateSessionComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [UpdateSessionComponent]
		}).compileComponents();
    
		fixture = TestBed.createComponent(UpdateSessionComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
