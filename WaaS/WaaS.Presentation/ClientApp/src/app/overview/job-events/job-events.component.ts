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
    //this.loadJobEvents();
    this.loadTestEvents();

    this.jobEventsModalRef = this.modalService.show(this.jobEventsModalTemplateRef, this.modalConfig);
  }

  loadJobEvents() {
    this.jobService.getScrapeJobEvents().subscribe(
      events => {
        this.scrapeJobEvents = events;
      }
    );
  }

  loadTestEvents(){
    this.scrapeJobEvents = [
      { id: 0, HttpResponseCode: 200, HttpResponseTimeInMs: 200, Message: "testasdf", TimeStamp: "11-12-2019" },
      { id: 1, HttpResponseCode: 404, HttpResponseTimeInMs: 200, Message: "asdfadsf", TimeStamp: "12-12-2019" },
      { id: 2, HttpResponseCode: 200, HttpResponseTimeInMs: 200, Message: "hzzjd", TimeStamp: "13-12-2019" },
      { id: 3, HttpResponseCode: 200, HttpResponseTimeInMs: 200, Message: "vbcxb", TimeStamp: "14-12-2019" }
    ];
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
