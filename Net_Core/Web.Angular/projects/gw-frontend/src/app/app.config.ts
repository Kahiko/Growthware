import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom } from '@angular/core';
import { ActivatedRoute, provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
// Application
import { routes } from './app.routes';
import { LoaderInterceptor } from '@growthware/core/loader';
import { ErrorInterceptor } from '@growthware/common/interceptors';
import { JwtInterceptor } from '@growthware/common/interceptors';
import { appInitializer } from './app.initializer';
// Library Services
import { AccountService } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { DynamicTableService } from '@growthware/core/dynamic-table';
import { NavigationService } from '@growthware/core/navigation';

export function tokenGetter() {
	return sessionStorage.getItem('jwt');
}
  
export const appConfig: ApplicationConfig = {
	// Called by main.ts:bootstrapApplication
	providers: [
		provideRouter(routes), 
		provideAnimations(), 
		importProvidersFrom(
			JwtModule.forRoot({
				config: {
					tokenGetter: tokenGetter,
					allowedDomains: ['localhost:5001'],
					disallowedRoutes: []
				},
			}),
		), 
		provideHttpClient(withInterceptors([
			LoaderInterceptor,
			ErrorInterceptor,
			JwtInterceptor
		])),
		{ provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [ActivatedRoute, AccountService, ConfigurationService, DynamicTableService, NavigationService] },
	]
};
