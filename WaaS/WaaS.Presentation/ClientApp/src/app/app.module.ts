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
import { environment } from 'src/environments/environment';

import { AboutComponent } from './about/about.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtInterceptor } from './authentication/jwt/jwt.interceptor';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { AddJobComponent } from './overview/add-job/add-job.component';
import { OverviewComponent } from './overview/overview.component';
import { PipesModule } from './pipes/pipes.module';

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
    AddJobComponent
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
    AlertModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    {
      provide: RECAPTCHA_SETTINGS, useValue: {
        siteKey: environment.reCaptchaSiteKey,
        size: 'invisible'
      } as RecaptchaSettings
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
