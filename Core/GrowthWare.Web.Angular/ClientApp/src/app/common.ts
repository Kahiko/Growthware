export class common {
  public static get baseURL(): string {
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
    if(!mURL.endsWith('/') || !mURL.endsWith('\\')) {
      mURL += '/';
    }
    return mURL;
  }
}
