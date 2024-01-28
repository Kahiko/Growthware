import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetLogLevelComponent } from './set-log-level.component';

describe('SetLogLevelComponent', () => {
	let component: SetLogLevelComponent;
	let fixture: ComponentFixture<SetLogLevelComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SetLogLevelComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SetLogLevelComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
