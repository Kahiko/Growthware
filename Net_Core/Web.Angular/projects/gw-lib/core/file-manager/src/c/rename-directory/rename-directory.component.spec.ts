import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { RenameDirectoryComponent } from './rename-directory.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('RenameDirectoryComponent', () => {
	let component: RenameDirectoryComponent;
	let fixture: ComponentFixture<RenameDirectoryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [RenameDirectoryComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(RenameDirectoryComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
