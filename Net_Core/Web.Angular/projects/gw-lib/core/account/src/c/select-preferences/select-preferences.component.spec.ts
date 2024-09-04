import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SelectPreferencesComponent } from './select-preferences.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SelectPreferencesComponent', () => {
	let component: SelectPreferencesComponent;
	let fixture: ComponentFixture<SelectPreferencesComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [SelectPreferencesComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(SelectPreferencesComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
