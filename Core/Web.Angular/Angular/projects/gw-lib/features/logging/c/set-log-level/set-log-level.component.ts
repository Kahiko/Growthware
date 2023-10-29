import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
// Feature
import { LoggingService } from '../../logging.service';

@Component({
  selector: 'gw-lib-set-log-level',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,

    MatButtonModule,
    MatSelectModule,
  ],
  templateUrl: './set-log-level.component.html',
  styleUrls: ['./set-log-level.component.scss']
})
export class SetLogLevelComponent implements OnInit {
  
  selectedLogLevel: number = 0;

  validLogLevels  = [
    { id: 0, name: "Debug"  },
    { id: 1, name: "Info"   },
    { id: 2, name: "Warn"   },
    { id: 3, name: "Error"  },
    { id: 4, name: "Fatal"  }
  ];

  constructor(
    private _LoggingSvc: LoggingService,
  ) {
  }

  ngOnInit() {
    this._LoggingSvc.getLogLevel().then((response: number) => {
      this.selectedLogLevel = parseInt(response.toString());
    });    
  }

  onbtnSave(): void {
    this._LoggingSvc.setLogLevel(this.selectedLogLevel);
  }
}
