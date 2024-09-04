import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SelectSecurityEntityComponent } from './select-security-entity.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SelectSecurityEntityComponent', () => {
	let component: SelectSecurityEntityComponent;
	let fixture: ComponentFixture<SelectSecurityEntityComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [SelectSecurityEntityComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(SelectSecurityEntityComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
