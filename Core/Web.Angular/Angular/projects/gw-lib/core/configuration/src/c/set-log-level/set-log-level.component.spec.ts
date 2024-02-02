import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SetLogLevelComponent } from './set-log-level.component';

describe('SetLogLevelComponent', () => {
	let component: SetLogLevelComponent;
	let fixture: ComponentFixture<SetLogLevelComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SetLogLevelComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SetLogLevelComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
