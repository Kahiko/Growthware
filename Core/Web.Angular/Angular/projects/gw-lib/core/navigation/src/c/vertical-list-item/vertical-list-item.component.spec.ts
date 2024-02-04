import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
// Feature
import { NavLink } from '../../nav-link.model';
import { VerticalListItemComponent } from './vertical-list-item.component';

describe('VerticalListItemComponent', () => {
	let component: VerticalListItemComponent;
	let fixture: ComponentFixture<VerticalListItemComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				VerticalListItemComponent,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
		
		fixture = TestBed.createComponent(VerticalListItemComponent);
		component = fixture.componentInstance;
		component.item = new NavLink('', '', '', '', '', 0, '', true, '');
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
