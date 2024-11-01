import { ComponentFixture, TestBed, inject } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
// import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BehaviorSubject, Subject } from 'rxjs';

import { AppComponent } from './app.component';

import { AccountInformation, IAccountInformation } from '@growthware/core/account';
import { ISecurityEntityProfile, SecurityEntityProfile, SecurityEntityService } from '@growthware/core/security-entities';
import { ConfigurationService } from '@growthware/core/configuration';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { signal } from '@angular/core';

class MockAccountService {
	private _AccountInformationChangedSubject = new BehaviorSubject<IAccountInformation>(new AccountInformation);
	accountInformationChanged$ = this._AccountInformationChangedSubject.asObservable();
}

class MockConfigurationService {
	public applicationName = signal<string>('');
	public version = signal<string>('');

	public changeApplicationName(applicationName: string): void {
		this.applicationName.set(applicationName);
	}
}

class MockSecurityEntityService {
	private _SecurityEntityProfile = new SecurityEntityProfile();

	constructor() {
		const mSecurityEntityProfile = new SecurityEntityProfile();
		mSecurityEntityProfile.id = 1;
		mSecurityEntityProfile.name = 'System';
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
	let fixture: ComponentFixture<AppComponent>;
	let component: AppComponent;
	const dependencies = {
		'accountSvcMock': new MockAccountService(),
		'configurationSvcMock': new MockConfigurationService(),
		'securityEntitySvcMock': new MockSecurityEntityService()
	};

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [],
			imports: [AppComponent,
				// RouterTestingModule,
				NoopAnimationsModule],
			providers: [
				{ provide: 'AccountService', useValue: dependencies.accountSvcMock },
				{ provide: ConfigurationService, useValue: dependencies.configurationSvcMock },
				{ provide: SecurityEntityService, useValue: dependencies.securityEntitySvcMock },
				provideHttpClient(withInterceptorsFromDi()),
				provideHttpClientTesting(),
			]
		}).compileComponents();
	});

	beforeEach(() => {
		fixture = TestBed.createComponent(AppComponent);
		component = fixture.componentInstance;
	});

	it('should create the app', () => {
		expect(component).toBeTruthy();
	});

	it('should have the \'gw-frontend\' title', () => {
		expect(component.title).toEqual('gw-frontend');
	});

	it('should render title `CS Angular.io`', inject([ConfigurationService], (mockConfigSvc: MockConfigurationService) => {
		const mNewValue = 'CS Angular.io';
		mockConfigSvc.changeApplicationName(mNewValue);
		fixture.detectChanges(); // the signal is dirty and we need to call detectChanges
		expect(component.title).toEqual(mNewValue);
	}));
});
