import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SelectPreferencesComponent } from './select-preferences.component';

describe('SelectPreferencesComponent', () => {
	let component: SelectPreferencesComponent;
	let fixture: ComponentFixture<SelectPreferencesComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SelectPreferencesComponent,
				HttpClientTestingModule,
				NoopAnimationsModule
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(SelectPreferencesComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
