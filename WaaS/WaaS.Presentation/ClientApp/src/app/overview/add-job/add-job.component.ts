import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

import { OverviewService } from '../overview-service/overview.service';
import { ScrapeJob } from '../scrape-job';

@Component({
  selector: 'app-add-job',
  templateUrl: './add-job.component.html',
  styleUrls: ['./add-job.component.scss']
})
export class AddJobComponent implements OnInit {

  addScrapeJobForm: FormGroup;
  addScrapeJobModalRef: BsModalRef;

  faPlus = faPlus;

  constructor(
    private readonly jobsService: OverviewService,
    private readonly formBuilder: FormBuilder,
    private readonly modalService: BsModalService
  ) { }

  ngOnInit() {
    this.addScrapeJobForm = this.formBuilder.group({
      url: ['', [Validators.required]],
      regexPattern: ['', [Validators.required]],
      alternateEmail: ['']
    });
  }

  createScrapeJob(){
    // TODO implement
    console.log("it works");
  }

  openAddScrapeJobModal(template: TemplateRef<any>) {
    this.addScrapeJobModalRef = this.modalService.show(template, {});
  }

  onAddButtonClick() {
    // TODO Modal
    const tempJob = new ScrapeJob();
    tempJob.name = 'testJob';
    tempJob.pattern = 'pattern';
    tempJob.url = new URL('http://www.test.com');
    this.jobsService.addScrapeJob(tempJob).subscribe(
      () => {}
    );
  }

}
