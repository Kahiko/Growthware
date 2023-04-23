import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
// Library
import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { PickListModule } from '@Growthware/Lib/src/lib/features/pick-list';
import { SnakeListModule } from '@Growthware/Lib/src/lib/features/snake-list';
// Feature Components
import { EncryptDecryptComponent } from './c/encrypt-decrypt/encrypt-decrypt.component';
import { GuidHelperComponent } from './c/guid-helper/guid-helper.component';
import { RandomNumbersComponent } from './c/random-numbers/random-numbers.component';
// Feature Modules
// import { SecurityRoutingModule } from './security-routing.module';


@NgModule({
  declarations: [
    EncryptDecryptComponent,
    GuidHelperComponent,
    RandomNumbersComponent,
  ],
  imports: [
    // SecurityRoutingModule,
    CommonModule,
    DynamicTableModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    PickListModule,
    ReactiveFormsModule,
    SnakeListModule,
  ]
})
export class SecurityModule { }
