import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { UploadComponent } from './upload.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('UploadComponent', () => {
	let component: UploadComponent;
	let fixture: ComponentFixture<UploadComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [UploadComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(UploadComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
