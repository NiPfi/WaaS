import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RecaptchaComponent } from 'ng-recaptcha';
import { first } from 'rxjs/operators';

import { AuthService } from '../auth.service';
import { User } from '../user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  returnUrl: string;
  error = '';

  @ViewChild('captchaRef') reCaptcha: RecaptchaComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService
  ) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
      ]],
      recaptchaReactive: [null, Validators.required]
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
  }

  // convenience getter for easy access to form fields
  get form() { return this.registerForm.controls; }

  onSubmit(captchaResponse: string) {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    const userDto = new User(this.form.email.value, this.form.password.value);
    this.authService.register(userDto, captchaResponse)
      .pipe(first())
      .subscribe(
        data => {
          const email = (data as User).email;
          this.router.navigate(['verify-registration'], { queryParams: {email} });
        },
        error => {
          this.error = error;
          this.reCaptcha.reset();
        });
  }

  onErrorAlertClosed() {
    this.error = '';
  }

}
