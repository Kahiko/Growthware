import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/src/common-code';
// Feature
import { EncryptDecryptComponent } from '@Growthware/src/features/security/c/encrypt-decrypt/encrypt-decrypt.component';
import { GuidHelperComponent } from '@Growthware/src/features/security/c/guid-helper/guid-helper.component';
import { RandomNumbersComponent } from '@Growthware/src/features/security/c/random-numbers/random-numbers.component';

const childRoutes: Routes = [
  { path: '', component: EncryptDecryptComponent, canActivate: [AuthGuard] },
  { path: 'guid_helper', component: GuidHelperComponent },        // /security/guid_helper
  { path: 'random-numbers', component: RandomNumbersComponent },  // /security/random_numbers
];


@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports: [RouterModule]
})
export class SecurityRoutingModule { }
