export class common {
  public static get baseURL(): string {
    let mLocation = window.location.hostname;
    let mPort = window.location.port;
    let mProtocol = window.location.protocol
    if (mPort === "80" || mPort.length === 0) {
        mPort = "";
    } else {
        mPort = ":" + mPort;
    }
    let mUrl = mProtocol + "//" + mLocation + mPort
    if(!mUrl.endsWith('/') || !mUrl.endsWith('\\')) {
      mUrl += '/';
    }
    return mUrl;
  }
}
