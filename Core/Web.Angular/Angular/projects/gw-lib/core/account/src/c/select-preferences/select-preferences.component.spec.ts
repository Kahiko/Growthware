import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPreferencesComponent } from './select-preferences.component';

describe('SelectPreferencesComponent', () => {
	let component: SelectPreferencesComponent;
	let fixture: ComponentFixture<SelectPreferencesComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SelectPreferencesComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(SelectPreferencesComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
