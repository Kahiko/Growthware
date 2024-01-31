import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BehaviorSubject, Subject } from 'rxjs';

import { AppComponent } from './app.component';

import { AccountService } from '@growthware/core/account';
import { ISecurityEntityProfile, SecurityEntityProfile } from '@growthware/core/security-entities';

class FakeConfigurationService {
	private _ApplicationNameSubject = new BehaviorSubject<string>('');
	public applicationName$ = this._ApplicationNameSubject.asObservable();
	public version$ = new Subject<string>();

	public changeApplicationName(applicationName: string): void {
		this._ApplicationNameSubject.next(applicationName);
	}
}

class FakeSecurityEntityService {
	private _SecurityEntityProfile = new SecurityEntityProfile();

	constructor() { 
		const mSecurityEntityProfile = new SecurityEntityProfile();
		mSecurityEntityProfile.id = 1;
		mSecurityEntityProfile.name = 'Test';
		mSecurityEntityProfile.skin = 'default';
		this.changeSecurityEntity(mSecurityEntityProfile);		
	}

	public getSecurityEntity(): ISecurityEntityProfile {
		return this._SecurityEntityProfile;
	}

	public changeSecurityEntity(securityEntityProfile: ISecurityEntityProfile): void {
		this._SecurityEntityProfile = securityEntityProfile;
	}
}

describe('AppComponent', () => {
	beforeEach(async () => {
		const dependencies = {
			AccountService,
			'ConfigurationService': new FakeConfigurationService(),
			'SecurityEntityService': new FakeSecurityEntityService()
		};
		await TestBed.configureTestingModule({
			imports: [
				AppComponent,
				RouterTestingModule,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [
				AccountService,
				{provide: 'ConfigurationService', useValue: dependencies.ConfigurationService},
				{provide: 'SecurityEntityService', useValue: dependencies.SecurityEntityService},
			]
		}).compileComponents();
	});

	it('should create the app', () => {
		const fixture = TestBed.createComponent(AppComponent);
		const app = fixture.componentInstance;
		expect(app).toBeTruthy();
	});

	it('should have the \'gw-frontend\' title', () => {
		const fixture = TestBed.createComponent(AppComponent);
		const app = fixture.componentInstance;
		expect(app.title).toEqual('gw-frontend');
	});

	it('(not yet implemented) should render title', () => {
		const fixture = TestBed.createComponent(AppComponent);
		fixture.detectChanges();
		expect(true).toBeTrue();
	});
});
