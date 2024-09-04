import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { HierarchicalHorizontalComponent } from './hierarchical-horizontal.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('HierarchicalHorizontalComponent', () => {
	let component: HierarchicalHorizontalComponent;
	let fixture: ComponentFixture<HierarchicalHorizontalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [HierarchicalHorizontalComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(HierarchicalHorizontalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
