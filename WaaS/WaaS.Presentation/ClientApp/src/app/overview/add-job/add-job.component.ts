import { Component, EventEmitter, OnInit, Output, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { first } from 'rxjs/internal/operators/first';
import { ValidationService } from 'src/app/error-handling/form-validation/validation-service/validation.service';

import { OverviewService } from '../overview-service/overview.service';
import { ScrapeJob } from '../scrape-job';

@Component({
  selector: 'app-add-job',
  templateUrl: './add-job.component.html',
  styleUrls: ['./add-job.component.scss']
})
export class AddJobComponent implements OnInit {

  @Output() jobAdded = new EventEmitter();

  addScrapeJobForm: FormGroup;
  addScrapeJobModalRef: BsModalRef;

  errorMessage = '';

  faPlus = faPlus;

  constructor(
    private readonly jobsService: OverviewService,
    private readonly formBuilder: FormBuilder,
    private readonly modalService: BsModalService
  ) { }

  ngOnInit() {
    this.addScrapeJobForm = this.formBuilder.group({
      scrapeJobName: ['', [Validators.required]],
      url: ['', [Validators.required]],
      regexPattern: ['', [Validators.required]],
      alternativeEmail: ['', [Validators.email]]
    });
  }

  // convenience getter for easy access to form fields
  get form() { return this.addScrapeJobForm.controls; }

  createScrapeJob(){
    if (this.addScrapeJobForm.invalid) {
      ValidationService.validateAllFormFields(this.addScrapeJobForm);
      return;
    }

    const job = new ScrapeJob();
    job.name = this.addScrapeJobForm.controls.scrapeJobName.value;
    job.url = this.addScrapeJobForm.controls.url.value;
    job.pattern = this.addScrapeJobForm.controls.regexPattern.value;
    job.alternativeEmail = this.addScrapeJobForm.controls.alternativeEmail.value;

    this.jobsService.addScrapeJob(job)
    .pipe(first())
      .subscribe(
        () => {
          this.jobAdded.emit();
          this.addScrapeJobModalRef.hide();
        },
        error => {
          this.errorMessage = error;
        }
      )
      ;
  }



  openAddScrapeJobModal(template: TemplateRef<any>) {
    this.addScrapeJobModalRef = this.modalService.show(template, {});
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
