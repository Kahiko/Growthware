import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { PagerComponent } from './pager.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('PagerComponent', () => {
	let component: PagerComponent;
	let fixture: ComponentFixture<PagerComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [PagerComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
			.compileComponents();
    
		fixture = TestBed.createComponent(PagerComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
