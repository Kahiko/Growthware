import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'gw-lib-hierarchical-horizontal-flyout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hierarchical-horizontal-flyout.component.html',
  styleUrls: ['./hierarchical-horizontal-flyout.component.scss']
})
export class HierarchicalHorizontalFlyoutComponent implements OnInit {

  @Input() backgroundColor: string = 'lightblue'; // #1bc2a2
  @Input() hoverBackgroundColor: string = '#2c3e50'; // #2c3e50
  @Input() fontColor: string = 'white';
  @Input() name: string = '';
  @Input() height: string = '32px';

  constructor() { }

  ngOnInit(): void {
    // do nothing atm
    document.documentElement.style.setProperty('--fontColor', this.fontColor);
    document.documentElement.style.setProperty('--height', this.height);
    document.documentElement.style.setProperty('--hoverBackgroundColor', this.hoverBackgroundColor);
    document.documentElement.style.setProperty('--ulBackgroundColor', this.backgroundColor);
    document.documentElement.style.setProperty('--ulLiBackgroundColor', this.backgroundColor);
  }
}