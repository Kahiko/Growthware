import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IToastMessage, ToastMessage } from '../../toast-message.model';
import { ToastService } from '../../toast.service';
@Component({
  selector: 'gw-lib-toaster',
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.scss'],
})
export class ToasterComponent implements OnDestroy, OnInit {
  public currentToasts: IToastMessage[] = [];
  private _ToastSub: Subscription = new Subscription();

  constructor(
    private _ToastSvc: ToastService,
    private _ChangeDetectorRef: ChangeDetectorRef
  ) {}

  ngOnDestroy(): void {
    this._ToastSub.unsubscribe();
  }

  ngOnInit(): void {
    this._ToastSub = this._ToastSvc.toastEvents.subscribe(
      (toasts: ToastMessage) => {
        const currentToast: ToastMessage = {
          eventType: toasts.eventType,
          title: toasts.title,
          message: toasts.message,
        };
        this.currentToasts.push(currentToast);
        this._ChangeDetectorRef.detectChanges();
      }
    );
  }

  dispose(index: number) {
    this.currentToasts.splice(index, 1);
    this._ChangeDetectorRef.detectChanges();
  }
}
