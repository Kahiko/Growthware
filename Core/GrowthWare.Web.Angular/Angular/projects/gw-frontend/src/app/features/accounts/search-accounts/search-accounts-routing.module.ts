import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchAccountsComponent } from './search-accounts.component';

const routes: Routes = [{ path: '', component: SearchAccountsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SearchAccountsRoutingModule { }
