import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HierarchicalVerticalFlyoutComponent } from './hierarchical-vertical-flyout.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('HierarchicalVerticalFlyoutComponent', () => {
	let component: HierarchicalVerticalFlyoutComponent;
	let fixture: ComponentFixture<HierarchicalVerticalFlyoutComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [HierarchicalVerticalFlyoutComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalVerticalFlyoutComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
