import { TestBed } from '@angular/core/testing';

import { JwtHelperService } from './jwt-helper.service';

describe('JwtHelperService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: JwtHelperService = TestBed.get(JwtHelperService);
    expect(service).toBeTruthy();
  });
});
