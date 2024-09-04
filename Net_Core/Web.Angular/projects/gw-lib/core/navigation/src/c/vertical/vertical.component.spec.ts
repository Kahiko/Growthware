import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { VerticalComponent } from './vertical.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('VerticalComponent', () => {
	let component: VerticalComponent;
	let fixture: ComponentFixture<VerticalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [VerticalComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
			.compileComponents();
    
		fixture = TestBed.createComponent(VerticalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
