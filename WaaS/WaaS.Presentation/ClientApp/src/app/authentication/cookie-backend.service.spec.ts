import { TestBed } from '@angular/core/testing';
import { CookieModule } from 'ngx-cookie';

import { CookieBackendService } from './cookie-backend.service';

describe('CookieBackendService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      CookieModule.forRoot()
    ],
    providers: [
      { provide: 'COOKIE', useValue: '' }
    ]
  }));

  it('should be created', () => {
    const service: CookieBackendService = TestBed.get(CookieBackendService);
    expect(service).toBeTruthy();
  });
});
