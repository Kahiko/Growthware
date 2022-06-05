import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { GWCommon } from '@Growthware/Lib';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.scss']
})
export class FetchDataComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  private _httpClient: HttpClient

  constructor(private _GWCommon: GWCommon, httpClient: HttpClient) {
    this._httpClient = httpClient;
  }

  ngOnInit(): void {
    const mURL = this._GWCommon.baseURL + 'weatherforecast';
    this._httpClient.get<WeatherForecast[]>(mURL).subscribe({
      next: (result) => {
        this.forecasts = result;
      },
      error: (e) => {
        console.error(e)
      },
      complete: () => {
        // console.info('complete')
      }
    });
  }
}
