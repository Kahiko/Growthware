import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { GuidHelperComponent } from './guid-helper.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('GuidHelperComponent', () => {
	let component: GuidHelperComponent;
	let fixture: ComponentFixture<GuidHelperComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [GuidHelperComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(GuidHelperComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
