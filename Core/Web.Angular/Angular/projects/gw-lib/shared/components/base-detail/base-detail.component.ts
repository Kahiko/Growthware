import { Component, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
// Library
import { DataService } from '@Growthware/shared/services';
import { LogLevel, LoggingService } from '@Growthware/features/logging';
import { ModalService } from '@Growthware/features/modal';
import { SecurityService, ISecurityInfo, SecurityInfo } from '@Growthware/features/security';

export interface IBaseDetailComponent extends BaseDetailComponent {
    populateProfile(): void;
}

@Component({
    selector: 'gw-lib-base-search',
    template: ``,
    styles: [
    ]
})
export abstract class BaseDetailComponent implements IBaseDetailComponent, OnDestroy {
    protected _Subscription: Subscription = new Subscription();

    frmProfile!: FormGroup;

    protected canDelete: boolean = false;
    protected canSave: boolean = false;
    protected _SecurityInfo: ISecurityInfo = new SecurityInfo();

    protected _ProfileSvc: any = {};
    protected _DataSvc!: DataService;
    protected _LoggingSvc!: LoggingService;
    protected _ModalSvc!: ModalService;
    protected _SecuritySvc!: SecurityService;

    ngOnDestroy(): void {
        this._Subscription.unsubscribe();
        this._ProfileSvc.editReason = '';
        this._ProfileSvc.editRow = '';
    }

    protected onCancel(): void {
        this.onClose();
    }

    protected onClose(): void {
        if(this._ProfileSvc.editReason.toLocaleLowerCase() != 'newprofile') {
            this._ModalSvc.close(this._ProfileSvc.editModalId);
        } else {
            this._ModalSvc.close(this._ProfileSvc.addModalId);
        }
        this._LoggingSvc.toast('BaseDetailComponent.onClose', 'Function Details:', LogLevel.Error);
    }

    protected onDelete(): void {
        this._LoggingSvc.toast('BaseDetailComponent.onDelete', 'Function Details:', LogLevel.Error);
        this.onClose();
    }

    protected onSubmit(form: FormGroup): void {
        this._LoggingSvc.toast('BaseDetailComponent.onSubmit', 'Function Details:', LogLevel.Error);
        this.populateProfile();
        this.save();
    }

    get controls() {
        return this.frmProfile.controls;
    }

    abstract populateProfile(): void;
    abstract save(): void;

}