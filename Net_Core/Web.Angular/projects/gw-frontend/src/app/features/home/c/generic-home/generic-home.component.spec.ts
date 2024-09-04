import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BehaviorSubject, Subject } from 'rxjs';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { GenericHomeComponent } from './generic-home.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

class FakeConfigurationService {
	private _ApplicationNameSubject = new BehaviorSubject<string>('Test');
	public applicationName$ = this._ApplicationNameSubject.asObservable();
	public version$ = new Subject<string>();

	public changeApplicationName(applicationName: string): void {
		this._ApplicationNameSubject.next(applicationName);
	}
}

describe('GenericHomeComponent', () => {
	let component: GenericHomeComponent;
	let fixture: ComponentFixture<GenericHomeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [GenericHomeComponent],
    imports: [],
    providers: [
        { provide: 'ConfigurationService', useClass: FakeConfigurationService },
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting()
    ]
}).compileComponents();

		fixture = TestBed.createComponent(GenericHomeComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();

	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
