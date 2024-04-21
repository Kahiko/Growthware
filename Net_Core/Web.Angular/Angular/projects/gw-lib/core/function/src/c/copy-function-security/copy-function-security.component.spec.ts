import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { CopyFunctionSecurityComponent } from './copy-function-security.component';

describe('CopyFunctionSecurityComponent', () => {
	let component: CopyFunctionSecurityComponent;
	let fixture: ComponentFixture<CopyFunctionSecurityComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				CopyFunctionSecurityComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(CopyFunctionSecurityComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
