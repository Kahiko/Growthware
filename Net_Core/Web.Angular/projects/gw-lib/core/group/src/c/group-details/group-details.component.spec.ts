import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { GroupDetailsComponent } from './group-details.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('GroupDetailsComponent', () => {
	let component: GroupDetailsComponent;
	let fixture: ComponentFixture<GroupDetailsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    imports: [GroupDetailsComponent,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
			.compileComponents();
    
		fixture = TestBed.createComponent(GroupDetailsComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('(not yet implemented) should create', () => {
		expect(component).toBeTruthy();
	});
});
