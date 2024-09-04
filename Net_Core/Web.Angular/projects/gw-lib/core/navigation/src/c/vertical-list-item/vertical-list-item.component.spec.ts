import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
// Feature
import { NavLink } from '../../nav-link.model';
import { VerticalListItemComponent } from './vertical-list-item.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('VerticalListItemComponent', () => {
	let component: VerticalListItemComponent;
	let fixture: ComponentFixture<VerticalListItemComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [VerticalListItemComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
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
