import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
// Application
import { routes } from './app.routes';
import { LoaderInterceptor } from './interceptors/loader.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { appInitializer } from './app.initializer';
// Library Services
import { AccountService } from '@growthware/core/account';
import { ConfigurationService } from '@growthware/core/configuration';
import { DynamicTableService } from '@growthware/core/dynamic-table';

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
		{ provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AccountService, ConfigurationService, DynamicTableService] },
	]
};
