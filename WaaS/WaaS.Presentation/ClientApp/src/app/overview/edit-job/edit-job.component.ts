import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { first } from 'rxjs/internal/operators/first';
import { Subscription } from 'rxjs';
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
  onHiddenSubscription: Subscription;
  modalConfig = {
    backdrop: true,
    ignoreBackdropClick: true
  }


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

  saveScrapeJob(){
    if (this.editScrapeJobForm.invalid) {
      ValidationService.validateAllFormFields(this.editScrapeJobForm);
      return;
    }

    const job = this.scrapeJob ? this.scrapeJob : new ScrapeJob();
    job.name = this.form.scrapeJobName.value;
    job.url = this.form.url.value;
    job.pattern = this.form.regexPattern.value;
    job.alternativeEmail = this.form.alternativeEmail.value;

    if(job.id == null || job.id == 0){
      this.createScrapeJob(job);
    }
    else if (job === this.scrapeJob){
      this.editScrapeJobModalRef.hide();
      return;
    } else{
      this.updateScrapeJob(job);
    }
  }

  createScrapeJob(job: ScrapeJob){
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
      );
  }

  updateScrapeJob(job: ScrapeJob){
    this.jobsService.updateScrapeJob(job)
    .pipe(first())
      .subscribe(
        () => {
          this.jobEdited.emit();
          this.editScrapeJobModalRef.hide();
        },
        error => {
          this.errorMessage = error;
        }
      );
  }

  openCreateModal(){
    this.scrapeJob = null;
    this.openEditScrapeJobModal(this.editScrapeJobModalTemplateRef);
  }

  openEditModal(job: ScrapeJob){
    this.scrapeJob = job;
    this.form.scrapeJobName.setValue(job.name);
    this.form.url.setValue(job.url);
    this.form.regexPattern.setValue(job.pattern);
    this.form.alternativeEmail.setValue(job.alternativeEmail);

    this.openEditScrapeJobModal(this.editScrapeJobModalTemplateRef);
  }

  openEditScrapeJobModal(template: TemplateRef<any>) {

    this.onHiddenSubscription = this.modalService.onHidden.subscribe((reason: string) => {
      this.onModalHidden();
    });

    this.editScrapeJobModalRef = this.modalService.show(template, this.modalConfig);
  }

  onModalHidden(){
    this.editScrapeJobForm.reset();
    this.errorMessage = "";
    this.scrapeJob = null;
    this.onHiddenSubscription.unsubscribe();
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
