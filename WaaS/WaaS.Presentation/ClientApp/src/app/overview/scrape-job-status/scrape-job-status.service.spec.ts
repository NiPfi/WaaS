import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { ScrapeJobStatusService } from './scrape-job-status.service';

describe('ScrapeJobStatusService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule,
      RouterTestingModule
    ]
  }));

  it('should be created', () => {
    const service: ScrapeJobStatusService = TestBed.get(ScrapeJobStatusService);
    expect(service).toBeTruthy();
  });
});
