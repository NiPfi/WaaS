import { TestBed, async, inject } from '@angular/core/testing';

import { JwtInterceptor } from './jwt.interceptor';

describe('JwtInterceptor', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [JwtInterceptor]
    });
  });

  it('should ...', inject([JwtInterceptor], (guard: JwtInterceptor) => {
    expect(guard).toBeTruthy();
  }));
});
