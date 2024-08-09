
import { FormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
// Library
// Feature
import { SysAdminService } from '../../sys-admin.service';
import { ILineCount, LineCount } from '../../line-count.model';

@Component({
	selector: 'gw-core-line-count',
	standalone: true,
	imports: [
		FormsModule,
		MatButtonModule,
		MatTabsModule
	],
	templateUrl: './line-count.component.html',
	styleUrls: ['./line-count.component.scss']
})
export class LineCountComponent implements OnInit {

	public countInfo: ILineCount = new LineCount();

	public excludePattern: string = '';
	public lineCount: string = '';

	constructor(private _SysAdminSvc: SysAdminService) { }

	ngOnInit(): void {
		const excludePattern: string[] = [];
		excludePattern.push('ASSEMBLYINFO');
		excludePattern.push('.DESIGNER');
		excludePattern.push('JQUERY-');
		excludePattern.push('JQUERY.VALIDATE');
		excludePattern.push('MODERNIZR-');
		excludePattern.push('jquery.tmpl.');
		excludePattern.push('jquery.unobtrusive');
		excludePattern.push('angular');
		this.countInfo.excludePattern = excludePattern.join(', ');
		this.countInfo.theDirectory = 'D:/Development/Growthware/Net_Core';
	}

	onBtnCount() {
		this.lineCount = '';
		this._SysAdminSvc.getLineCount(this.countInfo).then((lineCount: string) => {
			// console.log('lineCount:', lineCount);
			this.lineCount = lineCount;
		});
	}
}
