import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { VerificationService } from './verification.service';

describe('VerificationService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ]
  }));

  it('should be created', () => {
    const service: VerificationService = TestBed.get(VerificationService);
    expect(service).toBeTruthy();
  });
});
