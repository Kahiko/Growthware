import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HorizontalComponent } from './horizontal.component';

describe('HorizontalComponent', () => {
	let component: HorizontalComponent;
	let fixture: ComponentFixture<HorizontalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				HorizontalComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(HorizontalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
