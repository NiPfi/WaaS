import { Injectable } from '@angular/core';

import { ScrapeJob } from '../scrape-job';

@Injectable({
  providedIn: 'root'
})
export class OverviewService {

  constructor() { }

  getScrapeJobs(): ScrapeJob[] {
    return [];
    // TODO get from backend
  }
}
