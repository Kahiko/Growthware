import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FileListComponent } from './file-list.component';

describe('FileListComponent', () => {
	let component: FileListComponent;
	let fixture: ComponentFixture<FileListComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				FileListComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(FileListComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
