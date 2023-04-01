import { Component, OnInit } from '@angular/core';
import {delay} from 'rxjs/operators';
// Feature
import { LoaderService } from '../../loader.service';

@Component({
  selector: 'gw-lib-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {
  loading: boolean = false;

  constructor(
    private _LoaderSvc: LoaderService
  ) { }

  ngOnInit(): void {
    this.listenToLoading();
  }

  private listenToLoading(): void {
    this._LoaderSvc.loadingChanged
      .pipe(delay(0)) // This prevents a ExpressionChangedAfterItHasBeenCheckedError for subsequent requests
      .subscribe((loading) => {
        this.loading = loading;
      });
  }
}
