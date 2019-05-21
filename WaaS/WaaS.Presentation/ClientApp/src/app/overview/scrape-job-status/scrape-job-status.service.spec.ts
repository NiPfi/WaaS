import { TestBed } from '@angular/core/testing';

import { ScrapeJobStatusService } from './scrape-job-status.service';

describe('ScrapeJobStatusService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ScrapeJobStatusService = TestBed.get(ScrapeJobStatusService);
    expect(service).toBeTruthy();
  });
});
