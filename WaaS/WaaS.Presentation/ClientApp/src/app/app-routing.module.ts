import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AboutComponent } from './about/about.component';
import { AuthGuard } from './authentication/auth.guard';
import { LoginComponent } from './authentication/login/login.component';
import { PasswordResetComponent } from './authentication/password-reset/password-reset.component';
import { RegisterComponent } from './authentication/register/register.component';
import {
  ResendConfirmationEmailComponent,
} from './authentication/resend-confirmation-email/resend-confirmation-email.component';
import { VerifyMailChangeComponent } from './authentication/verify/mail-change/verify-mail-change.component';
import { VerifyRegistrationComponent } from './authentication/verify/registration/verify-registration.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { HomeComponent } from './home/home.component';
import { OverviewComponent } from './overview/overview.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'profile',
    component: EditProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'about',
    component: AboutComponent
  },
  {
    path: 'overview',
    component: OverviewComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'verify-registration',
    component: VerifyRegistrationComponent
  },
  {
    path: 'verify-mail-change',
    component: VerifyMailChangeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'resend-confirmation-email',
    component: ResendConfirmationEmailComponent
  },
  {
    path: 'reset-password',
    component: PasswordResetComponent
  },

  // otherwise redirect to home
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
