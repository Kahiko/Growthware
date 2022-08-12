import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { CounterComponent } from "./counter/counter.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";

const childRoutes: Routes = [
  { path: '', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
];

@NgModule({
  imports: [RouterModule.forChild(childRoutes)],
  exports:[RouterModule]
})
export class MSDemoRoutingModule{}
