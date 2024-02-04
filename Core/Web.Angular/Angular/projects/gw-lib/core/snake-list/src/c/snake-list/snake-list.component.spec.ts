import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SnakeListComponent } from './snake-list.component';

describe('SnakeListComponent', () => {
	let component: SnakeListComponent;
	let fixture: ComponentFixture<SnakeListComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				SnakeListComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
    
		fixture = TestBed.createComponent(SnakeListComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
