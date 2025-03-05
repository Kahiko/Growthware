import { Component, inject, TemplateRef, ViewChild } from '@angular/core';
// Angular Material
import { MatButton } from '@angular/material/button';
// Library
import { TestLoggingComponent } from '../test-logging/test-logging.component';
import { ModalService, ModalOptions, ModalSize, WindowSize } from '@growthware/core/modal';

@Component({
  selector: 'gw-core-test-modal',
  standalone: true,
  imports: [
    MatButton
  ],
  templateUrl: './test-modal.component.html',
  styleUrl: './test-modal.component.scss'
})
export class TestModalComponent {

  private _ModalService = inject(ModalService);
  private _Component_ModalOptions = new ModalOptions('testComponent', 'Component', '', 1);
  private _String_ModalOptions = new ModalOptions('testString', 'String', '', 1);
  private _TemplateRef_ModalOptions = new ModalOptions('testTemplateRef', 'TemplateRef', '', 1);

  @ViewChild('templateRef', { read: TemplateRef }) private _TemplateRef!: TemplateRef<unknown>;

  onOpenModal(modalType: string): void {
    switch (modalType) {
      case 'component':
        /** $height: '575px'; $width: '575px';   */
        this._Component_ModalOptions.windowSize = new WindowSize(585, 575);
        this._Component_ModalOptions.contentPayLoad = TestLoggingComponent;
        this._ModalService.open(this._Component_ModalOptions);
        break;
      case 'string':
        this._String_ModalOptions.windowSize = ModalSize.Small;
        this._String_ModalOptions.contentPayLoad = 'This is a string modal';
        this._ModalService.open(this._String_ModalOptions);
        break;
      case 'templateRef':
        this._TemplateRef_ModalOptions.contentPayLoad = this._TemplateRef;
        this._TemplateRef_ModalOptions.windowSize = ModalSize.Normal;
        this._ModalService.open(this._TemplateRef_ModalOptions);
    }
  }
}
