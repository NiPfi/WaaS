import { Component, OnInit } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

import { OverviewService } from '../overview-service/overview.service';
import { ScrapeJob } from '../scrape-job';

@Component({
  selector: 'app-add-job',
  templateUrl: './add-job.component.html',
  styleUrls: ['./add-job.component.scss']
})
export class AddJobComponent implements OnInit {

  faPlus = faPlus;

  constructor(
    private readonly jobsService: OverviewService
  ) { }

  ngOnInit() {
  }

  onAddButtonClick() {
    // TODO Modal
    const tempJob = new ScrapeJob();
    tempJob.name = 'testJob';
    tempJob.pattern = new RegExp('Pattern');
    tempJob.url = new URL('http://www.test.com');
    this.jobsService.addScrapeJob(tempJob).subscribe(
      () => {}
    );
  }

}
