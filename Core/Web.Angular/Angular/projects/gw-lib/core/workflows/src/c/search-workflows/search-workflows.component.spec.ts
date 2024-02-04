import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SearchWorkflowsComponent } from './search-workflows.component';

describe('SearchWorkflowsComponent', () => {
	let component: SearchWorkflowsComponent;
	let fixture: ComponentFixture<SearchWorkflowsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SearchWorkflowsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
		fixture = TestBed.createComponent(SearchWorkflowsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
