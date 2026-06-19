import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'gw-frontend-default-footer',
    templateUrl: './default-footer.component.html',
    styleUrls: ['./default-footer.component.scss'],
    changeDetection: ChangeDetectionStrategy.Eager,
    imports: []
})
export class DefaultFooterComponent implements OnInit {

	constructor() { }

	ngOnInit(): void {
	}

}
