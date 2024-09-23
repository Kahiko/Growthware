import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
// Feature
import { NavLink } from '../../nav-link.model';
import { HierarchicalNavListItemComponent } from './hierarchical-nav-list-item';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('HierarchicalNavListItemComponent', () => {
	let component: HierarchicalNavListItemComponent;
	let fixture: ComponentFixture<HierarchicalNavListItemComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [HierarchicalNavListItemComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
		
		fixture = TestBed.createComponent(HierarchicalNavListItemComponent);
		component = fixture.componentInstance;
		component.item.apply(new NavLink('', '', '', '', '', 0, '', true, ''));
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
