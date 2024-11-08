import { ComponentFixture, TestBed, inject } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { signal } from '@angular/core';

import { AppComponent } from './app.component';

import { AuthenticationResponse, IAccountInformation, IAuthenticationResponse } from '@growthware/core/account';
import { ISecurityEntityProfile, SecurityEntityProfile, SecurityEntityService } from '@growthware/core/security-entities';
import { ConfigurationService } from '@growthware/core/configuration';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ClientChoices, IClientChoices } from '@growthware/core/clientchoices';

class MockAccountService {
	public authenticationResponse = signal<IAuthenticationResponse>(new AuthenticationResponse());
	public clientChoices = signal<IClientChoices>(new ClientChoices());

	constructor() { 
		this.setDeveloper();
	}

	public setDeveloper(): void {
		const mAccountInformation: IAuthenticationResponse = {
			"account":"Developer",
			"created":"2024-10-10T12:07:00",
			"email":"michael.regan@verizon.net",
			"firstName":"System",
			"id":3,"isSystemAdmin":true,
			"isVerified":false,
			"jwtToken":"",
			"lastName":"Developer",
			"location":"none",
			"middleName":"",
			"preferredName":"System-Developer",
			"status":1,
			"timeZone":-5,
			"updated":"2024-11-08T10:44:22"
		};
		const mClientChoices: IClientChoices = {
			"account": "Developer", 
			"securityEntityId": 1, 
			"securityEntityName": "System", 
			"action": "accounts", 
			"recordsPerPage": 10, 
			"colorScheme": "Blue", 
			"evenRow": "#6699cc", 
			"evenFont": "White", 
			"oddRow": "#b6cbeb", 
			"oddFont": "Black", 
			"headerRow": "#C7C7C7", 
			"headerFont": "Black", 
			"background": "#ffffff", 
		};
		
		this.authenticationResponse.set(mAccountInformation);
		this.clientChoices.set(mClientChoices);
	}

	public setMike(): void {
		const mAccountInformation: IAuthenticationResponse = {
			"account":"Mike",
			"created":"2024-10-10T12:07:00",
			"email":"michael.regan@verizon.net",
			"firstName":"System",
			"id":4,
			"isSystemAdmin":false,
			"isVerified":false,
			"jwtToken":"",
			"lastName":"Tester",
			"location":"none",
			"middleName":"",
			"preferredName":"System-Tester",
			"status":1,
			"timeZone":-5,
			"updated":"2024-11-01T06:44:33"
		};
		const mClientChoices: IClientChoices = {
			"account":"Mike",
			"securityEntityId":1,
			"securityEntityName":"System",
			"action":"home",
			"recordsPerPage":10,
			"colorScheme":"Blue",
			"evenRow":"#6699cc",
			"evenFont":"White",
			"oddRow":"#b6cbeb",
			"oddFont":"Black",
			"headerRow":"#C7C7C7",
			"headerFont":"Black",
			"background":"#ffffff"
		};
		
		this.authenticationResponse.set(mAccountInformation);
		this.clientChoices.set(mClientChoices);		
	}
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

	public async getSecurityEntity(id: number): Promise<ISecurityEntityProfile> {
		return new Promise<ISecurityEntityProfile>((resolve, reject) => {
			resolve(this._SecurityEntityProfile);
		});
	}

	public changeSecurityEntity(securityEntityProfile: ISecurityEntityProfile): void {
		this._SecurityEntityProfile = securityEntityProfile;
	}
}

describe('AppComponent', () => {
	let _Fixture: ComponentFixture<AppComponent>;
	let _Component: AppComponent;
	const _Dependencies = {
		'accountSvcMock': new MockAccountService(),
		'configurationSvcMock': new MockConfigurationService(),
		'securityEntitySvcMock': new MockSecurityEntityService()
	};

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [],
			imports: [AppComponent,
				RouterTestingModule.withRoutes([]),
				NoopAnimationsModule],
			providers: [
				{ provide: 'AccountService', useValue: _Dependencies.accountSvcMock },
				{ provide: ConfigurationService, useValue: _Dependencies.configurationSvcMock },
				{ provide: SecurityEntityService, useValue: _Dependencies.securityEntitySvcMock },
				provideHttpClient(withInterceptorsFromDi()),
				provideHttpClientTesting(),
			]
		}).compileComponents();
		_Fixture = TestBed.createComponent(AppComponent);
		_Component = _Fixture.componentInstance;
	});

	it('should create the app', () => {
		expect(_Component).toBeTruthy();
	});

	it('should have the \'gw-frontend\' title', () => {
		expect(_Component.title).toEqual('gw-frontend');
	});

	it('should render title `CS Angular.io`', inject([ConfigurationService], (mockConfigSvc: MockConfigurationService) => {
		const mNewValue = 'CS Angular.io';
		mockConfigSvc.changeApplicationName(mNewValue);
		_Fixture.detectChanges(); // the signal is dirty and we need to call detectChanges
		expect(_Component.title).toEqual(mNewValue);
	}));
});
