import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

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

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private modalService: BsModalService,
    private editProfileService: EditProfileService
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

    return this.editProfileService.updateEmail(this.changeEmailForm.controls.email.value);
  }

  onSubmitPassword() {
    if (this.changePasswordForm.invalid) {
      return;
    }

    return this.editProfileService.updatePassword(this.changePasswordForm.controls.currentPassword.value,
      this.changePasswordForm.controls.newPassword.value);
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deleteModalRef = this.modalService.show(template, { });
  }

  confirmDelete() {
    this.deleteModalRef.hide();
    this.editProfileService.deleteAccount();
  }

}
