import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/Lib/src/lib/guards';
// Feature
import { EncryptDecryptComponent } from './c/encrypt-decrypt/encrypt-decrypt.component';
import { GuidHelperComponent } from './c/guid-helper/guid-helper.component';
import { RandomNumbersComponent } from './c/random-numbers/random-numbers.component';

const childRoutes: Routes = [
  { path: '', component: EncryptDecryptComponent },
  { path: 'encryption-helper', component: EncryptDecryptComponent },
  { path: 'guid-helper', component: GuidHelperComponent },
  { path: 'random-numbers', component: RandomNumbersComponent },
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class SecurityRoutingModule { }
