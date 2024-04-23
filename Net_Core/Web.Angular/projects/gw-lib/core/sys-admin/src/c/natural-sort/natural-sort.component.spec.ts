import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { NaturalSortComponent } from './natural-sort.component';

describe('NaturalSortComponent', () => {
	let component: NaturalSortComponent;
	let fixture: ComponentFixture<NaturalSortComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				NaturalSortComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(NaturalSortComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
