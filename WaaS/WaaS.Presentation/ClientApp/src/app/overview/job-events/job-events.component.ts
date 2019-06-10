import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ScrapeJob } from '../scrape-job';
import { OverviewService } from '../overview-service/overview.service';
import { ScrapeJobEvent } from '../scrape-job-event';

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
  scrapeJobEvents: ScrapeJobEvent[];

  constructor(
    private readonly modalService: BsModalService,
    private readonly jobService: OverviewService
  ) { }

  ngOnInit() {
  }

  openJobEventsModal(job: ScrapeJob){
    this.scrapeJob = job;
    this.loadJobEvents(job.id);
    this.jobEventsModalRef = this.modalService.show(this.jobEventsModalTemplateRef, this.modalConfig);
    this.jobEventsModalRef.setClass("modal-lg");
  }

  loadJobEvents(scrapeJobId: number) {
    this.jobService.getScrapeJobEvents(scrapeJobId).subscribe(
      events => {
        this.scrapeJobEvents = events;
      }
    );
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
