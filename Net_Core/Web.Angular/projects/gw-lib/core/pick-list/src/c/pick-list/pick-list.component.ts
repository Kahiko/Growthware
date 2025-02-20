import { CommonModule } from '@angular/common';
import { Component, input, output, OnDestroy, OnInit, effect } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { ModalOptions, ModalService } from '@growthware/core/modal';
import { Observable, Subscription } from 'rxjs';

/**
 * Pick List currently uses "internal" arrays to mange the data.
 * TODO: Look into if this can be done directly using InputSignal
 */
@Component({
	selector: 'gw-core-pick-list',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatButtonModule,
		MatIconModule
	],
	templateUrl: './pick-list.component.html',
	styleUrls: ['./pick-list.component.scss']
})
export class PickListComponent implements OnDestroy, OnInit {
	// Properties Signals
	availableItems = input<Array<string>>([]);
	availableItemsChange = output<Array<string>>();
	availableItems$: Observable<Array<string>> = toObservable(this.availableItems);
	selectedItems = input<Array<string>>([]);
	selectedItemsChange = output<Array<string>>();
	selectedItems$: Observable<Array<string>> = toObservable(this.selectedItems);
	// properties
	allItemsText = input<string>('');
	header = input<string>('');
	id = input.required<string>();
	name = input<string>('');
	pickListTableContentsBackground = input<string>('pink');
	pickListTableContentsFont = input<string>('black');
	pickListTableHeaderBackground = input<string>('lightpink');
	pickListTableHelp = input<string>('');
	selectedItemsText = input<string>('');
	size = input<string>('8');
	width = input<string>('120');
	overAllWidth = 0;

	private _AvailableItems: Array<string> = [];
	private _FirstLoadAvailableItems: boolean = true;
	private _FirstLoadSelectedItems: boolean = true;
	private _ModalOptions!: ModalOptions;
	private _SelectedItems: Array<string> = [];
	private _Subscriptions: Subscription = new Subscription();

	sortOnChange: boolean = true;

	constructor(
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService
	) { 
		effect(() => {
			this.overAllWidth = (+this.width() * 2) + 50;
		});
	}

	ngOnInit(): void {
		this._ModalOptions = new ModalOptions(this.id() + '_Modal', this.header(), this.pickListTableHelp());
		this._Subscriptions.add(
			this.availableItems$.subscribe((data) => {
				if (this._FirstLoadAvailableItems && data.length > 0) {
					// on the first time loading we need to set the selected items
					this._AvailableItems = data;
					this._FirstLoadAvailableItems = false;
				}
			})
		);

		this._Subscriptions.add(
			this.selectedItems$.subscribe((data) => {
				if (this._FirstLoadSelectedItems && data.length > 0) {
					// on the first time loading we need to set the selected items
					this._SelectedItems = data;
					this._FirstLoadSelectedItems = false;
					// remove the selected items from the available items
					this.removeSelectedFromAvailable();
				}
				const mDataHasChanged = this.selectedItems() != data;
				if (mDataHasChanged) {
					// remove the selected items from the available items
					this.removeSelectedFromAvailable();
				}
			})
		);
		document.documentElement.style.setProperty('--pickListTableContentsBackground', this.pickListTableContentsBackground());
		document.documentElement.style.setProperty('--pickListTableContentsFont', this.pickListTableContentsFont());
		document.documentElement.style.setProperty('--pickListTableHeaderBackground', this.pickListTableHeaderBackground());
	}

	ngOnDestroy(): void {
		this._Subscriptions.unsubscribe();
	}

	onShowHelp(): void {
		this._ModalSvc.open(this._ModalOptions);
	}

	/**
	 * @description Removes the selected items from the available
	 */
	removeSelectedFromAvailable(): void {
		const mSelectedItems = this.selectedItems();
		// console.log('mSelectedItems', mSelectedItems);
		const mNewAvalibleRoles = this.availableItems().filter((role) => {
			const mRetVal: boolean = !mSelectedItems.includes(role);
			return mRetVal;
		});
		this._AvailableItems = mNewAvalibleRoles;
		this.sortInternalItemArrays();
	}

