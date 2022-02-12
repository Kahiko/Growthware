import { Component } from '@angular/core';
import { onMainContentChange } from 'src/animations/GWAnimations';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.scss'],
  templateUrl: './app.component.html',
  animations: [ onMainContentChange ]
})
export class AppComponent {
  public onSideNavChange: boolean = false;

  title = 'app';
}
