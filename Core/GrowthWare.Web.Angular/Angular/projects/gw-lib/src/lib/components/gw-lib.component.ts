import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'gw-lib-my-component',
  template: `
    <p>
      gw-lib works!
    </p>
  `,
  styles: [
  ]
})
export class GWLibComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    // do ntohing atm
    console.log('clilaxing')
  }

}
