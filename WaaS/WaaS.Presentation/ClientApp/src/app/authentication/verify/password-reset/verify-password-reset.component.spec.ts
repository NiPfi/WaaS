import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { MockPipe } from 'ng-mocks';
import { AlertModule } from 'ngx-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ConvertNewLinePipe } from 'src/app/pipes/new-line-pipe/convert-new-line.pipe';

import { VerifyPasswordResetComponent } from './verify-password-reset.component';

describe('VerifyPasswordResetComponent', () => {
  let component: VerifyPasswordResetComponent;
  let fixture: ComponentFixture<VerifyPasswordResetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        VerifyPasswordResetComponent,
        MockPipe(ConvertNewLinePipe)
      ],
      imports: [
        AlertModule,
        ReactiveFormsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        NgxSpinnerModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyPasswordResetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
