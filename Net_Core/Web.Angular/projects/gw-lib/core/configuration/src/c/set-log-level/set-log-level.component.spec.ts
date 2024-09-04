import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SetLogLevelComponent } from './set-log-level.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SetLogLevelComponent', () => {
	let component: SetLogLevelComponent;
	let fixture: ComponentFixture<SetLogLevelComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [SetLogLevelComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
			.compileComponents();
    
		fixture = TestBed.createComponent(SetLogLevelComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
