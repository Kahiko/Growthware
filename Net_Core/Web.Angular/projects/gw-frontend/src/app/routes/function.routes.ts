import { Routes } from '@angular/router';
// Library
import { CopyFunctionSecurityComponent, SearchfunctionsComponent } from '@growthware/core/function';

export const functionRoutes: Routes = [
	{ path: '', component: SearchfunctionsComponent },
	{ path: 'copyfunctionsecurity', component: CopyFunctionSecurityComponent },
];