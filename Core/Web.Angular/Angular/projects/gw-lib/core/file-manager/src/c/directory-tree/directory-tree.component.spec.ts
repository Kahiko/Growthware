import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { DirectoryTreeComponent } from './directory-tree.component';

describe('DirectoryTreeComponent', () => {
	let component: DirectoryTreeComponent;
	let fixture: ComponentFixture<DirectoryTreeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				DirectoryTreeComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			]
		}).compileComponents();
    
		fixture = TestBed.createComponent(DirectoryTreeComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
