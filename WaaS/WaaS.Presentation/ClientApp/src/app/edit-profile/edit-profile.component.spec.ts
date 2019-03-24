import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { AlertModule, ModalModule } from 'ngx-bootstrap';

import { AuthService } from '../authentication/auth.service';
import { ConvertNewLinePipe } from '../pipes/new-line-pipe/convert-new-line.pipe';
import { EditProfileComponent } from './edit-profile.component';

describe('EditProfileComponent', () => {
  let component: EditProfileComponent;
  let fixture: ComponentFixture<EditProfileComponent>;
  let authService: AuthService;
  let getUserEmailSpy;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        EditProfileComponent,
        ConvertNewLinePipe
      ],
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule,
        ModalModule.forRoot(),
        AlertModule.forRoot()
      ],
      providers: [
        AuthService
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProfileComponent);
    component = fixture.componentInstance;
    authService = fixture.debugElement.injector.get(AuthService);
    getUserEmailSpy = spyOn(authService, 'getUserEmail').and.returnValue(undefined);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
