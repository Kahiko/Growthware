import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { DirectoryTreeComponent } from './directory-tree.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('DirectoryTreeComponent', () => {
	let component: DirectoryTreeComponent;
	let fixture: ComponentFixture<DirectoryTreeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [DirectoryTreeComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(DirectoryTreeComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
