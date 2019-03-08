import { Component, OnInit, TemplateRef } from '@angular/core';
import { AuthService } from '../authentication/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
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
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deleteModalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirmDelete() {
    this.deleteModalRef.hide();
    this.editProfileService.deleteAccount();
  }

}
