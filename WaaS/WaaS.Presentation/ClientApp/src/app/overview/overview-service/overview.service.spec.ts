import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { OverviewService } from './overview.service';

describe('OverviewService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ]
  }));

  it('should be created', () => {
    const service: OverviewService = TestBed.get(OverviewService);
    expect(service).toBeTruthy();
  });
});
