import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatButtonModule } from '@angular/material/button';

import { CounterComponent } from "./counter/counter.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";
import { MSDemoRoutingModule } from "./ms-demo.routing.module";

@NgModule({
  declarations: [
    CounterComponent,
    FetchDataComponent
  ],
  imports:[
    CommonModule,
    MatButtonModule,
    MSDemoRoutingModule
  ]
})
export class MSDemoModule{}
