import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageDetailsComponent } from './message-details.component';

describe('MessageDetailsComponent', () => {
	let component: MessageDetailsComponent;
	let fixture: ComponentFixture<MessageDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [MessageDetailsComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(MessageDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
