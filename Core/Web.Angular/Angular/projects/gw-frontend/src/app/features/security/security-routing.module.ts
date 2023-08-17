import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { AuthGuard } from '@Growthware/common-code';
// Feature
import { EncryptDecryptComponent } from '@Growthware/features/security/c/encrypt-decrypt/encrypt-decrypt.component';
import { GuidHelperComponent } from '@Growthware/features/security/c/guid-helper/guid-helper.component';
import { RandomNumbersComponent } from '@Growthware/features/security/c/random-numbers/random-numbers.component';

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
