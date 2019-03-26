import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { MockComponent } from 'ng-mocks';
import { RecaptchaComponent } from 'ng-recaptcha';
import { AlertModule } from 'ngx-bootstrap';
import { CookieModule } from 'ngx-cookie';
import { NgxSpinnerModule } from 'ngx-spinner';

import { PipesModule } from '../../pipes/pipes.module';
import { RegisterComponent } from './register.component';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        RegisterComponent,
        MockComponent(RecaptchaComponent)
      ],
      imports: [
        AlertModule,
        ReactiveFormsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        PipesModule,
        NgxSpinnerModule,
        CookieModule.forRoot()
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
