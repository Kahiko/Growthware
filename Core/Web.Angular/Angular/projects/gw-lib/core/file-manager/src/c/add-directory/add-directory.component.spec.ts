import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDirectoryComponent } from './add-directory.component';

describe('AddDirectoryComponent', () => {
	let component: AddDirectoryComponent;
	let fixture: ComponentFixture<AddDirectoryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [AddDirectoryComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(AddDirectoryComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
