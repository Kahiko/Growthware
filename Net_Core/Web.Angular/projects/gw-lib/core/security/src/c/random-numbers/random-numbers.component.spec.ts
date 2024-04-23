import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { RandomNumbersComponent } from './random-numbers.component';

describe('RandomNumbersComponent', () => {
	let component: RandomNumbersComponent;
	let fixture: ComponentFixture<RandomNumbersComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				RandomNumbersComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(RandomNumbersComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
