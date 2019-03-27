import { Inject, Injectable } from '@angular/core';
import { CookieOptionsProvider, CookieService } from 'ngx-cookie';

@Injectable({
  providedIn: 'root'
})
export class CookieBackendService extends CookieService {

  private cookie = '';

  constructor(
    @Inject('COOKIE') private cookieHeader: object,
    optionsProvider: CookieOptionsProvider
  ) {
    super(optionsProvider);
    for (const key of Object.keys(cookieHeader)) {
      this.put(cookieHeader[key].key, cookieHeader[key].value);
    }
  }

  protected get cookieString(): string {
    return this.cookie;
  }

  protected set cookieString(val: string) {
    this.cookie = val;
  }
}
