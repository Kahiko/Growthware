import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
// Library
import { LogLevel, LoggingService } from '@Growthware/features/logging';
// Feature
import { ConfigurationService } from '../../configuration.service';
import { IDBInformation, DBInformation } from '../../db-Information.model';

@Component({
  selector: 'gw-lib-edit-db-information',
  templateUrl: './edit-db-information.component.html',
  styleUrls: ['./edit-db-information.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
  ]
})
export class EditDbInformationComponent implements OnInit {

  public dbInformation: IDBInformation = new DBInformation;
  public selectedInheritance: number = 1;
  public validInheritances= [
    { id: 0, name: "False" },
    { id: 1, name: "True" }
  ];

  constructor(
    private _ConfigurationSvc: ConfigurationService,
    private _LoggingSvc: LoggingService
  ) { }

  ngOnInit(): void {
    this._ConfigurationSvc.getDBInformation().then((response: IDBInformation) => {
      this.dbInformation = response;
      this.selectedInheritance = this.dbInformation.enableInheritance;
    });
  }

  public onBtsSave(): void {
    this._ConfigurationSvc.updateProfile(this.selectedInheritance).catch((error) => {
      this._LoggingSvc.toast('Save failed', 'Save', LogLevel.Error);
    }).then((_) => {
      this._LoggingSvc.toast('Successfully saved', 'Save', LogLevel.Success);
    });
  }
}
