import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FunctionDetailsComponent } from './function-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('FunctionDetailsComponent', () => {
	let component: FunctionDetailsComponent;
	let fixture: ComponentFixture<FunctionDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [FunctionDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(FunctionDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
