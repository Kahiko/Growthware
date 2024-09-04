import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HierarchicalVerticalComponent } from './hierarchical-vertical.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('HierarchicalVerticalComponent', () => {
	let component: HierarchicalVerticalComponent;
	let fixture: ComponentFixture<HierarchicalVerticalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [HierarchicalVerticalComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalVerticalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