	/**
	 * @description Sorts the internal item arrays (available and selected) then emits the changes
	 */
	private sortInternalItemArrays(): void {
		let sortedArray = this._AvailableItems.slice();
		sortedArray.sort(function (a, b) {
			return GWCommon.naturalSort(a, b);
		});
		this.availableItemsChange.emit(sortedArray);

		sortedArray = this._SelectedItems.slice();
		sortedArray.sort(function (a, b) {
			return GWCommon.naturalSort(a, b);
		});
		this.selectedItemsChange.emit(sortedArray);
	}

	/**
	 * @description Updates _AvailableItems and _SelectedItems arrays given the "source" and the HTMLOptionsCollection or HTMLCollectionOf<HTMLOptionElement>
	 * @param e 
	 * @param source 
	 * @param fromBoxItems 
	 */
	private _Switch(e: Event, source: string, fromBoxItems: HTMLOptionsCollection | HTMLCollectionOf<HTMLOptionElement>): void {
		e.stopPropagation();
		e.preventDefault();
		// get an instance of the "from" select box
		if (fromBoxItems.length > 0) {
			if (source == '_SrcList') {
				// We are switching from the left side or "avalible items" to the right side or "selected items"
				const mNewItems = this.updateArrays(fromBoxItems, this._AvailableItems, this._SelectedItems);
				this._AvailableItems = mNewItems.fromArray;
				this._SelectedItems = mNewItems.toArray;
			} else {
				// We are switching from the right side or "selected items" to the left side or "avalible items"
				const mNewItems = this.updateArrays(fromBoxItems, this._SelectedItems, this._AvailableItems);
				this._AvailableItems = mNewItems.toArray;
				this._SelectedItems = mNewItems.fromArray;
			}
			this.sortInternalItemArrays();
		}
	}

	/**
	 * @description Calls switch passing the "options" of the HTMLSelectElement
	 * @param e 
	 * @param source 
	 */
	switchAll(e: Event, source: string): void {
		const objFromBox = document.getElementById(this.id() + source)! as HTMLSelectElement;
		if (objFromBox.length > 0) {
			this._Switch(e, source, objFromBox.options);
		}
	}

	/**
	 * @description Calls switch passing the "selectedOptions" of the HTMLSelectElement
	 * @param e 
	 * @param source 
	 */
	switchList(e: Event, source: string): void {
		// get an instance of the "from" select box
		const objFromBox = document.getElementById(this.id() + source)! as HTMLSelectElement;
		if (objFromBox.length > 0) {
			this._Switch(e, source, objFromBox.selectedOptions);
		}
	}

	/**
	 * @description Updates the signal data from one signal to another
	 * @param fromBoxItems 
	 * @param fromArray 
	 * @param toArray 
	 * @returns the modified arrays
	 */
	updateArrays(fromBoxItems: HTMLOptionsCollection | HTMLCollectionOf<HTMLOptionElement>, fromArray: Array<string>, toArray: Array<string>): { fromArray: Array<string>, toArray: Array<string> } {
		const mNewItems: Array<string> = [];
		const mCurrentItems: Array<string> = [];
		// get the new items from the from box items
		for (let mSelectedIndex = 0; mSelectedIndex < fromBoxItems.length; mSelectedIndex++) {
			mNewItems.push(fromBoxItems[mSelectedIndex].text);
		}
		// get the current items from the from "to" signal
		for (let mSelectedIndex = 0; mSelectedIndex < toArray.length; mSelectedIndex++) {
			mCurrentItems.push(toArray[mSelectedIndex]);
		}
		// merge the new and current items
		const mMergedItems: Array<string> = [...mNewItems, ...mCurrentItems];
		// update the "to" signal with the merged items
		toArray = JSON.parse(JSON.stringify(mMergedItems));
		// remove the items from the "from" signal
		for (let mSelectedIndex = 0; mSelectedIndex < fromBoxItems.length; mSelectedIndex++) {
			fromArray = fromArray.filter((item) => {
				return item != fromBoxItems[mSelectedIndex].text;
			});
		}
		return { fromArray: fromArray, toArray: toArray };
	}
}
