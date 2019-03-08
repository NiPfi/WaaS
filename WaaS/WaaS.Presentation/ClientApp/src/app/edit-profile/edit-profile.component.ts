import { Component, OnInit } from '@angular/core';
import { AuthService } from '../authentication/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {

  changeEmailForm: FormGroup;
  changePasswordForm: FormGroup;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder
    ) { }

  ngOnInit() {
    this.changeEmailForm = this.formBuilder.group({
      email: [this.authService.getUserEmail(), [Validators.required, Validators.email]],
    });

    this.changePasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

}
