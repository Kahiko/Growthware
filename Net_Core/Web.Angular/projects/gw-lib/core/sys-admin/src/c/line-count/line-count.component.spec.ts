import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { LineCountComponent } from './line-count.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('LineCountComponent', () => {
	let component: LineCountComponent;
	let fixture: ComponentFixture<LineCountComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [LineCountComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(LineCountComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
