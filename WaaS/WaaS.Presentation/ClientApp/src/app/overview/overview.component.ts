import { Component, OnInit } from '@angular/core';

import { OverviewService } from './overview-service/overview.service';
import { ScrapeJob } from './scrape-job';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  public jobs: ScrapeJob[] = [];

  constructor(
    private readonly jobsService: OverviewService
  ) { }

  ngOnInit() {
    this.jobsService.getScrapeJobs().subscribe(
      jobs => {
        this.jobs = jobs
      }
    );
  }

}
