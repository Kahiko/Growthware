import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { SysAdminService } from './sys-admin.service';

describe('SysAdminService', () => {
	let service: SysAdminService;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				RouterTestingModule,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
		service = TestBed.inject(SysAdminService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});