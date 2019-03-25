import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { first } from 'rxjs/internal/operators/first';

import { AuthService } from '../authentication/auth.service';
import { EditProfileService } from './edit-profile.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {

  changeEmailForm: FormGroup;
  changePasswordForm: FormGroup;
  deleteModalRef: BsModalRef;

  successMessage = '';
  errorMessage = '';

  constructor(
    private readonly authService: AuthService,
    private readonly formBuilder: FormBuilder,
    private readonly modalService: BsModalService,
    private readonly editProfileService: EditProfileService,
    private readonly spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.changeEmailForm = this.formBuilder.group({
      email: [this.authService.getUserEmail(), [Validators.required, Validators.email]],
    });

    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', [Validators.required, Validators.minLength(8)]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmitEmail() {
    if (this.changeEmailForm.invalid) {
      return;
    }

    this.spinner.show();

    this.editProfileService.updateEmail(this.changeEmailForm.controls.email.value)
      .pipe(first())
      .subscribe(
        data => {
          this.successMessage = `Your E-Mail has been changed to ${data.email}`;
        },
        error => {
          this.errorMessage = error;
          this.spinner.hide();
        },
        () => {
          this.spinner.hide();
        }
      )
      ;
  }

  onSubmitPassword() {
    if (this.changePasswordForm.invalid) {
      return;
    }

    this.spinner.show();

    return this.editProfileService.updatePassword(
      this.changePasswordForm.controls.currentPassword.value,
      this.changePasswordForm.controls.newPassword.value)
      .pipe(first())
      .subscribe(
        () => {
          this.successMessage = 'Your password has been updated';
        },
        error => {
          this.errorMessage = error;
          this.spinner.hide();
        },
        () => this.spinner.hide()
      );
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deleteModalRef = this.modalService.show(template, {});
  }

  confirmDelete() {
    this.deleteModalRef.hide();
    this.editProfileService.deleteAccount();
  }

  onSuccessAlertClosed() {
    this.successMessage = '';
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
