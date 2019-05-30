import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ScrapeJob } from '../scrape-job';

@Component({
  selector: 'app-job-events',
  templateUrl: './job-events.component.html',
  styleUrls: ['./job-events.component.scss']
})
export class JobEventsComponent implements OnInit {

  @ViewChild('jobEventsModal') jobEventsModalTemplateRef: TemplateRef<any>;

  jobEventsModalRef: BsModalRef;

  modalConfig = {
    backdrop: true,
    ignoreBackdropClick: true
  }

  errorMessage = '';

  scrapeJob : ScrapeJob;

  constructor(
    private readonly modalService: BsModalService
  ) { }

  ngOnInit() {
  }

  openJobEventsModal(job: ScrapeJob){
    this.scrapeJob = job;
    this.jobEventsModalRef = this.modalService.show(this.jobEventsModalTemplateRef, this.modalConfig);
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
