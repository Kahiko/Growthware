import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { GuidHelperComponent } from './guid-helper.component';

describe('GuidHelperComponent', () => {
	let component: GuidHelperComponent;
	let fixture: ComponentFixture<GuidHelperComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				GuidHelperComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(GuidHelperComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
