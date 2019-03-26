import { TestBed } from '@angular/core/testing';

import { CookieBackendService } from './cookie-backend.service';

describe('CookieBackendService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CookieBackendService = TestBed.get(CookieBackendService);
    expect(service).toBeTruthy();
  });
});
