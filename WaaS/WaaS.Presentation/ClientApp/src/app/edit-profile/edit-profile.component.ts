import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
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
    private readonly editProfileService: EditProfileService
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

    this.editProfileService.updateEmail(this.changeEmailForm.controls.email.value)
      .pipe(first())
      .subscribe(
        data => {
          this.successMessage = `Your E-Mail has been changed to ${data.email}`;
        },
        error => {
          this.errorMessage = error;
        }
      )
      ;
  }

  onSubmitPassword() {
    if (this.changePasswordForm.invalid) {
      return;
    }

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
        }
      );
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deleteModalRef = this.modalService.show(template, {});
  }

  confirmDelete() {
    this.deleteModalRef.hide();
    this.editProfileService.deleteAccount()
      .pipe(first())
      .subscribe(
        () => {
          this.authService.logout();
        },
        error => {
          this.errorMessage = error;
        }
      );;
  }

  onSuccessAlertClosed() {
    this.successMessage = '';
  }

  onErrorAlertClosed() {
    this.errorMessage = '';
  }

}
