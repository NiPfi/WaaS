import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { faCircle, faHistory, faPen, faPlus, faToggleOff, faToggleOn, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { first } from 'rxjs/internal/operators/first';

import { EditJobComponent } from './edit-job/edit-job.component';
import { JobEventsComponent } from './job-events/job-events.component';
import { OverviewService } from './overview-service/overview.service';
import { ScrapeJob } from './scrape-job';
import { ScrapeJobStatusService } from './scrape-job-status/scrape-job-status.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit, OnDestroy {

  @ViewChild(EditJobComponent) editJobComponent: EditJobComponent;
  @ViewChild(JobEventsComponent) jobEventsComponent: JobEventsComponent;

  faPen = faPen;
  faTrashAlt = faTrashAlt;
  faToggleOn = faToggleOn;
  faToggleOff = faToggleOff;
  faPlus = faPlus;
  faHistory = faHistory;
  faCircle = faCircle;

  deleteModalRef: BsModalRef;
  modalConfig = {
    backdrop: true,
    ignoreBackdropClick: true
  };

  successMessage = '';
  errorMessage = '';

  public jobs: ScrapeJob[];

  public currentJobIndex: number;

  constructor(
    public readonly statusService: ScrapeJobStatusService,
    private readonly modalService: BsModalService,
    private readonly jobsService: OverviewService
  ) { }

  ngOnInit(): void {
    this.loadJobs();
    this.statusService.startConnection();
  }

  ngOnDestroy(): void {
    this.statusService.closeConnection();
  }

  onJobEdited() {
    this.loadJobs();
  }

  loadJobs() {
    this.jobsService.getScrapeJobs().subscribe(
      jobs => {
        this.jobs = jobs;
      }
    );
  }

  openCreateModal(){
    this.editJobComponent.openCreateModal();
  }

  openEditModal(i: number){
    this.editJobComponent.openEditModal(this.jobs[i]);
  }

  openEventsModal(i: number){
    this.jobEventsComponent.openJobEventsModal(this.jobs[i]);
  }

  openDeleteModal(template: TemplateRef<any>, i: number) {
    this.deleteModalRef = this.modalService.show(template, this.modalConfig);
    this.currentJobIndex = i;
  }

  confirmDelete(job: ScrapeJob) {
    this.deleteModalRef.hide();
    this.jobsService.deleteScrapeJob(job.id)
      .pipe(first())
      .subscribe(
        () => {
          this.successMessage = "Successfully deleted ScrapeJob";
          this.loadJobs();
        },
        error => {
          this.errorMessage = error;
        }
      );;
  }

  toggleEnabled(index: number){
    var job = this.jobs[index];
    this.jobsService.toggleEnabled(job)
      .pipe(first())
      .subscribe(
        () => {
          this.loadJobs();
        },
        error => {
          this.errorMessage = error;
        }
      );
  }

  onSuccessAlertClosed() {
    this.successMessage = '';
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
