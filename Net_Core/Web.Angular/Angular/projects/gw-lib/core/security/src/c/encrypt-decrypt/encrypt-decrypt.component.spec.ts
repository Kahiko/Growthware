import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { EncryptDecryptComponent } from './encrypt-decrypt.component';

describe('EncryptDecryptComponent', () => {
	let component: EncryptDecryptComponent;
	let fixture: ComponentFixture<EncryptDecryptComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				EncryptDecryptComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(EncryptDecryptComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
