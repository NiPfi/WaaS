import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RecaptchaComponent } from 'ng-recaptcha';
import { first } from 'rxjs/internal/operators/first';

import { VerificationService } from '../verify/verification-service/verification.service';

@Component({
  selector: 'app-resend-confirmation-email',
  templateUrl: './resend-confirmation-email.component.html',
  styleUrls: ['./resend-confirmation-email.component.scss']
})
export class ResendConfirmationEmailComponent implements OnInit {
  resendConfirmationForm: FormGroup;

  @ViewChild('captchaRef', { static: true }) reCaptcha: RecaptchaComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly verification: VerificationService,
    private readonly router: Router
  ) { }

  ngOnInit() {
    this.resendConfirmationForm = this.formBuilder.group({
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      recaptchaReactive: [null, Validators.required]
    });
  }

  get form() { return this.resendConfirmationForm.controls; }

  onSubmit(captchaResponse: string) {
    const email = this.form.email.value;
    // stop here if form is invalid
    if (this.resendConfirmationForm.invalid) {
      return;
    }
    this.verification.resendConfirmationEmail(email, captchaResponse)
      .pipe(first())
      .subscribe(
        () => {
          this.router.navigate(['verify-registration'], { queryParams: { email } });
        });
  }

}
