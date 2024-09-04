import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FileListComponent } from './file-list.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('FileListComponent', () => {
	let component: FileListComponent;
	let fixture: ComponentFixture<FileListComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [FileListComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(FileListComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
