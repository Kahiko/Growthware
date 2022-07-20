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

  public typeClass: string = '';
  public dateTime: string = new Date().toLocaleString()

  toast!: Toast;

  ngOnInit() {
    switch (EventType[this.type]) {
      case 'Info':
        this.typeClass = 'bg-info text-dark';
        break;
      case 'Warning':
        this.typeClass = 'bg-warning text-dark';
        break;
      case 'Success':
        this.typeClass = 'bg-success text-white';
        break;
      case 'Error':
        this.typeClass = 'bg-danger text-white';
        break;
      default:
        this.typeClass = 'bg-primary text-white';
        break;
    }
    this.show();
  }

  private show() {
    this.toast = new Toast(
      this.toastEl.nativeElement,
      this.type === EventType.Error ? { autohide: false, } : { delay: 3000, }
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
