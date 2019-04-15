import { Component, OnInit } from '@angular/core';

import { faPen, faTrashAlt, faToggleOn, faToggleOff } from '@fortawesome/free-solid-svg-icons';
import { OverviewService } from './overview-service/overview.service';
import { ScrapeJob } from './scrape-job';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  faPen = faPen;
  faTrashAlt = faTrashAlt;
  faToggleOn = faToggleOn;
  faToggleOff = faToggleOff;

  public jobs: ScrapeJob[];

  constructor(
    private readonly jobsService: OverviewService
  ) { }

  ngOnInit() {
    this.loadJobs();
  }

  onJobAdded() {
    this.loadJobs();
  }

  loadJobs() {
    this.jobsService.getScrapeJobs().subscribe(
      jobs => {
        this.jobs = jobs;
      }
    );
  }
}
