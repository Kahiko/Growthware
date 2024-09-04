import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FileManagerComponent } from './file-manager.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('FileManagerComponent', () => {
	let component: FileManagerComponent;
	let fixture: ComponentFixture<FileManagerComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [FileManagerComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(FileManagerComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
