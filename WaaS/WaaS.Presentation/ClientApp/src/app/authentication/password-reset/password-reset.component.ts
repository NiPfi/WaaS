import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RecaptchaComponent } from 'ng-recaptcha';
import { first } from 'rxjs/operators';

import { VerificationService } from '../verify/verification-service/verification.service';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss']
})
export class PasswordResetComponent implements OnInit {
  resetPasswordForm: FormGroup;

  @ViewChild('captchaRef') reCaptcha: RecaptchaComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly verification: VerificationService,
    private readonly router: Router
  ) { }

  ngOnInit() {
    this.resetPasswordForm = this.formBuilder.group({
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      recaptchaReactive: [null, Validators.required]
    });
  }

  get form() { return this.resetPasswordForm.controls; }

  onSubmit(captchaResponse: string) {
    const email = this.form.email.value;
    // stop here if form is invalid
    if (this.resetPasswordForm.invalid) {
      return;
    }
    this.verification.verifyPasswordReset(email, captchaResponse)
      .pipe(first())
      .subscribe(
        () => {
          this.router.navigate(['verify-password-reset'], { queryParams: { email } });
        });
  }
}
