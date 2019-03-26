import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ServerModule } from '@angular/platform-server';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';
import { RecaptchaModule } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { AlertModule, BsDropdownModule, ModalModule } from 'ngx-bootstrap';
import { CookieService } from 'ngx-cookie';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppModule } from './app.module';
import { CookieBackendService } from './authentication/cookie-backend.service';
import { PipesModule } from './pipes/pipes.module';

@NgModule({
  imports: [
    AppModule,
    PipesModule,
    ServerModule,
    ModuleMapLoaderModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    FontAwesomeModule,
    AlertModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [{ provide: CookieService, useClass: CookieBackendService }],
  bootstrap: [AppComponent]
})
export class AppServerModule { }
