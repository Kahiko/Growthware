import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDetailsComponent } from './group-details.component';

describe('GroupDetailsComponent', () => {
	let component: GroupDetailsComponent;
	let fixture: ComponentFixture<GroupDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [GroupDetailsComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(GroupDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
