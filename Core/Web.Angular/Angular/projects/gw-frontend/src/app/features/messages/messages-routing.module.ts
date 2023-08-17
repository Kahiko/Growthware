import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// Library
import { SearchMessagesComponent } from '@Growthware/src/features/message';

const routes: Routes = [
  { path: '', component: SearchMessagesComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MessagesRoutingModule { }
