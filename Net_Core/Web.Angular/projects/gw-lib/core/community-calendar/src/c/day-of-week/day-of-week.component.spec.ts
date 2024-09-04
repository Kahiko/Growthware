import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { DayOfWeekComponent } from './day-of-week.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('DayOfWeekComponent', () => {
	let component: DayOfWeekComponent;
	let fixture: ComponentFixture<DayOfWeekComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [DayOfWeekComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(DayOfWeekComponent);
		component = fixture.componentInstance;
		// fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
