import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

import { OverviewService } from '../overview-service/overview.service';
import { ScrapeJob } from '../scrape-job';

@Component({
  selector: 'app-add-job',
  templateUrl: './add-job.component.html',
  styleUrls: ['./add-job.component.scss']
})
export class AddJobComponent implements OnInit {

  @Output() jobAdded = new EventEmitter();

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
    tempJob.pattern = 'Pattern';
    tempJob.url = 'http://www.test.com';
    this.jobsService.addScrapeJob(tempJob).subscribe(
      () => {
        this.jobAdded.emit();
      }
    );
  }

}
