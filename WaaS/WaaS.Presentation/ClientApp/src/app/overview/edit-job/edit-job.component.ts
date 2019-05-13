import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { first } from 'rxjs/internal/operators/first';
import { ValidationService } from 'src/app/error-handling/form-validation/validation-service/validation.service';

import { OverviewService } from '../overview-service/overview.service';
import { ScrapeJob } from '../scrape-job';

@Component({
  selector: 'app-edit-job',
  templateUrl: './edit-job.component.html',
  styleUrls: ['./edit-job.component.scss']
})
export class EditJobComponent implements OnInit {

  @Output() jobEdited = new EventEmitter();

  @ViewChild('editScrapeJobModal') editScrapeJobModalTemplateRef: TemplateRef<any>;

  editScrapeJobForm: FormGroup;
  editScrapeJobModalRef: BsModalRef;

  errorMessage = '';

  scrapeJob : ScrapeJob;


  constructor(
    private readonly jobsService: OverviewService,
    private readonly formBuilder: FormBuilder,
    private readonly modalService: BsModalService
  ) { }

  ngOnInit() {
    this.editScrapeJobForm = this.formBuilder.group({
      scrapeJobName: ['', [Validators.required]],
      url: ['', [Validators.required]],
      regexPattern: ['', [Validators.required]],
      alternativeEmail: ['', [Validators.email]]
    });
  }

  // convenience getter for easy access to form fields
  get form() { return this.editScrapeJobForm.controls; }

  createScrapeJob(){
    if (this.editScrapeJobForm.invalid) {
      ValidationService.validateAllFormFields(this.editScrapeJobForm);
      return;
    }

    const job = new ScrapeJob();
    job.name = this.editScrapeJobForm.controls.scrapeJobName.value;
    job.url = this.editScrapeJobForm.controls.url.value;
    job.pattern = this.editScrapeJobForm.controls.regexPattern.value;
    job.alternativeEmail = this.editScrapeJobForm.controls.alternativeEmail.value;

    this.jobsService.addScrapeJob(job)
    .pipe(first())
      .subscribe(
        () => {
          this.jobEdited.emit();
          this.editScrapeJobModalRef.hide();
        },
        error => {
          this.errorMessage = error;
        }
      )
      ;
  }

  openCreateModal(){
    this.openEditScrapeJobModal(this.editScrapeJobModalTemplateRef);
  }

  openEditModal(job: ScrapeJob){
    this.scrapeJob = job;
    this.openEditScrapeJobModal(this.editScrapeJobModalTemplateRef);
  }

  openEditScrapeJobModal(template: TemplateRef<any>) {
    this.editScrapeJobModalRef = this.modalService.show(template, {});
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
