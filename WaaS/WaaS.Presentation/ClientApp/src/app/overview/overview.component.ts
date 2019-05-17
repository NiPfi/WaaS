import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';

import { faPen, faTrashAlt, faToggleOn, faToggleOff, faPlus } from '@fortawesome/free-solid-svg-icons';
import { OverviewService } from './overview-service/overview.service';
import { ScrapeJob } from './scrape-job';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { first } from 'rxjs/internal/operators/first';
import { EditJobComponent } from './edit-job/edit-job.component';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  @ViewChild(EditJobComponent) editJobComponent: EditJobComponent;

  faPen = faPen;
  faTrashAlt = faTrashAlt;
  faToggleOn = faToggleOn;
  faToggleOff = faToggleOff;
  faPlus = faPlus;

  deleteModalRef: BsModalRef;

  successMessage = '';
  errorMessage = '';

  public jobs: ScrapeJob[];

  public currentJobIndex: number;

  constructor(
    private readonly modalService: BsModalService,
    private readonly jobsService: OverviewService
  ) { }

  ngOnInit() {
    this.loadJobs();
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

  openDeleteModal(template: TemplateRef<any>, i: number) {
    this.deleteModalRef = this.modalService.show(template, {});
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

  onSuccessAlertClosed() {
    this.successMessage = '';
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
