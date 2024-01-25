import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchMessagesComponent } from './search-messages.component';

describe('SearchMessagesComponent', () => {
	let component: SearchMessagesComponent;
	let fixture: ComponentFixture<SearchMessagesComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SearchMessagesComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SearchMessagesComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
