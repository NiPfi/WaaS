import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { MockComponent } from 'ng-mocks';
import { RecaptchaComponent } from 'ng-recaptcha';
import { CookieModule } from 'ngx-cookie';

import { ResendConfirmationEmailComponent } from './resend-confirmation-email.component';

describe('ResendConfirmationEmailComponent', () => {
  let component: ResendConfirmationEmailComponent;
  let fixture: ComponentFixture<ResendConfirmationEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ResendConfirmationEmailComponent,
        MockComponent(RecaptchaComponent),
      ],
      imports: [
        ReactiveFormsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        CookieModule.forRoot()
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResendConfirmationEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
