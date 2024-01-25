import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenameDirectoryComponent } from './rename-directory.component';

describe('RenameDirectoryComponent', () => {
	let component: RenameDirectoryComponent;
	let fixture: ComponentFixture<RenameDirectoryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [RenameDirectoryComponent]
		})
			.compileComponents();
    
		fixture = TestBed.createComponent(RenameDirectoryComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
