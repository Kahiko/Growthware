import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FunctionDetailsComponent } from './function-details.component';

describe('FunctionDetailsComponent', () => {
	let component: FunctionDetailsComponent;
	let fixture: ComponentFixture<FunctionDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				FunctionDetailsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(FunctionDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
