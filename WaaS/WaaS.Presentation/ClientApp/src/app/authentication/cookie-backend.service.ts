import { Inject, Injectable } from '@angular/core';
import { CookieOptionsProvider, CookieService } from 'ngx-cookie';

@Injectable({
  providedIn: 'root'
})
export class CookieBackendService extends CookieService {

  private cookie: string;

  constructor(
    @Inject('COOKIE') private cookieHeader: any,
    optionsProvider: CookieOptionsProvider
  ) {
    super(optionsProvider);
    this.cookie = JSON.stringify(this.cookieHeader);
  }

  protected get cookieString(): string {
    return this.cookie;
  }

  protected set cookieString(val: string) {
    this.cookie = val;
  }
}
