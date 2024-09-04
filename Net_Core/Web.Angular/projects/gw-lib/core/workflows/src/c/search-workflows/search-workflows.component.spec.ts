import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SearchWorkflowsComponent } from './search-workflows.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SearchWorkflowsComponent', () => {
	let component: SearchWorkflowsComponent;
	let fixture: ComponentFixture<SearchWorkflowsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [SearchWorkflowsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
		fixture = TestBed.createComponent(SearchWorkflowsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
