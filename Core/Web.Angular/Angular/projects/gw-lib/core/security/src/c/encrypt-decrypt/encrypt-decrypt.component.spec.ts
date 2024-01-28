import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EncryptDecryptComponent } from './encrypt-decrypt.component';

describe('EncryptDecryptComponent', () => {
	let component: EncryptDecryptComponent;
	let fixture: ComponentFixture<EncryptDecryptComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [EncryptDecryptComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(EncryptDecryptComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
