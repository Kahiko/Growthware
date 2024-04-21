import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { AddDirectoryComponent } from './add-directory.component';

describe('AddDirectoryComponent', () => {
	let component: AddDirectoryComponent;
	let fixture: ComponentFixture<AddDirectoryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				AddDirectoryComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(AddDirectoryComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
