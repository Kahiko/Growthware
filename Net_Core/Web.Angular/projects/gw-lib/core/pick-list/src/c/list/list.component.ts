import { Component, input, output, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { ModalOptions, ModalService, ModalSize } from '@growthware/core/modal';
import { encapsulateStyle } from '@angular/compiler';

@Component({
	selector: 'gw-core-list',
	standalone: true,
	imports: [
		CommonModule,

		MatButtonModule,
		MatIconModule,
	],
	templateUrl: './list.component.html',
	styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

	private _ModalOptions!: ModalOptions;
	private _ModalSvc = inject(ModalService);

	allItemsText = input<string>('');
	availableItems = input<Array<string>>([]);
	availableItemsChange = output<Array<string>>();
	pickListTableContentsBackground = input<string>('pink');
	fontColor = input<string>('black');
	header = input<string>('');
	pickListTableHeaderBackground = input<string>('lightpink');
	id = input<string>('');
	name = input<string>('');
	pickListTableHelp = input<string>('');
	selectedItemsText = input<string>('');
	size = input<string>('8');
	width = input<string>('120');

	ngOnInit(): void {
		this._ModalOptions = new ModalOptions(this.id() + '_Modal', this.header(), this.pickListTableHelp(), ModalSize.Small);
		document.documentElement.style.setProperty('--pickListTableContentsBackground', this.pickListTableContentsBackground());
		document.documentElement.style.setProperty('--pickListTableHeaderBackground', this.pickListTableHeaderBackground());
	}

	onShowHelp(): void {
		this._ModalSvc.open(this._ModalOptions);
	}
}
