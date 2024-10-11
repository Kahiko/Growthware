import { Component, computed, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
// Features
import { ToastService } from '../../toast.service';
import { ToastComponent } from '../toast/toast.component';

@Component({
	selector: 'gw-core-toaster',
	standalone: true,
	imports: [
		FormsModule,
		ToastComponent
	],
	templateUrl: './toaster.component.html',
	styleUrls: ['./toaster.component.scss'],
})
export class ToasterComponent {
	private _ToastSvc = inject(ToastService);

	public currentToasts = computed(() => this._ToastSvc.currentToasts());

}
