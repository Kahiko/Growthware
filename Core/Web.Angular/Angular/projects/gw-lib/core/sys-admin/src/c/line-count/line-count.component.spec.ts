import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { LineCountComponent } from './line-count.component';

describe('LineCountComponent', () => {
	let component: LineCountComponent;
	let fixture: ComponentFixture<LineCountComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				LineCountComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(LineCountComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
