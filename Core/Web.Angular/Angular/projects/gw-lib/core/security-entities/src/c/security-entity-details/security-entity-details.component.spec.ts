import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SecurityEntityDetailsComponent } from './security-entity-details.component';

describe('SecurityEntityDetailsComponent', () => {
	let component: SecurityEntityDetailsComponent;
	let fixture: ComponentFixture<SecurityEntityDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SecurityEntityDetailsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(SecurityEntityDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
