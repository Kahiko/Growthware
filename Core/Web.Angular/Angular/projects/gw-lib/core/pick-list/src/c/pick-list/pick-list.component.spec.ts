import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { PickListComponent } from './pick-list.component';

describe('PickListComponent', () => {
	let component: PickListComponent;
	let fixture: ComponentFixture<PickListComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				PickListComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(PickListComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
