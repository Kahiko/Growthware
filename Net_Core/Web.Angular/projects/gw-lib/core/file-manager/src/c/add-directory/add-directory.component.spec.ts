import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { AddDirectoryComponent } from './add-directory.component';
import { provideHttpClient, withInterceptorsFromDi, withXhr } from '@angular/common/http';

describe('AddDirectoryComponent', () => {
	let component: AddDirectoryComponent;
	let fixture: ComponentFixture<AddDirectoryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [AddDirectoryComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withXhr(), withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(AddDirectoryComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
