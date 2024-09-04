import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { WorkflowDetailsComponent } from './workflow-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('WorkflowDetailsComponent', () => {
	let component: WorkflowDetailsComponent;
	let fixture: ComponentFixture<WorkflowDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [WorkflowDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
		fixture = TestBed.createComponent(WorkflowDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
