import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NaturalSortComponent } from './natural-sort.component';

describe('NaturalSortComponent', () => {
	let component: NaturalSortComponent;
	let fixture: ComponentFixture<NaturalSortComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [NaturalSortComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(NaturalSortComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
