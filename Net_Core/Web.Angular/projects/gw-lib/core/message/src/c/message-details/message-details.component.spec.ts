import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { MessageDetailsComponent } from './message-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('MessageDetailsComponent', () => {
	let component: MessageDetailsComponent;
	let fixture: ComponentFixture<MessageDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [MessageDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
    
		fixture = TestBed.createComponent(MessageDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
