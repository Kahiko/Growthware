import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { PagerComponent } from './pager.component';

describe('PagerComponent', () => {
	let component: PagerComponent;
	let fixture: ComponentFixture<PagerComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				PagerComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(PagerComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
