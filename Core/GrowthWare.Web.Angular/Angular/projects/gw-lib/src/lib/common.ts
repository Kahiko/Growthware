export class Common {
  static get baseURL(): string {
    let mCurrentLocation = window.location;
    let mPort = mCurrentLocation.port;
    const mCurrentPath = window.location.pathname;
    if (mPort === '80' || mPort.length === 0) {
      mPort = '';
    } else {
      mPort = ':' + mPort;
    }
    let mURL = mCurrentLocation.protocol + '//' + mCurrentLocation.hostname + mPort;
    const mLastSlashPos = mCurrentPath.lastIndexOf('/');
    if (mLastSlashPos !== 0) {
      mURL = mURL + '/' + mCurrentPath;
    } else {
      mURL = mURL + '/'
    }
    return mURL;
  }

  static isNullorEmpty(str: string) {
    if (!str || 0 === str.length || !str || /^\s*$/.test(str)) {
      return true;
    }
    return false;
  }

  static isNullOrUndefined(obj: any) {
    if (obj == null || obj === null || typeof obj === 'undefined') {
      return true;
    }
    return false;
}
}
