import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Toast } from 'bootstrap';
import { EventType } from '../../event-type.enum';
import { fromEvent, take } from 'rxjs';

@Component({
  selector: 'gw-lib-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent implements OnInit {
  @Output() disposeEvent = new EventEmitter();

  @ViewChild('toastElement', { static: true })
  toastEl!: ElementRef;

  @Input()
  type!: EventType;

  @Input()
  title!: string;

  @Input()
  message!: string;

  convertType: string = '';

  toast!: Toast;

  ngOnInit() {
    this.show();
    this.convertType = EventType[this.type];
  }

  show() {
    this.toast = new Toast(
      this.toastEl.nativeElement,
      this.type === EventType.Error
        ? {
            autohide: false,
          }
        : {
            delay: 3000,
          }
    );

    fromEvent(this.toastEl.nativeElement, 'hidden.bs.toast')
      .pipe(take(1))
      .subscribe(() => this.hide());

    this.toast.show();
  }

  hide() {
    this.toast.dispose();
    this.disposeEvent.emit();
  }
}
