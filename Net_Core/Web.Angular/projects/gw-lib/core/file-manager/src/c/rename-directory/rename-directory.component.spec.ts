import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { RenameDirectoryComponent } from './rename-directory.component';

describe('RenameDirectoryComponent', () => {
	let component: RenameDirectoryComponent;
	let fixture: ComponentFixture<RenameDirectoryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				RenameDirectoryComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(RenameDirectoryComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});