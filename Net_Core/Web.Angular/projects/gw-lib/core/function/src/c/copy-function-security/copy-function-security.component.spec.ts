import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { CopyFunctionSecurityComponent } from './copy-function-security.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('CopyFunctionSecurityComponent', () => {
	let component: CopyFunctionSecurityComponent;
	let fixture: ComponentFixture<CopyFunctionSecurityComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [CopyFunctionSecurityComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(CopyFunctionSecurityComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
