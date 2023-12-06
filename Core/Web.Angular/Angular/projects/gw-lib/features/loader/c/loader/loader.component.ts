import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
// Feature
import { LoaderService } from '../../loader.service';

@Component({
  selector: 'gw-lib-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements AfterViewInit, OnDestroy {
  private _Subscriptions: Subscription = new Subscription();
  loading: boolean = false;

  constructor(
    private _LoaderSvc: LoaderService
  ) { }

  ngAfterViewInit(): void {
    this._Subscriptions = this._LoaderSvc.stateChanged$
      .subscribe(value => {
        if (this.loading !== value) {
          this.loading = value;
        }
      });
  }

  ngOnDestroy(): void {
    // Though this is very unlikely you just never know how this is going to be used
    this._Subscriptions.unsubscribe();
  }
}
