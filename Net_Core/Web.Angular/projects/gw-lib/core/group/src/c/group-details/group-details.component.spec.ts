import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { GroupDetailsComponent } from './group-details.component';

describe('GroupDetailsComponent', () => {
	let component: GroupDetailsComponent;
	let fixture: ComponentFixture<GroupDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				GroupDetailsComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(GroupDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
