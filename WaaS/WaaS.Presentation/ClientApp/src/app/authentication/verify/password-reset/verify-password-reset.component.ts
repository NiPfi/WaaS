import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { VerificationService } from '../verification-service/verification.service';

@Component({
  selector: 'app-verify-password-reset',
  templateUrl: './verify-password-reset.component.html',
  styleUrls: ['./verify-password-reset.component.scss']
})
export class VerifyPasswordResetComponent implements OnInit {
  resetPasswordForm: FormGroup;
  email: string;
  token: string;
  errorMessage: string;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly verification: VerificationService,
    private readonly formBuilder: FormBuilder,
  ) { }

  get form() { return this.resetPasswordForm.controls; }

  ngOnInit() {
    this.email = this.route.snapshot.queryParamMap.get('email');
    this.token = this.route.snapshot.queryParamMap.get('verificationToken');

    this.resetPasswordForm = this.formBuilder.group({
      password: ['', [
        Validators.required,
        Validators.minLength(8)
      ]]
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }
    if (this.email && this.token) {
      this.verification.doPasswordReset(this.email, this.form.password.value, this.token).subscribe(
        () => {
          this.router.navigate(['login']);
        },
        errorMessage => {
          this.errorMessage = errorMessage;
        }
      );
    }
  }

}
