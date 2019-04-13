import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CookieModule } from 'ngx-cookie';

import { AuthService } from '../auth.service';
import { VerificationService } from './verification.service';

describe('VerificationService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule,
      RouterTestingModule,
      CookieModule.forRoot()
    ],
    providers: [
      AuthService
    ]
  }));

  it('should be created', () => {
    const service: VerificationService = TestBed.get(VerificationService);
    expect(service).toBeTruthy();
  });
});
