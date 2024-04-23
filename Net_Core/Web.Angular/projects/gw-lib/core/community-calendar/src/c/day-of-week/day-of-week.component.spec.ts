import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { DayOfWeekComponent } from './day-of-week.component';

describe('DayOfWeekComponent', () => {
	let component: DayOfWeekComponent;
	let fixture: ComponentFixture<DayOfWeekComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				DayOfWeekComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(DayOfWeekComponent);
		component = fixture.componentInstance;
		// fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
