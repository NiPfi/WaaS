import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { VerificationService } from '../../verification-service/verification.service';

@Component({
  selector: 'app-verify-mail-change',
  templateUrl: './verify-mail-change.component.html',
  styleUrls: ['./verify-mail-change.component.scss']
})
export class VerifyMailChangeComponent implements OnInit {

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly verification: VerificationService
  ) { }

  public email: string;
  public token: string;
  public errorMessage: string;
  public successMessage: string;

  ngOnInit() {
    this.email = this.route.snapshot.queryParamMap.get('email');
    this.token = this.route.snapshot.queryParamMap.get('verificationToken');

    if (this.email && this.token) {
      this.verification.verifyEmailChange(this.email, this.token).subscribe(
        data => {
          this.successMessage = `Your E-Mail address has been changed to ${data.email}`;
        },
        errorMessage => {
          this.errorMessage = errorMessage;
        }
      );
    }
  }
}
