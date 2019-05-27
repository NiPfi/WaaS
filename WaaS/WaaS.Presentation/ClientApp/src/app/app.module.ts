import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RECAPTCHA_SETTINGS, RecaptchaModule, RecaptchaSettings } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { BsDropdownModule } from 'ngx-bootstrap';
import { AlertModule } from 'ngx-bootstrap/alert';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxSpinnerModule } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

import { AboutComponent } from './about/about.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtInterceptor } from './authentication/jwt/jwt.interceptor';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import {
  ResendConfirmationEmailComponent,
} from './authentication/resend-confirmation-email/resend-confirmation-email.component';
import { VerifyRegistrationComponent } from './authentication/verify/registration/verify-registration.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { ControlMessagesComponent } from './error-handling/form-validation/control-messages/control-messages.component';
import { ValidationService } from './error-handling/form-validation/validation-service/validation.service';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { HttpSpinnerInterceptor } from './http-spinner/http-spinner.interceptor';
import { OverviewComponent } from './overview/overview.component';
import { PipesModule } from './pipes/pipes.module';
import { VerifyMailChangeComponent } from './authentication/verify/mail-change/verify-mail-change.component';
import { PasswordResetComponent } from './authentication/password-reset/password-reset.component';
import { VerifyPasswordResetComponent } from './authentication/verify/password-reset/verify-password-reset.component';
import { EditJobComponent } from './overview/edit-job/edit-job.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    EditProfileComponent,
    AboutComponent,
    OverviewComponent,
    EditJobComponent,
    VerifyRegistrationComponent,
    ControlMessagesComponent,
    ResendConfirmationEmailComponent,
    VerifyMailChangeComponent,
    PasswordResetComponent,
    VerifyPasswordResetComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    PipesModule,
    FontAwesomeModule,
    NgxSpinnerModule,
    AlertModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpSpinnerInterceptor, multi: true },
    {
      provide: RECAPTCHA_SETTINGS, useValue: {
        siteKey: environment.reCaptchaSiteKey,
        size: 'invisible'
      } as RecaptchaSettings
    },
    [ ValidationService ]
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
