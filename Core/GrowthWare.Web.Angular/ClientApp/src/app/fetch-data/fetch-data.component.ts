import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { common } from 'src/app/common';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(http: HttpClient) {
    console.log(common.baseURL);
    const mURL = common.baseURL + 'weatherforecast';
    console.log(mURL);
    http.get<WeatherForecast[]>(mURL).subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
