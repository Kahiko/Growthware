import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchWorkflowsComponent } from './search-workflows.component';

describe('SearchWorkflowsComponent', () => {
	let component: SearchWorkflowsComponent;
	let fixture: ComponentFixture<SearchWorkflowsComponent>;

	beforeEach(() => {
		TestBed.configureTestingModule({
			declarations: [SearchWorkflowsComponent]
		});
		fixture = TestBed.createComponent(SearchWorkflowsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
