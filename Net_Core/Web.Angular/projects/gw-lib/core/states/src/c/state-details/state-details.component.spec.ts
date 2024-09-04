import { ComponentFixture, TestBed, inject } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { StateDetailsComponent } from './state-details.component';
import { StatesService } from '../../states.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
class MockStatesService {
	modalReason = '';
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	public selectedRow = {
		Status: ''
	};

}

describe('StateDetailsComponent', () => {
	let fixture: ComponentFixture<StateDetailsComponent>;
	let component: StateDetailsComponent;
	const dependencies = {
		'StatesService': new MockStatesService(),
	};

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [StateDetailsComponent,
        NoopAnimationsModule],
    providers: [
        { provide: 'StatesService', useValue: dependencies.StatesService },
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
    ]
}).compileComponents();
    
		fixture = TestBed.createComponent(StateDetailsComponent);
		component = fixture.componentInstance;
		// fixture.detectChanges(); is causing downstream errors to occur, at this poing I belive that the services need to be
		// mocked and injected property in order for this to work.
		// The error we are getting at this poing is: Cannot read properties of undefined (reading 'trim')
		//  at StateDetailsComponent.call [as ngOnInit] 
		// fixture.detectChanges();
	});

	it('(not yet implemented) should create', inject([StatesService], (profileSvc: MockStatesService) => {
		console.log('profileSvc', profileSvc);
		expect(component).toBeTruthy();
	}));
});
