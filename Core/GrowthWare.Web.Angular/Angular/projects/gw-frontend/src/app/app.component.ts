import { Component } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';

export const animateText = trigger('animateText', [
  state(
    'hide',
    style({
      width: 0,
      opacity: 0,
    })
  ),
  state(
    'show',
    style({
      width: 150,
      opacity: 1,
    })
  ),
  transition('hide => show', animate('3500ms ease-in')), // not working
  transition('show => hide', animate('200ms ease-out')), // not working
]);

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [animateText],
})
export class AppComponent {
  title = 'FrontEnd';
  showFiller = false;

  constructor() { }
}
