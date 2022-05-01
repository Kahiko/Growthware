import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

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

  constructor(httpClient: HttpClient) {
    this._httpClient = httpClient;
  }

  ngOnInit(): void {
    const mURL = this.getBaseURL() + '/weatherforecast';
    console.log(mURL);
    this._httpClient.get<WeatherForecast[]>(mURL).subscribe({
      next: (result) => {
        this.forecasts = result;
      },
      error: (e) => {
        console.error(e)
      },
      complete: () => {
        console.info('complete')
      }
    });
  }

  getBaseURL() {
    let mCurrentLocation = window.location;
    let mPort = mCurrentLocation.port;
    const mCurrentPath = window.location.pathname
    if (mPort === "80" || mPort.length === 0) {
        mPort = "";
    } else {
        mPort = ":" + mPort;
    }
    let mURL = mCurrentLocation.protocol + "//" + mCurrentLocation.hostname + mPort;
    const n = mCurrentPath.lastIndexOf("/");
    if (n !== 0) {
        mURL = mURL + '/' + mCurrentPath;
    }
    return mURL;
    // return window.location.origin;
  }
}
