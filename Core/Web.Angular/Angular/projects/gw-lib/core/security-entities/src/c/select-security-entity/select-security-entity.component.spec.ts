import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SelectSecurityEntityComponent } from './select-security-entity.component';

describe('SelectSecurityEntityComponent', () => {
	let component: SelectSecurityEntityComponent;
	let fixture: ComponentFixture<SelectSecurityEntityComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SelectSecurityEntityComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(SelectSecurityEntityComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
