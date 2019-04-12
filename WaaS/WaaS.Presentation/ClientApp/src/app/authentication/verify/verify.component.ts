import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { VerificationService } from '../verification-service/verification.service';

@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.scss']
})
export class VerifyComponent implements OnInit {

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly verification: VerificationService
  ) { }

  public email: string;
  public token: string;
  public errorMessage: string;

  ngOnInit() {
    this.email = this.route.snapshot.queryParamMap.get('email');
    this.token = this.route.snapshot.queryParamMap.get('verificationToken');

    if (this.email && this.token) {
      this.verification.verifyEmailConfirmation(this.email, this.token).subscribe(
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
