import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateAnonymousProfileComponent } from './update-anonymous-profile.component';

describe('UpdateAnonymousProfileComponent', () => {
	let component: UpdateAnonymousProfileComponent;
	let fixture: ComponentFixture<UpdateAnonymousProfileComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [UpdateAnonymousProfileComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(UpdateAnonymousProfileComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
