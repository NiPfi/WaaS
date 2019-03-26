import { Inject, Injectable } from '@angular/core';
import { CookieOptionsProvider, CookieService } from 'ngx-cookie';

@Injectable({
  providedIn: 'root'
})
export class CookieBackendService extends CookieService {

  constructor(
    @Inject('COOKIES') private cookies: any,
    optionsProvider: CookieOptionsProvider
  ) {
    super(optionsProvider);
  }

  protected get cookieString(): string {
    return this.cookies || '';
  }

  protected set cookieString(val: string) {
    this.cookies = val;
  }
}
