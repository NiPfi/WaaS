import { HttpClientTestingModule } from '@angular/common/http/testing';
import { inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CookieModule } from 'ngx-cookie';

import { JwtInterceptor } from './jwt.interceptor';

describe('JwtInterceptor', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        CookieModule.forRoot()
      ],
      providers: [JwtInterceptor]
    });
  });

  it('should ...', inject([JwtInterceptor], (guard: JwtInterceptor) => {
    expect(guard).toBeTruthy();
  }));
});
